//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: FraseAndamento.cs
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
	[ActiveRecord(Table="tb_frase_fluxo_andamento_ffa", Schema="adm_licitar")]
	public class FraseAndamento : ActiveRecordBase<FraseAndamento>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_frase_fluxo_andamento_ffa", SequenceName="adm_licitar.sq_frase_fluxo_andamento_ffa")]
		public virtual int Id
		{
			get;
			set;
		}	
		
		[BelongsTo("fk_cod_fluxo_andamento_fan")]
		public virtual FluxoAndamento FluxoAndamento
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_frase_fra")]
		public virtual Frase Frase
		{
			get;
			set;
		}
		
		public FraseAndamento()
		{
		}
		
		public FraseAndamento(int id)
		{
			this.Id = id;
		}
		
		public virtual FluxoAndamento[] ListarFluxosAndamento()
		{
			return FluxoAndamento.FindAll(Expression.Eq("FraseAndamento.Id", this.Id));
		}		
	}
}