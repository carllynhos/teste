// SrvTipoNumero.cs created with MonoDevelop
// User: guilhermefacanha at 11:36 26/3/2009
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
	
	
	public class SrvTipoNumero
	{

		public int getIdTipoNumeroPorDescricao(string descricao)
		{
			string idTipoNumero = "";
			string select = @"
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = '@descricao'";
			
			select = select.Replace("@descricao", descricao);
						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				idTipoNumero = row["pk_cod_tipo_numero_tnu"].ToString();
			}
			
			return string.IsNullOrEmpty(idTipoNumero) ? 0 : Convert.ToInt32(idTipoNumero);
			
		}
		
		
		public SrvTipoNumero()
		{
		}
	}
}
