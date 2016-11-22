// SrvUnidadeAdministrativa.cs created with MonoDevelop
// User: guilhermefacanha at 10:15 13/5/2009
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
	
	
	public class SrvUnidadeAdministrativa
	{
		public UnidadeAdministrativa[] listarUnidadesAdministrativasPorInstituicao(int idInstituicao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeAdministrativa));
			dc.CreateAlias("TipoUnidadeAdministrativa","tua");
			dc.CreateAlias("Instituicao","ins");
			dc.Add(Expression.Eq("Instituicao.Id",idInstituicao));
			dc.AddOrder(Order.Asc("ins.Sigla"));
			dc.AddOrder(Order.Asc("tua.Descricao"));

			return UnidadeAdministrativa.FindAll(dc);
		}

		public UnidadeAdministrativa[] getUnidadesAdministrativas(string tipo, string idInstituicao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeAdministrativa));
			dc.CreateAlias("TipoUnidadeAdministrativa","tua");
			dc.CreateAlias("Instituicao","ins");
			
			if(!string.IsNullOrEmpty(idInstituicao))
				dc.Add(Expression.Eq("Instituicao.Id",Convert.ToInt32(idInstituicao)));
			
			if(!string .IsNullOrEmpty(tipo))
				dc.Add(Expression.Eq("TipoUnidadeAdministrativa.Id",Convert.ToInt32(tipo)));
			
			dc.AddOrder(Order.Asc("ins.Sigla"));
			dc.AddOrder(Order.Asc("tua.Descricao"));

			return UnidadeAdministrativa.FindAll(dc);
		}
		
		
		
		public SrvUnidadeAdministrativa()
		{
		}
	}
}
