// MotivoAndamento.cs created with MonoDevelop
// User: marcelolima at 16:53 26/6/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	
	[Serializable]
	[ActiveRecord(Table="tb_motivo_andamento_man", Schema="adm_licitar")]
	public class MotivoAndamento : ActiveRecordBase<MotivoAndamento>
	{
		
		public MotivoAndamento()
		{
			
		}

		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_motivo_andamento_man", SequenceName="adm_licitar.sq_motivo_andamento_man")]
		public virtual int Id
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_atividade_ati")]
		public virtual Atividade Atividade
		{
			get;
			set;
		}
		
	    [Property("txt_descricao_man")]
		public virtual string Descricao
		{
			get;
			set;
		}		
		
	}
}
