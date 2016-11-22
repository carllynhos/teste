//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Cargo.cs
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
	[ActiveRecord(Table="tb_unidade_exercicio_funcao_uef", Schema="adm_licitar")]
	public class UnidadeExercicioFuncao : ActiveRecordBase<UnidadeExercicioFuncao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_unidade_exercicio_funcao_uef", SequenceName="adm_licitar.sq_unidade_exercicio_funcao_uef")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_descricao_cargo_uef")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_unidade_exercicio_uex")]
		public virtual UnidadeExercicio UnidadeExercicio
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_funcao_fun")]
		public virtual Funcao Funcao
		{
			get;
			set;
		}
				
		public UnidadeExercicioFuncao()
		{
		}
		
		public UnidadeExercicioFuncao(int id)
		{
			this.Id = id;
		}	
		
				/// <summary>
		/// Lista todas as pessoas da funcao.
		/// </summary>
		/// <returns>
		/// A <see cref="Pessoa"/>
		/// </returns>
		public virtual Pessoa[] ListarPessoas()
		{
			return Pessoa.FindAll(Expression.Eq("UnidadeExercicioFuncao.Id", this.Id));
		}

		public override string ToString()
		{
			return (this.UnidadeExercicio.Descricao+" / "+this.Funcao.Descricao +" / "+ this.Descricao);
		}

	}
}