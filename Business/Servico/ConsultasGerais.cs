// ConsultasGerais.cs created with MonoDevelop
// User: wanialdo at 09:34 10/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

using Licitar.Business.Dao;
using Licitar.Business.Dto;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Agrupa todas as funções de consulta genéricas e padrão, baseadas na consulta ao XML 
	/// responsável por armazenar as instruções de consulta da aplicação.
	/// </summary>
	public partial class ConsultasGerais : PostgreSqlDatabase
	{
		
		public ConsultasGerais()
		{
		}

		public DataTable ListarUltimaFase(int IdProcesso)
		{			
	  		return Consultar(ConsultaXML.retornarSQL("UltimaFase").Replace("@processo",IdProcesso.ToString()));
		}
		public DataTable ListarUltimoAndamento(int IdProcesso)
		{			
	  		return Consultar(ConsultaXML.retornarSQL("UltimaAndamento").Replace("@processo",IdProcesso.ToString()));
		}
		
		public DataTable ListarIdUltimoAndamento(int IdProcesso)
		{			
	  		return Consultar(ConsultaXML.retornarSQL("idUltimaAndamento").Replace("@processo",IdProcesso.ToString()));
		}
		
		
		/// <summary>
		/// Retorna um DataReader com todas as Unidades de Exercício disponíveis.
		/// </summary>
		public DataTable ListarUnidadesExercicio()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("UnidadesExercicio"));
		}
		
		public DataTable ListarSubUnidadesExercicio(int fkUnidadeExercicio)
		{			
	  		return Consultar(ConsultaXML.retornarSQL("SubUnidadesExercicioNivel2").Replace("@fk_cod_unidade_exercicio_uex",fkUnidadeExercicio.ToString()));
		}
		
		public DataTable ListarPessoasUnidadesExercicio(int fkUnidadeExercicio)
		{			
	  		return Consultar(ConsultaXML.retornarSQL("PessoasDaUnidadeExercicio").Replace("@unidadeexercicio",fkUnidadeExercicio.ToString()));
		}
		
		public DataTable ListarTipoNumero()
		{			
			string sql = ConsultaXML.retornarSQL("TipoNumero");			
	  		return Consultar(sql);
		}
			
		public DataTable ListarIdTipoAndamento(string fase, string andamento)
		{			
			string sql = ConsultaXML.retornarSQL("IdTipoAndamento").Replace(":Q1",andamento).Replace(":Q2",fase);			
	  		return Consultar(sql);
		}
		
		public DataTable ListarUltAndamentoNTramitar( int idProcesso)
		{			
			string sql = ConsultaXML.retornarSQL("UltimoAndamentoCadNtramitar").Replace("@processo",idProcesso.ToString());			
	  		return Consultar(sql);
		}
		
		
		public DataTable ListarLicitacoes(string filtro)
		{			
			string sql = ConsultaXML.retornarSQL("Licitacao");
	  		return Consultar(sql.Replace(":Q1",filtro));
		}
		
		public DataTable ListarAndamentoPorFase(string filtro)
		{			
			string sql = ConsultaXML.retornarSQL("AndamentoPorFase");
	  		return Consultar(sql.Replace(":Q1",filtro));
		}
		
		public DataTable ListarUnidadesExercicioNivel2()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("UnidadesExercicioNivel2"));
		}
		
		public DataTable ListarUnidadesExercicioNivel3()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("UnidadesExercicioNivel3"));
		}
		
		public DataTable ListarPregoeirosPresidentesVicePresidentes()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("PregoeiroPresidenteVicePresidente"));
		}

		public DataTable ListarApoios()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("PessoasApoio"));
		}
				
		public DataTable ListarLicitacoesEmAndamentoPorPregeiro(DTORelLicitacoesAndamentoPorPregoeiro dto)
		{			
	  		string sql = ConsultaXML.retornarSQL("RelLicitacoesPorPregoeiro");
			string filtros = "";
			
			if (!string.IsNullOrEmpty(dto.IdUnidadeExercicioNivel1))
				filtros += " and cod_unidade_exercicio_uex = " + dto.IdUnidadeExercicioNivel1;
			
			if (!string.IsNullOrEmpty(dto.idPessoa))
				filtros += " and cod_presidente_pregoeiro_pes = " + dto.idPessoa;
			
			if (!string.IsNullOrEmpty(dto.Estados))			
				filtros += " and txt_estado_processo in (" + dto.Estados + ")";

			filtros += " and (txt_motivo_concluido is null OR txt_motivo_concluido = '') ";
		
			sql = sql.Replace("@FILTROS", filtros);
			
			
			return ConsultarDT(sql);
		}
		
		public DataTable ListarTotalLicitacoesEmAndamentoConcluido()
		{
			string sql = ConsultaXML.retornarSQL("TotalProcessosAndamentoConcluido");
			return Consultar(sql);
			
		}

		public DataTable ListarLicitacoesEmAndamentoPorPregeiro(DTORelLicitacoesAndamentoPorPregoeiro dto, string processos, string pessoas)
		{			
	  		string sql = ConsultaXML.retornarSQL("RelLicitacoesPorPregoeiro");
			string filtros = "";
			
			if (!string.IsNullOrEmpty(dto.IdUnidadeExercicioNivel1))
				filtros += " and cod_unidade_exercicio_uex = " + dto.IdUnidadeExercicioNivel1;
			
			
			if (!string.IsNullOrEmpty(dto.Estados))			
				filtros += " and txt_estado_processo in (" + dto.Estados + ")";

			if(!string.IsNullOrEmpty(processos))
			{
				filtros += " and cod_processo_pro IN ("+processos+") ";
			}

			filtros += " and (txt_motivo_concluido is null OR txt_motivo_concluido = '') ";
		
			sql = sql.Replace("@FILTROS", filtros);
			
			
			return ConsultarDT(sql);
		}
		
		public DataTable DadosPorColuna(string campo)
		{			
			string sql = ConsultaXML.retornarSQL("DadosPorColuna");					

	  		return Consultar(sql.Replace(":Q1", campo));
		}
		
		public DataTable DadosPorColunaLicitacao(string campo,string codigo)
		{			
			
			string sql = ConsultaXML.retornarSQL("DadosPorColunaLicitacao");					

	  		return Consultar(sql.Replace(":Q1", campo).Replace(":Q2", codigo));
		}
		
		/// <summary>
		/// Retorna um DataReader com todos os Motivos de Conclusão
		/// </summary>
		public DataTable ListarMotivosConclusao()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("MotivoConclusao"));
		}
		
		/// <summary>
		/// Retorna um DataReader com todas as Instituicoes. 
		/// </summary>
		public DataTable ListarInstituicoes()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("Instituicoes"));
		}
		
		/// <summary>
		/// Retorna um DataReader com todas as Instituicoes. 
		/// </summary>
		public DataTable ListarNatureza()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("Natureza"));
		}

		/// <summary>
		/// Retorna um DataTable com as naturezas filtradas para o relatório de economia. 
		/// </summary>
		public DataTable ListarNaturezaEconomia()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("Natureza").Replace("0=0","0=0 AND txt_natureza_nat not in ('Concorrencia Nacional','Concurso','Leilao')"));
		}
		
		/// <summary>
		/// Retorna um DataReader com todas as modalidades que aparecem no tabelão.
		/// </summary>
		public DataTable ListarModalidades()
		{			
	  		return Consultar(ConsultaXML.retornarSQL("ModalidadesDoTabelao").Replace("0=0","0=0 AND txt_modalidade_mod <> 'SDP-SBQC' AND txt_modalidade_mod <> 'Manifestação de Interesse'"));
		}
		
	
		
		/// <summary>
		/// Consulta os andamentos das licitações, filtrado por Unidade de Exercício e por Modalidade.
		/// </summary>
		/// <param name="unidadeExercicio">
		/// Um número inteiro que representa o ID da Unidade de Exercício desejada.
		/// </param>
		/// <param name="Modalidade">
		/// Um número inteiro que representa o ID da Modalidade desejada.
		/// </param>
		/// <returns>
		/// Um DataReader com os dados retornados pelo banco.
		/// </returns>
		public DataTable AndamentoLicitacoes(string unidadeExercicio, string Modalidade)
		{			
			string sql = ConsultaXML.retornarSQL("AndamentoLicitacoes");
			
			if(!string.IsNullOrEmpty(unidadeExercicio))
			{
				sql+=" AND cod_unidade_exercicio_uex IN( "+unidadeExercicio+")";
			}
			if(!string.IsNullOrEmpty(Modalidade))
			{
				sql+=" AND cod_modalidade_mod IN ("+Modalidade+")";
			}
			
			sql+=" Order by txt_descricao_ins";
			
					
	  		return Consultar(sql);
		}
		
		public DataTable EconomiaPregao(EcononomiaPregao objEconomiaPregao, string tipoConclusao)
		{
			string ano = objEconomiaPregao.ano;
			string dataInicio = objEconomiaPregao.dataInicio.ToString("dd/MM/yyyy");
			string dataFim = objEconomiaPregao.dataFim.ToString("dd/MM/yyyy");
			string instituicao = objEconomiaPregao.instituicao;
			string modalidade = objEconomiaPregao.modalidade;
			string natureza = objEconomiaPregao.natureza;
						
			string sql = ConsultaXML.retornarSQL("EconomiaPregao");
			
			if(!string.IsNullOrEmpty(ano) || dataFim!="01/01/0001" || dataInicio!="01/01/0001" || !string.IsNullOrEmpty(instituicao) || !string.IsNullOrEmpty(modalidade) || !string.IsNullOrEmpty(natureza))
			{
					
				if(!string.IsNullOrEmpty(ano))
				{
					sql+=" AND extract(year from dat_realizacao_pan) = " + ano;
					
				}
				else if(dataInicio != "01/01/0001" && dataFim!="01/01/0001")
				{
					dataFim = objEconomiaPregao.dataFim.AddDays(1).ToString("dd/MM/yyyy");
					
					sql+=" AND dat_realizacao_pan Between '"+dataInicio+"' AND '"+dataFim+"' ";
					
				}
				
				if(!string.IsNullOrEmpty(instituicao))
				{
					sql+=" AND cod_instituicao_ins IN ("+instituicao+") ";
					
				}
				
				if(!string.IsNullOrEmpty(modalidade))
				{
					sql+=" AND cod_modalidade_mod IN ("+modalidade+") ";
					
				}
				
				if(!string.IsNullOrEmpty(natureza))
				{
					sql+=" AND cod_natureza_nat IN ("+natureza+") ";
					
				}
			}
			

			sql += " AND txt_motivo_concluido = '" + tipoConclusao +"'";
			
			sql += " ORDER BY txt_descricao_ins, txt_modalidade_mod, dat_realizacao_pan";
			
					
	  		return Consultar(sql);
		}
		

		public DataTable RelEconomiaPregao()
		{
			string sql = ConsultaXML.retornarSQL("RelEstruturaCentralLicitacoesGrid");
			return Consultar(sql);
			
			
		}
		
		
		
		public DataTable EconomiaPregao(EcononomiaPregao objEconomiaPregao)
		{
			string ano = objEconomiaPregao.ano;
			string dataInicio = objEconomiaPregao.dataInicio.ToString("dd/MM/yyyy");
			string dataFim = objEconomiaPregao.dataFim.ToString("dd/MM/yyyy");
			string instituicao = objEconomiaPregao.instituicao;
			string modalidade = objEconomiaPregao.modalidade;
			string natureza = objEconomiaPregao.natureza;
						
			string sql = ConsultaXML.retornarSQL("EconomiaPregao");
			
			if(!string.IsNullOrEmpty(ano) || dataFim!="01/01/0001" || dataInicio!="01/01/0001" || !string.IsNullOrEmpty(instituicao) || !string.IsNullOrEmpty(modalidade) || !string.IsNullOrEmpty(natureza))
			{
					
				if(!string.IsNullOrEmpty(ano))
				{
					sql+=" AND extract(year from dat_realizacao_pan) = " + ano;
					
				}
				else if(dataInicio != "01/01/0001" && dataFim!="01/01/0001")
				{
					dataFim = objEconomiaPregao.dataFim.AddDays(1).ToString("dd/MM/yyyy");
					
					sql+=" AND dat_realizacao_pan Between '"+dataInicio+"' AND '"+dataFim+"' ";
					
				}
				
				if(!string.IsNullOrEmpty(instituicao))
				{
					sql+=" AND cod_instituicao_ins IN ("+instituicao+") ";
					
				}
				
				if(!string.IsNullOrEmpty(modalidade))
				{
					sql+=" AND cod_modalidade_mod IN ("+modalidade+") ";
					
				}
				
				if(!string.IsNullOrEmpty(natureza))
				{
					sql+=" AND cod_natureza_nat IN ("+natureza+") ";
					
				}
			}

			sql += " ORDER BY txt_descricao_ins, txt_modalidade_mod, dat_realizacao_pan";

			
	  		return Consultar(sql);
		}
		
		
		

		public DataTable ListarUnidadesAdministrativasPorInstituicao(string filtro)
		{			
			string sql = ConsultaXML.retornarSQL("UnidadeAdministrativaPorInstituicao");
			
	  		return Consultar(sql.Replace(":Q1",filtro));
		}


		public DataTable ListarUniaoInstituicaoUnidadeAdministrativa()
		{
			string sql = ConsultaXML.retornarSQL("UnidadeAdministrativaUniaoInstituicao");
	  		return Consultar(sql);
		}
	}
}
