//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: AndamentoPossivel.cs
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
	[ActiveRecord(Table="tb_andamento_possivel_apo", Schema="adm_licitar")]
	public class AndamentoPossivel : ActiveRecordBase<AndamentoPossivel>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_andamento_possivel_apo", SequenceName="adm_licitar.sq_andamento_possivel_apo")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_fluxo_andamento_dado_fla")]
		public virtual FluxoAndamento FluxoAndamentoDado
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_fluxo_andamento_proximo_fla")]
		public virtual FluxoAndamento FluxoAndamentoProximo
		{
			get;
			set;
		}
		
		public AndamentoPossivel()
		{
		}
		
		public AndamentoPossivel(int id)
		{
			this.Id = id;
		}
	}
}