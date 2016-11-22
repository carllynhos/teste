//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: ValorProcesso.cs
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
	[ActiveRecord(Table="tb_valor_processo_vpr", Schema="adm_licitar")]
	public class ValorProcesso : ActiveRecordBase<ValorProcesso>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_valor_processo_vpr", SequenceName="adm_licitar.sq_valor_processo_vpr")]
		public virtual int Id
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
		
		[BelongsTo("fk_cod_fonte_valor_fva")]
		public virtual FonteValor FonteValor
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_tipo_valor_tva")]
		public virtual TipoValor TipoValor
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_moeda_moe")]
		public virtual Moeda Moeda
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_pessoa_pes")]
		public virtual Pessoa Pessoa
		{
			get;
			set;
		}
		
		[Property("vlr_processo_vpr")]
		public virtual double Valor
		{
			get;
			set;
		}
		
		[Property("dat_cadastro_vpr")]
		public virtual DateTime DataCadastro
		{
			get;
			set;
		}
		
		[Property("txt_descricao_vpr")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		public ValorProcesso()
		{
		}
		
		public ValorProcesso(int id)
		{
			this.Id = id;
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