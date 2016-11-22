//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Papel.cs
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
	[ActiveRecord(Table="tb_papel_pap", Schema="adm_licitar")]
	public class Papel : ActiveRecordBase<Papel>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_papel_pap", SequenceName="adm_licitar.sq_papel_pap")]
		public virtual int Id
		{
			get;
			set;
		}		

		[Property("txt_descricao_pap")]
		public virtual string Descricao
		{
			get;
			set;
		}			
		
		public Papel()
		{
		}
		
		public Papel(int id)
		{
			this.Id = id;
		}
		
		public virtual ProcessoPapelPessoa[] ListarPessoasDoPapel()
		{
			return ProcessoPapelPessoa.FindAll(Expression.Eq("Papel.Id",this.Id));
		}
		
		public static Papel[] Listar()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Papel));
			dc.AddOrder(Order.Asc("Descricao"));
			return Papel.FindAll(dc);
		}
		
		public static Papel[] Pesquisar(string pesquisa)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Papel));
			dc.AddOrder(Order.Asc("Descricao"));
			dc.Add(Expression.InsensitiveLike("Descricao", "%" + pesquisa + "%"));
			return Papel.FindAll(dc);
		}
		
		public override string ToString ()
		{
			return this.Descricao;
		}

		
	}
}