using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{			
	[Serializable]
	[ActiveRecord(Table="tb_processo_completo_replica_pcr", Schema="adm_licitar")]
	public class ProcessoCompletoReplica : ActiveRecordBase<ProcessoCompletoReplica>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_processo_completo_replica_pcr", SequenceName="adm_licitar.sq_processo_completo_replica_pcr")]
		public virtual int Id
		{
			get;
			set;
		}

		[Property("cod_processo_pro")]
		public virtual int IdProcesso
		{
			get;
			set;
		}
		
		[Property("cod_instituicao_ins")]
		public virtual int IdInstituicao
		{
			get;
			set;
		}
		
		[Property("txt_descricao_ins")]
		public virtual string Instituicao
		{
			get;
			set;
		}
		
		[Property("cod_area_are")]
		public virtual int IdArea
		{
			get;
			set;
		}

		[Property("txt_descricao_are")]
		public virtual string Area
		{
			get;
			set;
		}

		[Property("cod_unidade_exercicio_uex")]
		public virtual int IdUnidExercicio
		{
			get;
			set;
		}

		[Property("txt_descricao_uex")]
		public virtual string UnidExercicio
		{
			get;
			set;
		}

		[Property("txt_observacao_pro")]
		public virtual string ResumoObjeto
		{
			get;
			set;
		}

		[Property("cod_natureza_nat")]
		public virtual int IdNatureza
		{
			get;
			set;
		}

		[Property("txt_natureza_nat")]
		public virtual string Natureza
		{
			get;
			set;
		}

		[Property("cod_modalidade_mod")]
		public virtual int IdModalidade
		{
			get;
			set;
		}

		[Property("txt_modalidade_mod")]
		public virtual string Modalidade
		{
			get;
			set;
		}

		[Property("cod_tipo_licitacao_tli")]
		public virtual int IdTipoLicitacao
		{
			get; 
			set; 
		}

		[Property("txt_tipo_licitacao_tli")]
		public virtual string TipoLicitacao
		{
			get;
			set;
		}

		[Property("txt_numero_spu_npr")]
		public virtual string NumeroSpu
		{
			get;
			set;
		}

		[Property("txt_numero_licitacao_npr")]
		public virtual string NumeroLicitacao
		{
			get;
			set;
		}

		[Property("txt_numero_comprasnet_npr")]
		public virtual string NumeroComprasNet
		{
			get;
			set;
		}

		[Property("txt_numero_sisbb_npr")]
		public virtual string NumeroSisBB
		{
			get;
			set;
		}  

		[Property("txt_numero_ig_npr")]
		public virtual string NumeroIG
		{
			get;
			set;
		}       

		[Property("num_processo_estimado_global")]
		public virtual decimal ValorEstimadoGlobal
		{
			get;
			set; 
		}

		[Property("num_processo_a_ser_contratado")]
		public virtual decimal ValorASerContratado
		{
			get;
			set;
		}

		[Property("num_processo_fracassado")]
		public virtual decimal ValorFracassado
		{
			get;
			set;
		}

		[Property("num_processo_deserto")]
		public virtual decimal ValorDeserto
		{
			get;
			set;
		}

		[Property("num_processo_anulado")]
		public virtual decimal ValorAnulado
		{
			get;
			set;
		}

		[Property("num_processo_revogado")]
		public virtual decimal ValorRevogado
		{
			get;
			set;
		}

		[Property("num_processo_cancelado")]
		public virtual decimal ValorCancelado
		{
			get;
			set;
		}

		[Property("cod_presidente_pregoeiro_pes")]
		public virtual int IdPresidentePregoeiro
		{
			get;
			set;
		}

		[Property("txt_presidente_pregoeiro_pes")]
		public virtual string PresidentePregoeiro
		{
			get;
			set;
		}

		[Property("cod_papel_pap")]
		public virtual int IdPapel
		{
			get;
			set;
		}

		[Property("txt_papel_pap")]
		public virtual string Papel
		{
			get;
			set;
		}

		[Property("dat_cadastro_pan")]
		public virtual DateTime DataCadastro
		{
			get;
			set;
		}

		[Property("dat_entrada_pge_pan")]
		public virtual DateTime DataEntradaPge
		{
			get;
			set;
		}

		[Property("dat_realizacao_pan")]
		public virtual DateTime DataRealizacao
		{
			get;
			set;
		}
		
		[Property("dat_abertura_propostas_pan")]
		public virtual DateTime DataAberturaPropostas
		{
			get;
			set;
		}
				
		[Property("dat_adjudicacao_pan")]
		public virtual DateTime DataAdjudicacao
		{
			get;
			set;
		}
				
		[Property("dat_homologado_pan")]
		public virtual DateTime DataHomologado
		{
			get;
			set;
		}

		[Property("dat_conclusao_pan")]
		public virtual DateTime DataConclusao
		{
			get;
			set;
		}

		[Property("dat_devolucao_pan")]
		public virtual DateTime DataDevolucao
		{
			get;
			set;
		}

		[Property("dat_deserto_pan")]
		public virtual DateTime DataDeserto
		{
			get;
			set;
		}

		[Property("dat_revogado_pan")]
		public virtual DateTime DataRevogado
		{
			get;
			set;
		}

		[Property("dat_fracassado_pan")]
		public virtual DateTime DataFracassado
		{
			get;
			set;
		}
		
		[Property("dat_aprovacao_pan")]
		public virtual DateTime DataAprovacao
		{
			get;
			set;
		}

		[Property("dat_anulacao_pan")]
		public virtual DateTime DataAnulacao
		{
			get;
			set;
		}

		[Property("dat_sessao_abertura_proposta_comercial_pan")]
		public virtual DateTime DataSessaoAberturaPropostaComercial
		{
			get;
			set;
		}

		[Property("dat_sessao_abertura_proposta_tecnica_pan")]
		public virtual DateTime DataSessaoAberturaPropostaTecnica
		{
			get;
			set;
		}

		[Property("dat_sessao_resultado_proposta_comercial_pan")]
		public virtual DateTime DataSessaoResultadoPropostaComercial
		{
			get;
			set;
		}
	
		[Property("dat_sessao_resultado_proposta_tecnica_pan")]
		public virtual DateTime DataSessaoResultadoPropostaTecnica
		{
			get;
			set;
		}

		[Property("dat_sessao_resultado_habilitacao_pan")]
		public virtual DateTime DataSessaoResultadoHabilitacao
		{
			get;
			set;
		}
		
		[Property("dat_ultimo_andamento_pan")]
		public virtual DateTime DataUltimoAndamento
		{
			get;
			set;
		}

		[Property("txt_situacao_atual_sit")]
		public virtual string SituacaoAtual
		{
			get;
			set;
		}				

		[Property("cod_ultimo_andamento_pan")]
		public virtual int idUltimoAndamento
		{
			get;
			set;
		}
		
		[Property("txt_ultimo_andamento_pan")]
		public virtual string UltimoAndamento
		{
			get;
			set;
		}

		[Property("cod_ultima_fase_fas")]
		public virtual int idUltimaFase
		{
			get;
			set;
		}
		
		[Property("txt_ultima_fase_fas")]
		public virtual string UltimaFase
		{
			get;
			set;
		}
		
		[Property("txt_estado_processo")]
		public virtual string EstadoProcesso
		{
			get;
			set;
		}
		
		[Property("txt_motivo_concluido")]
		public virtual string MotivoConcluido
		{
			get;
			set;
		}
				
		[Property("txt_andamento_pan")]
		public virtual string Observacao
		{
			get;
			set;
		}
		
		[Property("cod_vencedor_pes")]
		public virtual string IdVencedor
		{
			get;
			set;
		}
		
		[Property("txt_vencedor_pes")]
		public virtual string Vencedor
		{
			get;
			set;
		}
		
		[Property("num_nao_contratado_vpr")]
		public virtual decimal VlNaoContratado
		{
			get;
			set;
		}		

		[Property("num_estimado_real_vpr")]
		public virtual decimal VlEstimadoReal
		{
			get;
			set;
		}
			
		[Property("num_economia_vpr")]
		public virtual decimal VlEconomia
		{
			get;
			set;
		}
		
		[Property("num_economia_porcent_vpr")]
		public virtual decimal VlEconomiaPorcent
		{
			get;
			set;
		}
		
		[Property("txt_ano_mes_inicio_processo")]
		public virtual string MesAnoInicio
		{
			get;
			set;
		}
		
		[Property("txt_ano_mes_final_processo")]
		public virtual string MesAnoFim
		{
			get;
			set;
		}
		
		[Property("cod_responsavel_cadastro_pes")]
		public virtual int IdResponsavelCadastro
		{
			get;
			set;
		}
		
		[Property("txt_responsavel_cadastro_pes")]
		public virtual string ResponsavelCadastro
		{
			get;
			set;
		}
				
		[Property("txt_responsavel_conclusao_pes")]
		public virtual string ResponsavelConclusao
		{
			get;
			set;
		}
		
		[Property("cod_responsavel_conclusao_pes")]
		public virtual int IdResponsavelConclusao
		{
			get;
			set;
		}
		
		
		[Property("dat_responsavel_conclusao_pcr")]
		public virtual DateTime DataResponsavelConclusao
		{
			get;
			set;
		}
		
		[Property("dat_publicacao_pcr")]
		public virtual DateTime DataPublicacao
		{
			get;
			set;
		}
		
		[Property("txt_numero_manifestacao_npr")]
		public virtual string NumeroManifestacao
		{
			get;
			set;
		}
		
		[Property("dat_apresentacao_proposta_tecnica_pcr")]
		public virtual DateTime DataApresPropostaTecnica
		{
			get;
			set;
		}
		
		[Property("dat_apresentacao_proposta_financeira_pcr")]
		public virtual DateTime DataApresPropostaFinanceira
		{
			get;
			set;
		}
		
		[Property("dat_sessao_habilitacao_pcr")]
		public virtual DateTime DataSessaoHabilitacao
		{
			get;
			set;
		}
		
		[Property("dat_resultado_classificacao_tecnica_pcr")]
		public virtual DateTime DataResultadoClassifTecnica
		{
			get;
			set;
		}
		
		[Property("dat_resultado_classificacao_financeira_pcr")]
		public virtual DateTime DataResultadoClassifFinanceira
		{
			get;
			set;
		}
		
	}
}
