// UnidadeAdministrativa.cs created with MonoDevelop
// User: guilhermefacanha at 10:12 13/5/2009
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
	[ActiveRecord(Table="tb_unidade_administrativa_uad", Schema="adm_licitar")]
	public class UnidadeAdministrativa: ActiveRecordBase<UnidadeAdministrativa>
	{
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_unidade_administrativa_uad", SequenceName="adm_licitar.sq_unidade_administrativa_uad")]
		public virtual int Id
		{
			get;
			set;
		}	

		[BelongsTo("fk_cod_instituicao_ins")]
		public virtual Instituicao Instituicao
		{get;set;}

		[BelongsTo("fk_cod_tipo_unidade_administrativa_tua")]
		public virtual TipoUnidadeAdministrativa TipoUnidadeAdministrativa
		{get;set;}
		
		[Property("txt_cnpj_uad")]
		public virtual string Cnpj
		{
			get;
			set;
		}
		
		
		
		public UnidadeAdministrativa()
		{
		}
	}
}
