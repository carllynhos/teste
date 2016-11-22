// AtualizarDataEntradaPGE.cs created with MonoDevelop
// User: wanialdo at 09:40 6/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Licitar.Business.Dao;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Manutenção da Data de Entrada na PGE.
	/// </summary>
	public class SrvAtualizarDataEntradaPGE : PostgreSqlDatabase
	{
		/// <summary>
		/// Atualiza as datas de entrada dos processos que tiverem entrada posterior ao cadastro. 
		/// </summary>
		public bool AtualizarDatasEntrada()
		{
			DataTable dt = CarregarDatasEntrada();
			string sql = null;
			
			//TODO: Alterar a data para 1 minuto antes da data de cadastro, para que a entrada apareça primeiro.
			
			Console.WriteLine(dt.Rows.Count.ToString());
			for (int i=0; i <= dt.Rows.Count - 1; i++)
			{
				//try
				//{
				sql =  "update adm_licitar.tb_processo_andamento_pan ";
				sql += "set dat_cadastro_pan = '" + dt.Rows[i][3] + "' ";
				sql += "where pk_cod_processo_andamento_pan = " + dt.Rows[i][0];
				
				Console.WriteLine(sql);
				//ExecutarComando(sql);
				//}
				//catch 
				//{ 
					//Catch vazio aguardando correção na Trigger do Processo Completo.
				//}
			}
			return true;
		}
		
		/// <summary>
		/// Carrega os processos que têm data de entrada posterior ao cadastro. 
		/// </summary>
		protected DataTable CarregarDatasEntrada()
		{
	  		return Consultar(ConsultaXML.retornarSQL("DatasEntradaCorrigir"));
		}
	}
	
}
