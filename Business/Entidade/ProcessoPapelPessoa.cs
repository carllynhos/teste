//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: PessoaProcesso.cs
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
	[ActiveRecord(Table="tb_processo_papel_pessoa_ppp", Schema="adm_licitar")]
	public class ProcessoPapelPessoa : ActiveRecordBase<ProcessoPapelPessoa>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_processo_papel_pessoa_ppp", SequenceName="adm_licitar.sq_processo_papel_pessoa_ppp")]
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
				
		[BelongsTo("fk_cod_papel_pap")]
		public virtual Papel Papel
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

		[Property("boo_principal_ppp")]
		public virtual bool PregoeiroPrincipal
		{
			get;set;
		}
		
		public ProcessoPapelPessoa()
		{
		}
		
		public ProcessoPapelPessoa(int id)
		{
			this.Id = id;
		}
		
		//Realização de Auditoria:////////////////////////////////////////////////////////////////////
		public override void Delete()
		{		
			Console.WriteLine("drogbaaaaaaraaaaaaaaaaaaaaaaa");
			try {
				new AuditoriaSistema().RealizarAuditoria(this.Id);
				base.Delete();
				Console.WriteLine("teste123349583958394859385");
			} catch (Exception ex) {
				Console.WriteLine("araaaaaaaaaaaaaaaaa");
				throw;
			}
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