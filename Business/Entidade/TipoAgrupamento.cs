
using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_tipo_agrupamento_tia", Schema="adm_licitar")]
	
	public class TipoAgrupamento : ActiveRecordBase<TipoAgrupamento>
	{
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_tipo_agrupamento_tia", SequenceName="adm_licitar.sq_tipo_agrupamento_tia")]	
		public virtual int Id
		{
			get;
			set;

		}
		[Property("descricao_tia")]
		public virtual string Descricao
		{
			get;
			set;
		}
	}
}
