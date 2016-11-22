// SrvWorkFlow.cs created with MonoDevelop
// User: guilhermefacanha at 10:37 8/5/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections.Generic;
using Npgsql;
using NHibernate;
using NHibernate.Expression;
using Castle.ActiveRecord.Queries;
using Licitar.Business.Dao;
using Licitar.Business.Entidade;

namespace Licitar.Business.Servico
{	
	public class SrvWorkFlow
	{
		public Workflow getWorkflowByDescricao(string descricao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Workflow));
			dc.Add(Expression.InsensitiveLike("Descricao","%"+descricao+"%"));

			return Workflow.FindFirst(dc);
		}

		public ModalidadeUnidadeExercicio getModalidadeUnidadeExercicio(int idModalidade, int idWorkFlow)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ModalidadeUnidadeExercicio));
			dc.Add(Expression.Eq("Modalidade.Id",idModalidade));
			dc.Add(Expression.Eq("Workflow.Id",idWorkFlow));
			
			return ModalidadeUnidadeExercicio.FindFirst(dc);
		}

		public Workflow getWorkFlowByModalidade(int idModalidade)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ModalidadeUnidadeExercicio));
			ModalidadeUnidadeExercicio obj = ModalidadeUnidadeExercicio.FindFirst(dc);
			return obj.Workflow;
		}
		
		public SrvWorkFlow()
		{
		}
	}
}
