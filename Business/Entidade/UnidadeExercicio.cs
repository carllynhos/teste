//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: UnidadeExercicio.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 08/11/2008
// DESCRICAO: 
// ALTERADO POR: 
// DATA DA ALTERACAO: 
// MOTIVO DA ALTERACAO:
// OBSERVACOES:
//----------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_unidade_exercicio_uex", Schema="adm_licitar")]
	public class UnidadeExercicio : ActiveRecordBase<UnidadeExercicio>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_unidade_exercicio_uex", SequenceName="adm_licitar.sq_unidade_exercicio_uex")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_descricao_uex")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		[Property("txt_sigla_uex")]
		public virtual string Sigla
		{
			get;
			set;
		}
		
		[Property("num_nivel_uex")]
		public virtual int Nivel
		{
			get;
			set;
		}

		[Property("dat_inicio_uex")]
		public virtual DateTime DataInicio
		{
			get;
			set;
		}

		[Property("dat_fim_uex")]
		public virtual DateTime DataFim
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_area_are")]
		public virtual Area Area
		{
			get;
			set;
		}		
		
		[BelongsTo("fk_cod_unidade_exercicio_uex")]
		public virtual UnidadeExercicio UnidadeExercicioPai
		{
			get;
			set;
		}

		[Property("boo_forcar_tramitacao_uex")]
		public virtual bool ForcarTramitacao
		{
			get;
			set;
		}

		[Property("boo_processos_associados_uex")]
		public virtual bool ProcessosAssociados
		{
			get;
			set;
		}
		
		public UnidadeExercicio()
		{
		}
		
		public UnidadeExercicio(int id)
		{
			this.Id = id;
		}
		
		/// <summary>
		/// Lista todos os cargos da unidade de exercicio.
		/// </summary>
		/// <returns>
		/// A <see cref="Cargo"/>
		/// </returns>
		public virtual UnidadeExercicioFuncao[] ListarUnidadesExercicioFuncao()
		{
			return UnidadeExercicioFuncao.FindAll(Expression.Eq("UnidadeExercicio.Id", this.Id));
		}
		
		/// <summary>
		/// Lista todas as sub unidades de exercicio.
		/// </summary>
		/// <returns>
		/// A <see cref="UnidadeExercicio"/>
		/// </returns>
		public virtual UnidadeExercicio[] ListarSubUnidadesExercicio()
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(UnidadeExercicio));
			pesquisa.CreateAlias("UnidadeExercicioPai", "UnExPai");
			pesquisa.Add(Expression.Eq("UnExPai.Id", this.Id));
			pesquisa.AddOrder(Order.Asc("Descricao"));
			return UnidadeExercicio.FindAll(pesquisa);
		}
		
		/// <summary>
		/// Lista todas as modalidades da unidade de exercicio.
		/// </summary>
		/// <returns>
		/// A <see cref="ModalidadeUnidadeExercicio"/>
		/// </returns>
		public virtual ModalidadeUnidadeExercicio[] ListarModalidades()
		{
			return ModalidadeUnidadeExercicio.FindAll(Expression.Eq("UnidadeExercicio.Id", this.Id));
		}
		
		/// <summary>
		/// Lista todos os andamentos da unidade de exercicio.
		/// </summary>
		/// <returns>
		/// A <see cref="ProcessoAndamento"/>
		/// </returns>
		public virtual ProcessoAndamento[] ListarAndamentos()
		{
			return ProcessoAndamento.FindAll(Expression.Eq("UnidadeExercicio.Id", this.Id));
		}

		public override string ToString()
		{
			return this.Descricao;
		}

		public override void Save()
		{
			this.Descricao = this.Descricao.ToUpper();
			base.Save();
		}
		
		public static UnidadeExercicio[] ListarSubUnidadesExercicio(int idUnidadePai)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(UnidadeExercicio)); 			
			pesquisa.Add(Expression.Sql(" dat_fim_uex = '-infinity' or dat_fim_uex is null ")); 		
			pesquisa.CreateAlias("UnidadeExercicioPai", "UnExPai");
			pesquisa.Add(Expression.Eq("UnExPai.Id", idUnidadePai));
			pesquisa.AddOrder(Order.Asc("Descricao"));
			return UnidadeExercicio.FindAll(pesquisa);
		}

		public static List<Pessoa> ListarPessoasDaSubUnidade(int idSubUnidade)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncaoPessoa));
			dc.CreateAlias("UnidadeExercicioFuncao", "sue");
			dc.Add(Expression.Eq("sue.UnidadeExercicio.Id", idSubUnidade));			
			dc.Add(Expression.Sql(" dat_fim_efp = '-infinity' or dat_fim_efp is null "));
			dc.AddOrder(Order.Asc("sue.UnidadeExercicio"));
			dc.CreateAlias("Pessoa", "pes");			
			dc.AddOrder(Order.Asc("pes.Nome"));
			UnidadeExercicioFuncaoPessoa[] uefp = UnidadeExercicioFuncaoPessoa.FindAll(dc);
			List<Pessoa> pessoasDaSubUnidade = new List<Pessoa>();
			for (int i = 0; i < uefp.Length; i++) pessoasDaSubUnidade.Add(uefp[i].Pessoa);			
			return pessoasDaSubUnidade;
		}
	}
}
