using System;
using Castle.ActiveRecord;
using System.Collections;
using System.Collections.Generic;


namespace Licitar.Business.Entidade
{
	[ActiveRecord(Table="vw_consulta_receber_tramitar_crt", Schema="adm_licitar", Mutable=false)]
	public class ConsultaReceberTramitar : ActiveRecordBase<ConsultaReceberTramitar>
	{
		#region Construtores

		public ConsultaReceberTramitar()
		{
		}


//		public override string ToString()
//		{
//			return this.NumeroComTipo.ToString();
//		}
//		
//		public string NumeroComTipo
//		{
//			get { return (!string.IsNullOrEmpty(this.Numero)? (this.TipoNumero.ToString() +": "+ this.Numero.ToString()):"Não Informado"); }
//		}

		#endregion

		#region Propriedades
		// Propriedade: Processo
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_processo_pro")]
		public virtual int Processo
		{
			get; set;
		}		
		
		// Propriedade: PessoaCadastro
		[Property("cod_pessoa_and_rec")]
		public virtual int IdPessoaUltAndReceber
		{
			get; set;
		}

		// Propriedade: PessoaCadastro
		[Property("cod_pessoa_cadastro")]
		public virtual int IdPessoaCadastro
		{
			get; set;
		}
		
		// Propriedade: PessoaCadastro
		[Property("txt_pessoa_cadastro")]
		public virtual string TxtPessoaCadastro
		{
			get; set;
		}
		
			// Propriedade: PessoaDestino
		[Property("cod_pessoa_tramitou")]
		public virtual int IdPessoaDestinoAndamento
		{
			get; set;
		}

		// Propriedade: PessoaDestino
		[Property("txt_pessoa_tramitou")]
		public virtual string TxtPessoaDestinoAndamento
		{
			get; set;
		}
		
		// Propriedade: Numero spu
		[Property("numero_spu")]
		public virtual string NumeroSpu
		{
			get; set;
		}
		
		// Propriedade: Numero
		[Property("numero_licitacao")]
		public virtual string NumeroLicitacao
		{
			get; set;
		}
		
//		// Propriedade: TipoNumero
//		[Property("tipo_numero")]
//		public virtual string TipoNumero
//		{
//			get; set;
//		}

//		// Propriedade: InstituicaoOrigem
//		[Property("instituicao")]
//		public virtual int InstituicaoOrigem
//		{
//			get; set;
//		}
		
		// Propriedade: NomeInstituicaoOrigem
		[Property("instituicao")]
		public virtual string NomeInstituicaoOrigem
		{
			get; set;
		}
		
//		// Propriedade: AreaResponsavel
//		[Property("fk_cod_inst_area_resp_iar")]
//		public virtual int AreaResponsavel
//		{
//			get; set;
//		}
		
		// Propriedade: ObjetoProcesso
		[Property("txt_resumo_objeto_pro")]
		public virtual string ObjetoProcesso
		{
			get; set;
		}
		
//		// Propriedade: PessoaCadastro
//		[Property("pessoa_cadastro_andam_pes")]
//		public virtual int PessoaCadastroAndamento
//		{
//			get; set;
//		}
		
	
		
		// Propriedade: UnidadeExDestino
		[Property("unidade_exercicio")]
		public virtual string UnidadeExDestino
		{
			get; set;
		}
		
//		// Propriedade: Codigo do TipoAndamento TAN
//		[Property("pk_cod_tipo_andamento_tan")]
//		public virtual int TipoAndamentoTan
//		{
//			get; set;
//		}	
		
//		// Propriedade: Codigo do TipoAndamento TAN
//		[Property("txt_tipo_andamento_tan")]
//		public virtual string TipoAndamentoTan
//		{
//			get; set;
//		}	
		
//		// Propriedade: TipoAndamento ATA
//		[Property("pk_cod_tipo_andamento_ata")]
//		public virtual int TipoAndamento
//		{
//			get; set;
//		}
		
		// Propriedade: NomeTipoAndamento TAN
		[Property("txt_tipo_andamento_tan")]
		public virtual string NomeTipoAndamento
		{
			get; set;
		}
		
		// Propriedade: DataCadastroProcesso
		[Property("data_cadastro_pan")]
		public virtual DateTime DataCadastroProcesso
		{
			get; set;
		}
		
		// Propriedade: DataCadastroProcesso
		[Property("data_ultimo_andamento")]
		public virtual DateTime DataUltimoAndamento
		{
			get; set;
		}
		

		// Propriedade: DataCadastroProcesso
		[Property("data_ultimo_tramitar_receber")]
		public virtual DateTime DataUltimoTramitarReceber
		{
			get; set;
		}

		
//		// Propriedade: PessoasDoProcesso
//		[Property("pessoas_processo")]
//		public virtual string PessoasDoProcesso
//		{
//			get; set;
//		}	
		
//		// Propriedade: Pessoa Cadastrante do Processo
//		[Property("pessoa_cadastrante_processo_ppp")]
//		public virtual int PessoaCadastranteProcesso
//		{
//			get; set;
//		}		
		
//		// Propriedade: Pessoa Autor do Processo
//		[Property("pessoa_autor")]
//		public virtual string PessoaAutor
//		{
//			get; set;
//		}	

		#endregion
	}
}

