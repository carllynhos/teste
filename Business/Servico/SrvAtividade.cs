// SrvAtividade.cs created with MonoDevelop
// User: guilhermefacanha at 16:53 16/3/2009
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
using System.Data;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{	
	
	public class SrvAtividade
	{
		public static Atividade[] listarAtividades()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.IsNotNull("Modulo"));
			dc.AddOrder(Order.Asc("Modulo"));
			dc.AddOrder(Order.Asc("Descricao"));

			return Atividade.FindAll(dc);
		}

		public int getAtividadePorUrl(string url)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.InsensitiveLike("Url","%"+url+"%"));

			Atividade obj = Atividade.FindFirst(dc);

			return obj == null ? 0 : obj.Id;
		}

		public static Atividade[] listarAtividadesModulo(string modulo)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.IsNotNull("Modulo"));
			dc.Add(Expression.Eq("Modulo.Id",Convert.ToInt32(modulo)));
			dc.AddOrder(Order.Asc("Modulo"));
			dc.AddOrder(Order.Asc("Descricao"));

			return Atividade.FindAll(dc);
		}

		public static Atividade[] listarAtividadesAndamentos()
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Atividade));
            pesquisa.Add(Expression.Eq("TipoAndamento",true));
			pesquisa.Add(Expression.Eq("TipoAndamentoVisivel",true));
			pesquisa.AddOrder(Order.Asc("Descricao"));

			return Atividade.FindAll(pesquisa);
		}

		public static Atividade[] listarAtividadesFaseModalidade(string fase, int modalidade)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Atividade));
            pesquisa.Add(Expression.Not(Expression.Eq("Descricao","RECEBER")));
            pesquisa.Add(Expression.Sql(@"this_.pk_cod_atividade_ati in 
			(select fan.fk_cod_atividade_ati from adm_licitar.tb_fluxo_andamento_fan fan 
			inner join adm_licitar.tb_workflow_wor wor on fan.fk_cod_workflow_wor=wor.pk_cod_workflow_wor 
			inner join adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu on wmu.fk_cod_workflow_wor=wor.pk_cod_workflow_wor 
			where fan.fk_cod_fase_fas= "+fase+" and wmu.fk_cod_modalidade_mod="+modalidade+")"));
			pesquisa.Add(Expression.Eq("TipoAndamento", true));
			
            //ALT_PROD:
            pesquisa.AddOrder(Order.Asc("Descricao"));

			return Atividade.FindAll(pesquisa);
		}

		public static Atividade[] listarAtividadesFaseModalidadeComFiltroVisivel(string fase, int modalidade)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Atividade));
            pesquisa.Add(Expression.Not(Expression.Eq("Descricao","RECEBER")));
            pesquisa.Add(Expression.Sql(@"this_.pk_cod_atividade_ati in 
			(select fan.fk_cod_atividade_ati from adm_licitar.tb_fluxo_andamento_fan fan 
			inner join adm_licitar.tb_workflow_wor wor on fan.fk_cod_workflow_wor=wor.pk_cod_workflow_wor 
			inner join adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu on wmu.fk_cod_workflow_wor=wor.pk_cod_workflow_wor 
			where fan.fk_cod_fase_fas= "+fase+" and boo_ativo_fan = true and wmu.fk_cod_modalidade_mod="+modalidade+")"));
			pesquisa.Add(Expression.Eq("TipoAndamento", true));
			pesquisa.Add(Expression.Eq("TipoAndamentoVisivel",true));
			
            //ALT_PROD:
            pesquisa.AddOrder(Order.Asc("Descricao"));

			return Atividade.FindAll(pesquisa);
		}

		public List<int> listarAtividadesPadrao()
		{
			List<int> lista = new List<int>();
			
			string select = @"
			SELECT pk_cod_atividade_ati
			FROM adm_licitar.tb_atividade_ati ati
			WHERE boo_atividade_aberta_ati = true";
						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);

			foreach(DataRow row in dt.Rows)
			{				
				lista.Add(Convert.ToInt32(row[0].ToString()));
			}
						
			return lista;
		}

		public List<Atividade> listarObjsAtividadesPadrao()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.Eq("AtividadePadrao",true));
			dc.Add(Expression.Eq("ExibirNoMenu",true));

			Atividade[] atividades = Atividade.FindAll(dc);
			List<Atividade> lista = new List<Atividade>();

			foreach(Atividade a in atividades)
			{
				lista.Add(a);
			}

			return lista;
		}

		public List<Atividade> listarObjsAtividadesPadraoModulo(int modulo)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.Eq("AtividadePadrao",true));
			dc.Add(Expression.Eq("ExibirNoMenu",true));
			dc.Add(Expression.Eq("Modulo.Id",modulo));

			Atividade[] atividades = Atividade.FindAll(dc);
			List<Atividade> lista = new List<Atividade>();

			foreach(Atividade a in atividades)
			{
				lista.Add(a);
			}

			return lista;
		}

		public Atividade RecuperarAtividade (string strAtividade)
		{
			DetachedCriteria pesqAtividade = DetachedCriteria.For(typeof(Atividade));
			pesqAtividade.Add(Expression.Eq("Descricao",strAtividade));

			return Atividade.FindOne(pesqAtividade);
		}
		
		
		public SrvAtividade()
		{
		}
	}
}
