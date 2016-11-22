// SrvUnidadeExercicioFuncao.cs created with MonoDevelop
// User: guilhermefacanha at 17:12 24/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using Castle.ActiveRecord.Queries;

using Licitar.Business.Entidade;
using Licitar.Business.Dao;

namespace Licitar.Business.Servico
{
	
	
	public class SrvUnidadeExercicioFuncao
	{

		public static UnidadeExercicioFuncao[] listarUEF(string ue, string sue, string fun)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncao));
			dc.AddOrder(Order.Asc("Descricao"));

			if(!string.IsNullOrEmpty(ue) && string.IsNullOrEmpty(sue))
			{
				dc.Add(Expression.Eq("UnidadeExercicio.Id",Convert.ToInt32(ue)));
			}
			if(!string.IsNullOrEmpty(sue))
			{
				dc.Add(Expression.Eq("UnidadeExercicio.Id",Convert.ToInt32(sue)));
			}
			if(!string.IsNullOrEmpty(fun))
			{
				dc.Add(Expression.Eq("Funcao.Id",Convert.ToInt32(fun)));
			}
			
			return UnidadeExercicioFuncao.FindAll(dc);
		}

		public static UnidadeExercicioFuncao getUnidadeExercicioFuncaoPorSubUnidadeFuncao(string sub, string fun)
		{
			if(string.IsNullOrEmpty(sub) || string.IsNullOrEmpty(fun))
			{
				return null;
			}
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncao));
			dc.Add(Expression.Eq("Funcao.Id",Convert.ToInt32(fun)));
			dc.Add(Expression.Eq("UnidadeExercicio.Id",Convert.ToInt32(sub)));

			return UnidadeExercicioFuncao.FindFirst(dc);
		}

		public static bool existeUnidadeExercicioFuncao(string sue, string fun, string desc)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncao));
			dc.AddOrder(Order.Asc("Descricao"));

			dc.Add(Expression.Eq("Descricao",desc));
			dc.Add(Expression.Eq("UnidadeExercicio.Id",Convert.ToInt32(sue)));
			dc.Add(Expression.Eq("Funcao.Id",Convert.ToInt32(fun)));
			
			
			UnidadeExercicioFuncao obj = UnidadeExercicioFuncao.FindFirst(dc);

			return obj!=null;
		}
		
		public SrvUnidadeExercicioFuncao()
		{
		}
	}
}
