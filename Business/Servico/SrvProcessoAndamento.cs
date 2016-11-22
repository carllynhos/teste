// srvProcessoAndamento.cs created with MonoDevelop
// User: wanialdo at 17:41 21/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using NHibernate.Expression;

using Licitar.Business.Dao;
using Licitar.Business.Entidade;
using System.Collections.Generic;
using Licitar.Business.Dto;
using Licitar.Business.Persistencia;
using NHibernate.Expression;
using Castle.ActiveRecord;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;
using System.Collections;


namespace Licitar.Business.Servico
{
	public class SrvProcessoAndamento : PostgreSqlDatabase
	{
		public DataTable listarUltimoAndametoProcesso(string idProcesso)
		{			
			string select =@"

			SELECT
		
			pan.pk_cod_processo_andamento_pan, 
			pes.txt_nome_pes, 
			pan.dat_cadastro_pan,
			pan.fk_cod_unidade_exercicio_uex,
			fas.txt_descricao_fas, 
			sti.txt_descricao_sit, 
			ati.txt_descricao_ati, 
			pan.txt_andamento_pan, 
			pan.dat_andamento_pan, 
			pes2.txt_nome_pes as destinatario
			
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			LEFT JOIN adm_licitar.tb_fase_fas fas on fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
			LEFT JOIN adm_licitar.tb_situacao_sit sti on sti.pk_cod_situacao_sit = fan.fk_cod_situacao_sit
			INNER JOIN adm_licitar.tb_pessoa_pes pes on pes.pk_cod_pessoa_pes = pan.fk_cod_pessoa_cadastrante_pes
			LEFT JOIN adm_licitar.tb_pessoa_pes pes2 on pes2.pk_cod_pessoa_pes = pan.fk_cod_pessoa_pes
			WHERE pan.fk_cod_processo_pro = @idProcesso
			ORDER BY pan.dat_cadastro_pan DESC, pan.pk_cod_processo_andamento_pan DESC
			LIMIT 1";

			select = select.Replace("@idProcesso",idProcesso);

			DataTable dt = Consultar(select);

			return dt;
		}

		public int getIdUltimoAndamentoProcesso(int idProcesso)
		{
			string select =@"SELECT pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan
			WHERE fk_cod_processo_pro = @processo
			AND fk_cod_processo_andamento_pan is null
			ORDER BY dat_cadastro_pan DESC, pk_cod_processo_andamento_pan DESC
			LIMIT 1";

			select = select.Replace("@processo",idProcesso.ToString());

			DataTable dt = Consultar(select);
			int idPA = 0;

			foreach(DataRow row in dt.Rows)
			{
				idPA = Convert.ToInt32(row[0].ToString());
			}
			
			return idPA == null ? 0 : idPA;
		}

		public string getFaseAndamentoProcessoByData(string idProcesso, string data)
		{
			string select =@"SELECT fas.txt_descricao_fas as fase
							FROM adm_licitar.tb_processo_andamento_pan pan
							INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
							INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
							INNER JOIN adm_licitar.tb_fase_fas fas ON fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
							WHERE pan.dat_cadastro_pan >= '@data'
							AND pan.fk_cod_processo_pro = '@id'
							ORDER BY pan.dat_cadastro_pan ASC
							LIMIT 1
							";

			select = select.Replace("@id",idProcesso);
			select = select.Replace("@data",data);

			DataTable dt = Consultar(select);
			string resultado = "";

			foreach(DataRow row in dt.Rows)
			{
				resultado = row["fase"].ToString();
			}
			
			return resultado;
		}

		public ProcessoAndamento[] listarAndamentosProcesso(string idProcesso)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(ProcessoAndamento));
			pesquisa.Add(Expression.Eq("Processo.Id", int.Parse(idProcesso)));
			Order[] ordem = new Order[] { Order.Desc("DataCadastro") };

