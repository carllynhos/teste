// SrvClassificacao.cs created with MonoDevelop
// User: guilhermefacanha at 11:41 3/4/2009
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
	
	
	public class SrvClassificacao
	{
		public Classificacao[] listarClassificacoes(string tipoLicitacao, string modalidade, string natureza)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Classificacao));
			dc.AddOrder(Order.Asc("TipoLicitacao"));
			dc.AddOrder(Order.Asc("Modalidade"));
			dc.AddOrder(Order.Asc("Natureza"));

			if(!string.IsNullOrEmpty(tipoLicitacao))
			{
				dc.Add(Expression.Eq("TipoLicitacao.Id",Convert.ToInt32(tipoLicitacao)));
			}
			if(!string.IsNullOrEmpty(modalidade))
			{
				dc.Add(Expression.Eq("Modalidade.Id",Convert.ToInt32(modalidade)));
			}
			if(!string.IsNullOrEmpty(natureza))
			{
				dc.Add(Expression.Eq("Natureza.Id",Convert.ToInt32(natureza)));
			}

			return Classificacao.FindAll(dc);
		}

		public static Classificacao getClassificacao(int tipoLici, int mod, int nat)
		{
			DetachedCriteria dc2 = DetachedCriteria.For(typeof(Classificacao));
			dc2.CreateAlias("TipoLicitacao","til");
			dc2.CreateAlias("Natureza", "nat");
			dc2.CreateAlias("Modalidade","mod");
			dc2.Add(Expression.Eq("til.Id",tipoLici));
			dc2.Add(Expression.Eq("nat.Id",nat));
			dc2.Add(Expression.Eq("mod.Id",mod));

			return Classificacao.FindFirst(dc2);
		}

		public static Classificacao[] getClassificacoes(string modalidade, string natureza)
		{
			DetachedCriteria dc2 = DetachedCriteria.For(typeof(Classificacao));
			dc2.CreateAlias("Modalidade","mod");
			dc2.CreateAlias("Natureza","nat");
			dc2.Add(Expression.Eq("mod.Id",Convert.ToInt32(modalidade)));
			dc2.Add(Expression.Eq("nat.Id",Convert.ToInt32(natureza)));
			return Classificacao.FindAll(dc2);
		}
		
		public SrvClassificacao()
		{
		}
	}
}
