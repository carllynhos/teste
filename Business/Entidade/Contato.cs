//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Contato.cs
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
	[ActiveRecord(Table="tb_contato_con", Schema="adm_licitar")]
	public class Contato : ActiveRecordBase<Contato>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_contato_con", SequenceName="adm_licitar.sq_contato_con")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[BelongsTo("fk_cod_pessoa_juridica_pes")]
		public virtual Pessoa PessoaJuridica
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_pessoa_fisica_pes")]
		public virtual Pessoa PessoaFisica
		{
			get;
			set;
		}
		
		public override string ToString ()
		{
			return PessoaFisica.Nome;
		}

		
		public Contato()
		{
		}
		
		public Contato(int id)
		{
			this.Id = id;
		}
	}
}