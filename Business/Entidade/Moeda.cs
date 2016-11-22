//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Moeda.cs
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
	[ActiveRecord(Table="tb_moeda_moe", Schema="adm_licitar")]
	public class Moeda : ActiveRecordBase<Moeda>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_moeda_moe", SequenceName="adm_licitar.sq_moeda_moe")]
		public virtual int Id
		{
			get;
			set;
		}	
		
	    [Property("txt_descricao_moe")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		public Moeda()
		{
		}
		
		public Moeda(int id)
		{
			this.Id = id;
		}
		
		public virtual ValorProcesso[] ListaValoresProcesso()			
		{
			return ValorProcesso.FindAll(Expression.Eq("Moeda.Id",this.Id));
		}
		
		public override string ToString ()
		{
			return Descricao;
		}
	}
}