// SrvUnidadeExercicioFuncaoPessoa.cs created with MonoDevelop
// User: guilhermefacanha at 18:21 12/3/2009
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


namespace Licitar.Business.Servico
{
	
	
	public class SrvUnidadeExercicioFuncaoPessoa
	{

		public UnidadeExercicioFuncaoPessoa getUniExerPessPorPessoa(int idPessoa)
		{			
			string select = @"
			SELECT pk_cod_unidade_exercicio_funcao_pessoa_efp 
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp
			WHERE fk_cod_pessoa_pes = ";

			select+= idPessoa + " LIMIT 1";
			
			UnidadeExercicioFuncaoPessoa obj = null;
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				obj = UnidadeExercicioFuncaoPessoa.Find(Convert.ToInt32(row[0].ToString()));
			}
			
			return obj;
			
		}

		public List<int> getUnidadesPessoa(int idPessoa)
		{
			string select = @"
			SELECT fk_cod_unidade_exercicio_funcao_uef 
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp
			WHERE fk_cod_pessoa_pes = @select and dat_fim_efp = '-infinity' ";

			select = select.Replace("@select", (idPessoa.ToString()));
           		
			List<int> lista = new List<int> ();
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				UnidadeExercicioFuncao obj = UnidadeExercicioFuncao.Find(Convert.ToInt32(row[0].ToString()));
				if(obj!=null && obj.UnidadeExercicio != null)
				{
					lista.Add(obj.UnidadeExercicio.Id);
				}
			}
			
