using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using Licitar.Business.Entidade;
using NHibernate.Expression;
using Licitar.Business.Dto;
using Licitar.Business.Dao;
using Licitar.Business.Utilidade;
using System.Data;
using Licitar.Business.Persistencia;
using System.Text;
using System.Web;
using Npgsql;

namespace Licitar.Business.Utilidade
{
	public class DTOFiltroRelGerencialLicitacoesAndamento
	{
		public string NumSpu { get; set; }
		public List<string> IdsInstituicao { get; set; }
		public List<string> IdsModalidade { get; set; }
		public List<string> IdsNatureza { get; set; }
		public List<string> SituacoesFase { get; set; }
		public string DataInicio { get; set; }
		public string DataFim { get; set; }
	}
	
	public class DTOResultadoRelGerencialLicitacoesAndamento
	{
		public int TotalRegistrosEncontrados { get; set; }
		public DataSet DataSetResultado { get; set; }
		public string DataEmissao { get { return DateTime.Now.ToString("dd/MM/yyyy"); } }			
	}
	
	public class SrvRelGerencialLicitacoesAndamento
	{
		public static DataSet ListarSituacaoFase()
		{			
			string select = " SELECT DISTINCT txt_situacao_fase_pcm FROM adm_licitar.tb_processo_completo_pcm WHERE txt_situacao_fase_pcm is not null AND txt_situacao_fase_pcm <> '' ORDER BY txt_situacao_fase_pcm ASC ";
			return new PostgreSqlDatabase().ExecutarConsulta(select);			
		}
		
		public static DTOResultadoRelGerencialLicitacoesAndamento ListarRelatorio(DTOFiltroRelGerencialLicitacoesAndamento dtoFiltro)
		{
			DTOResultadoRelGerencialLicitacoesAndamento dtoResultado = new DTOResultadoRelGerencialLicitacoesAndamento();
			
			string select = @"
				SELECT

				cod_processo_pro as idProcesso,
				txt_numero_spu_npr as num_spu,
				txt_descricao_ins as instituicao,
				txt_modalidade_mod as modalidade,
				txt_numero_licitacao_npr as num_licitacao,
				txt_observacao_pro as objeto,
				txt_situacao_fase_pcm as situacao_fase,
				txt_detalhamento_situacao_fase_pcm as det_situacao_fase, 
				txt_presidente_pregoeiro_pes as responsavel,
				num_estimado_real_vpr as valor_estimado,
				txt_vencedor_pes as vencedor,
				dat_registro_situacao_pan as data_registro_situacao
				
				FROM 
				
				adm_licitar.tb_processo_completo_pcm
				
				WHERE

				0=0			
			";
			
			if (!string.IsNullOrEmpty(dtoFiltro.NumSpu))
			{
				select += " AND txt_numero_spu_npr ILIKE '%@num_spu%' ";
				select = select.Replace("@num_spu", dtoFiltro.NumSpu);
			}			
			
			if (dtoFiltro.IdsInstituicao.Count > 0)
			{
				select += " AND cod_instituicao_ins IN (@idsInstituicao) ";
				string instituicoes = string.Empty;
				for (int i = 0; i < dtoFiltro.IdsInstituicao.Count; i++)
				{
					instituicoes += dtoFiltro.IdsInstituicao[i];
					if (i < (dtoFiltro.IdsInstituicao.Count - 1))
						instituicoes += ", ";
				}
				select = select.Replace("@idsInstituicao", instituicoes);
			}
			
			if (dtoFiltro.IdsModalidade.Count > 0)
			{
				select += " AND cod_modalidade_mod IN (@idsModalidade) ";
				string modalidades = string.Empty;
				for (int i = 0; i < dtoFiltro.IdsModalidade.Count; i++)
				{
					modalidades += dtoFiltro.IdsModalidade[i];
					if (i < (dtoFiltro.IdsModalidade.Count - 1))
						modalidades += ", ";
				}
				select = select.Replace("@idsModalidade", modalidades);
			}
			
			if (dtoFiltro.IdsNatureza.Count > 0)
			{
				select += " AND cod_natureza_nat IN (@idsNatureza) ";
				string naturezas = string.Empty;
				for (int i = 0; i < dtoFiltro.IdsNatureza.Count; i++)
				{
					naturezas += dtoFiltro.IdsNatureza[i];
					if (i < (dtoFiltro.IdsNatureza.Count - 1))
						naturezas += ", ";
				}
				select = select.Replace("@idsNatureza", naturezas);
			}
			
			if (dtoFiltro.SituacoesFase.Count > 0)
			{
				select += " AND txt_situacao_fase_pcm IN (@situacoes_fase) ";
				string situacoesFase = string.Empty;
				for (int i = 0; i < dtoFiltro.SituacoesFase.Count; i++)
				{
					situacoesFase += "'"+dtoFiltro.SituacoesFase[i]+"'";
					if (i < (dtoFiltro.SituacoesFase.Count - 1))
						situacoesFase += ", ";
				}
				select = select.Replace("@situacoes_fase", situacoesFase);
			}
				
			Console.WriteLine(">>>SELECT RELATORIO= " + select);
			PostgreSqlDatabase db = new PostgreSqlDatabase();
			dtoResultado.DataSetResultado = db.ExecutarConsulta(select);
			dtoResultado.TotalRegistrosEncontrados = dtoResultado.DataSetResultado.Tables[0].Rows.Count;
						
			return dtoResultado;
		}
	}
	
	
}
