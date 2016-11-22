// SrvAtividadeUnidadeExercicioPermissao.cs created with MonoDevelop
// User: guilhermefacanha at 10:45 25/3/2009
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
	
	
	public class SrvAtividadeUnidadeExercicioPermissao
	{

		public static AtividadeUnidadeExercicioPermissao[] listarPermissoes(string ue, string permissao)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeUnidadeExercicioPermissao));
			dc.AddOrder(Order.Asc("UnidadeExercicio"));
			dc.AddOrder(Order.Asc("Atividade"));

			if(!string.IsNullOrEmpty(ue))
				dc.Add(Expression.Eq("UnidadeExercicio.Id",Convert.ToInt32(ue)));
			if(!string.IsNullOrEmpty(permissao))
				dc.Add(Expression.Eq("Permissao.Id",Convert.ToInt32(permissao)));

			return AtividadeUnidadeExercicioPermissao.FindAll(dc);
			
		}
		public AtividadeUnidadeExercicioPermissao[] listarAtividadesSubUnidades(List<int> lista)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeUnidadeExercicioPermissao));
			dc.Add(Expression.In("UnidadeExercicio.Id",lista));

			return AtividadeUnidadeExercicioPermissao.FindAll(dc);
		}

		public virtual AtividadeUnidadeExercicioPermissao[] ListarAtividadesMenu(int idModulo, List<string> listaUnidades)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeUnidadeExercicioPermissao));
			dc.CreateAlias("Atividade", "ativ");
			dc.CreateAlias("UnidadeExercicio", "uni");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.Add(Expression.In("uni.Descricao", listaUnidades));
			dc.CreateAlias("ativ.Modulo", "Mod");
			dc.Add(Expression.Eq("Mod.Id", idModulo));
			return AtividadeUnidadeExercicioPermissao.FindAll(dc);
		}

		public virtual AtividadeUnidadeExercicioPermissao[] ListarAtividadesMenu(List<string> listaUnidades)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeUnidadeExercicioPermissao));
			dc.CreateAlias("Atividade", "ativ");
			dc.CreateAlias("UnidadeExercicio", "uni");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.Add(Expression.In("uni.Descricao", listaUnidades));
			dc.CreateAlias("ativ.Modulo", "Mod");
			
			return AtividadeUnidadeExercicioPermissao.FindAll(dc);
		}
		
		public SrvAtividadeUnidadeExercicioPermissao()
		{
		}
	}
}
