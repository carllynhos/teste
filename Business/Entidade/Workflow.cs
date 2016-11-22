// Workflow.cs created with MonoDevelop
// User: diogolima at 14:47 30/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_workflow_wor", Schema="adm_licitar")]
	public class Workflow : ActiveRecordBase<Workflow>
	{
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_workflow_wor", SequenceName="adm_licitar.sq_workflow_wor")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_area_are")]
		public virtual Area Area
		{
			get;
			set;
		}
		
		[Property("txt_descricao_wor")]
		public virtual string Descricao
		{
			get;
			set;
		}
				

		public Workflow()
		{
			Area = new Area();
		}
		
		public Workflow(int id)
		{
			this.Id = id;
		}
	}
}
