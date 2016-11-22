// clDBCalendario.cs created with MonoDevelop
// User: janiojunior at 09:11 12/9/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Npgsql;
using System.Configuration;
using System.Text;

namespace Calendario_LIcitacao
{
	public sealed class clDBCalendario
	{
		public static DateTime[] GetPregoesDia(DateTime DtAtual)
		{
			DataTable rtn = null;
			DateTime[] datas = null;
			
			try
			{
				Npgsql.NpgsqlConnection conex = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
			
				if(conex.State == ConnectionState.Closed) conex.Open();
							
				NpgsqlDataAdapter adp = new NpgsqlDataAdapter();
				NpgsqlCommand cmd = new NpgsqlCommand();
				cmd.Connection = conex;
				cmd.CommandText = GetConsulta(DtAtual);
				
				DataSet ds = new DataSet();
				adp.SelectCommand = cmd;
				adp.Fill(ds);
				if (ds.Tables[0].Rows.Count>0) 
				{ 
					rtn = ds.Tables[0];
					datas = new DateTime[ds.Tables[0].Rows.Count];
					
					for (int i = 0; i <= rtn.Rows.Count - 1; i++)
					{
						datas[i] = Convert.ToDateTime(rtn.Rows[i][0]);
					}
				}
				
				conex.Close();
				conex.Dispose();
			}
			catch(Exception e)
			{
				Console.WriteLine("Erro: {0}", e.Message);
				datas = null;
			}
			
			return datas;
		}
		
		private static string GetConsulta(DateTime DtAtual)
		{
			StringBuilder consult = new StringBuilder();
			
			consult.Append("SELECT dat_inicio ");
	        consult.Append("FROM adm_pge.vw_agenda_transmissao ");
	        consult.Append("WHERE cancelada = 0 and date_part('MONTH', dat_inicio) = {0} and date_part('YEAR', dat_inicio) = {1}");
			
			return string.Format(consult.ToString(),DtAtual.Month.ToString(),DtAtual.Year.ToString());
		}
		
		
/*		public static bool GetPregoesDia(DateTime DtAtual)
		{
			bool rtn = false;
			try
			{
				Npgsql.NpgsqlConnection conex = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
			
				if(conex.State == ConnectionState.Closed)
					conex.Open();
							
				string date = DtAtual.Year.ToString() +"-" +DtAtual.Month.ToString() +"-" +DtAtual.Day.ToString();
				NpgsqlDataAdapter adp = new NpgsqlDataAdapter();
				NpgsqlCommand cmd = new NpgsqlCommand();
				cmd.Connection = conex;
				cmd.CommandText = GetConsulta(date);
				
				DataSet ds = new DataSet();
				Console.WriteLine("Select: {0}", cmd.CommandText);
				adp.SelectCommand = cmd;
				adp.Fill(ds);
				rtn = ds.Tables[0].Rows.Count>0?true:false;
				conex.Close();
				conex.Dispose();
			}
			catch(Exception e)
			{
				Console.WriteLine("Erro: {0}", e.Message);
				rtn = false;
			}
			return rtn;
		}
		
		private static string GetConsulta(string DtAtual)
		{
			StringBuilder consult = new StringBuilder();
			
			consult.Append("SELECT pk_cod_processo_andamento_pan ");
	        consult.Append("FROM adm_pge.vw_agenda_transmissao_age ");
	        consult.Append("WHERE cancelada = 0 and '{0}' between to_date(to_char(dat_inicio,'DD/MM/YYYY'), 'DD/MM/YYYY') and to_date(to_char(dat_prorrogacao,'DD/MM/YYYY'),'DD/MM/YYYY')");
			
			return string.Format(consult.ToString(),DtAtual);
		}*/
	}
}
