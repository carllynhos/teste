// SrvFrase.cs created with MonoDevelop
// User: guilhermefacanha at 14:22 20/5/2009
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
	public class SrvFrase
	{

		public Frase[] listarFrases(string descricao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Frase));
			dc.AddOrder(Order.Asc("Descricao"));
			if(!string.IsNullOrEmpty(descricao))
			{
				dc.Add(Expression.InsensitiveLike("Descricao","%"+descricao+"%"));
			}

			return Frase.FindAll(dc);
		}

		public Frase[] listarFrases()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Frase));
			dc.AddOrder(Order.Asc("Descricao"));
			
			return Frase.FindAll(dc);
		}
		
		public SrvFrase()
		{
		}
	}
}
