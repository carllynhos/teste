// SrvUnidadeExercicio.cs created with MonoDevelop
// User: guilhermefacanha at 15:33 7/4/2009
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
	
	
	public class SrvUnidadeExercicio
	{
		public UnidadeExercicio[] listarUnidadeExercicioByArea(int idArea)
		{
			DetachedCriteria pesqExercicio = DetachedCriteria.For(typeof(UnidadeExercicio));
			pesqExercicio.Add(Expression.Eq("Area.Id", idArea));
			pesqExercicio.AddOrder(Order.Asc("Descricao"));

			return UnidadeExercicio.FindAll(pesqExercicio);
		}

		public UnidadeExercicio[] listarUnidades()
		{
			DetachedCriteria pesqExercicio = DetachedCriteria.For(typeof(UnidadeExercicio));
			pesqExercicio.Add(Expression.Eq("Area.Id", 12));
			pesqExercicio.Add(Expression.IsNull("UnidadeExercicioPai"));
			pesqExercicio.AddOrder(Order.Asc("Descricao"));

			return UnidadeExercicio.FindAll(pesqExercicio);
		}

		public DataTable listarSubUnidadesExercicio(string idUnidadeExercicio)
		{
			string select = @"
			SELECT pk_cod_unidade_exercicio_uex as id, txt_descricao_uex as descricao
			FROM adm_licitar.tb_unidade_exercicio_uex uex
			WHERE fk_cod_area_are = 12
			AND (dat_fim_uex = '-infinity' OR dat_fim_uex is null)
			AND fk_cod_unidade_exercicio_uex = @idUnidadeExercicio
			ORDER BY txt_descricao_uex";

			select = select.Replace("@idUnidadeExercicio",idUnidadeExercicio);
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			return dt;
		}

		public DataTable listarMacroUnidades()
		{
			string select = @"
			SELECT pk_cod_unidade_exercicio_uex as id, txt_descricao_uex as descricao
			FROM adm_licitar.tb_unidade_exercicio_uex uex
			WHERE fk_cod_area_are = 12
			AND fk_cod_unidade_exercicio_uex is null
			AND (dat_fim_uex = '-infinity' OR dat_fim_uex is null)
			ORDER BY txt_descricao_uex";
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			return dt;
		}

		public int listarIdMacroUnidade(string descricao)
		{
			string select = @"
			SELECT fk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_uex
			WHERE txt_descricao_uex = '@desc'";

			select = select.Replace("@desc", descricao);
           		
			int id = 0;
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				id = Convert.ToInt32(row[0].ToString());
			}
			
			return id;
		}
		
		//Criado para WebService
		public int listarIdMacroUnidades(string descricao)
		{
			string select = @"
				SELECT distinct uexMacro.pk_cod_unidade_exercicio_uex 
			FROM adm_licitar.tb_unidade_exercicio_uex uexMacro, adm_licitar.tb_unidade_exercicio_uex uexMicro where uexMicro.fk_cod_unidade_exercicio_uex = uexMacro.pk_cod_unidade_exercicio_uex
			and upper(uexMacro.txt_descricao_uex) = upper('@desc')";

			select = select.Replace("@desc", descricao);
           		
			int id = 0;
			
			DataTable dt = new PostgreSqlDatabase().Consultar(select);		
			
			foreach(DataRow row in dt.Rows)
			{							
				id = Convert.ToInt32(row[0].ToString());
			}
			
			return id;
		}
		//Criado para WebService
		public int listarIdMicroUnidade(string descricao)
		{
			string select = @"
			
		SELECT uexMicro.pk_cod_unidade_exercicio_uex,uexMacro.pk_cod_unidade_exercicio_uex,  uexMacro.fk_cod_unidade_exercicio_uex,uexMacro.txt_descricao_uex,uexMicro.fk_cod_unidade_exercicio_uex,uexMicro.txt_descricao_uex
			FROM adm_licitar.tb_unidade_exercicio_uex uexMacro, adm_licitar.tb_unidade_exercicio_uex uexMicro where uexMicro.fk_cod_unidade_exercicio_uex = uexMacro.pk_cod_unidade_exercicio_uex
			and upper(uexMicro.txt_descricao_uex) = upper('@desc')
			";

			select = select.Replace("@desc", descricao);
           		
			int id = 0;
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				id = Convert.ToInt32(row[0].ToString());
			}
			
			return id;
		}
		

		public List<int> listarMacroUnidadesPorMicroUnidades(List<int> microUnidades)
		{
			string unidades = SrvGerais.transformarListaEmString(microUnidades);
			string select = @"
			SELECT DISTINCT fk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_uex
			WHERE pk_cod_unidade_exercicio_uex IN (@unidades)";

			select = select.Replace("@unidades", unidades);
           		
			List<int> lista = new List<int> ();
					
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{				
				lista.Add(Convert.ToInt32(row[0].ToString()));
			}
			
			return lista;
		}
		
		public SrvUnidadeExercicio()
		{
		}
	}
}
