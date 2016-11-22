//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Modulo.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 14/01/2009
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
	[ActiveRecord(Table="tb_modulo_mod", Schema="adm_licitar")]
	public class Modulo : ActiveRecordBase<Modulo>
	{        
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_modulo_mod", SequenceName="adm_licitar.sq_modulo_mod")]
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
		       
		public Modulo()
		{
		}
		
		public Modulo(int id)
		{
			this.Id = id;
		}	
	
		public virtual Atividade[] ListarAtividades()
		{			
			return Atividade.FindAll(Expression.Eq("Modulo.Id", this.Id));
		}

		public static Modulo[] listarModulos()
		{
			DetachedCriteria dcModulos = DetachedCriteria.For(typeof(Modulo));
			dcModulos.AddOrder(Order.Asc("Descricao"));

			return FindAll(dcModulos);
		}

		public override string ToString ()
		{
			return Descricao;
		}
	}
}