//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: AtividadePessoa.cs
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
	[ActiveRecord(Table="tb_atividade_pessoa_permissao_app", Schema="adm_licitar")]
	public class AtividadePessoaPermissao : ActiveRecordBase<AtividadePessoaPermissao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_atividade_pessoa_permissao_app", SequenceName="adm_licitar.sq_atividade_pessoa_permissao_app")]
		public virtual int Id
		{
			get;
			set;
		}

        [BelongsTo("fk_cod_permissao_per")]
        public virtual Permissao Permissao
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

        [BelongsTo("fk_cod_pessoa_pes")]
        public virtual Pessoa Pessoa
        {
            get;
            set;
        }

        [Property("boo_acesso_permitido_ape")]
        public virtual bool AcessoPermitido
        {
            get;
            set;
        }
		
		public AtividadePessoaPermissao()
		{
		}
		
		public AtividadePessoaPermissao(int id)
		{
			this.Id = id;
		}
	}
}