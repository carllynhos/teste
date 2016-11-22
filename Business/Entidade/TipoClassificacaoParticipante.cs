
using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections;
using System.Collections.Generic;


namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_tipo_classificacao_fornecedor_tcf", Schema="adm_licitar")]
	
	public class TipoClassificacaoParticipante : ActiveRecordBase<TipoClassificacaoParticipante>
	{
		[PrimaryKey(PrimaryKeyType.Sequence,"pk_cod_tipo_classificacao_fornecedor_tcf", SequenceName="adm_licitar.sq_tipo_classificacao_fornecedor_tcf")]
		public virtual int Id 
		{
			get;
			set;
		}
		
		[Property("txt_descricao_tipo_classificacao_fornecedor")]
		public virtual string Descricao {
			get;
			set;
		}
		
		public TipoClassificacaoParticipante(){
			
		}
		
		public TipoClassificacaoParticipante(int id)
		{			
			this.Id = id;
		}
	}
}