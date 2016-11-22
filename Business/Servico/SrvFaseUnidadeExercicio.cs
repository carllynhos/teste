// SrvFaseUnidadeExercicio.cs created with MonoDevelop
// User: danilo at 14:32 31/3/2009
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
using Licitar.Business.Servico;

namespace Licitar.Business.Servico
{
	public class SrvFaseUnidadeExercicio
	{
		public bool possuiFaseMarcacao(int idPessoa)
		{		
			SrvUnidadeExercicioFuncaoPessoa objSrvUEFP = new SrvUnidadeExercicioFuncaoPessoa();
			List<string> listaSubUnidades = objSrvUEFP.listarSubUnidadesPessoa(idPessoa);
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(FaseUnidadeExercicio));
			dc.CreateAlias("Fase","fas");
			dc.CreateAlias("UnidadeExercicio","uex");
			
			dc.Add(Expression.In("uex.Descricao",listaSubUnidades));
			dc.Add(Expression.Eq("fas.Descricao","MARCAÇÃO"));
			dc.Add(Expression.Or(Expression.Eq("DataFim",Convert.ToDateTime("01/01/0001 00:00:00")),Expression.IsNull("DataFim")));
	
			return FaseUnidadeExercicio.FindFirst(dc) != null;
		}
		
	}
}
