using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;

using Licitar.Business.Dao;

namespace Licitar.Business.Utilidade
{
	public class SrvActiveRecord
	{
		public static void CarregarEntidades()
		{
			List<Assembly> assemblies = new List<Assembly>();
			assemblies.Add(Assembly.Load("Licitar.Business"));
			ActiveRecordStarter.ResetInitializationFlag();
			ActiveRecordStarter.Initialize(assemblies.ToArray(), Conexao.GetConfigSource());
		}
	}
}