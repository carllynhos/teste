// PapelPessoa.cs created with MonoDevelop
// User: guilhermefacanha at 13:32 30/6/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	
	[Serializable]
	[ActiveRecord(Table="tb_papel_pessoa_ppe", Schema="adm_licitar")]
	public class PapelPessoa:ActiveRecordBase<PapelPessoa>
	{
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_papel_pessoa_ppe", SequenceName="adm_licitar.sq_papel_pessoa_ppe")]
		public virtual int Id
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_papel_pap")]
		public virtual Papel Papel
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_pessoa_pes")]
		public virtual Pessoa Pessoa
		{
			get;
			set;
		}

		[Property("dat_inicio_ppe")]
		public virtual DateTime DataInicio
		{
			get;set;
		}

		[Property("dat_fim_ppe")]
		public virtual DateTime DataFim
		{
			get;set;
		}
		
		public PapelPessoa()
		{
		}
		public PapelPessoa(int id)
		{
			this.Id = id;
		}
	}
}
