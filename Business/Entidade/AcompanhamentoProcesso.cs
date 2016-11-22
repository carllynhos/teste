// AcompanhamentoProcesso.cs created with MonoDevelop
// User: guilhermefacanha at 16:26 2/7/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using Licitar.Business.Entidade;
using System.Collections.Generic;


namespace Licitar.Business.Entidade
{	
	[Serializable]
	[ActiveRecord(Table="tb_acompanhamento_processo_apr", Schema="adm_licitar")]
	public class AcompanhamentoProcesso:ActiveRecordBase<AcompanhamentoProcesso>
	{

		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_acompanhamento_processo_apr", SequenceName="adm_licitar.sq_acompanhamento_processo_apr")]
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

		[BelongsTo("fk_cod_tipo_acompanhamento_tac")]
		public virtual TipoAcompanhamento TipoAcompanhamento
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_acompanhamento_processo_apr")]
		public virtual AcompanhamentoProcesso FkAcompanhamentoProcesso
		{
			get;
			set;
		}		

		[Property("dat_inicio_apr")]
		public virtual DateTime dataInicial
		{
			get;
			set;
		}
		
		[Property("num_dias_vencimento_apr")]
		public virtual int diasVencimento
		{
			get;
			set;
		}

		public AcompanhamentoProcesso()
		{
		}
	}

	
}
