//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Frase.cs
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
	[ActiveRecord(Table="tb_frase_fra", Schema="adm_licitar")]
	public class Frase : ActiveRecordBase<Frase>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_frase_fra", SequenceName="adm_licitar.sq_frase_fra")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_fra")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		public Frase()
		{
		}
		
		public Frase(int id)
		{
			this.Id = id;
		}
		
		public virtual FraseAndamento[] ListarFrasesAndamento()
		{
			return FraseAndamento.FindAll(Expression.Eq("Frase.Id",this.Id));
		}
	}
}