// SrvPapel.cs created with MonoDevelop
// User: guilhermefacanha at 13:41 30/6/2009
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
using Licitar.Business.Persistencia;
using Castle.ActiveRecord;

namespace Licitar.Business.Servico
{	
	public class SrvPapel:PostgreSqlDatabase
	{
		public DataTable listarPapel()
		{
			string select =@"
			SELECT pap.pk_cod_papel_pap as id, pap.txt_descricao_pap as descricao
			FROM adm_licitar.tb_papel_pap pap
			";

			return Consultar(select);

		}
		
		public SrvPapel()
		{
		}
	}
}
