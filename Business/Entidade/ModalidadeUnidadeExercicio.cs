//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: ModalidadeUnidadeExercicio.cs
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
	[ActiveRecord(Table="tb_workflow_modalidade_unidade_exercicio_wmu", Schema="adm_licitar")]
	public class ModalidadeUnidadeExercicio : ActiveRecordBase<ModalidadeUnidadeExercicio>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_workflow_modalidade_unidade_exercicio_wmu", SequenceName="adm_licitar.sq_workflow_modalidade_unidade_exercicio_wmu")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[BelongsTo("fk_cod_modalidade_mod")]
		public virtual Modalidade Modalidade
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
		
		[BelongsTo("fk_cod_workflow_wor")]
		public virtual Workflow Workflow
		{
			get;
			set;
		}
		
		public ModalidadeUnidadeExercicio()
		{
			Modalidade = new Modalidade();
			Workflow = new Workflow();
		}
		
		public ModalidadeUnidadeExercicio(int id)
		{
			this.Id = id;
		}
		
		public virtual FluxoAndamento[] ListarFluxosAndamento()
		{
			return FluxoAndamento.FindAll(Expression.Eq("Workflow.Id", this.Id));
		}
	}
}