			return ProcessoAndamento.FindAll(pesquisa, ordem);
		}

		public ProcessoAndamento[] listarAndamentosSemCorrigidosProcesso(string idProcesso)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(ProcessoAndamento));
			pesquisa.Add(Expression.Eq("Processo.Id", int.Parse(idProcesso)));			
			pesquisa.Add(Expression.IsNull("AndamentoCorrigido"));
			Order[] ordem = new Order[] { Order.Desc("DataCadastro"), Order.Desc("Id") };

			return ProcessoAndamento.FindAll(pesquisa, ordem);
		}
		
		public Frase[] FrasesAndamento(int idAtividade,int fluxoAndamento)
		{
			string sql = @"Select fra.*
			  from adm_licitar.tb_fluxo_andamento_fan fan, adm_licitar.tb_frase_fluxo_andamento_ffa ffa, adm_licitar.tb_frase_fra fra
			 where fan.pk_cod_fluxo_andamento_fan = ffa.fk_cod_fluxo_andamento_fan
			   and ffa.fk_cod_frase_fra = fra.pk_cod_frase_fra
			   and fan.fk_cod_atividade_ati = " + idAtividade.ToString() +" and pk_cod_fluxo_andamento_fan = "+fluxoAndamento; 	
						
			DataTable dt = Consultar(sql);
			int[] ids = null;
			
			if ((dt != null) && (dt.Rows.Count > 0))
			{
				ids = new int[dt.Rows.Count];
				
				for (int i = 0; i <= dt.Rows.Count - 1; i++)
					ids[i] = Convert.ToInt32(dt.Rows[i][0]);
			}
				
			if (ids != null) 
				return Frase.FindAll(Expression.In("Id", ids));

			return null;
		}
		
		public DataTable ConsultarProcessoAndamento(string where,string OrderBy)
		{
			string sql = @"select pk_cod_processo_andamento_pan as Id, pesCad.txt_nome_pes as Cadastrante, pan.dat_cadastro_pan as DataCadastro,fas.txt_descricao_fas as Fase,ati.txt_descricao_ati as TipoAndamento,
				txt_andamento_pan as Observacao, pes.txt_nome_pes as pesTramitou, pan.dat_andamento_pan as DataAndamento, pan.fk_cod_processo_andamento_pan as andamentocorridigo
				
				from adm_licitar.tb_processo_andamento_pan pan
				left join adm_licitar.tb_pessoa_pes pes on pes.pk_cod_pessoa_pes = pan.fk_cod_pessoa_pes 
				left join adm_licitar.tb_pessoa_pes pesCad on pesCad.pk_cod_pessoa_pes = pan.fk_cod_pessoa_cadastrante_pes 
				left join adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
				left join adm_licitar.tb_fase_fas fas on fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
				left join adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati where (1=1) and "+where+" "+OrderBy; 
				
				
			DataTable dt = Consultar(sql);
			
			return dt;
		}
		

		public Frase[] FrasesAndamento(int idAtividade, int idModalidade, int idUnidadeExercicio, int idFase, int idSituacao)
		{
				
			string sql = @"Select fra.*
			  from adm_licitar.tb_fluxo_andamento_fan fan, adm_licitar.tb_frase_fluxo_andamento_ffa ffa, adm_licitar.tb_frase_fra fra
			 where fan.pk_cod_fluxo_andamento_fan = ffa.fk_cod_fluxo_andamento_fan
			   and ffa.fk_cod_frase_fra = fra.pk_cod_frase_fra
			   and fan.fk_cod_atividade_ati = " + idAtividade.ToString()
				+ " and fan.fk_cod_fase_fas = " + idFase.ToString()
				+ " and fan.fk_cod_situacao_sit = " + idSituacao.ToString()
				;
				
			DataTable dt = Consultar(sql);
			int[] ids = null;
			
			if ((dt != null) && (dt.Rows.Count > 0))
			{
				ids = new int[dt.Rows.Count];
				
				for (int i = 0; i <= dt.Rows.Count - 1; i++)
					ids[i] = Convert.ToInt32(dt.Rows[i][0]);
			}
				
			return Frase.FindAll(Expression.In("Id", ids));
		}
		
		public DataTable GetTipoAndamentoAgenda(int idProcessoAndamento)
		{
			string select = @"
				select txt_descricao_ati, dat_andamento_pan from adm_licitar.tb_agenda_age age
				join adm_licitar.tb_processo_andamento_pan pan on pan.pk_cod_processo_andamento_pan = age.fk_cod_processo_andamento_pan
				join adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
				join adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati

				where fk_cod_processo_pro in(

				select fk_cod_processo_pro from adm_licitar.tb_agenda_age age
				join adm_licitar.tb_processo_andamento_pan pan on pan.pk_cod_processo_andamento_pan = age.fk_cod_processo_andamento_pan
				where 
				age.fk_cod_processo_andamento_pan = @fk_cod_processo_andamento_pan)
				and txt_descricao_ati = 'DISPUTA' OR txt_descricao_ati='LICITAÇÃO MARCADA' OR
				txt_descricao_ati= 'LICITAÇÃO MARCADA EM CONTINUIDADE' OR
				txt_descricao_ati='SESSÃO DE RESULTADO - HABILITAÇÃO' OR
				txt_descricao_ati='SESSÃO DE RESULTADO - PROPOSTA COMERCIAL' OR
				txt_descricao_ati='SESSÃO DE ABERTURA - PROPOSTA COMERCIAL' OR
				txt_descricao_ati='SESSÃO DE ABERTURA - PROPOSTA TÉCNICA' OR
				txt_descricao_ati='SESSÃO DE RESULTADO - PROPOSTA TÉCNICA'

				order by pan.dat_andamento_pan desc LIMIT 1	

			";
			
			select = select.Replace("@fk_cod_processo_andamento_pan", idProcessoAndamento.ToString());			
			DataTable dt = Consultar(select);
			
			return dt;
		}
		
		public int getUltimoAndamentoProcesso(int idProcesso)
		{
			string idStrAndamento = "";
			string select = @"
			SELECT fan.fk_cod_atividade_ati
			FROM adm_licitar.tb_processo_pro pro, adm_licitar.tb_processo_andamento_pan pan, adm_licitar.tb_fluxo_andamento_fan fan
			WHERE pro.pk_cod_processo_pro = pan.fk_cod_processo_pro
			AND pan.fk_cod_processo_andamento_pan is null
			AND fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			AND pro.pk_cod_processo_pro = @pk_cod_processo_pro
			ORDER BY pan.dat_cadastro_pan DESC, pan.pk_cod_processo_andamento_pan DESC
			LIMIT 1";
			
			select = select.Replace("@pk_cod_processo_pro", idProcesso.ToString());
			
			DataTable dt = Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				idStrAndamento = row["fk_cod_atividade_ati"].ToString();
			}
			
			return string.IsNullOrEmpty(idStrAndamento) ? 0 : Convert.ToInt32(idStrAndamento);
			
		}

		public ProcessoAndamento getObjetoUltimoAndamentoProcesso(int idProcesso)
		{
			string idStrAndamento = "";
			string select = @"
			SELECT pan.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_pro pro, adm_licitar.tb_processo_andamento_pan pan
			WHERE pro.pk_cod_processo_pro = pan.fk_cod_processo_pro
			AND pan.fk_cod_processo_andamento_pan is null			
			AND pro.pk_cod_processo_pro = @pk_cod_processo_pro
			ORDER BY pan.dat_cadastro_pan DESC, pan.pk_cod_processo_andamento_pan DESC
			LIMIT 1";
			
			select = select.Replace("@pk_cod_processo_pro", idProcesso.ToString());
			
			DataTable dt = Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				idStrAndamento = row["pk_cod_processo_andamento_pan"].ToString();
			}

			int id = 0;
			int.TryParse(idStrAndamento, out id);
			
			if(id>0)
				return ProcessoAndamento.Find(id);
			else
				return null;	
			
		}


		public List<StructMigrarAndamentos> listarTodosAndamentos()
		{			
			string select = @"
			SELECT pk_cod_processo_andamento_pan, fk_cod_pessoa_cadastrante_pes
			FROM adm_licitar.tb_processo_andamento_pan order by pk_cod_processo_andamento_pan";
			
			List<StructMigrarAndamentos> lista = new List<StructMigrarAndamentos>();
			
			DataTable dt = Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				StructMigrarAndamentos obj = new StructMigrarAndamentos();
				obj.id = Convert.ToInt32(row["pk_cod_processo_andamento_pan"].ToString());
				obj.pessoa = Convert.ToInt32(row["fk_cod_pessoa_cadastrante_pes"].ToString());
				lista.Add(obj);
			}
			
			return lista;
			
		}

		public int getMAXAndamentoProcesso()
		{
			string idStrAndamento = "";
			string select = @"
			SELECT MAX(pk_cod_processo_andamento_pan) as num
			FROM adm_licitar.tb_processo_andamento_pan";
					
			DataTable dt = Consultar(select);
			
			idStrAndamento = dt.Rows[0]["num"].ToString();
			
			
			return string.IsNullOrEmpty(idStrAndamento) ? 0 : Convert.ToInt32(idStrAndamento);
			
		}
		
		public int getTipoAndamentoPorNome(string nome)
		{
			string select = @"
			SELECT pk_cod_atividade_ati
			FROM adm_licitar.tb_atividade_ati
			WHERE txt_descricao_ati = @nome
			";
			string idTipoAnd = ""; 
			
			select = select.Replace("@nome","'"+nome+"'");
					
			DataTable dt = Consultar(select);
			
			foreach(DataRow row in dt.Rows)
			{
				idTipoAnd = row["pk_cod_atividade_ati"].ToString();
			}
			
			return string.IsNullOrEmpty(idTipoAnd) ? 0 : Convert.ToInt32(idTipoAnd);
		}

		public int getFluxoPorAndamentoFaseWorkflow(string andamento, string fase, string workflow)
		{
			string select = @"
			SELECT fan.pk_cod_fluxo_andamento_fan as id
			FROM adm_licitar.tb_fluxo_andamento_fan fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_fase_fas fas ON fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
			WHERE fas.txt_descricao_fas = @fase
			AND ati.boo_tipo_andamento_ati = true
			AND ati.txt_descricao_ati = @andamento 
			AND fan.fk_cod_workflow_wor = @workflow 
			";
			string idTipoAnd = ""; 
			
			select = select.Replace("@fase","'"+fase+"'");
			select = select.Replace("@andamento","'"+andamento+"'");
			select = select.Replace("@workflow","'"+workflow+"'");
					
			DataTable dt = Consultar(select);
			
			foreach(DataRow row in dt.Rows)
			{
				idTipoAnd = row["id"].ToString();
			}
			
			return string.IsNullOrEmpty(idTipoAnd) ? 0 : Convert.ToInt32(idTipoAnd);
		}

		public ProcessoAndamento[] listarAndamentosProcesso(int idPai)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento));
			dc.AddOrder(Order.Asc("DataCadastro"));
			dc.Add(Expression.Eq("Processo.Id", idPai));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));

			return ProcessoAndamento.FindAll(dc);
		}

		public ProcessoAndamento[] listarAndamentosProcesso(int idPai, bool corrigido, bool desc)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento));
			if(desc)
				dc.AddOrder(Order.Desc("DataCadastro"));
			else
				dc.AddOrder(Order.Asc("DataCadastro"));
			
			dc.AddOrder(Order.Desc("Id"));
			dc.Add(Expression.Eq("Processo.Id", idPai));
			if(corrigido)
				dc.Add(Expression.IsNull("AndamentoCorrigido"));

			return ProcessoAndamento.FindAll(dc);
		}

		public static bool VerificarUltimaRealizacaoDataMaiorQueDataRemarcacao(int idProcesso)
		{				
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 			
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));
			dc.Add(Expression.IsNull("AndamentoAdiado"));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Atividade", "ati");
			dc.Add(Expression.Sql(@"
			 this_.pk_cod_processo_andamento_pan in 
			(select pan.pk_cod_processo_andamento_pan from adm_licitar.tb_processo_andamento_pan pan
			inner join adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			inner join adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			and fk_cod_processo_andamento_remarcado_pan is null	
			and fk_cod_processo_andamento_pan is null
			where fk_cod_processo_pro = "+idProcesso+@"  and txt_descricao_ati in
			(
			 'LICITAÇÃO MARCADA', 'LICITAÇÃO MARCADA EM CONTINUIDADE', 'DISPUTA', 'SESSÃO FINAL',  'SESSÃO DE RESULTADO - HABILITAÇÃO',  'SESSÃO DE RESULTADO - PROPOSTA COMERCIAL', 'SESSÃO DE ABERTURA - PROPOSTA COMERCIAL',  'SESSÃO DE ABERTURA - PROPOSTA TÉCNICA', 'SESSÃO DE RESULTADO - PROPOSTA TÉCNICA'
			) and dat_andamento_pan >= '" 
						                      + DateTime.Now +
						                      @"' order by dat_andamento_pan desc limit 1)"));
			
			
			;
			ProcessoAndamento andamento = ProcessoAndamento.FindOne(dc);
			if (andamento != null)
				return true;
			return false;
		}

		public static bool VerificarSeJaFoiDadoAndamentoAgendaComDataMenorQueAtual(int idProcesso)
		{				
			string[] andamentosAgenda = 
			{ 
				"LICITAÇÃO MARCADA", 
				// MF INICIO
				"LICITAÇÃO MARCADA EM CONTINUIDADE",
				//MF FIM
				"DISPUTA", 
				"SESSÃO FINAL", 
				"SESSÃO DE RESULTADO - HABILITAÇÃO", 
				"SESSÃO DE RESULTADO - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA TÉCNICA", 
				"SESSÃO DE RESULTADO - PROPOSTA TÉCNICA"  
			};
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 			
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Atividade", "ati");
			dc.Add(Expression.In("ati.Descricao", andamentosAgenda));
			dc.Add(Expression.Sql(" this_.dat_andamento_pan <= '" + DateTime.Now + "' ")); 					
			ProcessoAndamento[] andamentos = ProcessoAndamento.FindAll(dc);
			
			if (andamentos.Length > 0) 
				return true; 
			
			return false;				
		}
		
		public bool VerificarExistenciaValorEstimadoGlobal(int idProcesso)
		{
			DetachedCriteria pesqValorEstGlobal = DetachedCriteria.For(typeof(ValorProcesso));
				pesqValorEstGlobal.CreateAlias("TipoValor","tpvalor");
				pesqValorEstGlobal.CreateAlias("Processo","proc");
				pesqValorEstGlobal.Add(Expression.Eq("tpvalor.Descricao","Estimado Global"));
				pesqValorEstGlobal.Add(Expression.Eq("proc.Id",idProcesso));
			return ValorProcesso.Exists(pesqValorEstGlobal);
		}


		public void MarcarUltimaDataRealizacao(int idProcesso,ProcessoAndamento fkCodProcAndRemarcado)
		{
			string[] andamentosAgenda = 
			{ 
				"LICITAÇÃO MARCADA", 
				// MF Inicio
				"LICITAÇÃO MARCADA EM CONTINUIDADE",
				// MF Fim
				"DISPUTA", 
				"SESSÃO FINAL", 
				"SESSÃO DE RESULTADO - HABILITAÇÃO", 
				"SESSÃO DE RESULTADO - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA TÉCNICA", 
				"SESSÃO DE RESULTADO - PROPOSTA TÉCNICA"  
			};
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 			
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));
			dc.Add(Expression.IsNull("AndamentoAdiado"));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Atividade", "ati");
			dc.Add(Expression.In("ati.Descricao", andamentosAgenda));
			dc.Add(Expression.Sql(" this_.dat_andamento_pan >= '" + DateTime.Now + "' order by this_.dat_andamento_pan desc "));
			ProcessoAndamento andamento = ProcessoAndamento.FindFirst(dc);
			if (andamento != null)
			{
				andamento.AndamentoAdiado = fkCodProcAndRemarcado;
				andamento.SaveAndFlush();
			}
		}

		public void DesMarcarUltimaDataRealizacao(int fkCodProcAndRemarcado)
		{			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 									
			dc.Add(Expression.Eq("AndamentoAdiado",ProcessoAndamento.Find(fkCodProcAndRemarcado)));
			
			ProcessoAndamento[] andamento = ProcessoAndamento.FindAll(dc);
			
			if (andamento.Length > 0)
			{
				foreach (ProcessoAndamento procAnd in andamento) {
					procAnd.AndamentoAdiado = null;
					procAnd.SaveAndFlush();
				}								
			}			
		}

		public DateTime RetornarUltimaDataRealizacao(int idProcesso)
		{
			string[] andamentosAgenda = 
			{ 
				"LICITAÇÃO MARCADA", 
				// MF inicio
				"LICITAÇÃO MARCADA EM CONTINUIDADE",
				// MF fim
				"DISPUTA", 
				"SESSÃO FINAL", 
				"SESSÃO DE RESULTADO - HABILITAÇÃO", 
				"SESSÃO DE RESULTADO - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA TÉCNICA", 
				"SESSÃO DE RESULTADO - PROPOSTA TÉCNICA"  
			};
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 			
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));
			dc.Add(Expression.IsNull("AndamentoAdiado"));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Atividade", "ati");
			dc.Add(Expression.In("ati.Descricao", andamentosAgenda));
			dc.Add(Expression.Sql(" this_.dat_andamento_pan >= '" + DateTime.Now + "' order by this_.dat_andamento_pan desc ")); 					
			ProcessoAndamento andamento = ProcessoAndamento.FindFirst(dc);

			return andamento.DataAndamento;
		}

		public int  RetornarUltimoIdDataMarcacao(int idProcesso,int processoAndamentoAdiado)
		{
			string[] andamentosAgenda = 
			{ 
				"LICITAÇÃO MARCADA", 
				"LICITAÇÃO MARCADA EM CONTINUIDADE",
				"DISPUTA", 
				"SESSÃO FINAL", 
				"SESSÃO DE RESULTADO - HABILITAÇÃO", 
				"SESSÃO DE RESULTADO - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA TÉCNICA", 
				"SESSÃO DE RESULTADO - PROPOSTA TÉCNICA"  
			};
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 			
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));	
			dc.CreateAlias("AndamentoAdiado", "adi");
			dc.Add(Expression.Eq("adi.Id",processoAndamentoAdiado));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Atividade", "ati");
			dc.Add(Expression.In("ati.Descricao", andamentosAgenda));
			dc.Add(Expression.Sql(" this_.dat_andamento_pan >= '" + DateTime.Now + "' order by this_.dat_andamento_pan desc ")); 					
			ProcessoAndamento andamento = ProcessoAndamento.FindFirst(dc);

			return andamento.Id;
		}

		public Frase[] RetornaFrasesProntas(int idProcesso,string tipoAndamento)
		{
					SrvProcesso srvProc = new SrvProcesso();
					SrvFluxoAndamento srvFluxo = new SrvFluxoAndamento();
			
					Processo oProcesso = Processo.Find(idProcesso);
					SrvProcessoAndamento srvPA = new SrvProcessoAndamento();	                
					FluxoAndamento objFluxoAndamento = srvFluxo.RetornaFluxoAndamento(oProcesso.Classificacao.Modalidade.Descricao,tipoAndamento);
					if (objFluxoAndamento != null)
						return srvPA.FrasesAndamento(Atividade.BuscarPorDescricao(tipoAndamento).Id,objFluxoAndamento.Id); 
					else
						return null;
		}

		
		public static bool VerificarSeJaFoiDadoAndamentoAgendaComDataIgual(int idProcesso, DateTime data)
		{				
			string[] andamentosAgenda = 
			{ 
				"LICITAÇÃO MARCADA", 
				// MF Inicio
				"LICITAÇÃO MARCADA EM CONTINUIDADE",
				// MF Fim
				"DISPUTA", 
				"SESSÃO FINAL", 
				"SESSÃO DE RESULTADO - HABILITAÇÃO", 
				"SESSÃO DE RESULTADO - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA TÉCNICA", 
				"SESSÃO DE RESULTADO - PROPOSTA TÉCNICA"  
			};
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 			
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));
			dc.Add(Expression.IsNull("AndamentoAdiado"));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Atividade", "ati");
			dc.Add(Expression.In("ati.Descricao", andamentosAgenda));
			dc.Add(Expression.Sql(" this_.dat_andamento_pan = '" + data + "' ")); 					
			ProcessoAndamento[] andamentos = ProcessoAndamento.FindAll(dc);
			
			if (andamentos.Length > 0) 
				return true; 
			
			return false;				
		}
		
		public static bool VerificarSeJaPassouPelaFase(string descricaoFase, int idProcesso)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento));
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Fase", "fas");
			dc.Add(Expression.InsensitiveLike("fas.Descricao", descricaoFase));			
			ProcessoAndamento[] andamentos = ProcessoAndamento.FindAll(dc);
			
			if (andamentos.Length > 0) 
				return true; 
			
			return false;				
			
		}
		
		public static bool VerificarSeExisteAssociacaoAntesDeExcluir(int idProcessoAndamentoExcluir)
		{			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento));
			dc.Add(Expression.Eq("AndamentoCorrigido.Id", idProcessoAndamentoExcluir));
			if (ProcessoAndamento.FindAll(dc).Length > 0)
				return true;
			
			return false;
		}

		public  string RegrasAndamentos(string tipoAndamento, string modalidade, int idProcesso)
		{
			string retorno = string.Empty;
			if(tipoAndamento.ToUpper().Equals("CONCLUÍDO") || tipoAndamento.ToUpper().Equals("EM CONFERÊNCIA"))
			{
				if(modalidade.ToUpper().Equals("PREGÃO ELETRÔNICO") || modalidade.ToUpper().Equals("PREGÃO PRESENCIAL"))
				{
					DetachedCriteria dcPregoeiro = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
					dcPregoeiro.CreateAlias("Processo","pro").CreateAlias("Papel","pap");
					dcPregoeiro.Add(Expression.Eq("pro.Id",idProcesso));
					dcPregoeiro.Add(Expression.Eq("pap.Descricao","PREGOEIRO"));

					DetachedCriteria dcProcessoAndamento = DetachedCriteria.For(typeof(ProcessoAndamento));
					dcProcessoAndamento.CreateAlias("Processo","pro").CreateAlias("FluxoAndamento","flu").CreateAlias("flu.Fase","fas");
					dcProcessoAndamento.Add(Expression.Eq("pro.Id",idProcesso));
					dcProcessoAndamento.Add(Expression.Eq("fas.Descricao","REALIZAÇÃO"));
					
					if(!ProcessoPapelPessoa.Exists(dcPregoeiro) && ProcessoAndamento.Exists(dcProcessoAndamento))
						retorno = "FAVOR CADASTRAR UM PREGOEIRO ANTES DE EFETUAR A CONCLUSÃO";												
				}
			}
			return retorno;
		}

		public bool RetornaPublicacaoEdital (int idProcesso)
		{
			bool retorno = false;
			DetachedCriteria dcPesqPubEdital = DetachedCriteria.For(typeof(ProcessoAndamento),"pan");
			dcPesqPubEdital.CreateAlias("FluxoAndamento","fan").CreateAlias("fan.Atividade","ati").CreateAlias("Processo","pro")
				.Add(Expression.Eq("ati.Descricao","Publicação de Edital").IgnoreCase()).Add(Expression.Eq("pro.Id",idProcesso));
			dcPesqPubEdital.SetProjection(Property.ForName("Id"));
			if(ProcessoAndamento.Exists(dcPesqPubEdital))
				retorno = true;
			
			return retorno;
		}

		public void AdiamentodeAndamentosCorrigidos(int idUltimoCorrige,int fkProcAndAdiado)
		{
											
			try
			{											

				DetachedCriteria pesqAndamentosCorrigidos = DetachedCriteria.For(typeof(ProcessoAndamento));			
				pesqAndamentosCorrigidos.Add(Expression.Eq("AndamentoCorrigido.Id",idUltimoCorrige)).AddOrder(Order.Desc("Id"));
				
				ProcessoAndamento procAnd = ProcessoAndamento.FindFirst(pesqAndamentosCorrigidos);
				if(procAnd != null)
				{
					procAnd.AndamentoAdiado = new ProcessoAndamento(fkProcAndAdiado);
					procAnd.Update();
					
					AdiamentodeAndamentosCorrigidos(procAnd.Id,fkProcAndAdiado);
					
				}
			}
			catch(Exception ex)
			{
				throw;
			}
						
		}

		public ProcessoAndamento getUltimoAndamentoProcessoPorTipo(int idProcesso, string tipoAndamento)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento));
			dc.CreateAlias("FluxoAndamento","fan");
			dc.CreateAlias("fan.Atividade","ati");

			dc.Add(Expression.Eq("Processo.Id",idProcesso));
			dc.Add(Expression.Eq("ati.Descricao",tipoAndamento));
			dc.AddOrder(Order.Desc("DataCadastro"));
			dc.AddOrder(Order.Desc("Id"));

			return ProcessoAndamento.FindFirst(dc);
		}
		
		public bool VerificarAserContratadoOuSemExito(int idProcesso)
		{			
			bool retorno = true;
			string[] valoresSemExito = {"CANCELADO","REVOGADO","DESERTO","ANULADO","FRACASSADO"};
			DetachedCriteria dc = DetachedCriteria.For(typeof(ValorProcesso));
			dc.CreateAlias("TipoValor","tv");
			dc.Add(Expression.In("tv.Descricao",valoresSemExito));
			dc.CreateAlias("Processo","pro");
			dc.Add(Expression.Eq("pro.Id",idProcesso));
			dc.SetProjection(Projections.Sum("Valor"));
			
			if(ValorProcesso.Exists(dc))
				retorno = false;
				
			return retorno;	
			
		}
		
		///Método criado para a central de digitalização de processos salvar andamentos retornando o id do andamento usado
		public int SalvarAndamento(int idProcesso, ref FileUpload fileUpload, Pessoa pessoa, string descricaoModalidade, string faseProcesso, string endereco, string observacaoAndamento)
		{
			Processo oProcesso = Processo.Find(idProcesso);
			ProcessoAndamento oProcessoAndamento = new ProcessoAndamento();
			SrvFluxoAndamento srvFluxo = new SrvFluxoAndamento();
			SrvProcesso srvProc = new SrvProcesso();
			
			SrvMotivoAndamento srvMotivoAnd = new SrvMotivoAndamento();				
			oProcessoAndamento.Cadastrante = pessoa;
			oProcessoAndamento.DataCadastro = DateTime.Now;
			oProcessoAndamento.DataAndamento = DateTime.Now;
			oProcessoAndamento.Andamento = observacaoAndamento;
			oProcessoAndamento.Processo = srvProc.ListarProcesso(idProcesso);	
				
			oProcessoAndamento.FluxoAndamento =  srvFluxo.RetornaFluxoAndamento(oProcesso.Classificacao.Modalidade.Descricao,descricaoModalidade,faseProcesso);
				
			if(!string.IsNullOrEmpty(fileUpload.FileName))
			{
				if(!ValidarExtensao(fileUpload.FileName))
				{
					throw new Exception("Por favor, verificar se a extensão do arquivo está correta. Tipo de arquivo inválido. Só é aceito arquivos: .pdf, .p7s. Para mairores detalhes entre em contato com a CTI.");
				}
				else
				{
					if(oProcessoAndamento.FluxoAndamento!=null)
					{
						oProcessoAndamento.SaveAndFlush();		
						SalvarArquivo(oProcessoAndamento.Id, ref fileUpload, endereco.Replace("@ESTRUTURAREPOSITORIO", SrvDigitalizacao.getEnderecoRegraRepositorio(idProcesso,oProcessoAndamento.Id)));
						DAOGenerico dao = new DAOGenerico();
						//dao.AtualizarTabelaoComPublicacaoEdital(oProcessoAndamento.Processo.Id,oProcessoAndamento.Id.ToString());
					}
					else
					{
						throw new Exception("NÃO EXISTE FLUXO ANDAMENTO CADASTRADO PARA ESSE TIPO DE PROCESSO");
					}
				}
			}
			else
				throw new Exception("É necessário escolher um arquivo para anexar ao processo.");
			return oProcessoAndamento.Id;
						
		}
		
		
		///Método criado para a central de digitalização de processos para validar se o arquivo enviado é p7s ou pdf
		public static bool ValidarExtensao(string nomeArquivo)
		{		
			List<string> extensoesValidas = new List<string>() { ".pdf", ".p7s"};			
			if (extensoesValidas.Contains(GetExtensao(nomeArquivo))) return true; return false;			
		}
		
		///Método criado para a central de digitalização de processos para verificar a extensão de arquivos
		public static string GetExtensao(string nomeArquivo)
		{
			string extensao = string.Empty;
			int indicePonto = 0;
			for (int i = nomeArquivo.Length - 1; i >= 0; i--)
			{
				if (nomeArquivo[i].ToString() == ".")
				{
					indicePonto = i;
					break;		
				}
			}
			
			extensao = nomeArquivo.Substring(indicePonto);		
			return extensao;
		}
		
		///Método criado para a central de digitalização de processos para salvar arquivos digitalizados
		public void SalvarArquivo(int procAnd, ref FileUpload fileUpload, string endereco)
		{	
			try
			{
				EnviarImagem(procAnd, ref fileUpload, endereco);
			}
			catch(DirectoryNotFoundException ex)
			{	
				Directory.CreateDirectory(endereco.Replace("idAndamento",procAnd.ToString()));
				EnviarImagem(procAnd, ref fileUpload, endereco);
			}
		}
		
		///Método criado para a central de digitalização de processos para gravar arquivos no diretório
		protected void EnviarImagem(int procAnd, ref FileUpload fileUpload, string endereco)
		{
			if (fileUpload.FileName != "")
			{
				string arquivo = RemoverCaracteresIndesejados(fileUpload.FileName);
				fileUpload.SaveAs(endereco.Replace("idAndamento",procAnd.ToString()) + arquivo);
			}
		}

		///Método criado para a central de digitalização de processos para remover caracteres indesejados
		public static string RemoverCaracteresIndesejados(string texto)
        {
            string comCaracteresIndesejados = "!@#$%š&*()-?:{}][ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç ";
            string semCaracteresIndesejados = "_________________AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc_";
            for (int i = 0; i < comCaracteresIndesejados.Length; i++)
            {
                texto = texto.Replace(comCaracteresIndesejados[i].ToString(), semCaracteresIndesejados[i].ToString()).Trim();
            }
			
            return texto;
        }		
		
		///Método criado para a central de digitalização de processos para listar arquivos digitais anexados ao processo
		public static ArrayList ListarArquivosDigitalizados(int idProcesso)
		{
			Processo oProcesso = Processo.Find(idProcesso);
			ProcessoAndamento[] andamentos = oProcesso.ListarAndamentos();
			ArrayList array = new ArrayList();
			foreach (ProcessoAndamento andamento in andamentos)
			{
				
				if (andamento.FluxoAndamento.Atividade.AndamentoDigitalizacao)
				{
					array.Add(andamento);
				}
				
			}
			return array;
		}

	}
}
