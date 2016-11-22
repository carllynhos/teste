//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: ProcessoAndamento.cs
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

	public struct StructMigrarAndamentos
	{
		public int id;
		public int pessoa;
	}
	
	[Serializable]
	[ActiveRecord(Table="tb_processo_andamento_pan", Schema="adm_licitar")]
	public class ProcessoAndamento : ActiveRecordBase<ProcessoAndamento>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_processo_andamento_pan", SequenceName="adm_licitar.sq_processo_andamento_pan")]
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
		
		[BelongsTo("fk_cod_unidade_exercicio_uex")]
		public virtual UnidadeExercicio UnidadeExercicio
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
		
		[BelongsTo("fk_cod_pessoa_cadastrante_pes")]
		public virtual Pessoa Cadastrante
		{
			get;
			set;
		}
		
		
		[BelongsTo("fk_cod_processo_andamento_pan")]
		public virtual ProcessoAndamento AndamentoCorrigido
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_processo_andamento_remarcado_pan")]
		public virtual ProcessoAndamento AndamentoAdiado
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_unidade_exercicio_funcao_pessoa_efp")]
		public virtual UnidadeExercicioFuncaoPessoa UnidadeExercicioFuncaoPessoa
		{
			get;
			set;
		}		
			
		[BelongsTo("fk_cod_fluxo_andamento_fan")]
		public virtual FluxoAndamento FluxoAndamento
		{
			get;
			set;
		}
		
		[Property("txt_andamento_pan")] //observacao do processo
		public virtual string Andamento
		{
			get;
			set;
		}
		
		[Property("dat_andamento_pan")]
		public virtual DateTime DataAndamento
		{
			get;
			set;
		}

		[Property("dat_cadastro_pan")]
		public virtual DateTime DataCadastro
		{
			get;
			set; //Data Padrão diretamente no Banco. Data Atual.
		}
		
		[Property("dat_termino_pan")]
		public virtual DateTime DataTermino
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_motivo_andamento_pan")]
		public virtual MotivoAndamento MotivoAndamento
		{
			get;
			set;
		}

		[Property("boo_remarcado_pan")]
		public virtual bool Remarcado
		{
			get;
			set;
		}

		
//		[Property("cod_processo_andamento_pan_sisaf", Insert=false, Update=false)]
//		public virtual int IdAntigo
//		{
//			get;
//			set; //Controle da ligação com o SISAF e do Upload de arquivos. Automático no banco.
//		}
		
		public ProcessoAndamento()
		{
		}
		
		public ProcessoAndamento(int id)
		{
			this.Id = id;
		}
		
		#region Realização de Auditoria:
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
		#endregion
	}
}