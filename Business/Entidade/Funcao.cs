//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Funcao.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 08/11/2008
// DESCRICAO: 
// ALTERADO POR: 
// DATA DA ALTERACAO: 
// MOTIVO DA ALTERACAO:
// OBSERVACOES:
//----------------------------------------------------------------------------------------------------------------------------------------------

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_funcao_fun", Schema="adm_licitar")]
	public class Funcao : ActiveRecordBase<Funcao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_funcao_fun", SequenceName="adm_licitar.sq_funcao_fun")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_fun")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		
		public Funcao()
		{
		}
		
		public Funcao(int id)
		{
			this.Id = id;
		}
		
		/// <summary>
		/// Lista todas as atividades da funcao.
		/// </summary>
		/// <returns>
		/// A <see cref="AtividadeFuncao"/>
		/// </returns>
		public virtual AtividadeFuncaoPermissao[] ListarAtividades()
		{
			return AtividadeFuncaoPermissao.FindAll(Expression.Eq("Funcao.Id", this.Id));
		}
		
		/// <summary>
		/// Lista todas as atividades da funcao que devem aparecer no menu
		/// </summary>
		/// <returns>
		/// A <see cref="AtividadeFuncaoPermissao"/>
		/// </returns>
		public virtual AtividadeFuncaoPermissao[] ListarAtividadesMenu(int idModulo)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeFuncaoPermissao));
			dc.CreateAlias("Atividade", "ativ");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.Add(Expression.Eq("Funcao.Id", this.Id));
			dc.CreateAlias("ativ.Modulo", "Mod");
			dc.Add(Expression.Eq("Mod.Id", idModulo));
			return AtividadeFuncaoPermissao.FindAll(dc);
		}
		
		public virtual AtividadeFuncaoPermissao[] ListarAtividadesMenu()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadeFuncaoPermissao));
			dc.CreateAlias("Atividade", "ativ");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.Add(Expression.Eq("Funcao.Id", this.Id));
			dc.CreateAlias("ativ.Modulo", "Mod");
			
			return AtividadeFuncaoPermissao.FindAll(dc);
		}
		
		/// <summary>
		/// Lista todas os Cargos da funcao.
		/// </summary>
		/// <returns>
		/// A <see cref="CargoFuncao"/>
		/// </returns>
		public virtual UnidadeExercicioFuncao[] ListarUnidadeExercicioFuncao()
		{
			return UnidadeExercicioFuncao.FindAll(Expression.Eq("Funcao.Id", this.Id));
		}
		
		public override string ToString ()
		{
			return this.Descricao;
		}

	}
}