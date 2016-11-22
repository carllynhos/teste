//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Atividade.cs
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
	[ActiveRecord(Table="tb_classificacao_cla", Schema="adm_licitar")]
	public class Classificacao : ActiveRecordBase<Classificacao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_classificacao_cla", SequenceName="adm_licitar.sq_classificacao_cla")]
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
	
		[BelongsTo("fk_cod_tipo_licitacao_tli")]
		public virtual TipoLicitacao TipoLicitacao
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_natureza_nat")]
		public virtual Natureza Natureza
		{
			get;
			set;
		}
		
		public Classificacao()
		{
			Natureza = new Natureza();
			Modalidade = new Modalidade();
			TipoLicitacao = new TipoLicitacao();
		}
		
		public Classificacao(int id)
		{
			this.Id = id;
		}
	}
}