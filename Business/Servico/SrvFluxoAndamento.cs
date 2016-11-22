// SrvFluxoAndamento.cs created with MonoDevelop
// User: guilhermefacanha at 11:44 3/4/2009
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


	public class SrvFluxoAndamento
	{
	
		public static FluxoAndamento getFluxoAndamentoCadastroDataEntPGE(string modalidade)
		{
			DetachedCriteria dcFluxo2 = DetachedCriteria.For(typeof(FluxoAndamento));				
			dcFluxo2.CreateAlias("Fase","fas");
	    	dcFluxo2.CreateAlias("Atividade","ati");
			dcFluxo2.Add(Expression.Eq("fas.Descricao","CADASTRO"));
			dcFluxo2.Add(Expression.Eq("ati.Descricao","DATA DE ENTRADA NA PGE"));
			dcFluxo2.Add(Expression.Sql("this_.fk_cod_workflow_wor in (select wor.pk_cod_workflow_wor from adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu inner join adm_licitar.tb_workflow_wor wor on wmu.fk_cod_workflow_wor = wor.pk_cod_workflow_wor where wmu.fk_cod_modalidade_mod='"+modalidade+"')"));
			return FluxoAndamento.FindFirst(dcFluxo2);
		}
	
		public static FluxoAndamento getFluxoAndamentoCadastroCadastradoPGE(string modalidade)
		{
			DetachedCriteria dcFluxo = DetachedCriteria.For(typeof(FluxoAndamento));				
			dcFluxo.CreateAlias("Fase","fas");
	    	dcFluxo.CreateAlias("Atividade","ati");
			dcFluxo.Add(Expression.Eq("fas.Descricao","CADASTRO"));
			dcFluxo.Add(Expression.Eq("ati.Descricao","CADASTRADO"));
			dcFluxo.Add(Expression.Sql("this_.fk_cod_workflow_wor in (select wor.pk_cod_workflow_wor from adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu inner join adm_licitar.tb_workflow_wor wor on wmu.fk_cod_workflow_wor = wor.pk_cod_workflow_wor where wmu.fk_cod_modalidade_mod='"+modalidade+"')"));
			return FluxoAndamento.FindFirst(dcFluxo);
		}

		public int getFluxosPorFaseAndamentoModalidade(string fase, string andamento, string modalidade)
		{
			string select = @"
			SELECT distinct fan.pk_cod_fluxo_andamento_fan as id
			FROM adm_licitar.tb_fluxo_andamento_fan fan
			INNER JOIN adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu ON wmu.fk_cod_workflow_wor = fan.fk_cod_workflow_wor
			INNER JOIN adm_licitar.tb_modalidade_mod mod ON mod.pk_cod_modalidade_mod = wmu.fk_cod_modalidade_mod
			WHERE fan.fk_cod_fase_fas = @fase
			AND fan.fk_cod_atividade_ati = @andamento
			AND mod.pk_cod_modalidade_mod = @modalidade";
			
			select = select.Replace("@fase", fase);
			select = select.Replace("@andamento", andamento);
			select = select.Replace("@modalidade", modalidade);
			
			DataTable dt = new PostgreSqlDatabase().Consultar(select);

			string idFluxo = "";
			int id = 0;

			foreach(DataRow row in dt.Rows)
			{
				idFluxo = row["id"].ToString();
			}

			if(int.TryParse(idFluxo,out id))
			{
				return id;
			}
			else
			{
				return 0;
			}
		}

		public FluxoAndamento RetornaFluxoAndamento(string modalidade,string tipoAndamento)
		{
			
			DetachedCriteria pesqAtividade = DetachedCriteria.For(typeof(Atividade));
			pesqAtividade.Add(Expression.Eq("Descricao",tipoAndamento).IgnoreCase());
			
			DetachedCriteria pesqFase = DetachedCriteria.For(typeof(Fase));
			pesqFase.Add(Expression.Eq("Descricao","REALIZAÇÃO"));

			
			DetachedCriteria pesqWorkFlow = DetachedCriteria.For(typeof(Workflow));
			pesqWorkFlow.Add(Expression.Sql(@"
				this_.pk_cod_workflow_wor =
				(select fk_cod_workflow_wor from adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu
				inner join adm_licitar.tb_modalidade_mod mod on mod.pk_cod_modalidade_mod = wmu.fk_cod_modalidade_mod
				where txt_descricao_mod ilike '"+modalidade+@"')"));
		

			Atividade.FindFirst(pesqAtividade);
			Workflow.FindFirst(pesqWorkFlow);
			Fase.FindFirst(pesqFase);

			
			DetachedCriteria pesqFluxoAndamento = DetachedCriteria.For(typeof(FluxoAndamento));
			pesqFluxoAndamento.CreateAlias("Atividade","ati").CreateAlias("Fase","fas").CreateAlias("Workflow","work");
			pesqFluxoAndamento.Add(Expression.Eq("ati.Id",Atividade.FindFirst(pesqAtividade).Id));
			pesqFluxoAndamento.Add(Expression.Eq("fas.Id",Fase.FindFirst(pesqFase).Id));
			pesqFluxoAndamento.Add(Expression.Eq("work.Id",Workflow.FindFirst(pesqWorkFlow).Id));
			pesqFluxoAndamento.Add(Expression.Eq("CombinacaoAtiva",true));

			
						
			FluxoAndamento fluxoAnd = FluxoAndamento.FindFirst(pesqFluxoAndamento);
						
			return FluxoAndamento.FindFirst(pesqFluxoAndamento);
			
		}

		public FluxoAndamento RetornaFluxoAndamento(string modalidade,string tipoAndamento,string fase)
		{
			DetachedCriteria pesqAtividade = DetachedCriteria.For(typeof(Atividade));
			pesqAtividade.Add(Expression.Eq("Descricao",tipoAndamento).IgnoreCase());
			
			DetachedCriteria pesqFase = DetachedCriteria.For(typeof(Fase));
			pesqFase.Add(Expression.Eq("Descricao",fase.ToUpper()).IgnoreCase());

			
			DetachedCriteria pesqWorkFlow = DetachedCriteria.For(typeof(Workflow));
			pesqWorkFlow.Add(Expression.Sql(@"
				this_.pk_cod_workflow_wor =
				(select fk_cod_workflow_wor from adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu
				inner join adm_licitar.tb_modalidade_mod mod on mod.pk_cod_modalidade_mod = wmu.fk_cod_modalidade_mod
				where txt_descricao_mod ilike '"+modalidade+@"')"));
		

			Atividade.FindFirst(pesqAtividade);
			Workflow.FindFirst(pesqWorkFlow);
			Fase.FindFirst(pesqFase);

			
			DetachedCriteria pesqFluxoAndamento = DetachedCriteria.For(typeof(FluxoAndamento));
			pesqFluxoAndamento.CreateAlias("Atividade","ati").CreateAlias("Fase","fas").CreateAlias("Workflow","work");
			pesqFluxoAndamento.Add(Expression.Eq("ati.Id",Atividade.FindFirst(pesqAtividade).Id));
			pesqFluxoAndamento.Add(Expression.Eq("fas.Id",Fase.FindFirst(pesqFase).Id));
			pesqFluxoAndamento.Add(Expression.Eq("work.Id",Workflow.FindFirst(pesqWorkFlow).Id));
			pesqFluxoAndamento.Add(Expression.Eq("CombinacaoAtiva",true));

			return FluxoAndamento.FindFirst(pesqFluxoAndamento);
			
		}
			
		public SrvFluxoAndamento()
		{
		}
	}
}
