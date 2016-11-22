//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: TipoValor.cs
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
	[ActiveRecord(Table="tb_tipo_valor_tva", Schema="adm_licitar")]
	public class TipoValor : ActiveRecordBase<TipoValor>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_tipo_valor_tva", SequenceName="adm_licitar.sq_tipo_valor_tva")]
		public virtual int Id
		{
			get;
			set;
		}	
		
		[Property("txt_descricao_tva")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		[Property(" num_ordem_tva")]
		public virtual int Ordem
		{
			get;
			set;
		}
		
		public TipoValor()
		{
		}
		
		public TipoValor(int id)
		{
			this.Id = id;
		}
		
		public override string ToString ()
		{
			return Descricao;
		}
		
		public virtual ValorProcesso[] ListaValoresProcesso()
		{
			return ValorProcesso.FindAll(Expression.Eq("TipoValor",this.Id));
		}
	}
}