// SrvAcompanhamentoProcesso.cs created with MonoDevelop
// User: guilhermefacanha at 19:23 2/7/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

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
using System.Data;
using Npgsql;

namespace Licitar.Business.Servico
{
	public struct structAcompanhamentoProposta
	{
		public int cod{get;set;}
		public int codGarantia{get;set;}
		public int processo{get;set;}
		public string spu{get;set;}
		public string licitacao{get;set;}
		public DateTime data{get;set;}
		public string objeto{get;set;}
		public string instituicao{get;set;}
		public string andamento{get;set;}
		public int diasFaltandoProposta{get;set;}
		public int diasFaltandoGarantia{get;set;}
		public int diasProposta{get;set;}
		public int diasGarantia{get;set;}
		public DateTime dataRenovacaoProposta{get;set;}
		public DateTime dataRenovacaoGarantia{get;set;}
		public DateTime vencimentoProposta{get;set;}
		public DateTime vencimentoGarantia{get;set;}		
	}
	
	public class SrvAcompanhamentoProcesso:PostgreSqlDatabase
	{
		public List<int> listarRenovacaoPropostaPorAcompanhamento(string id)
		{
			string select = @"
							SELECT pk_cod_acompanhamento_processo_apr as cod
							FROM adm_licitar.tb_acompanhamento_processo_apr
							WHERE fk_cod_acompanhamento_processo_apr = '@id'
							AND fk_cod_tipo_acompanhamento_tac = 
							(
							SELECT pk_cod_tipo_acompanhamento_tac
							FROM adm_licitar.tb_tipo_acompanhamento_tac
							WHERE txt_descricao_tac = 'RENOVAÇÃO PROPOSTA' 
							)
							ORDER BY pk_cod_acompanhamento_processo_apr
							";

			select = select.Replace("@id",id);

			DataTable dt = Consultar(select);

			List<int> lista = new List<int>();

			foreach(DataRow row in dt.Rows)
			{
				lista.Add(Convert.ToInt32(row["cod"].ToString()));
			}

			return lista;
			
		}

