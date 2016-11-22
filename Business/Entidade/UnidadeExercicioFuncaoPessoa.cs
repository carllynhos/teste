// UnidadeExercicioFuncaoPessoa.cs created with MonoDevelop
// User: guilhermefacanha at 15:41 12/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate.Expression;
using Licitar.Business.Entidade;
using Licitar.Business.Servico;
using System.Collections.Generic;
using System.Collections;


namespace Licitar.Business.Entidade
{
	
	[ActiveRecord(Table="tb_unidade_exercicio_funcao_pessoa_efp", Schema="adm_licitar")]
	public class UnidadeExercicioFuncaoPessoa : ActiveRecordBase<UnidadeExercicioFuncaoPessoa>
	{
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_unidade_exercicio_funcao_pessoa_efp", SequenceName="adm_licitar.sq_unidade_exercicio_funcao_pessoa_efp")]
		public virtual int Id
		{
			get;
			set;
		}

		[Property("dat_inicio_efp")]
		public virtual DateTime DataInicio
		{
			get;
			set;
		}

		[Property("dat_fim_efp")]
		public virtual DateTime DataFim
		{
			get;
			set;
		}

		[BelongsTo("fk_cod_unidade_exercicio_funcao_uef")]
		public virtual UnidadeExercicioFuncao UnidadeExercicioFuncao
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
		
		public UnidadeExercicioFuncaoPessoa()
		{
			
		}

		public static UnidadeExercicioFuncaoPessoa[] GetUnidadeExercicioFuncaoPessoa(int idPessoa)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncaoPessoa));
			dc.Add(Expression.Eq("Pessoa.Id", idPessoa));
			return UnidadeExercicioFuncaoPessoa.FindAll(dc);
		}
		
		public static bool ExisteUnidadeExercicioFuncaoPessoa(int idUnidadeExercicioFuncao, int idPessoa)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncaoPessoa));
			dc.Add(Expression.Eq("UnidadeExercicioFuncao.Id", idUnidadeExercicioFuncao));
			dc.Add(Expression.Eq("Pessoa.Id", idPessoa));
			if (UnidadeExercicioFuncaoPessoa.FindAll(dc).Length > 0)			
				return true;
			
			return false;
		}
	}
}
