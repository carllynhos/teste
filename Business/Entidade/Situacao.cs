//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Situacao.cs
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
	[ActiveRecord(Table="tb_situacao_sit", Schema="adm_licitar")]
	public class Situacao : ActiveRecordBase<Situacao>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_situacao_sit", SequenceName="adm_licitar.sq_situacao_sit")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_sit")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		public Situacao()
		{
		}
		
		public Situacao(int id)
		{
			this.Id = id;
		}
		
		public virtual FluxoAndamento[] ListarFluxosAndamento()
		{
			return FluxoAndamento.FindAll(Expression.Eq("Situacao.Id", this.Id));
		}	
	}
}