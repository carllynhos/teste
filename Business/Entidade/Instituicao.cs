//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Instituicao.cs
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
	[ActiveRecord(Table="tb_instituicao_ins", Schema="adm_licitar")]
	public class Instituicao : ActiveRecordBase<Instituicao>
	{        
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_instituicao_ins", SequenceName="adm_licitar.sq_instituicao_ins")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_descricao_ins")]
		public virtual string Descricao
		{			
			get;
			set;			
		}
		
		[Property("txt_sigla_ins")]
		public virtual string Sigla
		{
			get;
			set;
		}				
		
		public virtual string Cnpj
		{
			get {return UnidadeAdministrativa.FindFirst(Expression.Eq("Instituicao",(new Instituicao(this.Id)))).Cnpj;}			
			set{}
		}
        
		public Instituicao()
		{
		}
		
		public Instituicao(int id)
		{
			this.Id = id;
		}
			
		
		public override string ToString ()
		{
			return this.Sigla;
		}
		
		/// <summary>
		/// Lista todas as areas da instituicao.
		/// </summary>
		/// <returns>
		/// A <see cref="Area"/>
		/// </returns>
		public virtual Area[] ListarAreas()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Area));
			dc.Add(Expression.Eq("Instituicao.Id", this.Id));
			dc.AddOrder(Order.Asc("Descricao"));
			
			return Area.FindAll(dc);
		}
		
		/// <summary>
		/// Lista todos os processos da instituicao.
		/// </summary>
		/// <returns>
		/// A <see cref="Processo"/>
		/// </returns>
		public virtual Processo[] ListarProcessos()
		{
			return Processo.FindAll(Expression.Eq("Instituicao.Id", this.Id));
		}

		public static Instituicao[] listarInstituicoes()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Instituicao));
			dc.AddOrder(Order.Asc("Sigla"));
			return FindAll(dc);
		}
		
		
		
	}
}