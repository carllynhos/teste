//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: FaseUnidadeExercicio.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 31/03/2009
// DESCRICAO: 
// ALTERADO POR: 
// DATA DA ALTERACAO: 
// MOTIVO DA ALTERACAO:
// OBSERVACOES:
//----------------------------------------------------------------------------------------------------------------------------------------------

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using Licitar.Business.Dto;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_fase_unidade_exercicio_fue", Schema="adm_licitar")]
	public class FaseUnidadeExercicio : ActiveRecordBase<FaseUnidadeExercicio>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_fase_unidade_exercicio_fue", SequenceName="adm_licitar.sq_fase_unidade_exercicio_fue")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[BelongsTo("fk_cod_fase_fas")]
		public virtual Fase Fase
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
		
		[Property("dat_inicio_fue")]
		public virtual DateTime DataInicio
		{
			get;
			set;
		}

		[Property("dat_fim_fue")]
		public virtual DateTime DataFim
		{
			get;
			set;
		}		
		
		public FaseUnidadeExercicio()
		{
		}
		
		public FaseUnidadeExercicio(int id)
		{
			this.Id = id;
		}		
	}
}