		public List<int> listarRenovacaoGarantiaPorAcompanhamento(string id)
		{
			string select = @"
							SELECT pk_cod_acompanhamento_processo_apr as cod
							FROM adm_licitar.tb_acompanhamento_processo_apr
							WHERE fk_cod_acompanhamento_processo_apr = '@id'
							AND fk_cod_tipo_acompanhamento_tac = 
							(
							SELECT pk_cod_tipo_acompanhamento_tac
							FROM adm_licitar.tb_tipo_acompanhamento_tac
							WHERE txt_descricao_tac = 'RENOVAÇÃO GARANTIA' 
							)
							ORDER BY pk_cod_acompanhamento_processo_apr
							";

			select = select.Replace("@id",id);

			DataTable dt = Consultar(select);

			List<int> lista = new List<int>();

			foreach(DataRow row in dt.Rows)
			{
				lista.Add(Convert.ToInt32(row["cod"].ToString()));
			}

			return lista;
			
		}

		
		public DataTable listarAcompanhamentosProposta()
		{
			string select = @"
			SELECT apr.pk_cod_acompanhamento_processo_apr as id, 
			apr.fk_cod_processo_pro as processo, apr.dat_inicio_apr as data,
			apr.num_dias_vencimento_apr as dias_vencimento, 
			apr.fk_cod_acompanhamento_processo_apr, 
			tac.txt_descricao_tac as tipo,
			apr2.pk_cod_acompanhamento_processo_apr as id_garantia,
			apr2.num_dias_vencimento_apr as dias_garantia,
			apr3.dat_inicio_apr as data_renovacao_proposta,
			apr4.dat_inicio_apr as data_renovacao_garantia,
			pcm.txt_numero_spu_npr as spu, 
			pcm.txt_numero_licitacao_npr as licitacao,
			pcm.txt_descricao_ins as instituicao, 
			pcm.txt_observacao_pro as objeto, 
			pcm.txt_andamento_pan as andamento,
			CASE WHEN apr3.num_dias_vencimento_apr is not null THEN
			((date(date(apr.dat_inicio_apr)) + apr.num_dias_vencimento_apr + apr3.num_dias_vencimento_apr) - date(now())) 
			ELSE
			((date(date(apr.dat_inicio_apr)) + apr.num_dias_vencimento_apr) - date(now())) END as dias_faltando,
			CASE WHEN apr4.num_dias_vencimento_apr is not null THEN
			(date(date(apr.dat_inicio_apr)) + apr2.num_dias_vencimento_apr + apr4.num_dias_vencimento_apr) - date(now())
			ELSE (date(date(apr.dat_inicio_apr)) + apr2.num_dias_vencimento_apr) - date(now()) END as dias_faltando_garantia,
			CASE WHEN apr3.num_dias_vencimento_apr is not null THEN
			(date(date(apr.dat_inicio_apr)) + apr.num_dias_vencimento_apr + apr3.num_dias_vencimento_apr)
			ELSE
			(date(date(apr.dat_inicio_apr)) + apr.num_dias_vencimento_apr) END as data_vencimento_proposta,
			CASE WHEN apr4.num_dias_vencimento_apr is not null THEN
			(date(date(apr.dat_inicio_apr)) + apr2.num_dias_vencimento_apr+apr4.num_dias_vencimento_apr)
			ELSE
			(date(date(apr.dat_inicio_apr)) + apr2.num_dias_vencimento_apr) END as data_vencimento_garantia
			FROM adm_licitar.tb_acompanhamento_processo_apr apr 
			INNER JOIN adm_licitar.tb_tipo_acompanhamento_tac tac on tac.pk_cod_tipo_acompanhamento_tac = apr.fk_cod_tipo_acompanhamento_tac
			INNER JOIN adm_licitar.tb_processo_completo_pcm pcm on pcm.cod_processo_pro = apr.fk_cod_processo_pro
			LEFT JOIN adm_licitar.tb_acompanhamento_processo_apr apr2 on (apr2.fk_cod_processo_pro = apr.fk_cod_processo_pro AND apr2.dat_inicio_apr = apr.dat_inicio_apr AND 
			apr2.fk_cod_tipo_acompanhamento_tac = 
			(
			SELECT pk_cod_tipo_acompanhamento_tac
			FROM adm_licitar.tb_tipo_acompanhamento_tac
			WHERE txt_descricao_tac = 'GARANTIA'
			)
			)
			LEFT JOIN adm_licitar.tb_acompanhamento_processo_apr apr3 on (apr3.fk_cod_processo_pro = apr.fk_cod_processo_pro AND 
			apr3.fk_cod_tipo_acompanhamento_tac = 
			(
			SELECT pk_cod_tipo_acompanhamento_tac
			FROM adm_licitar.tb_tipo_acompanhamento_tac
			WHERE txt_descricao_tac = 'RENOVAÇÃO PROPOSTA'
			)
			)
			LEFT JOIN adm_licitar.tb_acompanhamento_processo_apr apr4 on (apr4.fk_cod_processo_pro = apr.fk_cod_processo_pro AND 
			apr4.fk_cod_tipo_acompanhamento_tac = 
			(
			SELECT pk_cod_tipo_acompanhamento_tac
			FROM adm_licitar.tb_tipo_acompanhamento_tac
			WHERE txt_descricao_tac = 'RENOVAÇÃO GARANTIA'
			)
			)
			WHERE tac.txt_descricao_tac='PROPOSTA'
			ORDER BY dias_faltando DESC

			 ";
						
			return Consultar(select);

		}

