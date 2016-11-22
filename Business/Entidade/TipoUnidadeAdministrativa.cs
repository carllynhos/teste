// TipoUnidadeAdministrativa.cs created with MonoDevelop
// User: guilhermefacanha at 12:08 13/5/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using Licitar.Business.Entidade;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_tipo_unidade_administrativa_tua", Schema="adm_licitar")]
	public class TipoUnidadeAdministrativa: ActiveRecordBase<TipoUnidadeAdministrativa>
	{

		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_tipo_unidade_administrativa_tua", SequenceName="adm_licitar.sq_tipo_unidade_administrativa_tua")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_descricao_tua")]
		public virtual string Descricao
		{
			get;
			set;
		}

		public override string ToString()
		{
			return this.Descricao;
		}


		
		public TipoUnidadeAdministrativa()
		{
		}
	}
}
