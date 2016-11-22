//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Atividade.cs
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
	[ActiveRecord(Table="tb_atividade_ati", Schema="adm_licitar")]
	public class Atividade : ActiveRecordBase<Atividade>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_atividade_ati", SequenceName="adm_licitar.sq_atividade_ati")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_nivel_ati")]
		public virtual string Nivel
		{
			get;
			set;
		}
		
		[Property("txt_descricao_ati")]
		public virtual string Descricao
		{
			get;
			set;
		}
		
		[Property("txt_url_ati")]
		public virtual string Url
		{
			get;
			set;
		}
		
		[Property("boo_tipo_andamento_ati")]
		public virtual bool TipoAndamento
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_atividade_ati")]
		public virtual Atividade AtividadePai
		{
			get;
			set;
		}
	
		[BelongsTo("fk_cod_modulo_mod")]
		public virtual Modulo Modulo
		{
			get;
			set;
		}
		
		[Property("boo_exibir_no_menu_ati")]
		public virtual bool ExibirNoMenu
		{
			get;
			set;
		}

		[Property("boo_tipo_andamento_visivel_ati")]
		public virtual bool TipoAndamentoVisivel
		{
			get;
			set;
		}

		[Property("boo_atividade_aberta_ati")]
		public virtual bool AtividadePadrao
		{
			get;
			set;
		}
		
		[Property("boo_andamento_digitalizacao_ati")]
		public virtual bool AndamentoDigitalizacao
		{
			get;
			set;
		}

		public Atividade()
		{
		}
		
		public Atividade(int id)
		{
			this.Id = id;
		}
		
		/// <summary>
		/// Lista todas as funcoes que possuem a atividade.
		/// </summary>
		/// <returns>
		/// A <see cref="AtividadeFuncao"/>
		/// </returns>
		public virtual AtividadeFuncaoPermissao[] ListarFuncoes()
		{
			return AtividadeFuncaoPermissao.FindAll(Expression.Eq("Atividade.Id", this.Id));
		}
		
		/// <summary>
		/// Lista todas as sub atividades.
		/// </summary>
		/// <returns>
		/// A <see cref="Atividade"/>
		/// </returns>
		public virtual Atividade[] ListarSubAtividades()
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Atividade));
			pesquisa.CreateAlias("AtividadePai", "atvPai");
			pesquisa.Add(Expression.Eq("atvPai.Id", this.Id));
			return Atividade.FindAll(pesquisa);
		}
		
		/// <summary>
		/// Lista todas as AtividadePessoa da Atividade.
		/// </summary>
		/// <returns>
		/// A <see cref="AtividadePessoa"/>
		/// </returns>
		public virtual AtividadePessoaPermissao[] ListarAtividadesPessoa()
		{
			return AtividadePessoaPermissao.FindAll(Expression.Eq("Atividade.Id", this.Id));
		}
		
		/// <summary>
		/// Lista todos os FluxoAndamento deste tipo.
		/// </summary>
		/// <returns>
		/// A <see cref="FluxoAndamento"/>
		/// </returns>
		public virtual FluxoAndamento[] ListarFluxosAndamento()
		{
			return FluxoAndamento.FindAll(Expression.Eq("Atividade.Id", this.Id));
		}
		
		public override string ToString ()
		{
			return Descricao;
		}
		
		public static Atividade BuscarPorDescricao(string descricao)
		{
			bool like = true;
			string comp = "";
			if(like) comp = "%";
			string str = (string.IsNullOrEmpty(descricao)) ? "" : string.Format("{0}{1}", descricao, comp);
			return FindFirst(Expression.InsensitiveLike("Descricao", str));
		}

		public static Atividade BuscarTipoAndamentoPorDescricao(string descricao)
		{
			bool like = true;
			string comp = "";
			if(like) comp = "%";
			string str = (string.IsNullOrEmpty(descricao)) ? "" : string.Format("{0}{1}", descricao, comp);
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.InsensitiveLike("Descricao", str));
			dc.Add(Expression.Eq("TipoAndamento",true));
			return FindFirst(dc);
		}
		
		public static Atividade[] ListarAtividades()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.Eq("TipoAndamento", false));
			dc.AddOrder(Order.Asc("Descricao"));
			return Atividade.FindAll(dc);
		}
		
		public static Atividade[] ListarTiposDeAndamento()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Atividade));
			dc.Add(Expression.Eq("TipoAndamento", true));
			dc.AddOrder(Order.Asc("Descricao"));			
			
			return Atividade.FindAll(dc);
		}
		
		public static Atividade[] ListarTiposDeAndamentoDaFase(int idFase, int idModalidade)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Atividade));                   
			pesquisa.Add(Expression.Sql(@"this_.pk_cod_atividade_ati in 
			(select fan.fk_cod_atividade_ati from adm_licitar.tb_fluxo_andamento_fan fan 
			inner join adm_licitar.tb_workflow_wor wor on fan.fk_cod_workflow_wor=wor.pk_cod_workflow_wor 
			inner join adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu on wmu.fk_cod_workflow_wor=wor.pk_cod_workflow_wor 
			where fan.fk_cod_fase_fas= "+idFase+" and fan.boo_ativo_fan and wmu.fk_cod_modalidade_mod="+idModalidade+")"));
			pesquisa.Add(Expression.Eq("TipoAndamento", true));
            pesquisa.AddOrder(Order.Asc("Descricao"));
			return Atividade.FindAll(pesquisa);
		}
	}
}