using System;
using System.Configuration;
using Npgsql;

using Licitar.Business.Utilidade;

namespace Licitar.Business.Persistencia
{
	public class PostgreSqlConnection
	{    
		public NpgsqlConnection CriarConexao()
		{	    
			NpgsqlConnection npgsql = null;
			if ( ConfigurationManager.ConnectionStrings != null
			    && ConfigurationManager.ConnectionStrings[Conexao.GetServidor()] != null)
			{
				npgsql = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[Conexao.GetServidor()].ToString());
			}
			
			return npgsql; 
		}	
	}
}