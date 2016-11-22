
using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections;
using System.Collections.Generic;


namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_agrupamento_agr", Schema="adm_licitar")]
	
	public class Agrupamento: ActiveRecordBase<Agrupamento>
	{
		[PrimaryKey(PrimaryKeyType.Sequence,"pk_cod_agrupamento_agr", SequenceName="adm_licitar.sq_agrupamento_agr")]
		public virtual int Id 
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_processo_pro")]
		public virtual Processo Processo {
			get;
			set;
		}
		
		[BelongsTo("fk_cod_tipo_agrupamento_tia")]
		public virtual TipoAgrupamento TipoAgrupamento {
			get;
			set;
		}
		
		[Property("numero_grupolote")]
		public virtual int NumeroGrupoLote {
			get;
			set;
		}
		
		[Property("valor")]
		public virtual decimal Valor {
			get;
			set;
		}
		
		[HasMany]
		public virtual IList<Item> Itens {
			get;
			set;
		}
		
		public Agrupamento(){
			
		}
		
		public Agrupamento(int id)
		{			
			this.Id = id;
		}

	}
}