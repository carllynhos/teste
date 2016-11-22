//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Area.cs
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
	[ActiveRecord(Table="tb_area_are", Schema="adm_licitar")]
	public class Area : ActiveRecordBase<Area>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_area_are", SequenceName="adm_licitar.sq_area_are")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_descricao_are")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		[Property("txt_sigla_are")]
		public virtual string Sigla
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_instituicao_ins")]
		public virtual Instituicao Instituicao
		{
			get;
			set;
		}
		
		public Area()
		{
		}
		
		public Area(int id)
		{
			this.Id = id;
		}
		
		/// <summary>
		/// Lista todas as unidades de exercicio da area.
		/// </summary>
		/// <returns>
		/// A <see cref="UnidadeExercicio"/>
		/// </returns>
		public virtual UnidadeExercicio[] ListarUnidadesExercicio()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicio));
			dc.AddOrder(Order.Asc("Descricao"));			
			return UnidadeExercicio.FindAll(Expression.Eq("Area.Id", this.Id));
		}

		public override string ToString()
		{
			return this.Descricao;
		}
	}
}