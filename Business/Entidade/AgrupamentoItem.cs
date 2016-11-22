
using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_agrupamento_item_agi", Schema="adm_licitar")]
	
	public class AgrupamentoItem : ActiveRecordBase<AgrupamentoItem>
	{
		[PrimaryKey(PrimaryKeyType.Sequence,"pk_cod_agrupamento_item_agi", SequenceName="adm_licitar.sq_agrupamento_item_agi")]
		public virtual int Id 
		{
			get;
			set;
		}
		[Property("fk_cod_processo_pro")]
		public virtual int ProcessoItem{
			get;
			set;
		}
		[Property("fk_cod_tipo_agrupamento_tia")]
		public virtual int TipoAgrupamentoItem {
			get;
			set;
		}
		[Property("codigo_item")]
		public virtual decimal CodigoItem{
			get;
			set;
		}
		[Property("descricao_item")]
		public virtual string DescricaoItem{
			get;
			set;
		}
		[Property("descricao_agrupamento")]
		public virtual string DescricaoAgrupamento{
			get;
			set;
		}
		[Property("codigo_agrupamento")]
		public virtual int CodigoAgrupamento{
			get;
			set;
		}
		
		public AgrupamentoItem(){
			
		}
		
		public AgrupamentoItem(int id)
		{			
			this.Id = id;
		}

	}
}