// ValorReplica.cs created with MonoDevelop
// User: diogolima at 18:45 17/2/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	
	[Serializable]
	[ActiveRecord(Table="tb_valor_replica_vre", Schema="adm_licitar")]
	public class ValorReplica : ActiveRecordBase<ValorReplica>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_valor_replica_vre", SequenceName="adm_licitar.sq_valor_replica_vre")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("pk_vencedor_vre")]
		public virtual int IdVencedor
		{
			get;
			set;
		}
		
		[Property("txt_descricao_vencedor_vre")]
		public virtual string Vencedor
		{
			get;
			set;
		}
		
		[Property("num_estimado_vre")]
		public virtual decimal ValorEstimado
		{
			get;
			set;
		}
		
		[Property("num_contratado_vre")]
		public virtual decimal ValorContratado
		{
			get;
			set;
		}
		
		[Property("num_fracassado_vre")]
		public virtual decimal ValorFracassado
		{
			get;
			set;
		}
		
		[Property("num_deserto_vre")]
		public virtual decimal ValorDeserto
		{
			get;
			set;
		}
		
		[Property("num_revogado_vre")]
		public virtual decimal ValorRevogado
		{
			get;
			set;
		}
		
		[Property("num_anulado_vre")]
		public virtual decimal ValorAnulado
		{
			get;
			set;
		}
		
		[Property("num_cancelado_vre")]
		public virtual decimal ValorCancelado
		{
			get;
			set;
		}
		
		[Property("fk_cod_processo_completo_replica_pcr")]
		public virtual int IdProcessoReplica
		{
			get;
			set;
		}
		
		public ValorReplica()
		{
		}
	}
}
