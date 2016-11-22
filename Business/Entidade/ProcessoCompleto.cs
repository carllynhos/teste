//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Processo.cs
// CRIADO POR: Marcelo Almeida
// DATA DA CRIACAO: 30/09/2010
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
	[ActiveRecord(Table="tb_processo_completo_pcm", Schema="adm_licitar")]
	public class ProcessoCompleto : ActiveRecordBase<ProcessoCompleto>
	{
		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_processo_completo_pcm", SequenceName="adm_licitar.sq_processo_completo_pcm")]
		public virtual int Pk
		{
			get;
			
			set;
			
		}
		
		[Property("cod_processo_pro")]
		public virtual int? Id
		{
			get;
			
			set;
			
		}	
		
		
		[BelongsTo("cod_instituicao_ins")]
		public virtual Instituicao Instituicao
		{
			get;
			set;
		}
		
		
		[Property("txt_descricao_ins")]
		public virtual string StrInstituicao
		{
			get;
			set;
		}
		
		[BelongsTo("cod_modalidade_mod")]
		public virtual Modalidade Modalidade
		{
			get;
			set;
		}
		
		[Property("txt_modalidade_mod")]
		public virtual string StrModalidade
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
		
		[Property("cod_tipo_licitacao_tli")]
		public virtual int CodTipoLicitacao
		{
			get;
			set;
		}
		
		[Property("txt_tipo_licitacao_tli")]
		public virtual string StrTipoLicitacao
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
		
		[Property("txt_numero_mapp_npr")]
		public virtual string NumeroMapp
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
		
		[Property("num_processo_estimado_global")]
		public virtual double? ValorEstimadoGlobal
		{
			get;
			set;
		}
		
		[Property("num_nao_contratado_vpr")]
		public virtual double? ValorNaoContratado
		{
			get;
			set;
		}
		
		[Property("num_processo_a_ser_contratado")]
		public virtual double? ValorASerContratado
		{
			get;
			set;
		}
				
		[Property("num_processo_fracassado")]
		public virtual double? ValorFracassado
		{
			get;
			set;
		}
	
		[Property("num_processo_deserto")]
		public virtual double? ValorDeserto
		{
			get;
			set;
		}
		
		[Property("num_processo_cancelado")]
		public virtual double? ValorCancelado
		{
			get;
			set;
		}
		
		[Property("num_processo_anulado")]
		public virtual double? ValorAnulado
		{
			get;
			set;
		}
		
		[Property("num_processo_revogado")]
		public virtual double? ValorRevogado
		{
			get;
			set;
		}
		
		[Property("num_estimado_real_vpr")]
		public virtual double? ValorEstimadoReal
		{
			get;
			set;
		}
		
		[Property("num_economia_vpr")]
		public virtual double? ValorEconomia
		{
			get;
			set;
		}
		
		[Property("num_economia_porcent_vpr")]
		public virtual double? ValorEconomiaPerc
		{
			get;
			set;
		}
		
		
		//também data entrada na pge
		[Property("dat_cadastro_pan")]
		public virtual DateTime? DataCadastro
		{
			get;
			set;
		}
		
		//também data entrada na pge
		[Property("dat_cadastro_entrada_pge_pan")]
		public virtual DateTime? DataCadEntradaPGE
		{
			get;
			set;
		}
		
		//também data entrada na pge
		[Property("dat_andamento_entrada_pge_pan")]
		public virtual DateTime? DataAndEntradaPGE
		{
			get;
			set;
		}
		
	    //também data entrada na pge
		[Property("dat_entrada_pge_pan")]
		public virtual DateTime? DataEntradaPGE
		{
			get;
			set;
		}
		
		[Property("dat_conclusao_pan")]
		public virtual DateTime? DataConclusao
		{
			get;
			set;
		}
		
		[Property("dat_andamento_conclusao_pan")]
		public virtual DateTime? DataAndConclusao
		{
			get;
			set;
		}
		
		[Property("dat_cadastro_conclusao_pan")]
		public virtual DateTime? DataCadConclusao
		{
			get;
			set;
		}

		
	    [Property("dat_cadastro_marcacao_pan")]
		public virtual DateTime? DataCadMarcacao
		{
			get;
			set;
		}
		
		[Property("dat_andamento_marcacao_pan")]
		public virtual DateTime? DataAndMarcacao
		{
			get;
			set;
		}
		
		
		[Property("dat_andamento_realizacao_pan")]
		public virtual DateTime? DataAndRealizacao
		{
			get;
			set;
		}
		
		[Property("dat_cadastro_realizacao_pan")]
		public virtual DateTime? DataCadRealizacao
		{
			get;
			set;
		}
		
		[Property("dat_realizacao_pan")]
		public virtual DateTime? DataRealizacao
		{
			get;
			set;
		}
		
		[Property("dat_cadastro_abertura_propostas_pan")]
		public virtual DateTime? DataCadAberturaPropostas
		{
			get;
			set;
		}
		
		[Property("dat_andamento_abertura_propostas_pan")]
		public virtual DateTime? DataAndAberturaPropostas
		{
			get;
			set;
		}
		
		[Property("dat_abertura_propostas_pan")]
		public virtual DateTime? DataAberturaPropostas
		{
			get;
			set;
		}
		
		
		[Property("dat_cadastro_acolhimento_propostas_pan")]
		public virtual DateTime? DataCadAcolhimentoPropostas
		{
			get;
			set;
		}
		
		[Property("dat_andamento_acolhimento_propostas_pan")]
		public virtual DateTime? DataAndAcolhimentoPropostas
		{
			get;
			set;
		}
		
		[Property("dat_acolhimento_propostas_pan")]
		public virtual DateTime? DataAcolhimentoPropostas
		{
			get;
			set;
		}
		
		
		[Property("boo_processo_digital")]
		public virtual bool ProcessoDigital
		{
			get;
			set;
		}
		
		[Property("txt_numero_portaldigital_npr")]
		public virtual string NumeroDigital
		{
			get;
			set;
		}
		
		[Property("cod_unidade_exercicio_uex")]
		public virtual int CodUex
		{
			get;
			set;
		}
		
		[Property("txt_descricao_uex")]
		public virtual string StrUex
		{
			get;
			set;
		}
		
		[Property("cod_presidente_pregoeiro_pes")]
		public virtual int? CodPresidentePregoeiro
		{
			get;
			set;
		}
		
		[Property("txt_presidente_pregoeiro_pes")]
		public virtual string StrPresidentePregoeiro
		{
			get;
			set;
		}

		[Property("cod_papel_pap")]
		public virtual int? CodPapel
		{
			get;
			set;
		}
		
		[Property("txt_papel_pap")]
		public virtual string StrPapel
		{
			get;
			set;
		}
		
		[Property("cod_natureza_nat")]
		public virtual int CodNatureza
		{
			get;
			set;
		}
		
		[Property("txt_natureza_nat")]
		public virtual string StrNatureza
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
		
		[Property("txt_situacao_atual_sit")]
		public virtual string SituacaoProcesso
		{
			get;
			set;
		}
		
		[Property("txt_ultimo_cadastrante_pes")]
		public virtual string CadastranteProcesso
		{
			get;
			set;
		}
		
		[Property("txt_vencedor_pes")]
		public virtual string StrVencedor
		{
			get;
			set;
		}
		
		[Property("boo_auditado_pro")]
		public virtual bool? ProcessoAuditado
		{
			get;
			set;
		}
		
	    [Property("boo_auditando_pro")]
		public virtual bool? ProcessoAuditando
		{
			get;
			set;
		}				
		
		[Property("cod_unidade_administrativa_uad")]
		public virtual int CodUnidadeAdministrativa
		{
			get;
			set;
		}
		
		[Property("txt_numero_viproc_npr")]
		public virtual string NumeroViproc
		{
			get;
			set;
		}
		
		[Property("txt_publicacao_edital_pcm")]
		public virtual string StrPublicacaoEdital
		{
			get;
			set;
		}

		
		
		
		
		public ProcessoCompleto()
		{
		}
		
		public ProcessoCompleto(int pk)
		{			
			this.Pk = pk;
		}
		
		
		
	}
}
