//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Processo.cs
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
	[ActiveRecord(Table="tb_processo_pro", Schema="adm_licitar")]
	public class Processo : ActiveRecordBase<Processo>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_processo_pro", SequenceName="adm_licitar.sq_processo_pro")]
		public virtual int Id
		{
			get;
			set;
		}		

		[Property("boo_registro_preco_pro")]
		public virtual bool RegistroPreco{
			get;
			set;
		}

		
		[Property("dat_inicio_pro")]
		public virtual DateTime DataInicio
		{
			get;
			set;
		}
		
		[Property("dat_fim_pro")]
		public virtual DateTime DataFim
		{
			get;
			set;
		}
		
		[Property("dat_ultimo_andamento_pro")]
		public virtual DateTime DataUltimoAndamento
		{
			get;
			set;
		}
		
		[Property("boo_finalizado_pro")]
		public virtual bool Finalizado
		{
			get;
			set;
		}
		
		[Property("boo_auditado_pro")]
		public virtual bool Auditado
		{
			get;
			set;
		}
		
		[Property("boo_auditando_pro")]
		public virtual bool Auditando
		{
			get;
			set;
		}
		
		[Property("txt_resumo_objeto_pro")]
		public virtual string ResumoObjeto
		{
			get;
			set;
		}
		
		[Property("txt_observacao_pro")]
		public virtual string Observacao
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_classificacao_cla")]
		public virtual Classificacao Classificacao
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
		
		[BelongsTo("fk_cod_unidade_exercicio_uex")]
		public virtual UnidadeExercicio UnidadeExercicio
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_unidade_administrativa_uad")]
		public virtual UnidadeAdministrativa UnidadeAdministrativa
		{
			get;
			set;
		}
		
		public Processo()
		{
		}
		
		public Processo(int id)
		{
			this.Id = id;
		}
		
		/// <summary>
		/// Lista todas as pessoas envolvidas no processo.
		/// </summary>
		/// <returns>
		/// A <see cref="PessoaProcesso"/>
		/// </returns>
		public virtual ProcessoPapelPessoa[] ListarPessoasEnvolvidas()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dc.CreateAlias("Papel","pap");
			dc.Add(Expression.Eq("Processo.Id", this.Id));
			dc.Add(Expression.Not(Expression.Eq("pap.Descricao", "VENCEDOR")));
			return ProcessoPapelPessoa.FindAll(dc);
		}
		
		/// <summary>
		/// Lista todos os valores do processo.
		/// </summary>
		/// <returns>
		/// A <see cref="ValorProcesso"/>
		/// </returns>
		public virtual ValorProcesso[] ListarValores()
		{			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ValorProcesso));
			dc.Add(Expression.Eq("Processo.Id", this.Id));
			dc.CreateAlias("TipoValor", "tva");
			dc.AddOrder(Order.Asc("tva.Ordem"));	
			dc.AddOrder(Order.Asc("tva.Descricao"));
			return ValorProcesso.FindAll(dc);			
		}
		
		/// <summary>
		/// Lista as agendas do processo.
		/// </summary>
		/// <returns>
		/// A <see cref="Agenda"/>
		/// </returns>
		public virtual Agenda[] ListarAgendas()
		{
			return Agenda.FindAll(Expression.Eq("Processo.Id", this.Id));
		}
		
		/// <summary>
		/// Lista os andamentos do processo.
		/// </summary>
		/// <returns>
		/// A <see cref="ProcessoAndamento"/>
		/// </returns>
		public virtual ProcessoAndamento[] ListarAndamentos()
		{
			return ProcessoAndamento.FindAll(Order.Asc("DataCadastro"),Expression.Eq("Processo.Id", this.Id));
		}
		
		/// <summary>
		/// Lista os numeros do processo.
		/// </summary>
		/// <returns>
		/// A <see cref="ProcessoAndamento"/>
		/// </returns>
		public virtual NumeroProcesso[] ListaNumeros()
		{
			return NumeroProcesso.FindAll(Expression.Eq("Processo.Id", this.Id));
		}
		
		public void ReceberIncluindoPessoa(Pessoa responsavel, bool UnidExercicio,string fase)
		{
			Atividade tipo = Atividade.BuscarPorDescricao("Receber");			
			RegistrarAndamentoIncluindoPessoa(tipo, responsavel, null, string.Empty,true, UnidExercicio,fase);
		}	
				
		
		private void RegistrarAndamentoIncluindoPessoa(Atividade tipoAndamento, Pessoa responsavel, Pessoa destinatario, string observacao, bool incluirData, bool UnidExercicio,string fase)
		{		
			ProcessoAndamento andamento = new ProcessoAndamento();
			
			DetachedCriteria dcFluxo = DetachedCriteria.For(typeof(FluxoAndamento));				
			dcFluxo.CreateAlias("Fase","fas");
			dcFluxo.CreateAlias("Atividade","ati");
			dcFluxo.Add(Expression.Eq("fas.Descricao",fase));
			dcFluxo.Add(Expression.Eq("ati.Descricao",tipoAndamento.Descricao));
			dcFluxo.Add(Expression.Sql("this_.fk_cod_workflow_wor in (select wor.pk_cod_workflow_wor from adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu inner join adm_licitar.tb_workflow_wor wor on wmu.fk_cod_workflow_wor = wor.pk_cod_workflow_wor where wmu.fk_cod_modalidade_mod="+this.Classificacao.Modalidade.Id+")"));
			FluxoAndamento objFluxo = FluxoAndamento.FindFirst(dcFluxo);	
			                
			//INSERIR ANDAMENTO
			andamento.Cadastrante = responsavel;				
			andamento.Processo = this;
			andamento.FluxoAndamento = objFluxo;
			andamento.Pessoa = destinatario;
			if (incluirData)
				andamento.DataCadastro = DateTime.Now;
			
			andamento.Andamento = observacao ?? null;
			andamento.SaveAndFlush();	
		}
		
		public static string numSPU(int id)
		{
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(NumeroProcesso));			
			dc.CreateAlias("TipoNumero","tn");
			dc.Add(Expression.Eq("Processo.Id", id));
			dc.Add(Expression.Eq("tn.Descricao","SPU"));							
			NumeroProcesso np = new NumeroProcesso();
			np = NumeroProcesso.FindFirst(dc);			

			if (np != null)
				return np.numeroProcesso;

			return "";
		}	
		
		public static string numVIPROC(int id)
		{
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(NumeroProcesso));			
			dc.CreateAlias("TipoNumero","tn");
			dc.Add(Expression.Eq("Processo.Id", id));
			dc.Add(Expression.Eq("tn.Descricao","VIPROC"));							
			NumeroProcesso np = new NumeroProcesso();
			np = NumeroProcesso.FindFirst(dc);			

			if (np != null)
				return np.numeroProcesso;

			return "";
		}
		
		public static string numLicitacao(int id)
		{
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(NumeroProcesso));			
			dc.CreateAlias("TipoNumero","tn");
			dc.Add(Expression.Eq("Processo.Id", id));
			dc.Add(Expression.Eq("tn.Descricao","LICITAÇÃO"));			
			NumeroProcesso np = new NumeroProcesso();
			np = NumeroProcesso.FindFirst(dc);			

			if (np != null)
				return np.numeroProcesso;

			return "";
		}

		public static string DataMarcacao(int idProcesso)
		{
		
			string[] andamentosAgenda = 
			{ 
				"LICITAÇÃO MARCADA", 
				"DISPUTA", 
				"SESSÃO FINAL", 
				"SESSÃO DE RESULTADO - HABILITAÇÃO", 
				"SESSÃO DE RESULTADO - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA COMERCIAL", 
				"SESSÃO DE ABERTURA - PROPOSTA TÉCNICA", 
				"SESSÃO DE RESULTADO - PROPOSTA TÉCNICA"  
			};
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento)); 			
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.IsNull("AndamentoCorrigido"));
			dc.Add(Expression.IsNull("AndamentoAdiado"));
			dc.CreateAlias("FluxoAndamento", "fan");
			dc.CreateAlias("fan.Atividade", "ati");
			dc.Add(Expression.In("ati.Descricao", andamentosAgenda));
			dc.Add(Expression.Sql(" this_.dat_andamento_pan >= '" + DateTime.Now + "' order by this_.dat_andamento_pan desc ")); 					
			ProcessoAndamento andamento = ProcessoAndamento.FindFirst(dc);
			string retorno = string.Empty;
				if(andamento != null && Utilidade.UtdValidador.ValidarData(Convert.ToString(andamento.DataAndamento)))
					retorno = andamento.DataAndamento.ToString("dd/MM/yyyy");
			return retorno;
		
		}
		
		public static DateTime DataProcesso(int id)
		{
			return DataProcesso(id, "DATA DE ENTRADA NA PGE");
		}

		
		
		public static DateTime DataProcesso(int id, string tipoData)
		{
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoAndamento));			
			dc.CreateAlias("FluxoAndamento","fa");
			dc.CreateAlias("fa.Atividade","at");
			dc.Add(Expression.Eq("Processo.Id", id));
			dc.Add(Expression.Eq("at.Descricao",tipoData));
			dc.AddOrder(Order.Asc("DataCadastro"));
			
			ProcessoAndamento pa = ProcessoAndamento.FindFirst(dc);

			if (pa != null)
				return pa.DataAndamento;

			return DateTime.MinValue;
		}			
		
		#region auditoria
		public override void Delete()
		{			
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Delete();
		}
		
		public override void Save()
		{
			Console.WriteLine("classe processo ="+this.Id);
			new AuditoriaSistema().RealizarAuditoria(this.Id);	
			base.Save();
		}
		
		public override void Update()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Update();
		}
		
		public override void Create()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Create();
		}
		
		public override void DeleteAndFlush()
		{			
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.DeleteAndFlush();
		}
		
		public override void SaveAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.SaveAndFlush();
		}
		
		public override void UpdateAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.UpdateAndFlush();
		}
		
		public override void CreateAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.CreateAndFlush();
		}		
		#endregion
	}
}