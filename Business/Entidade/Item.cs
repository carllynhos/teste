
using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_item_ite", Schema="adm_licitar")]
	
	public class Item : ActiveRecordBase<Item>
	{
		[PrimaryKey(PrimaryKeyType.Sequence,"pk_cod_item_ite", SequenceName="adm_licitar.sq_item_ite")]
		public virtual int Id 
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_agrupamento_agr")]
		public virtual Agrupamento Agrupamento {
			get;
			set;
		}
		
		
		[Property("numero_item")]
		public virtual int NumeroItem{
			get;
			set;
		}
		
		[Property("codigo_item")]
		public virtual int CodigoItem{
			get;
			set;
		}
		
		[Property("descricao_item")]
		public virtual string DescricaoItem{
			get;
			set;
		}
				
		public Item(){
			
		}
		
		public Item(int id)
		{			
			this.Id = id;
		}

	}
}