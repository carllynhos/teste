// RelatorioGerencial.cs created with MonoDevelop
// User: diogolima at 14:13 11/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using System.Collections;
using System.Data;
using Npgsql;
using System.Collections.Generic;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Classe para manutenção de relatórios gerenciais gerados e salvos.
	/// </summary>
	[ActiveRecord(Table="tb_relatorio_gerencial_rge", Schema="adm_licitar")]
	public class SrvRelatorioGerencial : ActiveRecordBase<SrvRelatorioGerencial>
	{
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_relatorio_gerencial_rge", SequenceName="adm_licitar.sq_relatorio_gerencial_rge")]
		public virtual int Id {get; set;}
		
		[Property("txt_descricao_rge")]
		public virtual string Descricao {get;set;}
		
		[Property("txt_campos_rge")]
		public virtual string Campos {get; set;}
		
		[Property("txt_filtros_rge")]
		public virtual string Filtros {get; set;}
		
		[Property("txt_classificacao_rge")]
		public virtual string Classificacao {get; set;}
		
		[Property("txt_totalizadores_rge")]
		public virtual string Totalizadores {get; set;}
		
		[Property("txt_agrupar_rge")]
		public virtual string Group {get; set;}
		
		[Property("fk_cod_pessoa_pes")]
		public virtual int Pessoa {get; set;}
		
		/// <summary>
		/// Construtor.
		/// </summary>
		public SrvRelatorioGerencial()
		{
		}
		
		/// <summary>
		/// Construtor. 
		/// </summary>
		public SrvRelatorioGerencial(int id)
		{
			this.Id = id;
		}
		
		
		
		/// <summary>
		/// Método que retorna os processos concluídos com exito
		/// </summary>
		/// <returns>
		/// A <see cref="DataTable"/>
		/// </returns>
		public DataTable RetornaProcessosConcluidosExito(DateTime dataInicial, DateTime dataFinal, string idsModalidade)
		{
			NpgsqlCommand cmd = new NpgsqlCommand();
			
			cmd.CommandText = @"				
					select txt_descricao_ins,txt_modalidade_mod, txt_numero_licitacao_npr, txt_numero_spu_npr, 
					txt_observacao_pro, txt_situacao_atual_sit, dat_cadastro_conclusao_pan,
					num_estimado_real_vpr, num_nao_contratado_vpr, num_processo_a_ser_contratado,
					num_economia_vpr, cod_processo_pro from adm_licitar.tb_processo_completo_pcm pcm
					where dat_cadastro_conclusao_pan between '@dataInicial' and '@dataFinal 23:59:59'
					and num_processo_a_ser_contratado is not null and num_processo_a_ser_contratado > 0
					AND txt_estado_processo = 'FINALIZADO' @modalidade
					order by txt_descricao_ins,txt_modalidade_mod
					";
			if (idsModalidade != null && idsModalidade!= String.Empty)
			{
				cmd.CommandText = cmd.CommandText.Replace("@modalidade"," and cod_modalidade_mod in (" + idsModalidade + ") ");
			}
			else
			{
				cmd.CommandText = cmd.CommandText.Replace("@modalidade"," ");	
			}
			cmd.CommandText = cmd.CommandText.Replace("@dataInicial",dataInicial.Date.ToString("dd/MM/yyyy")).
				Replace("@dataFinal",dataFinal.Date.ToString("dd/MM/yyyy"));
			
	  		return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
			
		}
		
		
		/// <summary>
		/// Método que retorna os processos concluídos sem exito
		/// </summary>
		/// <returns>
		/// A <see cref="DataTable"/>
		/// </returns>
		public DataTable RetornaProcessosConcluidosSemExito(DateTime dataInicial, DateTime dataFinal, string idsModalidade)
		{
			NpgsqlCommand cmd = new NpgsqlCommand();
			
			cmd.CommandText = @"				
					select txt_descricao_ins,txt_modalidade_mod, txt_numero_licitacao_npr, txt_numero_spu_npr, 
					txt_observacao_pro, txt_situacao_atual_sit, dat_cadastro_conclusao_pan,
					num_nao_contratado_vpr, cod_processo_pro from adm_licitar.tb_processo_completo_pcm pcm
						where num_nao_contratado_vpr = num_processo_estimado_global
						and (num_processo_a_ser_contratado is  null or num_processo_a_ser_contratado <= 0)
						and txt_estado_processo = 'FINALIZADO'
						and dat_cadastro_conclusao_pan between '@dataInicial' and '@dataFinal 23:59:59' @modalidade
						order by txt_descricao_ins,txt_modalidade_mod
					";
			
			if (idsModalidade != null && idsModalidade!= String.Empty)
			{
				cmd.CommandText = cmd.CommandText.Replace("@modalidade"," and cod_modalidade_mod in (" + idsModalidade + ") ");
			}
			else
			{
				cmd.CommandText = cmd.CommandText.Replace("@modalidade"," ");	
			}
									
			cmd.CommandText = cmd.CommandText.Replace("@dataInicial",dataInicial.Date.ToString("dd/MM/yyyy")).
				Replace("@dataFinal",dataFinal.Date.ToString("dd/MM/yyyy"));
	  		return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
			
		}
		
		public DataTable getTotaisProcessosConcluidoPorModalidade(DateTime dtInicio,DateTime dtFim, bool auditado)
		{
			Console.WriteLine("getTotaisProcessosConcluidoPorModalidade");
			
			dtFim = dtFim.AddHours(23).AddMinutes(59).AddSeconds(59);
			
			String sql = @"
						select txt_modalidade_mod as modalidade, 
							count(*) as quantidade, 
							sum(num_processo_estimado_global) as estimadoglobal, 
							sum(num_processo_a_ser_contratado) as contratado,
							sum(num_nao_contratado_vpr) as semexito,
							sum(num_estimado_real_vpr) as estimadoreal,
							(case when sum(num_processo_a_ser_contratado) = 0 and sum(num_economia_vpr) = 0 then null else sum(num_economia_vpr)  end)  as economia,
							
							 (case when (sum(num_processo_estimado_global) - sum(num_nao_contratado_vpr)) <> 0 
							 then cast(((sum(num_economia_vpr) * 100) /  (sum(num_processo_estimado_global) - sum(num_nao_contratado_vpr))) as numeric (12,2))
							 else null end)
							  as economiaporcentagem 
							from adm_licitar.tb_processo_completo_pcm
							where txt_estado_processo = 'FINALIZADO'
							@DATACONCLUSAO
							@AUDITADO
							group by txt_modalidade_mod,cod_modalidade_mod
							order by txt_modalidade_mod,cod_modalidade_mod";
			
			if (dtInicio != DateTime.MinValue && dtFim !=  DateTime.MinValue)
				sql = sql.Replace("@DATACONCLUSAO"," and dat_conclusao_pan between '"+ 
				                  dtInicio +"' and '"+ 
				                  //dtInicio.ToString("yyyy-MM-dd") +"' and '"+ 
				                  //dtFim.ToString("yyyy-MM-dd HH24:MI:SS") +"' ");
				                  dtFim+"' ");
			else
				sql = sql.Replace("@DATACONCLUSAO","");
			if (auditado)
				sql = sql.Replace("@AUDITADO"," AND boo_auditado_pro = " + auditado);
			else
				sql = sql.Replace("@AUDITADO",String.Empty);
			
			
			Console.WriteLine("dtFim = "+dtFim + " dtinicio= "+dtInicio);
			Console.WriteLine(sql);
						
			return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(sql).Tables[0];
		}
		
		
	}
	
	
	
}
