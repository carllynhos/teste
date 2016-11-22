// TramitacaoUnidadeExercicio.cs created with MonoDevelop
// User: guilhermefacanha at 10:14 30/4/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//


using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	
	[ActiveRecord(Table="tb_tramitacao_unidade_exercicio_tue", Schema="adm_licitar")]
	public class TramitacaoUnidadeExercicio:ActiveRecordBase<TramitacaoUnidadeExercicio>
	{

		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_tramitacao_unidade_exercicio_tue", SequenceName="adm_licitar.sq_tramitacao_unidade_exercicio_tue")]
		public virtual int Id
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_unidade_exercicio_uex")]
		public virtual UnidadeExercicio UnidadeExercicio
		{
			get;set;
		}

		[BelongsTo("fk_cod_processo_pro")]
		public virtual Processo Processo
		{
			get;set;
		}

		[Property("dat_tramitacao_tue")]
		public virtual DateTime DataEntradaUnidade
		{
			get;
			set;
		}

		[Property("dat_encaminhado_setorial_tue")]
		public virtual DateTime? DataEncaminhadoSetorial
		{
			get;
			set;
		}

		[Property("dat_recebido_setorial_tue")]
		public virtual DateTime? DataRecebidoSetorial
		{
			get;
			set;
		}
		
		public TramitacaoUnidadeExercicio()
		{
		}
	}
}
