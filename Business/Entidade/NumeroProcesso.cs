//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: NumeroProcesso.cs
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
	[ActiveRecord(Table="tb_numero_processo_npr", Schema="adm_licitar")]
	public class NumeroProcesso : ActiveRecordBase<NumeroProcesso>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_numero_processo_npr", SequenceName="adm_licitar.sq_numero_processo_npr")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[BelongsTo("fk_cod_tipo_numero_tnu")]
		public virtual TipoNumero TipoNumero		
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_processo_pro")]
		public virtual Processo Processo
		{
			get;
			set;
		}
		
		[Property("dat_cadastro_npr")]
		public virtual DateTime DataCadastro
		{
			get;
			set;
		}
		
		[Property("txt_numero_processo_npr")]
		public virtual string numeroProcesso
		{
			get;
			set;
		}
		
		[Property("boo_principal_npr")]
		public virtual bool Principal
		{
			get;
			set;
		}		
		
		public NumeroProcesso()
		{
		}
		
		public NumeroProcesso(int id)
		{
			this.Id = id;
		}
		
		public virtual Processo[] ListaProcessos()
		{
			return Processo.FindAll(Expression.Eq("NumeroProcesso.Id",this.Id));
		}
		
		//Realização de Auditoria:////////////////////////////////////////////////////////////////////			
		public override void Delete()
		{			
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Delete();
		}
		
		public override void Save()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);		
			base.Save();
		}
		
		public override void Update()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Update();
		}
		
		public override void Create()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Create();
		}
		
		public override void DeleteAndFlush()
		{			
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.DeleteAndFlush();
		}
		
		public override void SaveAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.SaveAndFlush();
		}
		
		public override void UpdateAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.UpdateAndFlush();
		}
		
		public override void CreateAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.CreateAndFlush();
		}		
		//////////////////////////////////////////////////////////////////////////////////////////////
		 
	}
}