//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Modalidade.cs
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
	[ActiveRecord(Table="tb_modalidade_mod", Schema="adm_licitar")]
	public class Modalidade : ActiveRecordBase<Modalidade>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_modalidade_mod", SequenceName="adm_licitar.sq_modalidade_mod")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[Property("txt_descricao_mod")]
		public virtual string Descricao
		{
			get;
			set;
		}

		[Property("boo_agenda_mod")]
		public virtual bool Agenda
		{
			get;
			set;
		}
		
		public Modalidade()
		{
		}
		
		public override string ToString ()
		{
			return Descricao.ToString ();
		}

		
		public Modalidade(int id)
		{
			this.Id = id;
		}
		
		public virtual ModalidadeUnidadeExercicio[] ListarModalidadesUnidadeExercicio()
		{
			return ModalidadeUnidadeExercicio.FindAll(Expression.Eq("Modalidade.Id", this.Id));
		}
		
		public virtual Processo[] ListarProcessos()
		{
			return Processo.FindAll(Expression.Eq("Modalidade.Id", this.Id));
		}

		public static Modalidade[] ListarModalidadesAgenda()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Modalidade));
			dc.Add(Expression.Eq("Agenda", true));
			dc.AddOrder(Order.Asc("Descricao"));			
			return Modalidade.FindAll(dc);
		}
	}
}