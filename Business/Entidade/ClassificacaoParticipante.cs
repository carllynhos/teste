//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Atividade.cs
// CRIADO POR: Edivan de Castro 
// DATA DA CRIACAO: 21/06/2013
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
	[ActiveRecord(Table="tb_classificacao_participante_clp", Schema="adm_licitar")]
	public class ClassificacaoParticipante : ActiveRecordBase<ClassificacaoParticipante>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_classificacao_participante_clp", SequenceName="adm_licitar.sq_classificacao_participante_clp")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_agrupamento_agr")]
		public virtual Agrupamento Agrupamento
		{
			get;
			set;
		}
	
		[BelongsTo("fk_cod_pessoa_pes")]
		public virtual Pessoa Fornecedor
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_tipo_classificacao_fornecedor_tcf")]
		public virtual TipoClassificacaoParticipante TipoClassificacao
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
		
		[Property("txt_posicao_participante")]
		public virtual int PosicaoParticipante
		{
			get;
			set;
		}
		
		[Property("numero_valor")]
		public virtual decimal NumeroValor
		{
			get;
			set;
		}		
		
		public ClassificacaoParticipante()
		{
		}
		
		public ClassificacaoParticipante(int id)
		{
			this.Id = id;
		}
	}
}