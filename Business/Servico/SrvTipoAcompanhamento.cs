// SrvTipoAcompanhamento.cs created with MonoDevelop
// User: guilhermefacanha at 17:32 2/7/2009
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
using System.Data;
using Npgsql;

namespace Licitar.Business.Servico
{
	public class SrvTipoAcompanhamento:PostgreSqlDatabase
	{
		public DataTable listarTiposAcompanhamento()
		{
			string select = @"
			SELECT pk_cod_tipo_acompanhamento_tac as id, txt_descricao_tac as descricao
			FROM adm_licitar.tb_tipo_acompanhamento_tac
			WHERE boo_visivel_drop_tac = true
			ORDER BY txt_descricao_tac
			 ";
						
			return Consultar(select);

		}

		public int getCodigoTipoAcompanhamentoPorDescricao(string descricao)
		{
			string select = @"
			SELECT pk_cod_tipo_acompanhamento_tac as id
			FROM adm_licitar.tb_tipo_acompanhamento_tac
			WHERE txt_descricao_tac = '@descricao'
			LIMIT 1
			 ";

			select = select.Replace("@descricao",descricao);

			int id = 0;
						
			DataTable dt = Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				id = Convert.ToInt32(row["id"].ToString());
			}

			return id;
		}
		
		
		public SrvTipoAcompanhamento()
		{
		}
	}
}
