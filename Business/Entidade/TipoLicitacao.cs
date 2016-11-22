//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: TipoLicitacao.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 08/11/2008
// DESCRICAO: 
// ALTERADO POR: 
// DATA DA ALTERACAO: 
// MOTIVO DA ALTERACAO:
// OBSERVACOES:
//----------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_tipo_licitacao_tli", Schema="adm_licitar")]
	public class TipoLicitacao : ActiveRecordBase<TipoLicitacao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_tipo_licitacao_tli", SequenceName="adm_licitar.sq_tipo_licitacao_tli")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_tli")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		public TipoLicitacao()
		{
		}
		
		public TipoLicitacao(int id)
		{
			this.Id = id;
		}

		public override string ToString ()
		{
			return Descricao.ToString();
		}

		
		public virtual Processo[] ListarProcessos()
		{
			return Processo.FindAll(Expression.Eq("TipoLicitacao.Id", this.Id));
		}
		
		public virtual ProcessoPapelPessoa[] ListarPessoasProcesso()
        {
            return ProcessoPapelPessoa.FindAll(Expression.Eq("Pessoa.Id", this.Id));
        }
	}
}