using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Npgsql;
using NpgsqlTypes;

namespace Licitar.Business.Persistencia
{
	public struct parametroConsulta
	{
		public string parametro;
		public object valor;
		public string tipo;

		public parametroConsulta(string parametro, string tipo, object valor)
		{
			this.parametro = parametro;
			this.tipo = tipo;			this.valor = valor;

		}
	}
	
	public class PostgreSqlDatabase
	{		
		public void ExecutarComando(string sqlCommand)
        {
            NpgsqlCommand command = new NpgsqlCommand(sqlCommand, new PostgreSqlConnection().CriarConexao());

            try
            {
				command.CommandTimeout = 120;
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                command.Connection.Close();				
            }
        }

		public void ExecutarComando(NpgsqlCommand command)
        {
            try
            {                
				command.Connection = new PostgreSqlConnection().CriarConexao();
               	command.CommandTimeout = 120;
				command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                command.Connection.Close();				
            }
        }

		public DataSet ExecutarConsulta(NpgsqlCommand command)
	    {
            try
            {
	            DataSet dtsResultado = new DataSet();
				command.Connection = new PostgreSqlConnection().CriarConexao();
                command.CommandTimeout = 120;
                NpgsqlDataAdapter adaptador = new NpgsqlDataAdapter(command);                  	            
	            adaptador.Fill(dtsResultado);
                return dtsResultado;
	        }
	        catch
	        {
	        	throw;
	        }		
	    }

        public DataSet ExecutarConsulta(string sqlSelect)
        {
            try
            {
                NpgsqlCommand command = new NpgsqlCommand(sqlSelect, new PostgreSqlConnection().CriarConexao());
				command.CommandTimeout = 120;
                DataSet dtsResultado = new DataSet();                
                NpgsqlDataAdapter adaptador = new NpgsqlDataAdapter(command);
                adaptador.Fill(dtsResultado);
                return dtsResultado;
            }
            catch
            {
                throw;
            }
        }
		
		
		public DataSet ExecutarConsulta(string sqlSelect, List<parametroConsulta> parametros)
        {
            try
            {				
				NpgsqlCommand command = new NpgsqlCommand(sqlSelect, new PostgreSqlConnection().CriarConexao());
               			
				command.CommandTimeout = 120;
				foreach(parametroConsulta p in parametros)
				{
					if(p.tipo=="arraynumeric")
					{
						NpgsqlParameter parametro = new NpgsqlParameter();
						parametro.NpgsqlDbType =  NpgsqlDbType.Array | NpgsqlDbType.Numeric;
						parametro.ParameterName = p.parametro;
						parametro.Value = p.valor;
						command.Parameters.Add(parametro);						
					}
					else
					{command.Parameters.Add(p.parametro,p.valor);}
					
				}
				
				DataSet dtsResultado = new DataSet();
	            NpgsqlDataAdapter adaptador = new NpgsqlDataAdapter(command);
	            adaptador.Fill(dtsResultado);
	            return dtsResultado;
			
            }
            catch
            {
                throw;
            }
        }

		public NpgsqlDataReader ExecutarConsultaReader(string sqlSelect)
        {
            try
            {
                NpgsqlCommand command = new NpgsqlCommand(sqlSelect, new PostgreSqlConnection().CriarConexao());
				command.CommandTimeout = 120;                			               
                return command.ExecuteReader();
            }
            catch
            {
                throw;
            }
        }	

		public DataTable Consultar(string sql)
		{			
			DataSet ds = new DataSet();
			ds = ExecutarConsulta(sql);
			
			if (ds.Tables.Count > 0) return ds.Tables[0]; else return null;
		}

		public DataTable Consultar(string sql, List<parametroConsulta> parametros)
		{			
			DataSet ds = new DataSet();
			ds = ExecutarConsulta(sql, parametros);
			
			if (ds.Tables.Count > 0) return ds.Tables[0]; else return null;
		}
		
		public DataTable ConsultarDT(string sql)
		{
			return Consultar(sql);
		}
	
	    public int Nextval(string sequence)
	    {		        
	        return Convert.ToInt32(this.ExecutarConsulta("SELECT nextval('" + sequence + "')").Tables[0].Rows[0]["nextval"]);	        
	    }
	}	       
}
