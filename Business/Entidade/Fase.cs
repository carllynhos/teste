//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Fase.cs
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
	[ActiveRecord(Table="tb_fase_fas", Schema="adm_licitar")]
	public class Fase : ActiveRecordBase<Fase>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_fase_fas", SequenceName="adm_licitar.sq_fase_fas")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_fas")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		[Property("num_ordem_fas")]
		public virtual int Ordem
		{
			get;set;
		}
		
		public override string ToString ()
		{
			return Descricao;
		}
		
		public Fase()
		{
		}
		
		public Fase(int id)
		{
			this.Id = id;
		}
		
		public virtual FluxoAndamento[] ListarFluxosAndamento()
		{
			return FluxoAndamento.FindAll(Expression.Eq("Fase.Id",this.Id));
		}
	}
}