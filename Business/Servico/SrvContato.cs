// SrvContato.cs created with MonoDevelop
// User: guilhermefacanha at 15:31 7/4/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Data;
using System.Collections.Generic;
using Npgsql;
using NHibernate;
using NHibernate.Expression;
using Castle.ActiveRecord.Queries;
using Licitar.Business.Dao;
using Licitar.Business.Entidade;

namespace Licitar.Business.Servico
{
	
	
	public class SrvContato
	{

		public Contato[] listarContatosByIdPessoaJuridica(int idPessoaJuridica)
		{
			DetachedCriteria pesq = DetachedCriteria.For(typeof(Contato));
			pesq.Add(Expression.Eq("PessoaJuridica.Id", idPessoaJuridica));

			return Contato.FindAll(pesq);
		}
		
		public SrvContato()
		{
		}
	}
}