			return lista;
		}

		public List<int> getFuncoesDasUnidadeExercicioFuncao(List<int> listaUnidades)
		{
			string unidades = "";
			foreach (int i in listaUnidades) 
			{
				unidades += i.ToString()+",";
			}

			if(!string.IsNullOrEmpty(unidades))
			{
				unidades = unidades.Remove(unidades.Length-1);
			}
			
			string select = @"
			SELECT fk_cod_funcao_fun
			FROM adm_licitar.tb_unidade_exercicio_funcao_uef
			WHERE pk_cod_unidade_exercicio_funcao_uef IN (@unidades)";

			select = select.Replace("@unidades", unidades);
           		
			List<int> lista = new List<int> ();
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				lista.Add(Convert.ToInt32(row[0].ToString()));			
			}

			return lista;
		}

		public List<string> listarSubUnidadesPessoa(int idPessoa)
		{
			string select = @"
			SELECT uex.txt_descricao_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex
			WHERE fk_cod_pessoa_pes = @select and dat_fim_efp = '-infinity'";

			select = select.Replace("@select", (idPessoa.ToString()));
           		
			List<string> lista = new List<string> ();
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				lista.Add(row[0].ToString());				
			}
			
			return lista;
		}

		public bool isGestorMaster(int idPessoa)
		{
			bool isgestor = false;
			
			string select = @"
			SELECT uex.pk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex
			WHERE fk_cod_pessoa_pes = @select and dat_fim_efp = '-infinity'
			AND uex.txt_descricao_uex ilike 'gestão master'";

			select = select.Replace("@select", (idPessoa.ToString()));
           						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			
			foreach(DataRow row in dt.Rows)
			{				
				isgestor = true;
				break;
			}
			
			return isgestor;
		}

		public List<string> listarGestoesUsuarioLogado(List<int> unidadesPessoa)
		{
			string unidades = SrvGerais.transformarListaEmString(unidadesPessoa);
			string select = @"
			SELECT txt_descricao_uex
			FROM adm_licitar.tb_unidade_exercicio_uex
			WHERE txt_descricao_uex IN ('GESTÃO CELS','GESTÃO PREGÃO','GESTÃO CCC')
			AND pk_cod_unidade_exercicio_uex IN (@unidades)";

			select = select.Replace("@unidades", (unidades));

			List<string> lista = new List<string>();
           						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			
			foreach(DataRow row in dt.Rows)
			{				
				lista.Add(row["txt_descricao_uex"].ToString());
			}
			return lista;
		}

		public List<int> listarIdsSubUnidadesPessoa(int idPessoa)
		{
			string select = @"
			SELECT uex.pk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex
			WHERE fk_cod_pessoa_pes = @select and dat_fim_efp = '-infinity'";

			select = select.Replace("@select", (idPessoa.ToString()));
           		
			List<int> lista = new List<int> ();
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				lista.Add(Convert.ToInt32(row[0].ToString()));
			}
			
			return lista;
		}

		public int listarIdSubUnidadeOutros()
		{
			string select = @"
			SELECT pk_cod_unidade_exercicio_uex as id
			FROM adm_licitar.tb_unidade_exercicio_uex
			WHERE txt_descricao_uex = 'OUTROS'
			AND fk_cod_unidade_exercicio_uex is not null";
          		
			int id = 0;
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				id = Convert.ToInt32(row["id"].ToString());
			}
			
			return id;
		}
		
		public DataTable listarDropSubUnidadesPessoa(int idPessoa)
		{
			string select = @"
			SELECT uex.pk_cod_unidade_exercicio_uex as id, uex.txt_descricao_uex as descricao
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex
			WHERE fk_cod_pessoa_pes = @select and dat_fim_efp = '-infinity'";

			select = select.Replace("@select", (idPessoa.ToString()));
           						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			
			return dt;
		}

		public List<int> listarIdsMacroUnidadesPessoa(int idPessoa)
		{
			string select = @"
			SELECT DISTINCT uex.fk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex
			WHERE fk_cod_pessoa_pes = @select and dat_fim_efp = '-infinity'";

			select = select.Replace("@select", (idPessoa.ToString()));
           		
			List<int> lista = new List<int> ();
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				lista.Add(Convert.ToInt32(row[0].ToString()));
			}
			
			return lista;
		}
		

		public bool MostrarCheckProcessosAssociados(int idPessoa)
		{
			string select = @"
			SELECT uex.pk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex on uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex
			WHERE fk_cod_pessoa_pes = @select and dat_fim_efp = '-infinity' 
			AND uex.boo_processos_associados_uex = true";

			select = select.Replace("@select", (idPessoa.ToString()));
           						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			if(dt!=null && dt.Rows!=null && dt.Rows.Count>0)
			{
				return true;
			}
			else
			{
				return false;
			}			
			
		}

		public UnidadeExercicioFuncaoPessoa[] getUEFPporPessoa(int idPessoa)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncaoPessoa));
			dc.Add(Expression.Eq("Pessoa.Id", idPessoa));
			dc.Add(Expression.Eq("DataFim",Convert.ToDateTime("01/01/0001 00:00:00")));
			dc.AddOrder(Order.Asc("DataInicio"));

			return UnidadeExercicioFuncaoPessoa.FindAll(dc);
		}

		public UnidadeExercicioFuncaoPessoa getAtualUniExerPessPorPessoa(int idPessoa)
		{			
			string select = @"
			SELECT pk_cod_unidade_exercicio_funcao_pessoa_efp 
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp
			WHERE fk_cod_pessoa_pes = ";

			select+= idPessoa + " AND dat_fim_efp = '-infinity' LIMIT 1 ";
			
			UnidadeExercicioFuncaoPessoa obj = null;
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				obj = UnidadeExercicioFuncaoPessoa.Find(Convert.ToInt32(row[0].ToString()));
			}
			
			return obj;
			
		}	
		
		public UnidadeExercicioFuncaoPessoa[] listarUEFP(string SUE, string pessoa, string UE)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncaoPessoa));

			if(!string.IsNullOrEmpty(SUE))
			{
				dc.CreateAlias("UnidadeExercicioFuncao","sue");
				dc.Add(Expression.Eq("sue.UnidadeExercicio.Id",Convert.ToInt32(SUE)));
				dc.AddOrder(Order.Asc("sue.UnidadeExercicio"));
			}
			else if(!string.IsNullOrEmpty(UE))
			{
				dc.CreateAlias("UnidadeExercicioFuncao","sue");
				dc.CreateAlias("sue.UnidadeExercicio","ue");
				dc.Add(Expression.Eq("ue.UnidadeExercicioPai.Id",Convert.ToInt32(UE)));
				dc.AddOrder(Order.Asc("sue.UnidadeExercicio"));
			}
			if(!string.IsNullOrEmpty(pessoa))
				dc.Add(Expression.Eq("Pessoa.Id",Convert.ToInt32(pessoa)));

			return UnidadeExercicioFuncaoPessoa.FindAll(dc);
		}
		

		public UnidadeExercicioFuncaoPessoa[] ListarPregoeirosDaSubUnidade(int idSubUnidadeExercicio)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncaoPessoa));
			dc.CreateAlias("UnidadeExercicioFuncao", "uef");
			dc.CreateAlias("uef.UnidadeExercicio", "uex");
			dc.Add(Expression.Eq("uex.Id", idSubUnidadeExercicio));
			dc.CreateAlias("uef.Funcao", "fun");
			dc.Add(Expression.InsensitiveLike("fun.Descricao", "%PREGOEIRO%"));
			dc.Add(Expression.Or(Expression.Eq("DataFim",Convert.ToDateTime("01/01/0001 00:00:00")),Expression.IsNull("DataFim")));
			return UnidadeExercicioFuncaoPessoa.FindAll(dc);
		}		
	}
}