		public List<structAcompanhamentoProposta> listarAcompanhamentoProposta2()
		{
			string select = @"
			SELECT apr.pk_cod_acompanhamento_processo_apr as id, 
			apr.fk_cod_processo_pro as processo, apr.dat_inicio_apr as data,
			apr.num_dias_vencimento_apr as dias_vencimento, 
			apr.fk_cod_acompanhamento_processo_apr,
			apr2.pk_cod_acompanhamento_processo_apr as id_garantia,
			apr2.num_dias_vencimento_apr as dias_garantia,
			tac.txt_descricao_tac as tipo,
			pcm.txt_numero_spu_npr as spu, 
			pcm.txt_numero_licitacao_npr as licitacao,
			pcm.txt_descricao_ins as instituicao, 
			pcm.txt_observacao_pro as objeto, 
			pcm.txt_andamento_pan as andamento
			FROM adm_licitar.tb_acompanhamento_processo_apr apr 
			INNER JOIN adm_licitar.tb_tipo_acompanhamento_tac tac on tac.pk_cod_tipo_acompanhamento_tac = apr.fk_cod_tipo_acompanhamento_tac
			INNER JOIN adm_licitar.tb_processo_completo_pcm pcm on pcm.cod_processo_pro = apr.fk_cod_processo_pro
			LEFT JOIN adm_licitar.tb_acompanhamento_processo_apr apr2 on (apr2.fk_cod_processo_pro = apr.fk_cod_processo_pro AND apr2.dat_inicio_apr = apr.dat_inicio_apr AND 
			apr2.fk_cod_tipo_acompanhamento_tac = 
			(
			SELECT pk_cod_tipo_acompanhamento_tac
			FROM adm_licitar.tb_tipo_acompanhamento_tac
			WHERE txt_descricao_tac = 'GARANTIA'
			)
			)
			WHERE tac.txt_descricao_tac='PROPOSTA'
			 ";

			List<structAcompanhamentoProposta> lista = new List<structAcompanhamentoProposta>();
			structAcompanhamentoProposta obj = new structAcompanhamentoProposta();
			
			DataTable dt = Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				obj = new structAcompanhamentoProposta();
				obj.cod = Convert.ToInt32(row["id"].ToString());
				obj.processo = Convert.ToInt32(row["processo"].ToString());
				obj.spu = row["spu"].ToString();
				obj.licitacao = row["licitacao"].ToString();
				obj.objeto = row["objeto"].ToString();
				obj.andamento = row["andamento"].ToString();
				obj.instituicao = row["instituicao"].ToString();
				obj.data = Convert.ToDateTime(row["data"].ToString());
				obj.diasProposta = Convert.ToInt32(row["dias_vencimento"].ToString());
				obj.vencimentoProposta = obj.data.AddDays(obj.diasProposta);
				obj.codGarantia = string.IsNullOrEmpty(row["id_garantia"].ToString()) ? 0 : Convert.ToInt32(row["id_garantia"].ToString());
				

				List<int> listaRenovacoesProposta = this.listarRenovacaoPropostaPorAcompanhamento(obj.cod.ToString());
				AcompanhamentoProcesso objAcompanhamento = null;
				foreach(int i in listaRenovacoesProposta)
				{
					objAcompanhamento = AcompanhamentoProcesso.Find(i);
					obj.dataRenovacaoProposta = objAcompanhamento.dataInicial;
					obj.vencimentoProposta =  obj.vencimentoProposta.AddDays(objAcompanhamento.diasVencimento);					
				}

				if(obj.codGarantia>0)
				{
					obj.diasGarantia = Convert.ToInt32(row["dias_garantia"].ToString());
					obj.vencimentoGarantia = obj.data.AddDays(obj.diasGarantia);
					List<int> listaRenovacaoesGarantia = this.listarRenovacaoGarantiaPorAcompanhamento(obj.codGarantia.ToString());
					foreach(int i in listaRenovacaoesGarantia)
					{
						objAcompanhamento = AcompanhamentoProcesso.Find(i);
						obj.dataRenovacaoGarantia = objAcompanhamento.dataInicial;
						obj.vencimentoGarantia = obj.vencimentoGarantia.AddDays(objAcompanhamento.diasVencimento);
					}
				}
				
				obj.diasFaltandoProposta =(obj.vencimentoProposta - Data.RetornaDataNow()).Days;
				obj.diasFaltandoGarantia =  (obj.vencimentoGarantia - Data.RetornaDataNow()).Days;

				lista.Add(obj);
				
			}

			return lista;
			
		}

