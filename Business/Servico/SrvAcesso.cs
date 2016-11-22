// Acesso.cs created with MonoDevelop
// User: bruno at 17:17 10/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using Licitar.Business.Entidade;
using Licitar.Business.Dto;
using NHibernate.Expression;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Classe para acesso ao sistema.
	/// </summary>
	public class SrvAcesso : PostgreSqlDatabase
	{		
		public bool VerificarAcesso(string cpf, string senha, int IdModulo)
		{			
			//Pessoa[] oPessoa = Pessoa.FindAll(Expression.Eq("CpfCnpj", cpf), Expression.Eq("Senha", senha));	
			Pessoa[] oPessoa = Pessoa.FindAll(Expression.Eq("CpfCnpj", cpf));		


			if (oPessoa.Length == 1)
			{	
				oPessoa[0].Modulo = Modulo.Find(IdModulo);
				oPessoa[0].Anonimo = false;
				HttpContext.Current.Session["UsrLogado"] = oPessoa[0];
				HttpContext.Current.Session["listaAtividadesUsuario"] = oPessoa[0].ListarTodasAtividades();
				HttpContext.Current.Session["Log"] = true; 
				oPessoa[0].DataHoraUltimoLogin = DateTime.Now;
				oPessoa[0].Update();
				return true;
			}
			else if(bool.Parse(ConfigurationManager.AppSettings["AcessoAnonimo"].ToString()))
			{
				Pessoa anonimo = Pessoa.Find(Convert.ToInt32(ConfigurationManager.AppSettings["idPessoa"].ToString()));
				anonimo.Anonimo = true;
				anonimo.Modulo = Modulo.Find(IdModulo);
				HttpContext.Current.Session["UsrLogado"] = anonimo;
				return true;
			}
					
			HttpContext.Current.Session.Abandon();
			return false;
		}	
		
		/*
		public bool VerificarAcessoAtividade(string nomeArquivo, Pessoa oPessoa)
		{
			Atividade[] atividades = oPessoa.ListarTodasAtividades();
			foreach (Atividade oAtividade in atividades)
			{
				
			}
		}
		*/
	}	
	
}
