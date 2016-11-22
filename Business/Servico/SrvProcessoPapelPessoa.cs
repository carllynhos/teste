// SrvProcessoPapelPessoa.cs created with MonoDevelop
// User: guilhermefacanha at 11:14 11/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using NHibernate.Expression;
using Npgsql;
using Licitar.Business.Entidade;
namespace Licitar.Business.Servico
{
	public class SrvProcessoPapelPessoa
	{
		public static bool isProcessoPapelPessoaCadastrado(int processo, int papel, int pessoa)
		{
			DetachedCriteria dcPesqExistePPP = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dcPesqExistePPP.Add(Expression.Eq("Pessoa.Id",pessoa));
			dcPesqExistePPP.Add(Expression.Eq("Papel.Id",papel));
			dcPesqExistePPP.Add(Expression.Eq("Processo.Id",processo));
			
			ProcessoPapelPessoa objPPP = ProcessoPapelPessoa.FindFirst(dcPesqExistePPP);
			
			if(objPPP==null)
			{
				Console.WriteLine("EH NULO");
				return false;
			}
			else
			{
				Console.WriteLine("NAO EH NULO: "+objPPP.Id);
				return true;
			}
		}

		public ProcessoPapelPessoa existePPP(int papel, int pessoa, int processo)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dc.Add(Expression.Eq("Pessoa.Id",pessoa));
			dc.Add(Expression.Eq("Papel.Id",papel));
			dc.Add(Expression.Eq("Processo.Id",processo));

			return ProcessoPapelPessoa.FindFirst(dc);
		}

		public ProcessoPapelPessoa getPPPPregoeiroPrincipal(int processo)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dc.CreateAlias("Papel","pap");
			dc.Add(Expression.Eq("pap.Descricao","PREGOEIRO"));
			dc.Add(Expression.Eq("Processo.Id",processo));
			dc.Add(Expression.Eq("PregoeiroPrincipal",true));

			return ProcessoPapelPessoa.FindFirst(dc);
		}

		public ProcessoPapelPessoa getPPPPessoaProcessoPapelVencedor(string vencedor, int idPai)
		{
			DetachedCriteria pesqPPP = DetachedCriteria.For(typeof(ProcessoPapelPessoa),"ppp");
			pesqPPP.Add(Expression.Eq("Pessoa.Id",int.Parse(vencedor)))
			.Add(Expression.Eq("Papel.Id",Papel.FindOne(Expression.Eq("Descricao","VENCEDOR")).Id))
			.Add(Expression.Eq("Processo.Id",idPai));

			return ProcessoPapelPessoa.FindFirst(pesqPPP);
		}

		public static bool existeVencedor(int processo)
		{
			Console.WriteLine("testandoooo");
			 DetachedCriteria pesqVencedor = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
            pesqVencedor.Add(Expression.Eq("Processo.Id", processo));
            pesqVencedor.CreateAlias("Papel", "pap").Add(Expression.Eq("pap.Descricao", "VENCEDOR"));
            if (ProcessoPapelPessoa.Exists(pesqVencedor))
            {
				Console.WriteLine("true");
				return true;
			}
			else
			{
				Console.WriteLine("false");
				return false;
			}

		}

		public static ProcessoPapelPessoa getProcessoPapelPessoaPorProcessoPapelAnalista(Processo objProcesso)
		{
			DetachedCriteria dcPessoa = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dcPessoa.CreateAlias("Papel","pap");
			dcPessoa.Add(Expression.Eq("Processo",objProcesso));
			dcPessoa.Add(Expression.InsensitiveLike("pap.Descricao","ANALISTA DE PROCESSO"));
			
			return ProcessoPapelPessoa.FindFirst(dcPessoa);
		}

		public ProcessoPapelPessoa[] getPPPByProcessoPapelAnalista(Processo objProcesso)
		{
			DetachedCriteria dcPessoa = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dcPessoa.CreateAlias("Papel","pap");
			dcPessoa.Add(Expression.Eq("Processo",objProcesso));
			dcPessoa.Add(Expression.InsensitiveLike("pap.Descricao","ANALISTA DE PROCESSO"));
			
			return ProcessoPapelPessoa.FindAll(dcPessoa);
		}

		public ProcessoPapelPessoa getPPPporPessoaProcesso(int idPessoa, int idProcesso)
		{
			DetachedCriteria pesqPPP1 = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
						pesqPPP1.Add(Expression.Eq("Pessoa.Id",idPessoa))
					.Add(Expression.Eq("Processo.Id",idProcesso));
			return ProcessoPapelPessoa.FindFirst(pesqPPP1);
		}

		public ProcessoPapelPessoa[] listarPessoasProcesso(int idProcesso)
		{
			DetachedCriteria pesquisaPessoas = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			pesquisaPessoas.Add(Expression.Eq("Processo.Id", idProcesso));

			return ProcessoPapelPessoa.FindAll(pesquisaPessoas);
		}

		public ProcessoPapelPessoa[] listarPessoasPorProcessoPapelPrincipal(int idProcesso, int idPapel)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dc.Add(Expression.Eq("Processo.Id",idProcesso));
			dc.Add(Expression.Eq("Papel.Id",idPapel));

			return ProcessoPapelPessoa.FindAll(dc);
		}
		
		public SrvProcessoPapelPessoa()
		{
		}
	}
}
