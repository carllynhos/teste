//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: FluxoAndamento.cs
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
	[ActiveRecord(Table="tb_fluxo_andamento_fan", Schema="adm_licitar")]
	public class FluxoAndamento : ActiveRecordBase<FluxoAndamento>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_fluxo_andamento_fan", SequenceName="adm_licitar.sq_fluxo_andamento_fan")]
		public virtual int Id
		{
			get;
			set;
		}	
		
		[BelongsTo("fk_cod_atividade_ati")]
		public virtual Atividade Atividade
		{
			get;
			set;
		}
		
		/*[BelongsTo("fk_cod_modalidade_unidade_exercicio_mue")]
		public virtual ModalidadeUnidadeExercicio ModalidadeUnidadeExercicio
		{
			get;
			set;
		}*/
		
		[BelongsTo("fk_cod_workflow_wor")]
		public virtual Workflow Workflow
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_fase_fas")]
		public virtual Fase Fase
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_situacao_sit")]
		public virtual Situacao Situacao
		{
			get;
			set;
		}
		
		[Property("boo_informacao_obrigatoria_fan")]
		public virtual bool InformacaoObrigatoria
		{
			get;
			set;
		}
		
	    [Property("boo_ativo_fan")]
		public virtual bool CombinacaoAtiva
		{
			get;
			set;
		}
		
		
		[Property("txt_motivo_desativado_fan")]
		public virtual string MotivoDesativado
		{
			get;
			set;
		}
		
		
		public FluxoAndamento()
		{
		}
		
		public FluxoAndamento(int id)
		{
			this.Id = id;
		}
		
		public virtual ProcessoAndamento[] ListarProcessosAndamento()
		{
			return ProcessoAndamento.FindAll(Expression.Eq("FluxoAndamento.Id", this.Id));
		}
		
		public virtual FraseAndamento[] ListarFrasesAndamento()
		{
			return FraseAndamento.FindAll(Expression.Eq("FluxoAndamento.Id", this.Id));
		}

		public string TipoAndamento
		{
			get
			{
				return this.Atividade.Descricao;	
			}
		}
	}
}