// SrvAtividadePessoaPermissao.cs created with MonoDevelop
// User: guilhermefacanha at 16:55 16/3/2009
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

namespace Licitar.Business.Servico
{
	
	
	public class SrvAtividadePessoaPermissao
	{
		public struct camposAtivPermissao
		{
			public int pessoa;
			public int atividade;
			public int permissao;
			public bool acesso;

			public camposAtivPermissao(int a)
			{
				pessoa = a;
				atividade = a;
				permissao = a;
				acesso = false;
			}
		}

		public static void salvarAtividadePessoaPermissao(camposAtivPermissao obj)
		{
			if(obj.pessoa != 0 && obj.atividade != 0 && obj.permissao != 0)
			{
				AtividadePessoaPermissao objAPP = new AtividadePessoaPermissao();
				objAPP.Pessoa = new Pessoa();
				objAPP.Pessoa = Pessoa.Find(obj.pessoa);
				objAPP.Atividade = new Atividade();
				objAPP.Atividade = Atividade.Find(obj.atividade);
				objAPP.Permissao = new Permissao();
				objAPP.Permissao = Permissao.Find(obj.permissao);
				objAPP.AcessoPermitido = obj.acesso;

				objAPP.Save();
			}		

		}

		public AtividadePessoaPermissao[] listarAtividadesPessoa(int pessoa)
		{
			DetachedCriteria dcPermissaoPessoa = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dcPermissaoPessoa.CreateAlias("Atividade","at");
			dcPermissaoPessoa.Add(Expression.Eq("Pessoa.Id",pessoa));
			dcPermissaoPessoa.AddOrder(Order.Asc("at.Descricao"));

			return AtividadePessoaPermissao.FindAll(dcPermissaoPessoa);
		}

		public AtividadePessoaPermissao[] listarPessoasAtividade(int atividade)
		{
			DetachedCriteria dcPermissaoPessoa = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dcPermissaoPessoa.CreateAlias("Atividade","at");
			dcPermissaoPessoa.Add(Expression.Eq("Atividade.Id",atividade));
			dcPermissaoPessoa.AddOrder(Order.Asc("at.Descricao"));

			return AtividadePessoaPermissao.FindAll(dcPermissaoPessoa);
		}
		
		public SrvAtividadePessoaPermissao()
		{
		}
	}
}
