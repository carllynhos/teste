// DaoRelatorioGerencial.cs created with MonoDevelop
// User: wanialdo at 15:49 9/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Dao
{
	/// <summary>
	/// Classe responsável por fornecer os métodos e estruturas do relatório gerencial customizável.
	/// </summary>
	public partial class DAORelatorioGerencial : PostgreSqlDatabase
	{
		public DAORelatorioGerencial()
		{
		}

		/// <summary>
		/// Consulta os dados do relatório gerencial, com as opções escolhidas pelo usuário no 
		/// assistente de consulta.
		/// </summary>
		/// <returns>
		/// Um <see cref="IDataReader"/> contendo os dados retornados pelo banco.
		/// </returns>
		public DataTable listarTabelao(string campos, string filtros, string ordenacao, string group, List<int> listaIdsTipoAndamento)
		{				
  			string idsTipoAndamento = string.Empty;
			
			string sql = "SELECT pcm.pk_cod_processo_completo_pcm, " + campos + " FROM adm_licitar.tb_processo_completo_pcm pcm ";

			if (filtros != null && filtros != "")
			{
				if (listaIdsTipoAndamento.Count > 0)
				{
					for (int i = 0; i < listaIdsTipoAndamento.Count; i++)
						idsTipoAndamento += listaIdsTipoAndamento[i].ToString() + ",";

					idsTipoAndamento = idsTipoAndamento.Remove(idsTipoAndamento.Length - 1);
					
					sql += " JOIN adm_licitar.tb_processo_andamento_pan pan ON (pcm.cod_processo_pro = pan.fk_cod_processo_pro) ";
					sql += " JOIN adm_licitar.tb_fluxo_andamento_fan fan ON (fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan) ";
					sql += " JOIN adm_licitar.tb_atividade_ati ati ON (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati) ";

					filtros += " AND fan.fk_cod_atividade_ati IN (" + idsTipoAndamento + ") ";
				} 
				
				sql += " WHERE " + filtros;
			}
			
			if (group != null && group != "") 
				sql += " GROUP BY pcm.pk_cod_processo_completo_pcm, " + group;
			
			if (ordenacao != null && ordenacao != "") 
				sql += " ORDER BY " + ordenacao;				

			sql = sql.Replace("  ,  ", ",").Replace(" , ", ",").Replace(", ", ",").Replace(" ,", ",").Replace(",,", ",").Replace(",ORDER", " ORDER").Replace(",FROM", " FROM");
			
			Console.WriteLine("SELECTGERADO:" + sql);
			
			
			
			
			Console.WriteLine("sql ="+sql);
			
			return Consultar(sql);
		}
		
		/// <summary>
		/// Carrega e lista os campos do Relatório Gerencial.
		/// </summary>
		/// <returns>
		/// Um <see cref="camposRelatorioGerencial[]"/> que contém a coleção de campos disponíveis
		/// para o relatório.
		/// </returns>
		public camposRelatorioGerencial[] CamposRelatorio()
		{				
			//52 campos
			camposRelatorioGerencial[] dr = new camposRelatorioGerencial[51];
			camposRelatorioGerencial aux = null;
			int i = 0;

			aux = new camposRelatorioGerencial();
			aux.Titulo = "Instituição";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_descricao_ins";
			dr[i] = aux;
			i++;		
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Unidade de Exercício";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_descricao_uex";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Resumo do Objeto";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_observacao_pro";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Natureza";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_natureza_nat";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Modalidade";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_modalidade_mod";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Tipo da Licitação";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_tipo_licitacao_tli";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Número SPU";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_numero_spu_npr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Num. Proc. Licitação";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_numero_licitacao_npr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Num. Proc. ComprasNet";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_numero_comprasnet_npr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Num. Proc. SisBB";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_numero_sisbb_npr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Num. Proc. IG";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_numero_ig_npr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Nome Presidente/Pregoeiro";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_presidente_pregoeiro_pes";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Papel da Pessoa";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_papel_pap";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Situação Atual";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_situacao_atual_sit";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Último Andamento";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_ultimo_andamento_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Última Fase";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_ultima_fase_fas";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Estado do Processo";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_estado_processo";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Motivo da Conclusão";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_motivo_concluido";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Obs. Último Andamento";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_andamento_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vencedor";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_vencedor_pes";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data de Cadastro";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_cadastro_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Entrada na PGE";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_entrada_pge_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Realização";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_realizacao_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Abertura das Propostas";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_abertura_propostas_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Adjudicação";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_adjudicacao_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Homologação";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_homologado_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Conclusão";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_conclusao_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Devolução";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_devolucao_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data do Deserto";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_deserto_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Revogação";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_revogado_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data Fracassado";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_fracassado_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Aprovação";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_aprovacao_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Anulação";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_anulacao_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Abertura Proposta Comercial";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_sessao_abertura_proposta_comercial_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Abertura Proposta Técnica";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_sessao_abertura_proposta_tecnica_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Sessão Result. Prop. Comercial";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_sessao_resultado_proposta_comercial_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Sessão Result. Prop. Técnica";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_sessao_resultado_proposta_tecnica_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Data da Sessão Result. Habilitação";
			aux.Tipo = "DATA";
			aux.Campo = "pcm.dat_sessao_resultado_habilitacao_pan";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Ano/Mês do Início do Processo";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_ano_mes_inicio_processo";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Ano/Mês do Final do Processo";
			aux.Tipo = "TEXTO";
			aux.Campo = "pcm.txt_ano_mes_final_processo";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Não Contratado";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_nao_contratado_vpr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Estimado Real";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_estimado_real_vpr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Economia";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_economia_vpr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Economia Percent.";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_economia_porcent_vpr";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Estimado Global";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_processo_estimado_global";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. a ser Contratado";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_processo_a_ser_contratado";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Fracassado";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_processo_fracassado";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Deserto";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_processo_deserto";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Anulado";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_processo_anulado";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Revogado";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_processo_revogado";
			dr[i] = aux;
			i++;
			
			aux = new camposRelatorioGerencial();
			aux.Titulo = "Vlr. Cancelado";
			aux.Tipo = "NUMERO";
			aux.Campo = "pcm.num_processo_cancelado";
			dr[i] = aux;
			i++;
			
			return dr;
		}
	}
	
	/// <summary>
	/// Classe com a estrutura de armazenamento dos campso do relatório.
	/// </summary>
	public class camposRelatorioGerencial
	{
		protected string _titulo;
		public string Titulo
		{
			get { return _titulo; }
			set { _titulo = value; }
		}
		
		protected string _campo;
		public string Campo
		{
			get { return _campo; }
			set { _campo = value; }
		}

		protected string _tipo;
		public string Tipo
		{
			get { return _tipo; }
			set { _tipo = value; }
		}
		
		public camposRelatorioGerencial()
		{
		}
	}
	
	/// <summary>
	/// Classe com a estrutura de montagem dos filtros para o relatório gerencial.
	/// </summary>
	public class filtroRelatorioGerencial
	{
		public string Tipo {get; set;}  // Poder ser Texto, Data ou Numero.
		public int Modelo {get;set;}    // Para tipo Texto: 1-Contem, 2-Começa com, 3-Termina com.
		                                 // Para tipo Data/Numero: 1-Maior, 2-Maior ou igual, 3-Menor, 4-Menor ou igual, 5-Entre, 6-Igual

		/// <summary>
		/// Retorna a descrição para  modelo de comparativo para o filtro escolhido.
		/// </summary>
		/// <returns>
		/// Uma <see cref="System.String"/> que descreve o modelo.
		/// </returns>
		public string ModeloDescricao()
		{			
			switch(this.Modelo)
			{
			case 1: if(Tipo == "TEXTO") return "CONTÉM"; else return "MAIOR QUE";
			case 2: if(Tipo == "TEXTO") return "COMEÇA COM"; else return "MAIOR OU IGUAL QUE";
			case 3: if(Tipo == "TEXTO") return "TERMINA COM"; else return "MENOR QUE";
			case 4: return "MENOR OU IGUAL A";
			case 5: return "a";
			case 6: return "IGUAL A";
			case 7: return "É NULO";
			case 8: return "NÃO É NULO";
			default: return "";								
			}
		}
		
		public string Campo {get;set;}
		public string Titulo {get;set;}
		public string Inicio {get;set;} // É usado para todos os modelos de filtro
		public string Fim {get;set;}    // Só vai ter para data/numero do modelo 5
		
		public filtroRelatorioGerencial()
		{
			
		}
	}
}
