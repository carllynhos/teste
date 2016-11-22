// SrvNatureza.cs created with MonoDevelop
// User: guilhermefacanha at 18:54 7/5/2009
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
	
	
	public class SrvNatureza
	{
		public Natureza[] listarNatureza()
		{
			return Natureza.FindAll(Order.Asc("Descricao"));
		}
		
		public int listarIdNatureza(string strNatureza)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Natureza));
				dc.Add(Expression.InsensitiveLike("Descricao",strNatureza.ToUpper()));
			
			return Natureza.FindFirst(dc).Id;
		}
		
		public SrvNatureza()
		{
		}
	}
}
