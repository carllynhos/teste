//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: FonteValor.cs
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
	[ActiveRecord(Table="tb_fonte_valor_fva", Schema="adm_licitar")]
	public class FonteValor : ActiveRecordBase<FonteValor>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_fonte_valor_fva", SequenceName="adm_licitar.sq_fonte_valor_fva")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_fva")]
		public virtual string Descricao
		{
			get;set;
		}
		
		public FonteValor()
		{
		}
		
		public override string ToString ()
		{
			return Descricao;
		}
		
		public FonteValor(int id)
		{
			this.Id = id;
		}
	}
}