		public structAcompanhamentoProposta listarAcompanhamentoPropostaPorId(string idAcompanhamento)
		{
			string select = @"
			SELECT apr.pk_cod_acompanhamento_processo_apr as id, 
			apr.fk_cod_processo_pro as processo, apr.dat_inicio_apr as data,
			apr.num_dias_vencimento_apr as dias_vencimento, 
			apr.fk_cod_acompanhamento_processo_apr,
			apr2.pk_cod_acompanhamento_processo_apr as id_garantia,
			apr2.num_dias_vencimento_apr as dias_garantia,
			tac.txt_descricao_tac as tipo,
			pcm.txt_numero_spu_npr as spu, 
			pcm.txt_numero_licitacao_npr as licitacao,
			pcm.txt_descricao_ins as instituicao, 
			pcm.txt_observacao_pro as objeto, 
			pcm.txt_andamento_pan as andamento
			FROM adm_licitar.tb_acompanhamento_processo_apr apr 
			INNER JOIN adm_licitar.tb_tipo_acompanhamento_tac tac on tac.pk_cod_tipo_acompanhamento_tac = apr.fk_cod_tipo_acompanhamento_tac
			INNER JOIN adm_licitar.tb_processo_completo_pcm pcm on pcm.cod_processo_pro = apr.fk_cod_processo_pro
			LEFT JOIN adm_licitar.tb_acompanhamento_processo_apr apr2 on (apr2.fk_cod_processo_pro = apr.fk_cod_processo_pro AND apr2.dat_inicio_apr = apr.dat_inicio_apr AND 
			apr2.fk_cod_tipo_acompanhamento_tac = 
			(
			SELECT pk_cod_tipo_acompanhamento_tac
			FROM adm_licitar.tb_tipo_acompanhamento_tac
			WHERE txt_descricao_tac = 'GARANTIA'
			)
			)
			WHERE tac.txt_descricao_tac='PROPOSTA'
			AND apr.pk_cod_acompanhamento_processo_apr = @idAcomp
			 ";

			select = select.Replace("@idAcomp",idAcompanhamento);

			structAcompanhamentoProposta obj = new structAcompanhamentoProposta();
			
			DataTable dt = Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				obj = new structAcompanhamentoProposta();
				obj.cod = Convert.ToInt32(row["id"].ToString());
				obj.processo = Convert.ToInt32(row["processo"].ToString());
				obj.spu = row["spu"].ToString();
				obj.licitacao = row["licitacao"].ToString();
				obj.objeto = row["objeto"].ToString();
				obj.andamento = row["andamento"].ToString();
				obj.instituicao = row["instituicao"].ToString();
				obj.data = Convert.ToDateTime(row["data"].ToString());
				obj.diasProposta = Convert.ToInt32(row["dias_vencimento"].ToString());
				obj.vencimentoProposta = obj.data.AddDays(obj.diasProposta);
				obj.codGarantia = string.IsNullOrEmpty(row["id_garantia"].ToString()) ? 0 : Convert.ToInt32(row["id_garantia"].ToString());
				

				List<int> listaRenovacoesProposta = this.listarRenovacaoPropostaPorAcompanhamento(obj.cod.ToString());
				AcompanhamentoProcesso objAcompanhamento = null;
				foreach(int i in listaRenovacoesProposta)
				{
					objAcompanhamento = AcompanhamentoProcesso.Find(i);
					obj.dataRenovacaoProposta = objAcompanhamento.dataInicial;
					obj.vencimentoProposta =  obj.vencimentoProposta.AddDays(objAcompanhamento.diasVencimento);					
				}

				if(obj.codGarantia>0)
				{
					obj.diasGarantia = Convert.ToInt32(row["dias_garantia"].ToString());
					obj.vencimentoGarantia = obj.data.AddDays(obj.diasGarantia);
					List<int> listaRenovacaoesGarantia = this.listarRenovacaoGarantiaPorAcompanhamento(obj.codGarantia.ToString());
					foreach(int i in listaRenovacaoesGarantia)
					{
						objAcompanhamento = AcompanhamentoProcesso.Find(i);
						obj.dataRenovacaoGarantia = objAcompanhamento.dataInicial;
						obj.vencimentoGarantia = obj.vencimentoGarantia.AddDays(objAcompanhamento.diasVencimento);
					}
				}
				
				obj.diasFaltandoProposta =(obj.vencimentoProposta - Data.RetornaDataNow()).Days;
				obj.diasFaltandoGarantia =  (obj.vencimentoGarantia - Data.RetornaDataNow()).Days;				
			}

			return obj;			
		}

		public DataTable getParticipantesProposta(string idProcesso, bool todos)
		{
			string select = @"SELECT pes.txt_nome_pes as nome, ppp.boo_participante_ativo_ppp as ativo,ppp.pk_cod_processo_papel_pessoa_ppp as id
			FROM adm_licitar.tb_processo_papel_pessoa_ppp ppp
			INNER JOIN adm_licitar.tb_papel_pap pap ON pap.pk_cod_papel_pap = ppp.fk_cod_papel_pap
			INNER JOIN adm_licitar.tb_pessoa_pes pes ON pes.pk_cod_pessoa_pes = ppp.fk_cod_pessoa_pes
			WHERE ppp.fk_cod_processo_pro = @id
			AND pap.txt_descricao_pap = 'PARTICIPANTE'";
			
			if(!todos)
			{
				select += " AND ppp.boo_participante_ativo_ppp = true ";
			}

			select = select.Replace("@id",idProcesso);
			
			return Consultar(select);
		}
		
		public SrvAcompanhamentoProcesso()
		{
		}
	}
}
