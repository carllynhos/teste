// SrvFuncao.cs created with MonoDevelop
// User: guilhermefacanha at 12:52 24/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using NHibernate.Expression;

using Licitar.Business.Dao;
using Licitar.Business.Entidade;
using System.Collections.Generic;
using Licitar.Business.Dto;

namespace Licitar.Business.Servico
{
	
	
	public class SrvFuncao
	{
		public static Funcao[] listarFuncoes()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Funcao));
			dc.AddOrder(Order.Asc("Descricao"));

			return Funcao.FindAll(dc);
		}
		
		public SrvFuncao()
		{
		}
	}
}
