//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Natureza.cs
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
	[ActiveRecord(Table="tb_natureza_nat", Schema="adm_licitar")]
	public class Natureza : ActiveRecordBase<Natureza>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_natureza_nat", SequenceName="adm_licitar.sq_natureza_nat")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_nat")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		public Natureza()
		{
		}
		
		public Natureza(int id)
		{
			this.Id = id;
		}

		public override string ToString ()
		{
			return Descricao.ToString();
		}

		
		public virtual Processo[] ListarProcessos()
		{
			return Processo.FindAll(Expression.Eq("Natureza.Id", this.Id));
		}
	}
}