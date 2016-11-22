// srvModulo.cs created with MonoDevelop
// User: marcelolima at 15:55 13/7/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using Licitar.Business.Entidade;
using Licitar.Business.Dto;
using NHibernate.Expression;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	
	
	public class srvModulo
	{

		public Modulo[] ListarModulos()
		{
			return Modulo.FindAll(Order.Asc("Descricao"));
		}
		
		public srvModulo()
		{
		}

		
	}
}
