// SrvTipoUnidadeAdministrativa.cs created with MonoDevelop
// User: guilhermefacanha at 12:10 13/5/2009
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
	
	
	public class SrvTipoUnidadeAdministrativa
	{
		public TipoUnidadeAdministrativa[] listarTiposUnidadeAdministrativa()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(TipoUnidadeAdministrativa));
			dc.AddOrder(Order.Asc("Descricao"));

			return TipoUnidadeAdministrativa.FindAll(dc);
		}

		public TipoUnidadeAdministrativa[] getTiposUnidadeAdministrativa(string descricao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(TipoUnidadeAdministrativa));
			dc.AddOrder(Order.Asc("Descricao"));

			if(!string.IsNullOrEmpty(descricao))
				dc.Add(Expression.InsensitiveLike("Descricao","%"+descricao+"%"));

			return TipoUnidadeAdministrativa.FindAll(dc);
		}

		public TipoUnidadeAdministrativa getTipoSede()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(TipoUnidadeAdministrativa));
			dc.Add(Expression.InsensitiveLike("Descricao","sede"));

			return TipoUnidadeAdministrativa.FindFirst(dc);
		}
		
		public SrvTipoUnidadeAdministrativa()
		{
		}
	}
}
