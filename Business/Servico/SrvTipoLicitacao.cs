// SrvTipoLicitacao.cs created with MonoDevelop
// User: guilhermefacanha at 18:52 7/5/2009
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
	
	
	public class SrvTipoLicitacao
	{
		public TipoLicitacao[] listarTipoLicitacao()
		{
			return TipoLicitacao.FindAll(Order.Asc("Descricao"));
		}
		
		public int listarIdTipoLicitacao(string tipolicitacao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(TipoLicitacao));
				dc.Add(Expression.InsensitiveLike("Descricao",tipolicitacao.ToUpper()));
			
			return TipoLicitacao.FindFirst(dc).Id;
		}
		
		public SrvTipoLicitacao()
		{
		}
	}
}
