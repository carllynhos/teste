// SrvInstituicao.cs created with MonoDevelop
// User: guilhermefacanha at 11:06 3/4/2009
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
	
	
	public class SrvInstituicao
	{

		public Instituicao[] listarInstituicoes()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Instituicao));
			dc.AddOrder(Order.Asc("Sigla"));

			return Instituicao.FindAll(dc);
		}
		
		public int nomeInstituicao(string strInstituicao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Instituicao));
				dc.Add(Expression.Eq("Sigla",strInstituicao.ToUpper()));
			return Instituicao.FindFirst(dc).Id;
		}

		public Instituicao[] listarInstituicoesPGE()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Instituicao));
					dc.Add(Expression.Eq("Sigla","PGE"));
			return Instituicao.FindAll(dc);
		}
		
		public SrvInstituicao()
		{
		}
	}
}
