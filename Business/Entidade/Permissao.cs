//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Permissao.cs
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
	[ActiveRecord(Table="tb_permissao_per", Schema="adm_licitar")]
	public class Permissao : ActiveRecordBase<Permissao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_permissao_per", SequenceName="adm_licitar.sq_permissao_per")]
		public virtual int Id
		{
			get;
			set;
		}

        [Property("txt_descricao_per")]
        public virtual string Descricao
        {
            get;
            set;
        }

        [Property("txt_sigla_per")]
        public virtual string Sigla
        {
            get;
            set;
        }
		
		public Permissao()  
		{
		}
		
		public Permissao(int id)
		{
			this.Id = id;
		}
		
		public override string ToString ()
		{
			return Descricao;
		}

        /// <summary>
        /// Lista todas as AtividadePessoa com esta permissao.
        /// </summary>
        /// <returns></returns>
        public virtual AtividadePessoaPermissao[] ListarAtividadesPessoa()
        {
            return AtividadePessoaPermissao.FindAll(Expression.Eq("Permissao.Id", this.Id));
        }

        /// <summary>
        /// Lista todas as AtividadeFuncao com esta permissao.
        /// </summary>
        /// <returns></returns>
        public virtual AtividadeFuncaoPermissao[] ListarAtividadesFuncao()
        {
            return AtividadeFuncaoPermissao.FindAll(Expression.Eq("Permissao.Id", this.Id));
        }
		
		public static Permissao[] listarPermissoes()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Permissao));
			dc.AddOrder(Order.Asc("Descricao"));
			return FindAll(dc);
		}
	}
}