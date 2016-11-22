//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Auditorio.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 08/11/2008
// DESCRICAO: 
// ALTERADO POR: 
// DATA DA ALTERACAO: 
// MOTIVO DA ALTERACAO:
// OBSERVACOES: 
//----------------------------------------------------------------------------------------------------------------------------------------------

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_auditorio_aud", Schema="adm_licitar")]
	public class Auditorio : ActiveRecordBase<Auditorio>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_auditorio_aud", SequenceName="adm_licitar.sq_auditorio_aud")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_aud")]
		public virtual string Descricao
		{
			get;set;
		}
		
		public Auditorio()
		{
		}
		
		public Auditorio(int id)
		{
			this.Id = id;
		}
		
		public virtual Agenda[] ListaAgenda()
		{
			return Agenda.FindAll(Expression.Eq("Auditorio.Id",this.Id));
		}
		
		public virtual Camera[] ListaCameras()
		{
			return Camera.FindAll(Expression.Eq("Auditorio.Id",this.Id));
		}
	}
}