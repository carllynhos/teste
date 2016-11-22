
using System;
using Licitar.Business.Persistencia;
using System.Data;
using Licitar.Business.Entidade;
using Licitar.Business.Dto;
using System.Collections.Generic;


 

namespace Licitar.Business.Dao
{
	
	
	public class DAOConsultaProcesso
	{
		
		PostgreSqlDatabase postGreSql = new PostgreSqlDatabase();
		public DAOConsultaProcesso()
		{
		}
		
		public DataTable listarTabelao(DTOConProcesso dtoConProcesso, Pessoa usuarioLogado)
		{				
  			string idsTipoAndamento = string.Empty;
			
			string sql = "SELECT * FROM adm_licitar.tb_processo_completo_pcm pcm where 0=0 ";

			if (dtoConProcesso.IdInstituicao > 0)
			{
				sql+="and cod_instituicao_ins ="+dtoConProcesso.IdInstituicao;
			}
			//Verifica se no dto de filtro foi selecionado alguma modalidade
			if (dtoConProcesso.IdModalidade > 0)
			{			
				sql+="cod_modalidade_mod = "+dtoConProcesso.IdModalidade;
			}
		
			
			
			Console.WriteLine("sql ="+sql);
			return postGreSql.Consultar(sql);
		}
		/*
		public  List<DTOProcessoPapelPessoa> PesqProcessoPapelPessoa(int idProcesso)
		{				
  			List<DTOProcessoPapelPessoa> dtoProcPapPes = new List<DTOProcessoPapelPessoa>();
			Console.WriteLine("chegou aki 1");
			string sql = @" select pk_cod_processo_papel_pessoa_ppp as Id, txt_nome_pes as Nome, txt_descricao_pap as Papel  ,boo_principal_ppp as Principal
								from adm_licitar.tb_processo_papel_pessoa_ppp ppp
			inner join adm_licitar.tb_pessoa_pes pes on ppp.fk_cod_pessoa_pes = pes.pk_cod_pessoa_pes
			inner join adm_licitar.tb_papel_pap pap on ppp.fk_cod_papel_pap = pap.pk_cod_papel_pap
			where
			fk_cod_processo_pro = "+idProcesso+" and not pap.txt_descricao_pap = 'VENCEDOR' ";

			DataTable dtb = postGreSql.Consultar(sql);
			Console.WriteLine("chegou aki 2");
			Console.WriteLine("chegou aki 3");
			for (int i = 0;i<dtb.Rows.Count; i++ ) {
			 	DTOProcessoPapelPessoa dtoProc = new DTOProcessoPapelPessoa();
				Console.WriteLine("chegou aki 5");
				dtoProc.Id = Convert.ToString(dtb.Rows[i]["Id"]);
				dtoProc.Nome = Convert.ToString(dtb.Rows[i]["Nome"]);
				Console.WriteLine("chegou aki 6");
				dtoProc.Papel = Convert.ToString(dtb.Rows[i]["Papel"]);
				dtoProc.Principal = Convert.ToBoolean(dtb.Rows[i]["Principal"]);
				Console.WriteLine("chegou aki 7");
				dtoProcPapPes.Add(dtoProc);
				Console.WriteLine("chegou aki 8");
			 } 
				Console.WriteLine("chegou aki 9"+dtoProcPapPes.Count);		

			//return postGreSql.Consultar(sql);
			return dtoProcPapPes;
		}
		*/
		
		
	}
}
