using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Npgsql;

namespace Licitar.Business.Utilidade
{
	public class Conexao
	{			
		public static InPlaceConfigurationSource GetConfigSource()
		{		
			Hashtable properties = new Hashtable();			
			properties.Add("hibernate.connection.driver_class", "NHibernate.Driver.NpgsqlDriver");
			properties.Add("hibernate.dialect", "NHibernate.Dialect.PostgreSQLDialect");
			properties.Add("hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
			properties.Add("hibernate.connection.connection_string", ConfigurationManager.ConnectionStrings[GetServidor()].ConnectionString);
			InPlaceConfigurationSource source = new InPlaceConfigurationSource();			
			source.Add(typeof(ActiveRecordBase), properties);
			
			return source;
		}		

		public static string GetServidor()			
		{
			string servidor = "";
			if (HttpContext.Current != null)
			{
				servidor = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString();
			}
			else
			{
				servidor = "127.0.0.1"	;
			}
			return servidor;
		}
	}
}
