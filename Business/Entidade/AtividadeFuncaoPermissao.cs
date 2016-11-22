//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: AtividadeFuncao.cs
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
	[ActiveRecord(Table="tb_atividade_funcao_permissao_afp", Schema="adm_licitar")]
	public class AtividadeFuncaoPermissao : ActiveRecordBase<AtividadeFuncaoPermissao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_atividade_funcao_permissao_afp", SequenceName="adm_licitar.sq_atividade_funcao_permissao_afp")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_funcao_fun")]
		public virtual Funcao Funcao
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
		
		[BelongsTo("fk_cod_permissao_per")]
		public virtual Permissao Permissao
		{
			get;
			set;
		}
		
		public AtividadeFuncaoPermissao()
		{
		}
		
		public AtividadeFuncaoPermissao(int id)
		{
			this.Id = id;
		}
	}
}