//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Camera.cs
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
	[ActiveRecord(Table="tb_camera_cam", Schema="adm_licitar")]
	public class Camera : ActiveRecordBase<Camera>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_camera_cam", SequenceName="adm_licitar.sq_camera_cam")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[BelongsTo("fk_cod_auditorio_aud")]
		public virtual Auditorio Auditorio
		{
			get;
			set;
		}
		
		[Property("txt_descricao_cam")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		[Property("txt_url_cam")]
		public virtual string Url
		{
			get;
			set;
		}	
		
		public Camera()
		{
		}
		
		public Camera(int id)
		{
			this.Id = id;
		}	
	}
}