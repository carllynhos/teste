// TipoAcompanhamento.cs created with MonoDevelop
// User: guilhermefacanha at 16:26 2/7/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_tipo_acompanhamento_tac", Schema="adm_licitar")]
	public class TipoAcompanhamento:ActiveRecordBase<TipoAcompanhamento>
	{

		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_tipo_acompanhamento_tac", SequenceName="adm_licitar.sq_tipo_acompanhamento_tac")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_tac")]
		public virtual string Descricao
		{
			get;
			set;
		}

		[Property("boo_visivel_drop_tac")]
		public virtual bool Visivel
		{
			get;
			set;
		}

		public override string ToString()
		{
			return this.Descricao;
		}

		public TipoAcompanhamento(int id)
		{
			this.Id = id;
		}
		
		public TipoAcompanhamento()
		{
		}
	}
}
