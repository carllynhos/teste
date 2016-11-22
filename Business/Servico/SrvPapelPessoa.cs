// SrvPapelPessoa.cs created with MonoDevelop
// User: guilhermefacanha at 13:39 30/6/2009
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
	public class SrvPapelPessoa:PostgreSqlDatabase
	{
		public DataTable listarPapelPessoa(string papel,string pessoa)
		{
			string select =@"
			SELECT ppe.pk_cod_papel_pessoa_ppe as Id, pes.txt_nome_pes as Pessoa, pap.txt_descricao_pap as Papel, ppe.dat_inicio_ppe as inicio, ppe.dat_fim_ppe as fim
			FROM adm_licitar.tb_papel_pessoa_ppe ppe
			INNER JOIN adm_licitar.tb_pessoa_pes pes on pes.pk_cod_pessoa_pes = ppe.fk_cod_pessoa_pes
			INNER JOIN adm_licitar.tb_papel_pap pap on pap.pk_cod_papel_pap = ppe.fk_cod_papel_pap
			WHERE 0=0 			
			";

			if(!string.IsNullOrEmpty(papel))
			{
				select+=" AND ppe.fk_cod_papel_pap = '"+papel+"'";
			}
			if(!string.IsNullOrEmpty(pessoa))
			{
				select+=" AND pes.pk_cod_pessoa_pes = '"+pessoa+"'";
			}

			select+=" ORDER BY pes.txt_nome_pes, pap.txt_descricao_pap, ppe.dat_inicio_ppe DESC";

			return Consultar(select);
		
		}

		public int existePessoaCadastrada(string pessoa)
		{
			string select =@"
			SELECT ppe.pk_cod_papel_pessoa_ppe as Id
			FROM adm_licitar.tb_papel_pessoa_ppe ppe
			WHERE ppe.fk_cod_pessoa_pes = '@pessoa'
			AND (ppe.dat_fim_ppe is null OR ppe.dat_fim_ppe = '-infinity')
			";

			select = select.Replace("@pessoa",pessoa);

			DataTable dt = Consultar(select);

			int existe = 0;

			foreach(DataRow row in dt.Rows)
			{
				existe = Convert.ToInt32(row["Id"].ToString());
			}
			
			return existe;
		}

		public DataTable listarPessoaPorPapel(string papel)
		{
			string select =@"
			SELECT ppe.pk_cod_papel_pessoa_ppe as Id, pes.txt_nome_pes as Descricao
			FROM adm_licitar.tb_papel_pessoa_ppe ppe
			INNER JOIN adm_licitar.tb_pessoa_pes pes on pes.pk_cod_pessoa_pes = ppe.fk_cod_pessoa_pes
			WHERE ppe.fk_cod_papel_pap = '@papel'			
			";

			select = select.Replace("@papel",papel);

			return Consultar(select);
		}
		
		public SrvPapelPessoa()
		{
		}
	}
}
