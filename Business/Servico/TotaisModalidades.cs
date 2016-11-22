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
using Graficos.Graficos;
using Licitar.Business.Dao;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// serviço responsável pelos relatórios de Totais e Contagem de Processos Concluídos, 
	/// e seus respsctivos gráficos.
	/// </summary>
	public partial class TotaisModalidades : PostgreSqlDatabase
	{
		/// <summary>
		/// Construtor Padrão
		/// </summary>
		public TotaisModalidades()
		{
		}

        #region Numero de Processos Concluídos
		/// <summary>
		/// Conta e retorna todas as licitações concluídas agrupados por Modalidade
		/// </summary>
		public DataTable NumeroProcessosConcluidos()
		{			
	  		return NumeroProcessosConcluidos(null, null);
		}
		
		/// <summary>
		/// Conta e retorna todas as licitações concluídas agrupados por Modalidade
		/// e filtrado por período.
		/// </summary>
		/// <param name="AnoMesInicio">
		/// Um <see cref="System.String"/> com ano e mês, sem barra, de início do período.
		/// </param>
		/// <param name="AnoMesFim">
		/// Um <see cref="System.String"/> com ano e mês, sem barra, de final do período.
		/// </param>
		/// <returns>
		/// Um <see cref="IDataReader"/> com a lista de licitações.
		/// </returns>
		public DataTable NumeroProcessosConcluidos(string AnoMesInicio, string AnoMesFim)
		{			
			string sql = ConsultaXML.retornarSQL("ContagemConcluidosPorModalidade");
			string filtro = null;

			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo >= '" + AnoMesInicio + "'";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo <= '" + AnoMesFim + "'";
						
	  		return Consultar(sql.Replace("0=0", "0=0" + filtro)); 
		}
		
		/// <summary>
		/// Conta e retorna todas as licitações concluídas agrupados por Secretaria
		/// e filtrada por uma determinada Modalidade
		/// </summary>
		/// <param name="Modalidade">
		/// Um <see cref="System.String"/> com o nome da modalidade desejada
		/// </param>
		/// <returns>
		/// Um <see cref="IDataReader"/> com a lista de licitações.
		/// </returns>
		public DataTable NumeroConcluidosPorSecretaria(string Modalidade)
		{			
			return NumeroConcluidosPorSecretaria(Modalidade, null, null); 
		}
		
		/// <summary>
		/// Conta e retorna todas as licitações concluídas agrupados por Secretaria
		/// e filtrada por uma determinada Modalidade e período
		/// </summary>
		/// <param name="Modalidade">
		/// Um <see cref="System.String"/> com o nome da modalidade desejada
		/// </param>
		/// <param name="AnoMesInicio">
		/// Um <see cref="System.String"/> com ano e mês, sem barra, de início do período.
		/// </param>
		/// <param name="AnoMesFim">
		/// Um <see cref="System.String"/> com ano e mês, sem barra, de final do período.
		/// </param>
		/// <returns>
		/// Um <see cref="IDataReader"/> com a lista de licitações.
		/// </returns>
		public DataTable NumeroConcluidosPorSecretaria(string Modalidade, string AnoMesInicio, string AnoMesFim)
		{			
			string sql = ConsultaXML.retornarSQL("ContagemConcluidosPorSecretaria");
			string filtro = null;

			filtro = " AND txt_modalidade_mod = '" + Modalidade + "' ";
			
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo >= '" + AnoMesInicio + "'";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo <= '" + AnoMesFim + "'";
						
	  		return Consultar(sql.Replace("0=0", "0=0" + filtro)); 
		}
        #endregion
		
        #region Totais dos processos concluídos
		/// <summary>
		/// Calcula e retorna valores totais das licitações concluídas agrupado por modalidade
		/// </summary>
		/// <returns>
		/// Um <see cref="IDataReader"/> com a lista de licitações
		/// </returns>
		public DataTable TotaisProcessosConcluidos(bool auditado)
		{			
	  		return TotaisProcessosConcluidos(null, null, auditado);
		}
		
		/// <summary>
		/// Calcula e retorna valores totais das licitações concluídas agrupado por modalidade
		/// e filtrado por período
		/// </summary>
		/// <param name="AnoMesInicio">
		/// Um <see cref="System.String"/> representando o ano e mês de início sem barra
		/// </param>
		/// <param name="AnoMesFim">
		/// Um <see cref="System.String"/> representando o ano e mês de término sem barra
		/// </param>
		/// <returns>
		/// Um <see cref="IDataReader"/> com a lista de licitações
		/// </returns>
		public DataTable TotaisProcessosConcluidos(string AnoMesInicio, string AnoMesFim, bool auditado)
		{			
			string sql = ConsultaXML.retornarSQL("TotaisConcluidosPorModalidade");
			string filtro = null;

			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo >= '" + AnoMesInicio + "'";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo <= '" + AnoMesFim + "'";
			if (auditado)
				filtro += " AND boo_auditado_pro = true ";
			else
				filtro += " AND boo_auditado_pro = false ";

	  		return Consultar(sql.Replace("0=0", "0=0" + filtro)); 
		}
		
		/// <summary>
		/// Calcula e retorna valores totais das licitações concluídas agrupado por secretaria
		/// e filtrado por modalidade
		/// </summary>
		/// <param name="Modalidade">
		/// Um <see cref="System.String"/> com o nome da modalidade desejada
		/// </param>
		/// <returns>
		/// Um <see cref="IDataReader"/> com a lista de licitações
		/// </returns>
		public DataTable TotaisProcessosConcluidosSecretaria(string Modalidade, bool auditado)
		{			
			return TotaisProcessosConcluidosSecretaria(Modalidade, null, null, auditado); 
		}
		
		/// <summary>
		/// Calcula e retorna valores totais das licitações concluídas agrupado por secretaria
		/// e filtrado por modalidade
		/// </summary>
		/// <param name="Modalidade">
		/// Um <see cref="System.String"/> com o nome da modalidade desejada
		/// </param>
		/// <param name="AnoMesInicio">
		/// Um <see cref="System.String"/> representando o ano e mês de início sem barra
		/// </param>
		/// <param name="AnoMesFim">
		/// Um <see cref="System.String"/> representando o ano e mês de término sem barra
		/// </param>
		/// <returns>
		/// Um <see cref="IDataReader"/> com a lista de licitações
		/// </returns>
		public DataTable TotaisProcessosConcluidosSecretaria(string Modalidade, string AnoMesInicio, string AnoMesFim, bool auditado)
		{			
			string sql = ConsultaXML.retornarSQL("TotaisConcluidosPorSecretaria");
			string filtro = null;

			filtro = " AND upper(txt_modalidade_mod) = upper('" + Modalidade + "') ";
			
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo >= '" + AnoMesInicio + "'";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo <= '" + AnoMesFim + "'";
			if (auditado)
				filtro += " AND boo_auditado_pro = true ";

			Console.WriteLine("sql ="+sql);
	  		return Consultar(sql.Replace("0=0", "0=0" + filtro)); 
		}
		
		
		public DataTable TotaisProcessosConcluidosSecretaria(string modalidade, DateTime dtInicio, DateTime dtFim, bool auditado)
		{			
			
			String sql = @"select txt_natureza_nat as natureza, 
							count(*) as quantidade, 
							sum(num_processo_estimado_global) as estimado, 
							sum(num_processo_a_ser_contratado) as contratado,
							sum(num_nao_contratado_vpr) as semexito,
							sum(num_estimado_real_vpr) as estimadoreal,
							(case when sum(num_processo_a_ser_contratado) = 0 and sum(num_economia_vpr) = 0 then null else sum(num_economia_vpr)  end)  as economia,
	
							 (case when (sum(num_processo_estimado_global) - sum(num_nao_contratado_vpr)) <> 0 
							 then cast(((sum(num_economia_vpr) * 100) /  (sum(num_processo_estimado_global) - sum(num_nao_contratado_vpr))) as numeric (12,2))
							 else null end)
							  as economiaPorcentagem 
							from adm_licitar.tb_processo_completo_pcm
							where txt_estado_processo = 'FINALIZADO'
							@DATACONCLUSAO
							@AUDITADO
							@MODALIDADE
							group by txt_natureza_nat,cod_natureza_nat
							order by txt_natureza_nat,cod_natureza_nat";
			
			if (dtInicio != DateTime.MinValue && dtFim !=  DateTime.MinValue)
				sql = sql.Replace("@DATACONCLUSAO"," and dat_conclusao_pan between '"+ 
				                  dtInicio.ToString("yyyy-MM-dd") +" 00:00:00' and '"+ 
				                  dtFim.ToString("yyyy-MM-dd") +" 23:59:59' ");
			else
				sql = sql.Replace("@DATACONCLUSAO","");
			if (auditado)
				sql = sql.Replace("@AUDITADO"," AND boo_auditado_pro = " + auditado);
			else
				sql = sql.Replace("@AUDITADO",String.Empty);
			sql = sql.Replace("@MODALIDADE"," and  upper(txt_modalidade_mod) =  upper('" + modalidade + "')" );
						Console.WriteLine("natureza ="+sql);
			return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(sql).Tables[0];
			
			
	  		
		}
		
		
		
		
		
        #endregion
		
		//##### GRÁFICO: NÚMERO DE PROCESSOS CONCLUÍDOS
		
		/// <summary>
		/// Gera um gráfico de Pizza com o número de licitações concluídas por modalidade
		/// </summary>
		/// <returns>
		/// Um <see cref="System.String"/> representando o local e nome da imagem PNG gerada
		/// </returns>
		public string GraficoNumeroProcessosConcluidos()
		{
			string sql = ConsultaXML.retornarSQL("GraficoNumeroProcessos");
			
			Graficos.Graficos.GraficoPizza oGrafico = new GraficoPizza("Processos Concluídos", ConsultarDT(sql));
			
			oGrafico.Largura = 460;
			oGrafico.Altura = 300;
			oGrafico.TamanhoLegenda = 100;
			oGrafico.TamanhoFontTitulo = 10;
			
			return oGrafico.DesenharGrafico();
		}

		/// <summary>
		/// Gera um gráfico de Pizza com o número de licitações concluídas por tipo de conclusão
		/// filtrado por modalidade
		/// </summary>
		/// <param name="Modalidade">
		/// Um <see cref="System.String"/> com o nome da modalidade desejada
		/// </param>
		/// <returns>
		/// Um <see cref="System.String"/> representando o local e nome da imagem PNG gerada
		/// </returns>
		public string GraficoNumeroProcessosConcluidos(string Modalidade)
		{
			return GraficoNumeroProcessosConcluidos("Processos Concluídos: " + Modalidade, 
			                          Modalidade, null, null); 
		}

		/// <summary>
		/// Gera um gráfico de Pizza com o número de licitações concluídas por modalidade
		/// filtrado por período
		/// </summary>
		/// <param name="AnoMesInicio">
		/// Um <see cref="System.String"/> representando o ano e mês de início sem barra
		/// </param>
		/// <param name="AnoMesFim">
		/// Um <see cref="System.String"/> representando o ano e mês de término sem barra
		/// </param>
		/// <returns>
		/// Um <see cref="System.String"/> representando o local e nome da imagem PNG gerada
		/// </returns>
		public string GraficoNumeroProcessosConcluidos(string AnoMesInicio, string AnoMesFim)
		{
			return GraficoNumeroProcessosConcluidos("Processos Concluídos por Período: " 
                                      + AnoMesInicio + " a " + AnoMesFim, null, AnoMesInicio, AnoMesFim); 
		}

		/// <summary>
		/// Gera um gráfico de Pizza com o número de licitações concluídas por modalidade
		/// filtrado por período e modalidade
		/// </summary>
		/// <param name="Modalidade">
		/// Um <see cref="System.String"/> com o nome da modalidade desejada
		/// </param>
		/// <param name="AnoMesInicio">
		/// Um <see cref="System.String"/> representando o ano e mês de início sem barra
		/// </param>
		/// <param name="AnoMesFim">
		/// Um <see cref="System.String"/> representando o ano e mês de término sem barra
		/// </param>
		/// <returns>
		/// Um <see cref="System.String"/> representando o local e nome da imagem PNG gerada
		/// </returns>
		public string GraficoNumeroProcessosConcluidos(string Modalidade, string AnoMesInicio, string AnoMesFim)
		{
			return GraficoNumeroProcessosConcluidos("Processos Concluídos: " + Modalidade + " de " 
                                      + AnoMesInicio + " a " + AnoMesFim, Modalidade, null, null); 
		}

		/// <summary>
		/// Método protegido interno para geração do gráfico, especializado pelas diversas
		/// formas de geração do mesmo.
		/// </summary>
		/// <param name="titulo">
		/// Um <see cref="System.String"/> com o título desejado para o gráfico
		/// </param>
		/// <param name="Modalidade">
		/// Um <see cref="System.String"/> com o nome da modalidade desejada
		/// </param>
		/// <param name="AnoMesInicio">
		/// Um <see cref="System.String"/> representando o ano e mês de início sem barra
		/// </param>
		/// <param name="AnoMesFim">
		/// Um <see cref="System.String"/> representando o ano e mês de término sem barra
		/// </param>
		/// <returns>
		/// Um <see cref="System.String"/> representando o local e nome da imagem PNG gerada
		/// </returns>
		protected string GraficoNumeroProcessosConcluidos(string titulo, string Modalidade, string AnoMesInicio, string AnoMesFim)
		{
			string sql = ConsultaXML.retornarSQL("GraficoNumeroProcessos");
			string filtro = null;
					
			if (!String.IsNullOrEmpty(Modalidade)) 
				filtro += " AND txt_modalidade_mod = '" + Modalidade + "' ";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo >= '" + AnoMesInicio + "'";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo <= '" + AnoMesFim + "'";
			
			Graficos.Graficos.GraficoPizza oGrafico = new GraficoPizza(titulo, ConsultarDT(sql.Replace("0=0", "0=0" + filtro)));
			
			oGrafico.Largura = 500;
			oGrafico.Altura = 300;
			oGrafico.TamanhoFontTitulo = 10;
			
			return oGrafico.DesenharGrafico();
		}

		//##### GRÁFICO: TOTAIS DOS PROCESSOS CONCLUÍDOS
		
		/// <summary>
		/// Gera um gráfico de Pizza com o número de licitações concluídas por modalidade
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GraficoTotaisProcessosConcluidos()
		{
			return GraficoTotaisProcessosConcluidos("Economia por Modalidade", null, null, null); 
		}

		public string GraficoTotaisProcessosConcluidos(string Modalidade)
		{
			return GraficoTotaisProcessosConcluidos("Economia por Modalidade: " + Modalidade, 
			                          Modalidade, null, null); 
		}

		public string GraficoTotaisProcessosConcluidos(string AnoMesInicio, string AnoMesFim)
		{
			return GraficoTotaisProcessosConcluidos("Economia por Modalidade: Período de " 
                                      + AnoMesInicio + " a " + AnoMesFim, null, AnoMesInicio, AnoMesFim); 
		}

		public string GraficoTotaisProcessosConcluidos(string Modalidade, string AnoMesInicio, string AnoMesFim)
		{
			return GraficoTotaisProcessosConcluidos("Economia por Modalidade: " + Modalidade + " de " 
                                      + AnoMesInicio + " a " + AnoMesFim, Modalidade, null, null); 
		}

		protected string GraficoTotaisProcessosConcluidos(string titulo, string Modalidade, string AnoMesInicio, string AnoMesFim)
		{
			string sql = ConsultaXML.retornarSQL("GraficoTotaisConcluidosPorModalidade");
			string filtro = null;
					
			if (!String.IsNullOrEmpty(Modalidade)) 
				filtro += " AND txt_modalidade_mod = '" + Modalidade + "' ";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_inicio_processo >= '" + AnoMesInicio + "'";
			if (!string.IsNullOrEmpty(AnoMesInicio))
				filtro += " AND txt_ano_mes_final_processo <= '" + AnoMesFim + "'";
			
			Graficos.Graficos.GraficoPizza oGrafico = new GraficoPizza(titulo, ConsultarDT(sql.Replace("0=0", "0=0" + filtro)));
			
			oGrafico.Largura = 500;
			oGrafico.Altura = 300;
			oGrafico.TamanhoFontTitulo = 10;
			
			return oGrafico.DesenharGrafico();
		}

	}
}
