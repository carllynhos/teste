//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: TipoNumero.cs
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
	[ActiveRecord(Table="tb_tipo_numero_tnu", Schema="adm_licitar")]
	public class TipoNumero : ActiveRecordBase<TipoNumero>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_tipo_numero_tnu", SequenceName="adm_licitar.sq_tipo_numero_tnu")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_tnu")]
		public virtual string Descricao
		{
			get;set;
		}
		
		public TipoNumero()
		{
		}
		
		public TipoNumero(int id)
		{
			this.Id = id;
		}
		
		public override string ToString ()
		{
			return Descricao;
		}

		
		public virtual NumeroProcesso[] ListaNumerosProcesso()
		{
			return NumeroProcesso.FindAll(Expression.Eq("TipoNumero.Id",this.Id));
		}

	}
}