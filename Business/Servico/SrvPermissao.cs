// SrvPermissao.cs created with MonoDevelop
// User: diogolima at 19:01 15/1/2009
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
	public class SrvPermissao
	{	
		public static bool VerificarPermissao(int idPessoa, int idFuncao, string descricaoPermissao, string pagina)
		{
			Atividade oAtividade = Atividade.FindFirst(Expression.InsensitiveLike("Url","%" + pagina + "%"));

			//Primeiro verifica se a função tem a permissão desejada
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeFuncaoPermissao));
			dc.Add(Expression.Eq("Funcao.Id", idFuncao));
			dc.Add(Expression.Eq("Atividade.Id", oAtividade.Id));
			dc.CreateAlias("Permissao","per");
			dc.Add(Expression.Eq("per.Descricao", descricaoPermissao));
			

			if (AtividadeFuncaoPermissao.Exists(dc))
			{
				return true;
			}
			else
			{

				//Se a função não tem permissão, verifica se o usuário tem.
				DetachedCriteria dc2 = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
				dc2.Add(Expression.Eq("Pessoa.Id", idPessoa));
				dc2.Add(Expression.Eq("Atividade.Id", oAtividade.Id));
				dc2.CreateAlias("Permissao","per");
				dc2.Add(Expression.Eq("per.Descricao", descricaoPermissao));
				dc2.Add(Expression.Eq("AcessoPermitido", true));
				
				if (AtividadePessoaPermissao.Exists(dc2))
				{
					return true;
				}
			}
			
			return false;			
		}

		public bool VerificarPermissaoPorSubUnidade(int idPessoa, List<int> SubUnidade, string descricaoPermissao, string pagina)
		{
			Atividade oAtividade = Atividade.FindFirst(Expression.InsensitiveLike("Url","%" + pagina + "%"));
			
			//Primeiro verifica se a sub-unidade de exercicio tem a permissão desejada
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeUnidadeExercicioPermissao));
			dc.Add(Expression.In("UnidadeExercicio.Id", SubUnidade));
			dc.Add(Expression.Eq("Atividade.Id", oAtividade.Id));
			dc.CreateAlias("Permissao","per");
			dc.Add(Expression.Eq("per.Descricao", descricaoPermissao));
			
			if (AtividadeFuncaoPermissao.Exists(dc))
			{
				return true;
			}
			else
			{
				//Se a sub-unidade não tem permissão, verifica se o usuário tem.
				DetachedCriteria dc2 = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
				dc2.Add(Expression.Eq("Pessoa.Id", idPessoa));
				dc2.Add(Expression.Eq("Atividade.Id", oAtividade.Id));
				dc2.CreateAlias("Permissao","per");
				dc2.Add(Expression.Eq("per.Descricao", descricaoPermissao));
				dc2.Add(Expression.Eq("AcessoPermitido", true));
				
				if (AtividadePessoaPermissao.Exists(dc2))
				{
					return true;
				}
			}
						
			return false;			
		}
		
		public static bool VerificarPermissaoPorFuncao(string descricaoPermissao, int idFuncao, int idAtividade)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeFuncaoPermissao));
			dc.Add(Expression.Eq("Funcao.Id", idFuncao));
			dc.Add(Expression.Eq("Atividade.Id", idAtividade));
			dc.Add(Expression.InsensitiveLike("Permissao.Descricao", descricaoPermissao));
			if (AtividadeFuncaoPermissao.FindAll().Length > 0)
				return true;
			else
				return false;
		}
		
		public static bool VerificarPermissaoPorPessoa(int idPessoa, int idAtividade, string descricaoPermissao)
		{			
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dc.Add(Expression.Eq("AcessoPermitido", true));
			dc.Add(Expression.Eq("Atividade.Id", idAtividade));
			dc.Add(Expression.Eq("Pessoa.Id", idPessoa));
			dc.CreateAlias("Permissao", "PER");
			dc.Add(Expression.Eq("PER.Descricao", descricaoPermissao));
			if (AtividadePessoaPermissao.FindAll(dc).Length < 1)
				return false;
			
			return true;
		}
				
		public static bool VerificarPermissao(int idPessoa, string descricaoPermissao, string urlAtividade)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dc.CreateAlias("Permissao","per");
			dc.CreateAlias("Atividade","ati");
			dc.Add(Expression.Eq("Pessoa.Id",idPessoa));
			dc.Add(Expression.InsensitiveLike("ati.Url","%" +urlAtividade+"%"));
			dc.Add(Expression.Eq("per.Descricao",descricaoPermissao));
			
			if (AtividadePessoaPermissao.Exists(dc))
			{
				Console.WriteLine("Tem permisao de: " + descricaoPermissao);
				return true;
			}
			else
			{
				Console.WriteLine("NAO Tem permisao de: " + descricaoPermissao);
				return false;
			}
		
		}
	}
}
