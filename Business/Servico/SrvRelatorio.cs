// SrvRelatorio.cs created with MonoDevelop
// User: guilhermefacanha at 18:30 28/4/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Licitar.Business.Entidade;
using Licitar.Business.Utilidade;
using System.Data;
using Licitar.Business.Persistencia;
using System.Web;
using Npgsql;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using Licitar.Business.Entidade;
using NHibernate.Expression;

namespace Licitar.Business.Servico
{
	public struct structUnidade
	{
		public string unidade{get;set;}
		public List<ListItem> listaSubUnidades{get;set;}

		public structUnidade(bool t) : this()
		{
			this.unidade = "";			
			this.listaSubUnidades = new List<ListItem>();
		}
	}
	
	public struct structGridSubUnidade
	{
		public string unidade{get;set;}
		public DataTable dt{get;set;}

		public structGridSubUnidade(bool t) : this()
		{
			this.unidade = "";
			this.dt = new DataTable();
		}
	}
	
	public struct structGridUnidade
	{
		public string unidade{get;set;}
		public List<structGridSubUnidade> lista{get;set;}

		public structGridUnidade(bool t) : this()
		{
			this.unidade = "";
			this.lista = new List<structGridSubUnidade>();
		}
	}
	
	public class SrvRelatorio
	{
		public DataTable getSessionConsultaGerencialLicitacoesConcluidas()
		{
			return (DataTable)HttpContext.Current.Session["consultaGerenciaLicitacoesConcluidas"];	
		}
		public void setSessionConsultaGerencialLicitacoesConcluidas(DataTable dt)
		{
			HttpContext.Current.Session["consultaGerenciaLicitacoesConcluidas"] = dt;
		}

		public DataTable listarProcessosEmAndamentoPorModalidade(ListBox ltb)
		{
			List<int> lista = new List<int>();
			foreach(ListItem item in ltb.Items)
				lista.Add(int.Parse(item.Value));

			string instituicoes = SrvGerais.transformarListaEmString(lista);
			
			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.CommandText = @"SELECT 
							pcm.txt_modalidade_mod as modalidade, COUNT(pcm.cod_processo_pro) as quant
							FROM adm_licitar.tb_processo_completo_pcm pcm
							INNER JOIN adm_licitar.tb_tramitacao_unidade_exercicio_tue tue ON tue.fk_cod_processo_pro = pcm.cod_processo_pro
							WHERE pcm.txt_situacao_atual_sit not ilike ('%CONCLU%')";
			if(!string.IsNullOrEmpty(instituicoes))
				 cmd.CommandText+= " AND cod_instituicao_ins IN("+instituicoes+") ";
			
			cmd.CommandText+=" GROUP BY modalidade ";
			DataTable dt = new PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];

			return dt;
		}

	
		
		private List<structUnidade> getSubUnidadesAgrupadas(ListItemCollection listaSubUnid)
		{
			List<structUnidade> listaUnidades = new List<structUnidade>();
			bool existeUnidade = false;
			
			foreach(ListItem item in listaSubUnid)
			{
				string unidadePai = UnidadeExercicio.Find(Convert.ToInt32(item.Value)).UnidadeExercicioPai.Descricao;	
				foreach(structUnidade obj in listaUnidades)
				{
					if(obj.unidade == unidadePai)
					{
						obj.listaSubUnidades.Add(item);
						existeUnidade = true;
					}
				}
				if(!existeUnidade)
				{
					structUnidade obj = new structUnidade();
					obj.unidade = unidadePai;
					List<ListItem> lista = new List<ListItem>();
					lista.Add(item);
					obj.listaSubUnidades = lista;
					listaUnidades.Add(obj);
				}
				existeUnidade = false;
			}

			return listaUnidades;
		}

		private List<structUnidade> getTodasSubUnidadesAgrupadas()
		{
			List<structUnidade> listaUnidades = new List<structUnidade>();
			bool existeUnidade = false;

			string select = @"
					SELECT uex2.txt_descricao_uex as unidade, uex.txt_descricao_uex as subunidade, uex.pk_cod_unidade_exercicio_uex as cod
					FROM adm_licitar.tb_unidade_exercicio_uex uex
					LEFT JOIN adm_licitar.tb_unidade_exercicio_uex uex2 ON uex2.pk_cod_unidade_exercicio_uex = uex.fk_cod_unidade_exercicio_uex
					WHERE uex.fk_cod_unidade_exercicio_uex is not null
					ORDER BY uex2.txt_descricao_uex, uex.txt_descricao_uex
					";

			DataTable dt = new PostgreSqlDatabase().Consultar(select);

			string unidadeAtual = "";
			string proxUnidade = "";

			structUnidade objStructUnidade = new structUnidade();
			ListItem item;
			int cont = 0;
			int quant = dt.Rows.Count;

			foreach(DataRow row in dt.Rows)
			{
				cont++;
				item = new ListItem(row["subunidade"].ToString(),row["cod"].ToString());
				proxUnidade = row["unidade"].ToString();
								
				if(string.IsNullOrEmpty(unidadeAtual))
				{
					unidadeAtual = row["unidade"].ToString();
					objStructUnidade = new structUnidade(true);
					objStructUnidade.unidade = unidadeAtual;
					objStructUnidade.listaSubUnidades.Add(item);
				}
				else if(unidadeAtual != proxUnidade)
				{
					unidadeAtual = proxUnidade;
					listaUnidades.Add(objStructUnidade);
					objStructUnidade = new structUnidade(true);
					objStructUnidade.unidade = proxUnidade;
					objStructUnidade.listaSubUnidades.Add(item);
				}
				else if(unidadeAtual==proxUnidade)
				{
					objStructUnidade.listaSubUnidades.Add(item);
				}

				if(cont==quant && unidadeAtual==proxUnidade)
				{
					listaUnidades.Add(objStructUnidade);					
				}
				
			}

			return listaUnidades;
		}

		public DataTable consultarPgRelatorioPorInstituicao(ListBox ltbInstituicao, ListBox ltbModalidade, string dataInicial, string dataFinal, string totalDias, string operacao)
		{
			DataTable dt = new DataTable();
			try {
				if(string.IsNullOrEmpty(dataFinal))
					dataFinal = DateTime.Now.ToString("dd/MM/yyyy") + " 23:59:59";
					
						string instituicoes = SrvGerais.transformarListaEmStringValor(ltbInstituicao);
						string modalidades = SrvGerais.transformarListaEmStringValor(ltbModalidade);
						NpgsqlCommand cmd = new NpgsqlCommand();
						cmd.CommandText =  @"
								SELECT
								pcm.cod_processo_pro as cod,
								pcm.txt_descricao_ins as instituicao,
								pcm.txt_modalidade_mod as modulo,
								uex.txt_descricao_uex as subunidade,
								uex.pk_cod_unidade_exercicio_uex as codsubunidade,
								uex2.txt_descricao_uex as unidade,
								pcm.txt_situacao_atual_sit as situacao,
								pcm.txt_numero_licitacao_npr as licitacao,
								(case when pcm.txt_numero_spu_npr is null then pcm.txt_numero_viproc_npr else pcm.txt_numero_spu_npr end ) as spu,
								pcm.txt_observacao_pro as resumo,
								pcm.dat_realizacao_pan as datarealizacao,
								pcm.num_estimado_real_vpr as valorestimado,
								date(now()) - date(pcm.dat_entrada_pge_pan) AS totaldias,
								pcm.txt_numero_sisbb_npr as sisbb,
								pcm.txt_numero_comprasnet_npr as comprasnet,
								date(now()) - date(tue.dat_tramitacao_tue) as diasunidade,
								extract(day from adm_licitar.fn_calculo_tempo_setorial(pcm.cod_processo_pro::numeric,'0001/01/01'::date,@dataFinal::date)) as diasencaminhado 
								FROM adm_licitar.tb_processo_completo_pcm pcm
								INNER JOIN adm_licitar.tb_tramitacao_unidade_exercicio_tue tue ON tue.fk_cod_processo_pro = pcm.cod_processo_pro
								INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = tue.fk_cod_unidade_exercicio_uex
								LEFT JOIN adm_licitar.tb_unidade_exercicio_uex uex2 ON uex2.pk_cod_unidade_exercicio_uex = uex.fk_cod_unidade_exercicio_uex
								WHERE tue.pk_cod_tramitacao_unidade_exercicio_tue = 
								(
								SELECT pk_cod_tramitacao_unidade_exercicio_tue
								FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
								WHERE fk_cod_processo_pro = pcm.cod_processo_pro
								ORDER BY dat_tramitacao_tue DESC, pk_cod_tramitacao_unidade_exercicio_tue DESC
								LIMIT 1
								)
								
								";				
			
						if(!string.IsNullOrEmpty(instituicoes))
							cmd.CommandText += " AND pcm.cod_instituicao_ins IN("+instituicoes+") ";
			
						if(!string.IsNullOrEmpty(modalidades))
							cmd.CommandText += " AND pcm.cod_modalidade_mod IN("+modalidades+") ";
						
						if(string.IsNullOrEmpty(dataFinal) && string.IsNullOrEmpty(dataInicial))
						{
							cmd.CommandText += " AND pcm.txt_situacao_atual_sit not ilike ('%CONCLU%') ";
						}
						else
						{
							cmd.CommandText += " AND ((pcm.dat_conclusao_pan > @dataFinal AND pcm.txt_ultimo_andamento_pan ilike ('%CONCLU%')) OR pcm.txt_ultimo_andamento_pan not ilike ('%CONCLU%')) ";
						}
			
						if(!string.IsNullOrEmpty(totalDias) && !string.IsNullOrEmpty(operacao))
						{				
							if(operacao.Equals("1"))
								cmd.CommandText += " AND date(now()) - date(pcm.dat_entrada_pge_pan) > @totalDias ";
							else if(operacao.Equals("2"))
								cmd.CommandText +=" AND date(now()) - date(pcm.dat_entrada_pge_pan) = @totalDias ";
							else if(operacao.Equals("3"))
								cmd.CommandText +=" AND date(now()) - date(pcm.dat_entrada_pge_pan) < @totalDias ";
						}
						cmd.CommandText +=" ORDER BY instituicao, modulo, unidade, subunidade, situacao, licitacao, spu, resumo, datarealizacao ";						
						
						cmd.Parameters.Add("@totalDias",totalDias);				
						cmd.Parameters.Add("@dataFinal", dataFinal);
				
						dt = new PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
				} catch (Exception ex) 
				{
					throw new Exception(ex.Message);
				}
			return dt;
		}

		public static bool VerificarData(string dtTxt)
		{
			bool retorno = false;
			DateTime dtOut;
			if(DateTime.TryParse(dtTxt, out dtOut))
				retorno = true;
			return retorno;
		}

		public static bool VerificarInteiro(string intTxt)
		{
			bool retorno = false;
			int intOut;
			if(int.TryParse(intTxt, out intOut))
				retorno = true;
			return retorno;
		}

		public static string VerificarString(string strTxt)
		{			
			string strOut = "E'" + strTxt.Replace("'","\\'");		
			return strOut;
		}
		
		public decimal VerificaNumeroDecimal(string numero)
		{
			decimal numeroFinal = 0;
			
			if(decimal.TryParse(numero,out numeroFinal))
				numeroFinal = numeroFinal;
			return numeroFinal;	
		}

		public DataTable consultarPgRelatorioGerencialLicitacoesConcluidas(ListBox ltbInstituicao, ListBox ltbModalidade, ListBox ltbNatureza, string dataInicial, string dataFinal, string cnpj)
		{
			string instituicoes = SrvGerais.transformarListaEmStringValor(ltbInstituicao);
			string modalidades = SrvGerais.transformarListaEmStringValor(ltbModalidade);
			string naturezas = SrvGerais.transformarListaEmStringValor(ltbNatureza);

			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.CommandText = @"
					SELECT
					pcm.cod_processo_pro as cod,
					pcm.txt_descricao_ins as instituicao,
					pcm.txt_modalidade_mod as modulo,
					pcm.txt_situacao_atual_sit as situacao,
					pcm.txt_numero_licitacao_npr as licitacao,
					pcm.txt_numero_spu_npr as spu,
					pcm.txt_observacao_pro as resumo,
					pcm.txt_presidente_pregoeiro_pes as pregoeiro,
					pcm.txt_vencedor_pes as vencedor,
					pcm.num_processo_a_ser_contratado as valorcontratado,
					pcm.num_processo_fracassado as valorfracassado,
					pcm.num_economia_vpr as economia,
					pcm.num_economia_porcent_vpr as economiapercentual,
					pcm.dat_realizacao_pan as datarealizacao,
					pcm.num_processo_estimado_global as valorestimado,
					pcm.num_estimado_real_vpr as estimadoreal,
					pes.txt_cpf_cnpj_pes as cnpj,
					
					-- valores sem exito:
					pcm.num_processo_fracassado,		
					pcm.num_nao_contratado_vpr,		
					pcm.num_processo_anulado,
					pcm.num_processo_deserto,
					pcm.num_processo_revogado,
					pcm.num_processo_cancelado

					FROM adm_licitar.tb_processo_completo_pcm pcm
					LEFT JOIN adm_licitar.tb_pessoa_pes pes ON pes.pk_cod_pessoa_pes::text = pcm.cod_vencedor_pes	
					WHERE 
					txt_estado_processo='FINALIZADO'
				
					
					";

			if(!string.IsNullOrEmpty(instituicoes))				
				cmd.CommandText += " AND pcm.cod_instituicao_ins IN("+instituicoes+") ";

			if(!string.IsNullOrEmpty(modalidades))
				cmd.CommandText += " AND pcm.cod_modalidade_mod IN("+modalidades+") ";

			if(!string.IsNullOrEmpty(naturezas))
				cmd.CommandText += " AND pcm.cod_natureza_nat IN("+naturezas+") ";

			if (!string.IsNullOrEmpty(dataFinal) && !string.IsNullOrEmpty(dataInicial))
				cmd.CommandText += " AND (pcm.dat_conclusao_pan > @dataInicial AND pcm.dat_conclusao_pan <@dataFinal) "; 

			if(!string.IsNullOrEmpty(cnpj))
				cmd.CommandText += " AND pes.txt_cpf_cnpj_pes = @cnpj ";

			cmd.Parameters.Add("@dataInicial",dataInicial);			
			cmd.Parameters.Add("@dataFinal",dataFinal);
			cmd.Parameters.Add("@cnpj",cnpj);
			
			cmd.CommandText += " ORDER BY instituicao, modulo, situacao, licitacao, spu, resumo, datarealizacao ";
			DataTable dt = new PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
			
			return dt;
		}
		
		public List<structGridUnidade> consultarPgRelatorioUnidade(ListItemCollection listaUnidades, string totalDias, string operacao, string data)
		{
			string select = string.Empty;
			
			DataTable dt = null;
						
			List<structGridUnidade> listaGeral = new List<structGridUnidade>();
			List<structUnidade> listaSubUnidadesAgrupadas = listaUnidades.Count > 0 ? this.getSubUnidadesAgrupadas(listaUnidades) : this.getTodasSubUnidadesAgrupadas();

			foreach(structUnidade objStructUnidade in listaSubUnidadesAgrupadas)
			{
				structGridUnidade objStructGridUnidade = new structGridUnidade();
				objStructGridUnidade.unidade = objStructUnidade.unidade;
				
				List<structGridSubUnidade> listaStructGriSubUnidade = new List<structGridSubUnidade>();
				
				foreach(ListItem item in objStructUnidade.listaSubUnidades)
				{
					structGridSubUnidade objStructGridSubUnidade = new structGridSubUnidade();
					
					objStructGridSubUnidade.unidade = item.Text;

					NpgsqlCommand cmd = new NpgsqlCommand();
					cmd.CommandTimeout = 240;
					cmd.CommandText = @"
					SELECT
					pcm.cod_processo_pro as cod,
					pcm.txt_descricao_ins as instituicao,
					pcm.txt_modalidade_mod as modulo,
					uex.txt_descricao_uex as subunidade,
					uex2.txt_descricao_uex as unidade,
					pcm.txt_situacao_atual_sit as situacao,
					pcm.txt_numero_licitacao_npr as licitacao,
					(case when txt_numero_spu_npr is null then txt_numero_viproc_npr else txt_numero_spu_npr end ) as spu,
					pcm.txt_observacao_pro as resumo,
					pcm.dat_realizacao_pan as datarealizacao,
					pcm.txt_ultima_fase_fas as fase,
					tue.dat_tramitacao_tue as datatram, 
					pcm.num_estimado_real_vpr as valorestimado,
					date(now()) - date(pcm.dat_entrada_pge_pan) AS totaldias,
					pcm.txt_numero_sisbb_npr as sisbb,
					pcm.txt_numero_comprasnet_npr as comprasnet,
					date(now()) - date(tue.dat_tramitacao_tue) as diasunidade,
					CASE WHEN(tue.dat_recebido_setorial_tue is not null)
					THEN 
					date(tue.dat_recebido_setorial_tue) - date(tue.dat_encaminhado_setorial_tue)
					ELSE
					date(now()) - date(tue.dat_encaminhado_setorial_tue)
					END as diasencaminhado
					FROM adm_licitar.tb_processo_completo_pcm pcm
					INNER JOIN adm_licitar.tb_tramitacao_unidade_exercicio_tue tue ON tue.fk_cod_processo_pro = pcm.cod_processo_pro
					INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = tue.fk_cod_unidade_exercicio_uex
					LEFT JOIN adm_licitar.tb_unidade_exercicio_uex uex2 ON uex2.pk_cod_unidade_exercicio_uex = uex.fk_cod_unidade_exercicio_uex
					WHERE

					uex.pk_cod_unidade_exercicio_uex = @unidade

				
					";
								
					if(string.IsNullOrEmpty(data.Trim()))
					{
						cmd.CommandText +=@" and  pcm.txt_estado_processo <> 'FINALIZADO'
																AND tue.pk_cod_tramitacao_unidade_exercicio_tue =
																(
																SELECT pk_cod_tramitacao_unidade_exercicio_tue
																FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
																WHERE fk_cod_processo_pro = pcm.cod_processo_pro
																ORDER BY dat_tramitacao_tue DESC, pk_cod_tramitacao_unidade_exercicio_tue DESC
																LIMIT 1
																) ";
					}
					else
					{	
						cmd.CommandText +=@" and (pcm.dat_conclusao_pan  is null OR pcm.dat_conclusao_pan > @data)
																AND tue.pk_cod_tramitacao_unidade_exercicio_tue =
																(
																SELECT pk_cod_tramitacao_unidade_exercicio_tue
																FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
																WHERE fk_cod_processo_pro = pcm.cod_processo_pro
																AND dat_tramitacao_tue < @data
																ORDER BY dat_tramitacao_tue DESC, pk_cod_tramitacao_unidade_exercicio_tue DESC
																LIMIT 1
																) ";
						
						
					}

					if(!string.IsNullOrEmpty(totalDias) && !string.IsNullOrEmpty(operacao))
					{
						if(operacao.Equals("1"))
							cmd.CommandText+= " AND date(now()) - date(pcm.dat_entrada_pge_pan) > @totalDias ";
						else if(operacao.Equals("2"))
							cmd.CommandText+= " AND date(now()) - date(pcm.dat_entrada_pge_pan) = @totalDias ";
						else if(operacao.Equals("3"))
							cmd.CommandText+= " AND date(now()) - date(pcm.dat_entrada_pge_pan) < @totalDias ";						
					}
									

					cmd.CommandText +=" ORDER BY instituicao, modulo, unidade, subunidade, situacao, licitacao, spu, resumo, datarealizacao ";

					cmd.Parameters.Add("@unidade",item.Value);
					cmd.Parameters.Add("@data",data+" 23:59:59 ");
					cmd.Parameters.Add("@totalDias",totalDias);

					dt = new PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];

					objStructGridSubUnidade.dt = dt;
					
					listaStructGriSubUnidade.Add(objStructGridSubUnidade);					
				}

				objStructGridUnidade.lista = listaStructGriSubUnidade;
				listaGeral.Add(objStructGridUnidade);
			}

			return listaGeral;
		}

		public List<structGridUnidade> consultarPgRelatorioUnidadePorModalidade(string modalidade, ListBox ltb)
		{
			string select = string.Empty;

			List<string> lista = new List<string>();
			foreach(ListItem item in ltb.Items)
				lista.Add(item.Value);

			string instituicoes = SrvGerais.transformarListaEmString(lista);
			
			DataTable dt = null;
						
			List<structGridUnidade> listaGeral = new List<structGridUnidade>();
			List<structUnidade> listaSubUnidadesAgrupadas = this.getTodasSubUnidadesAgrupadas();

			foreach(structUnidade objStructUnidade in listaSubUnidadesAgrupadas)
			{
				structGridUnidade objStructGridUnidade = new structGridUnidade();
				objStructGridUnidade.unidade = objStructUnidade.unidade;
				
				List<structGridSubUnidade> listaStructGriSubUnidade = new List<structGridSubUnidade>();
				
				foreach(ListItem item in objStructUnidade.listaSubUnidades)
				{
					structGridSubUnidade objStructGridSubUnidade = new structGridSubUnidade();

					if(item.Text.ToUpper().Contains("PREGOEIRO"))
					{
						SrvUnidadeExercicioFuncaoPessoa objSrvUEFP = new SrvUnidadeExercicioFuncaoPessoa();
						UnidadeExercicioFuncaoPessoa[] pessoas = objSrvUEFP.ListarPregoeirosDaSubUnidade(Convert.ToInt32(item.Value));
						if(pessoas.Length>0)
							item.Text+=" ("+pessoas[0].Pessoa.Nome+")";						
					}
					
					objStructGridSubUnidade.unidade = item.Text;

					NpgsqlCommand cmd = new NpgsqlCommand();
					
					cmd.CommandText = @"
					SELECT
					pcm.cod_processo_pro as cod,
					pcm.txt_descricao_ins as instituicao,
					pcm.txt_modalidade_mod as modulo,
					uex.txt_descricao_uex as subunidade,
					uex2.txt_descricao_uex as unidade,
					pcm.txt_situacao_atual_sit as situacao,
					pcm.txt_numero_licitacao_npr as licitacao,
					pcm.txt_numero_spu_npr as spu,
					pcm.txt_observacao_pro as resumo,
					pcm.dat_realizacao_pan as datarealizacao,
					pcm.num_estimado_real_vpr as valorestimado,
					date(now()) - date(pcm.dat_entrada_pge_pan) AS totaldias,
					pcm.txt_numero_sisbb_npr as sisbb,
					pcm.txt_numero_comprasnet_npr as comprasnet,
					date(now()) - date(tue.dat_tramitacao_tue) as diasunidade,
					CASE WHEN(tue.dat_recebido_setorial_tue is not null)
					THEN 
					date(tue.dat_recebido_setorial_tue) - date(tue.dat_encaminhado_setorial_tue)
					ELSE
					date(now()) - date(tue.dat_encaminhado_setorial_tue)
					END as diasencaminhado
					FROM adm_licitar.tb_processo_completo_pcm pcm
					INNER JOIN adm_licitar.tb_tramitacao_unidade_exercicio_tue tue ON tue.fk_cod_processo_pro = pcm.cod_processo_pro
					INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = tue.fk_cod_unidade_exercicio_uex
					LEFT JOIN adm_licitar.tb_unidade_exercicio_uex uex2 ON uex2.pk_cod_unidade_exercicio_uex = uex.fk_cod_unidade_exercicio_uex
					WHERE pcm.txt_situacao_atual_sit not ilike ('%CONCLU%')
					AND tue.pk_cod_tramitacao_unidade_exercicio_tue = 
					(
					SELECT pk_cod_tramitacao_unidade_exercicio_tue
					FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
					WHERE fk_cod_processo_pro = pcm.cod_processo_pro
					ORDER BY dat_tramitacao_tue DESC, pk_cod_tramitacao_unidade_exercicio_tue DESC
					LIMIT 1
					)
					AND uex.pk_cod_unidade_exercicio_uex = @unidade
					AND pcm.txt_modalidade_mod = @modalidade


					";
					
					

					if(lista.Count!=0)
						cmd.CommandText += " AND cod_instituicao_ins IN("+instituicoes+") ";



					cmd.Parameters.Add("@unidade",item.Value);
					cmd.Parameters.Add("@modalidade",modalidade);
					cmd.CommandText += " ORDER BY instituicao, modulo, unidade, subunidade, situacao, licitacao, spu, resumo, datarealizacao ";
					dt = new PostgreSqlDatabase().Consultar(select);

					objStructGridSubUnidade.dt = dt;
					
					listaStructGriSubUnidade.Add(objStructGridSubUnidade);					
				}

				objStructGridUnidade.lista = listaStructGriSubUnidade;
				
				listaGeral.Add(objStructGridUnidade);
			}

			return listaGeral;
		}
		
		public DataSet ListarRelatorioLicitacoesMarcadas(string dataLicitacaoMarcada, List<string> listIdInstituicao, List<string> listIdModalidade)
		{		
			DateTime? data = null;
			string listaIdInstituicao = string.Empty;
			string listaIdModalidade = string.Empty;
			
			if (dataLicitacaoMarcada != "")
			{
				if (UtdValidador.ValidarData(dataLicitacaoMarcada))
				{
					data = Convert.ToDateTime(dataLicitacaoMarcada);
				}
				else
				{
					throw new Exception("Data Inválida.");
				}
			}

			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.CommandText = @"				
					SELECT distinct
				txt_numero_spu_npr as numero_spu,
				txt_numero_licitacao_npr as numero_licitacao,
				txt_observacao_pro as objeto,
				txt_descricao_ins as instituicao,
				txt_modalidade_mod as modalidade,
				dat_realizacao_pan as data_realizacao,
				num_processo_estimado_global as valor_estimado
				FROM 
				adm_licitar.tb_processo_completo_pcm pcm

				inner join adm_licitar.tb_processo_andamento_pan pan on pan.fk_cod_processo_pro = pcm.cod_processo_pro
				inner join adm_licitar.tb_agenda_age age on age.fk_cod_processo_andamento_pan = pan.pk_cod_processo_andamento_pan
				
				where 
				age.fk_cod_processo_andamento_remarcado_pan is null 
			";		
			
			if (data != null)
			{
				cmd.CommandText += " AND date(dat_agenda_pcm) >= date(@data_licitacao_marcada) ";				
			}			
			
			if (listIdInstituicao.Count != 0)
			{				
				listaIdInstituicao = listIdInstituicao[0];
				for (int i = 1; i < listIdInstituicao.Count; i++)
					listaIdInstituicao += "," + listIdInstituicao[i];				
				
				cmd.CommandText += " AND cod_instituicao_ins IN ("+listaIdInstituicao+") ";
			}			
			
			if (listIdModalidade.Count > 0)
			{				
				listaIdModalidade = listIdModalidade[0];
				for (int i = 1; i < listIdModalidade.Count; i++)
					listaIdModalidade += "," + listIdModalidade[i];	
				
				cmd.CommandText += " AND cod_modalidade_mod IN ("+listaIdModalidade+") ";				 
			}
			
			cmd.CommandText += " ORDER BY instituicao ASC, modalidade ASC ";		

			cmd.Parameters.Add("@data_licitacao_marcada",data.ToString());

			return new PostgreSqlDatabase().ExecutarConsulta(cmd);
		}


		public DataSet ListarRelatorioGerencialLicitacoesMarcadas(string dataLicitacaoMarcada, string IdUnidadeAdministrativa, string IdModalidade, string resumoObjeto, string tipoData)
		{		
			DateTime? data = null;
			
			if (dataLicitacaoMarcada != "" && dataLicitacaoMarcada != "dd/mm/aaaa")
			{
				if (UtdValidador.ValidarData(dataLicitacaoMarcada))
				{
					data = Convert.ToDateTime(dataLicitacaoMarcada);
				}
				else
				{
					throw new Exception("Data Inválida.");
				}
			}

			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.CommandText = @"					

				SELECT distinct
				txt_numero_spu_npr as numero_spu,
				txt_numero_licitacao_npr as numero_licitacao,
				txt_observacao_pro as objeto,
				txt_descricao_ins as instituicao,
				txt_modalidade_mod as modalidade,
				dat_realizacao_pan as data_realizacao,
				num_processo_estimado_global as valor_estimado
				FROM 
				adm_licitar.tb_processo_completo_pcm pcm

				inner join adm_licitar.tb_processo_andamento_pan pan on pan.fk_cod_processo_pro = pcm.cod_processo_pro
				inner join adm_licitar.tb_agenda_age age on age.fk_cod_processo_andamento_pan = pan.pk_cod_processo_andamento_pan
				inner join adm_licitar.tb_unidade_administrativa_uad uad on uad.fk_cod_instituicao_ins = pcm.cod_instituicao_ins

				where 
				age.fk_cod_processo_andamento_remarcado_pan is null 


			";		
			
			if (data != null)
			{
				if(tipoData=="DATA SELECIONADA")
				{
					cmd.CommandText += " AND date(dat_agenda_pcm) = date(@data_licitacao_marcada) ";					
				}
				else
				{
					cmd.CommandText += " AND date(dat_agenda_pcm) >= date(@data_licitacao_marcada) ";
				}
			}			
			
			
			if (!string.IsNullOrEmpty(IdModalidade))
			{											
				cmd.CommandText += " AND cod_modalidade_mod IN ("+IdModalidade+") ";
			}
			if (!string.IsNullOrEmpty(IdUnidadeAdministrativa))
				cmd.CommandText += " AND pk_cod_unidade_administrativa_uad IN ("+IdUnidadeAdministrativa+") ";


			if(!string.IsNullOrEmpty(resumoObjeto))
				cmd.CommandText += " and txt_observacao_pro ilike @resumoObjeto";
			
			cmd.CommandText += " ORDER BY instituicao ASC, modalidade ASC ";

			
			
			cmd.Parameters.Add("@resumoObjeto", "%"+resumoObjeto+"%");
			cmd.Parameters.Add("@data_licitacao_marcada", data.ToString());


			return new PostgreSqlDatabase().ExecutarConsulta(cmd);
		}
		
		public DataTable RetornaSqlConsultaRelatorioAteLicitacaoMarcada(string unidadeAdministrativa, string modalidade, string valor1, string valor2, string andamentos)
		{
			
			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.CommandText = @" 
			select distinct pcm.* from (select distinct cod_processo_pro as processo, txt_numero_spu_npr as NumeroSPU, txt_descricao_ins as instituicao, txt_numero_licitacao_npr as NumeroLicitacao, num_processo_estimado_global as ValorEstimado,
			dat_andamento_realizacao_pan as DataRealizacao, pcm.cod_instituicao_ins, pcm.cod_modalidade_mod, pcm.txt_ultima_fase_fas, pcm.txt_ultimo_andamento_pan, pcm.num_processo_estimado_global, pcm.txt_andamento_pan as Observacao
			 
			 from adm_licitar.tb_processo_completo_pcm pcm ) as pcm,
			 (select pk_cod_unidade_administrativa_uad,fk_cod_instituicao_ins from adm_licitar.tb_unidade_administrativa_uad uad) as uad 			
			where uad.fk_cod_instituicao_ins = pcm.cod_instituicao_ins	  
			  ";
			
			if(!string.IsNullOrEmpty(unidadeAdministrativa) && unidadeAdministrativa != "Todos" && unidadeAdministrativa != "0")
				cmd.CommandText +=" and uad.pk_cod_unidade_administrativa_uad in ("+unidadeAdministrativa+")";
			
			if(!string.IsNullOrEmpty(modalidade) && modalidade != "Todos" && modalidade != "0")
				cmd.CommandText +=" and pcm.cod_modalidade_mod in  ("+modalidade+")";
			
			cmd.CommandText +=" and ( 0=0 ";

			if(!string.IsNullOrEmpty(valor1))
				cmd.CommandText +=" and trim(to_char(num_processo_estimado_global,'999G999G999G999G999G999D99'))::money "+valor1;			
				
			
			if(!string.IsNullOrEmpty(valor2))
				cmd.CommandText +=" and trim(to_char(num_processo_estimado_global,'999G999G999G999G999G999D99'))::money "+valor2;			
			
			cmd.CommandText +=" ) ";

			
			if(!string.IsNullOrEmpty(andamentos))
				cmd.CommandText+=andamentos;
			return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
		}
		
		
		
		public string RetornaSqlConsultaLicitacoes(TextBox txtDataInicial,TextBox txtDataFinal, ListBox ltbAndamento,CheckBox CheckUltimo,
		     DropDownList ddlTipoNumero,TextBox txtNumero,ListBox ltbUnidadeAdm,ListBox ltbModalidade,DropDownList ddlNatureza,DropDownList ddlTipoLicitacao
		     ,DropDownList ddlPregoeiro,TextBox txtResumoObjeto,ref Panel pnlGrid, bool diferenciador)
		{


		    txtDataInicial.Text = txtDataInicial.Text.Replace("dd/mm/aaaa",string.Empty);
			txtDataFinal.Text = txtDataFinal.Text.Replace("dd/mm/aaaa",string.Empty);
		   
		   
			string sql="";
			string filtroInst="";
			string filtroNumero="";
			string filtroAndamento="";
			string filtroModalidade="";
			string filtroFase="";
			string filtroNatureza="";
			string filtroTipoLicitacao="";
			string filtroPregoeiro="";
			string filtroResumo="";
			string filtroUnidadeAdministrativa="";
			string filtroPeriodo="";
			string IdTipoAndamento="";
			string andamentos = string.Empty;
			string fases = string.Empty;
			if(ltbAndamento.Items.Count > 0 && ((ListItem) ltbAndamento.Items[0]).Value != "Todos#Todos")
			{
				if(!CheckUltimo.Checked)
				{
					andamentos +=" and (";
					for(int i=0; i<ltbAndamento.Items.Count;i++) 
					{
						
						string[] faseAndamento = ((ListItem) ltbAndamento.Items[i]).Value.Split('#');
						andamentos += " (";
						if(faseAndamento[0].ToString().Trim() != "Todos")
							andamentos +=" fas.txt_descricao_fas = '"+faseAndamento[0].ToString().Trim();
						if(faseAndamento[1].ToString().Trim() != "Todos")
							andamentos+= "' and ati.txt_descricao_ati ='"+faseAndamento[1].ToString().Trim();
						andamentos+="')";
						if(i<ltbAndamento.Items.Count-1)
						{
							andamentos +=" or ";	
						}
						else
							andamentos +=")";
					}

				}
				else
				{
					andamentos +=" and (";
					for(int i=0; i<ltbAndamento.Items.Count;i++) 
					{
						string[] faseAndamento = ((ListItem) ltbAndamento.Items[i]).Value.Split('#');
						andamentos += " (";
						if(faseAndamento[0].ToString().Trim() != "Todos")
							andamentos +=" Fase = '"+faseAndamento[0].ToString().Trim();
						if(faseAndamento[1].ToString().Trim() != "Todos")
							andamentos+= "' and UltimoAndamento ='"+faseAndamento[1].ToString().Trim();
						andamentos+="')";
						if(i<ltbAndamento.Items.Count-1)
						{
							andamentos +=" or ";	
						}
						else							
							andamentos +=")";
					}
				}
			}
			
			if (ddlTipoNumero.SelectedItem != null)
			{
				if(ddlTipoNumero.SelectedItem.Text != "Todos")
				{
					sql+=@" inner join adm_licitar.tb_numero_processo_npr npr on npr.fk_cod_processo_pro = pcm.cod_processo_pro 
                        inner join adm_licitar.tb_tipo_numero_tnu tnu on tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu ";
						filtroNumero += "  (txt_descricao_tnu = '" + ddlTipoNumero.SelectedItem.Text + "')";
				}
				else
				{
					sql += "  ";
				}
			}
			else
			{
				sql += "  ";
			}
				
			if(!string.IsNullOrEmpty(txtDataInicial.Text) && !string.IsNullOrEmpty(txtDataFinal.Text) )
			{
				
				if(CheckUltimo.Checked)
				{
					filtroPeriodo= andamentos+" and pcm.dat_ultimo_andamento_pan between '"+txtDataInicial.Text+" 00:00:00' and '"+txtDataFinal.Text+" 23:59:59' ";				
				}
				else
				{
					sql+=@" , ( select pan2.fk_cod_processo_pro, (case when max(pan2.dat_andamento_pan) is not null and max(pan2.dat_andamento_pan) <> '-infinity' then max(pan2.dat_andamento_pan) else max(pan2.dat_cadastro_pan) end) as data
                	from adm_licitar.tb_processo_andamento_pan pan2 
                 	inner join adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan2.fk_cod_fluxo_andamento_fan 
                 	inner join adm_licitar.tb_fase_fas fas on fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
                 	inner join adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
				 	where (1=1) "+andamentos+ @" group by  pan2.fk_cod_processo_pro)  as filtros  ";
					filtroPeriodo+=@" filtros.data between '"+txtDataInicial.Text+" 00:00:00' and '"+txtDataFinal.Text+" 23:59:59' and Processo = filtros.fk_cod_processo_pro ";
				}				
			}
			
			if(!string.IsNullOrEmpty(txtNumero.Text))
			{
				filtroNumero += " and (npr.txt_numero_processo_npr = E'" + txtNumero.Text.Replace("'","\\'") + "') ";
			}
			
			if (ltbUnidadeAdm.Items.Count >0)
			{
				string instituicaoTodas=string.Empty;
				string unidadeAdm=string.Empty;
				

				List<string> unid = new List<string>();
				List<string> inst = new List<string>();
					
					for(int i=0; i<ltbUnidadeAdm.Items.Count;i++)
						{
							string[] insti = ltbUnidadeAdm.Items[i].Text.Split('-');
							if(ltbUnidadeAdm.Items[i].Value == "Todos")
							{
								
								if(insti[0].Trim() != "Todos")
						        {
									inst.Add(insti[0].Trim());
								}
							}
					        else
							{
								unid.Add(ltbUnidadeAdm.Items[i].Value);
								
							}						        
						}	


						for(int i=0; i<inst.Count;i++)
						{
							if(i==0)
					        	instituicaoTodas+=" instituicao in (";
							instituicaoTodas += "'"+inst[i]+"'";
							if(i<inst.Count-1)
								instituicaoTodas +=",";
				        }
						for(int i=0; i<unid.Count;i++)
						{
							if(i==0)
					        	unidadeAdm+=" fkunidadeadministrativa in (";
							unidadeAdm += unid[i];
							if(i<unid.Count-1)
								unidadeAdm +=",";
				        }


	
				if(!string.IsNullOrEmpty(instituicaoTodas) || !string.IsNullOrEmpty(unidadeAdm))
				{
					filtroInst+="(";
				}
				if(!string.IsNullOrEmpty(instituicaoTodas))
				{
					instituicaoTodas += ")";
					filtroInst+=instituicaoTodas;
				}
				if(!string.IsNullOrEmpty(unidadeAdm))
				{
					unidadeAdm +=")";
					if(!string.IsNullOrEmpty(instituicaoTodas))
						filtroInst+=" or ";	
                    filtroInst+=unidadeAdm;
				}
				if(!string.IsNullOrEmpty(instituicaoTodas) || !string.IsNullOrEmpty(unidadeAdm))
				{
					filtroInst+=")";
				}
				
	
					
			}
			
			if (ltbModalidade.Items.Count >0 && ltbModalidade.Items[0].Text != "Todos")
			{
				filtroModalidade += "(";
				foreach(ListItem li in ltbModalidade.Items)
				{
					if(filtroModalidade.Length > 1)
						filtroModalidade += " OR ";
							
					filtroModalidade += " UPPER(txt_modalidade_mod) = UPPER('" + li.Text + "')";
				}
				
				filtroModalidade += ")";
			}
			sql += " where (1=1) ";
			if (!string.IsNullOrEmpty(sql)  && !CheckUltimo.Checked) 
			{
				if(string.IsNullOrEmpty(txtDataInicial.Text) && string.IsNullOrEmpty(txtDataFinal.Text))
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=@" and  exists
                 (select distinct pan2.fk_cod_processo_pro from adm_licitar.tb_processo_andamento_pan pan2 
                 inner join adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan2.fk_cod_fluxo_andamento_fan 
                 inner join adm_licitar.tb_fase_fas fas on fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
                 inner join adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati 
                 where (1=1) and pcm.Processo = pan2.fk_cod_processo_pro "+andamentos+")";				  
				}
				else
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=" and "+filtroPeriodo;
				}

			}
			if (ddlNatureza.SelectedItem.Text != "Todos") filtroNatureza = "(UPPER(txt_natureza_nat) = UPPER('" + ddlNatureza.SelectedItem.Text + "'))";
			if (ddlTipoLicitacao.SelectedItem.Text != "Todos") filtroTipoLicitacao = "( UPPER(txt_tipo_licitacao_tli) = UPPER('" + ddlTipoLicitacao.SelectedItem.Text + "'))";
			if (ddlPregoeiro.SelectedItem.Text != "Todos") filtroPregoeiro = "( UPPER(txt_presidente_pregoeiro_pes) = UPPER('" + ddlPregoeiro.SelectedItem.Text + "'))";
			if (!string.IsNullOrEmpty(txtResumoObjeto.Text)) filtroResumo = "(UPPER(txt_observacao_pro) ilike UPPER(E'%" + txtResumoObjeto.Text.Replace("'","\\'") + "%'))";
			
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroNumero)) sql += " AND ";
				sql += filtroNumero;			
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroInst)) sql += " AND ";
				sql += filtroInst;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroAndamento)) sql += " AND ";
				sql += filtroAndamento;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroModalidade)) sql += " AND ";
				sql += filtroModalidade; 
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroFase)) sql += " AND ";
				sql += filtroFase;			
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroNatureza)) sql += " AND ";
				sql += filtroNatureza;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroTipoLicitacao)) sql += " AND ";
				sql += filtroTipoLicitacao;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroPregoeiro)) sql += " AND ";
				sql += filtroPregoeiro;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroResumo)) sql += " AND ";
				sql += filtroResumo;

			
			if(!string.IsNullOrEmpty(sql)  && CheckUltimo.Checked)
			{
				if(string.IsNullOrEmpty(txtDataInicial.Text) && string.IsNullOrEmpty(txtDataFinal.Text) )
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=andamentos;				  
				}
				else
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=filtroPeriodo;
				}		
			}

			pnlGrid.Visible = false;
			sql+=" order by Instituicao,unidadeadministrativa,Modalidade,TipoLicitacao ";
			return sql;
		}

		public DataTable RetornaSqlConsultaLicitacoes(TextBox txtDataInicial,TextBox txtDataFinal, ListBox ltbAndamento,CheckBox CheckUltimo,
		     DropDownList ddlTipoNumero,TextBox txtNumero,ListBox ltbUnidadeAdm,ListBox ltbModalidade,DropDownList ddlNatureza,DropDownList ddlTipoLicitacao
		     ,DropDownList ddlPregoeiro,TextBox txtResumoObjeto,ref Panel pnlGrid)
		{

			NpgsqlCommand cmd = new NpgsqlCommand();
			
		    txtDataInicial.Text = txtDataInicial.Text.Replace("dd/mm/aaaa",string.Empty);
			txtDataFinal.Text = txtDataFinal.Text.Replace("dd/mm/aaaa",string.Empty);
		     ConsultasGerais cg = new ConsultasGerais();
			SrvProcesso srvProcesso = new SrvProcesso();


			string selectInicial = @" select * from (select distinct pc.* ,unidadeadministrativa.txt_descricao_tua as unidadeadministrativa,unidadeadministrativa.fk_cod_unidade_administrativa_uad as fkunidadeadministrativa from ( SELECT distinct cod_processo_pro as Processo, 
				(case when txt_numero_spu_npr is null then txt_numero_viproc_npr else txt_numero_spu_npr end ) as NumeroSPU, 
				txt_descricao_ins as Instituicao, 				
				txt_numero_licitacao_npr as NumeroLicitacao, 
				txt_modalidade_mod as Modalidade,
				txt_tipo_licitacao_tli as TipoLicitacao, 
				txt_ultimo_andamento_pan as UltimoAndamento,
				txt_observacao_pro as Objeto, 
				txt_ultima_fase_fas as Fase, 
				txt_situacao_atual_sit as Situacao,
				dat_realizacao_pan as DataRealizacao,
				dat_ultimo_andamento_pan as DataAndamento, 
				txt_andamento_pan as Observacao, 
				num_processo_estimado_global as ValorEstimado, 
				num_processo_a_ser_contratado as ValorContratado,
				num_processo_fracassado as ValorFracassado, 
				num_nao_contratado_vpr as ValorNaoContratado,
				num_estimado_real_vpr as ValorEstimadoReal,
				num_economia_vpr as Economia, 
				num_economia_porcent_vpr as EconomiaPorcentagem, 
				txt_vencedor_pes as Vencedor,
				num_processo_deserto as ValorDeserto, 
				num_processo_anulado as ValorAnulado,
				cod_processo_pro ,
  				cod_instituicao_ins ,
  				txt_descricao_ins ,
  				cod_area_are,
  				txt_descricao_are ,
  				cod_unidade_exercicio_uex ,
  				txt_descricao_uex ,
  				txt_observacao_pro ,
				cod_natureza_nat ,
				txt_natureza_nat ,
				cod_modalidade_mod ,
				txt_modalidade_mod ,
				cod_tipo_licitacao_tli ,
				txt_tipo_licitacao_tli ,
				txt_numero_spu_npr ,
				txt_numero_licitacao_npr ,
				txt_numero_comprasnet_npr ,
				txt_numero_sisbb_npr ,
				txt_numero_ig_npr ,
				num_processo_estimado_global ,
				num_processo_a_ser_contratado ,
				num_processo_fracassado ,
				num_processo_deserto ,
				num_processo_anulado ,
				num_processo_revogado ,
				num_processo_cancelado ,
				cod_presidente_pregoeiro_pes ,
				txt_presidente_pregoeiro_pes ,
				cod_papel_pap ,
				txt_papel_pap ,
				dat_cadastro_pan ,
				dat_entrada_pge_pan ,
				dat_realizacao_pan ,
				dat_abertura_propostas_pan ,
				dat_adjudicacao_pan ,
				dat_homologado_pan ,
				dat_conclusao_pan ,
				dat_devolucao_pan ,
				dat_deserto_pan ,
				dat_revogado_pan ,
				dat_fracassado_pan ,
				dat_aprovacao_pan ,
				dat_anulacao_pan ,
				dat_sessao_abertura_proposta_comercial_pan ,
				dat_sessao_abertura_proposta_tecnica_pan ,
				dat_sessao_resultado_proposta_comercial_pan ,
				dat_sessao_resultado_proposta_tecnica_pan ,
				dat_sessao_resultado_habilitacao_pan,
				txt_situacao_atual_sit ,
				cod_ultimo_andamento_pan,
				txt_ultimo_andamento_pan ,
				cod_ultima_fase_fas ,
				txt_ultima_fase_fas ,
				txt_estado_processo ,
				txt_motivo_concluido ,
				txt_andamento_pan ,
				txt_vencedor_pes ,
				cod_vencedor_pes ,
				num_nao_contratado_vpr ,
				num_estimado_real_vpr ,
				num_economia_vpr ,
				num_economia_porcent_vpr ,
				txt_ano_mes_inicio_processo ,
				txt_ano_mes_final_processo,
				dat_ultimo_andamento_pan ,
				cod_sub_unidade_exercicio_uex ,
				txt_descricao_sub_unidade_uex ,
				cod_unidade_exercicio_ultima_tramitacao_tue ,
				dat_unidade_exercicio_ultima_tramitacao_tue ,
				dat_recebido_setorial_tue ,
				dat_encaminhado_setorial_tue ,
				txt_vencedor_lote_vpr ,
				txt_ultimo_cadastrante_pes 
				
		   FROM adm_licitar.tb_processo_completo_pcm) as pc,(select pk_cod_processo_pro,fk_cod_unidade_administrativa_uad,txt_descricao_tua from adm_licitar.tb_processo_pro pro
		   left join adm_licitar.tb_instituicao_ins ins on ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins
		   left join adm_licitar.tb_unidade_administrativa_uad uad on uad.pk_cod_unidade_administrativa_uad = pro.fk_cod_unidade_administrativa_uad		
		   left join adm_licitar.tb_tipo_unidade_administrativa_tua tua on tua.pk_cod_tipo_unidade_administrativa_tua = uad.fk_cod_tipo_unidade_administrativa_tua
		) as unidadeadministrativa
		where pc.cod_processo_pro = unidadeadministrativa.pk_cod_processo_pro) as pcm ";

			
			string sql="";
			string filtroInst="";
			string filtroNumero="";
			string filtroAndamento="";
			string filtroModalidade="";
			string filtroFase="";
			string filtroNatureza="";
			string filtroTipoLicitacao="";
			string filtroPregoeiro="";
			string filtroResumo="";
			string filtroUnidadeAdministrativa="";
			string filtroPeriodo="";
			string IdTipoAndamento="";
			string andamentos = string.Empty;
			string fases = string.Empty;
			if(ltbAndamento.Items.Count > 0 && ((ListItem) ltbAndamento.Items[0]).Value != "Todos#Todos")
			{
				if(!CheckUltimo.Checked)
				{
					andamentos +=" and (";
					for(int i=0; i<ltbAndamento.Items.Count;i++) 
					{
						
						string[] faseAndamento = ((ListItem) ltbAndamento.Items[i]).Value.Split('#');
						andamentos += " (";
						if(faseAndamento[0].ToString().Trim() != "Todos")
							andamentos +=" fas.txt_descricao_fas = '"+faseAndamento[0].ToString().Trim();
						if(faseAndamento[1].ToString().Trim() != "Todos")
							andamentos+= "' and ati.txt_descricao_ati ='"+faseAndamento[1].ToString().Trim();
						andamentos+="')";
						if(i<ltbAndamento.Items.Count-1)
						{
							andamentos +=" or ";	
						}
						else
							andamentos +=")";
					}

				}
				else
				{
					andamentos +=" and (";
					for(int i=0; i<ltbAndamento.Items.Count;i++) 
					{
						string[] faseAndamento = ((ListItem) ltbAndamento.Items[i]).Value.Split('#');
						andamentos += " (";
						if(faseAndamento[0].ToString().Trim() != "Todos")
							andamentos +=" Fase = '"+faseAndamento[0].ToString().Trim();
						if(faseAndamento[1].ToString().Trim() != "Todos")
							andamentos+= "' and UltimoAndamento ='"+faseAndamento[1].ToString().Trim();
						andamentos+="')";
						if(i<ltbAndamento.Items.Count-1)
						{
							andamentos +=" or ";	
						}
						else							
							andamentos +=")";
					}
				}
			}
			
			if (ddlTipoNumero.SelectedItem != null)
			{
				if(ddlTipoNumero.SelectedItem.Text != "Todos")
				{
					sql+=@" inner join adm_licitar.tb_numero_processo_npr npr on npr.fk_cod_processo_pro = pcm.cod_processo_pro 
                        inner join adm_licitar.tb_tipo_numero_tnu tnu on tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu ";
						filtroNumero += "  (txt_descricao_tnu = '" + ddlTipoNumero.SelectedItem.Text + "')";
				}
				else
				{
					sql += "  ";
				}
			}
			else
			{
				sql += "  ";
			}
				
			if(!string.IsNullOrEmpty(txtDataInicial.Text) && !string.IsNullOrEmpty(txtDataFinal.Text) )
			{
				
				if(CheckUltimo.Checked)
				{
					filtroPeriodo= andamentos+" and pcm.dat_ultimo_andamento_pan between @txtDataInicial.Text and @txtDataFinal.Text ";				
				}
				else
				{
					sql+=@" , ( select pan2.fk_cod_processo_pro, (case when max(pan2.dat_andamento_pan) is not null and max(pan2.dat_andamento_pan) <> '-infinity' then max(pan2.dat_andamento_pan) else max(pan2.dat_cadastro_pan) end) as data
                	from adm_licitar.tb_processo_andamento_pan pan2 
                 	inner join adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan2.fk_cod_fluxo_andamento_fan 
                 	inner join adm_licitar.tb_fase_fas fas on fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
                 	inner join adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
				 	where (1=1) "+andamentos+ @" group by  pan2.fk_cod_processo_pro)  as filtros  ";
					
					filtroPeriodo+=@" filtros.data between @txtDataInicial.Text and @txtDataFinal.Text and Processo = filtros.fk_cod_processo_pro ";
				}				
			}
			
			if(!string.IsNullOrEmpty(txtNumero.Text))
			{
				filtroNumero += " and (npr.txt_numero_processo_npr = @txtNumero.Text ) ";
			}
			
			if (ltbUnidadeAdm.Items.Count >0)
			{
				string instituicaoTodas=string.Empty;
				string unidadeAdm=string.Empty;
				

				List<string> unid = new List<string>();
				List<string> inst = new List<string>();
					
					for(int i=0; i<ltbUnidadeAdm.Items.Count;i++)
						{
							string[] insti = ltbUnidadeAdm.Items[i].Text.Split('-');
							if(ltbUnidadeAdm.Items[i].Value == "Todos")
							{
								
								if(insti[0].Trim() != "Todos")
						        {
									inst.Add(insti[0].Trim());
								}
							}
					        else
							{
								unid.Add(ltbUnidadeAdm.Items[i].Value);
								
							}						        
						}	


						for(int i=0; i<inst.Count;i++)
						{
							if(i==0)
					        	instituicaoTodas+=" instituicao in (";
							instituicaoTodas += "'"+inst[i]+"'";
							if(i<inst.Count-1)
								instituicaoTodas +=",";
				        }
						for(int i=0; i<unid.Count;i++)
						{
							if(i==0)
					        	unidadeAdm+=" fkunidadeadministrativa in (";
							unidadeAdm += unid[i];
							if(i<unid.Count-1)
								unidadeAdm +=",";
				        }


	
				if(!string.IsNullOrEmpty(instituicaoTodas) || !string.IsNullOrEmpty(unidadeAdm))
				{
					filtroInst+="(";
				}
				if(!string.IsNullOrEmpty(instituicaoTodas))
				{
					instituicaoTodas += ")";
					filtroInst+=instituicaoTodas;
				}
				if(!string.IsNullOrEmpty(unidadeAdm))
				{
					unidadeAdm +=")";
					if(!string.IsNullOrEmpty(instituicaoTodas))
						filtroInst+=" or ";	
                    filtroInst+=unidadeAdm;
				}
				if(!string.IsNullOrEmpty(instituicaoTodas) || !string.IsNullOrEmpty(unidadeAdm))
				{
					filtroInst+=")";
				}
				
	
					
			}
			
			if (ltbModalidade.Items.Count >0 && ltbModalidade.Items[0].Text != "Todos")
			{
				filtroModalidade += "(";
				foreach(ListItem li in ltbModalidade.Items)
				{
					if(filtroModalidade.Length > 1)
						filtroModalidade += " OR ";
							
					filtroModalidade += " UPPER(txt_modalidade_mod) = UPPER('" + li.Text + "')";
				}
				
				filtroModalidade += ")";
			}
			sql += " where (1=1) ";
			if (!string.IsNullOrEmpty(sql)  && !CheckUltimo.Checked) 
			{
				if(string.IsNullOrEmpty(txtDataInicial.Text) && string.IsNullOrEmpty(txtDataFinal.Text))
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=@" and  exists
                 (select distinct pan2.fk_cod_processo_pro from adm_licitar.tb_processo_andamento_pan pan2 
                 inner join adm_licitar.tb_fluxo_andamento_fan fan on fan.pk_cod_fluxo_andamento_fan = pan2.fk_cod_fluxo_andamento_fan 
                 inner join adm_licitar.tb_fase_fas fas on fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
                 inner join adm_licitar.tb_atividade_ati ati on ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati 
                 where (1=1) and pcm.Processo = pan2.fk_cod_processo_pro "+andamentos+")";				  
				}
				else
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=" and "+filtroPeriodo;
				}

			}
			if (ddlNatureza.SelectedItem.Text != "Todos") filtroNatureza = "(UPPER(txt_natureza_nat) = UPPER('" + ddlNatureza.SelectedItem.Text + "'))";
			if (ddlTipoLicitacao.SelectedItem.Text != "Todos") filtroTipoLicitacao = "( UPPER(txt_tipo_licitacao_tli) = UPPER('" + ddlTipoLicitacao.SelectedItem.Text + "'))";
			if (ddlPregoeiro.SelectedItem.Text != "Todos") filtroPregoeiro = "( UPPER(txt_presidente_pregoeiro_pes) = UPPER('" + ddlPregoeiro.SelectedItem.Text + "'))";
			if (!string.IsNullOrEmpty(txtResumoObjeto.Text)) filtroResumo = "(UPPER(txt_observacao_pro) ilike UPPER(@txtResumoObjeto.Text))";
			
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroNumero)) sql += " AND ";
				sql += filtroNumero;			
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroInst)) sql += " AND ";
				sql += filtroInst;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroAndamento)) sql += " AND ";
				sql += filtroAndamento;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroModalidade)) sql += " AND ";
				sql += filtroModalidade; 
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroFase)) sql += " AND ";
				sql += filtroFase;			
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroNatureza)) sql += " AND ";
				sql += filtroNatureza;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroTipoLicitacao)) sql += " AND ";
				sql += filtroTipoLicitacao;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroPregoeiro)) sql += " AND ";
				sql += filtroPregoeiro;
			if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(filtroResumo)) sql += " AND ";
				sql += filtroResumo;

			
			if(!string.IsNullOrEmpty(sql)  && CheckUltimo.Checked)
			{
				if(string.IsNullOrEmpty(txtDataInicial.Text) && string.IsNullOrEmpty(txtDataFinal.Text) )
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=andamentos;				  
				}
				else
				{
					if( !string.IsNullOrEmpty(andamentos))
						sql+=filtroPeriodo;
				}		
			}

			pnlGrid.Visible = false;
			sql+=" order by Instituicao,unidadeadministrativa,Modalidade,TipoLicitacao ";
			cmd.CommandText = selectInicial;
			cmd.CommandText += sql;
			
			cmd.Parameters.Add("@txtDataInicial.Text",txtDataInicial.Text);
			cmd.Parameters.Add("@txtDataFinal.Text",txtDataFinal.Text);
			cmd.Parameters.Add("@txtNumero.Text",txtNumero.Text);
			cmd.Parameters.Add("@txtResumoObjeto.Text","%"+txtResumoObjeto.Text+"%");

			
			return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
		}
		
		public static DataTable ListarRelatorioLicitacoesConcluidas(string Modalidade, string Pregoeiro, string Instituicao, DateTime DataInicio, DateTime DataFim, bool auditado)
		{
			NpgsqlCommand cmd = new NpgsqlCommand();
			
			cmd.CommandText = @"
				SELECT 	
				cod_processo_pro,
				txt_descricao_ins, 
				txt_modalidade_mod, 
				txt_numero_licitacao_npr, 
				(case when txt_numero_spu_npr is null then txt_numero_viproc_npr else txt_numero_spu_npr end ) as txt_numero_spu_npr, 
				txt_situacao_atual_sit, 
				txt_presidente_pregoeiro_pes, 
				txt_vencedor_pes, 
				pcm.txt_observacao_pro,
				to_char(dat_entrada_pge_pan,'dd/MM/yyyy') as dat_entrada_pge_pan,
				to_char(dat_conclusao_pan,'dd/MM/yyyy') as dat_conclusao_pan, txt_andamento_pan,
				dat_conclusao_pan-dat_entrada_pge_pan as TempoPermanencia,
				num_processo_estimado_global, 
				num_processo_a_ser_contratado,
				num_estimado_real_vpr, 
				num_economia_vpr, 
				num_economia_porcent_vpr,				
				num_processo_fracassado,		
				num_nao_contratado_vpr,		
				num_processo_anulado,
				num_processo_deserto,
				num_processo_revogado,
				num_processo_cancelado				
				FROM 
				adm_licitar.tb_processo_completo_pcm pcm
				inner join adm_licitar.tb_processo_pro pro on pro.pk_cod_processo_pro = pcm.cod_processo_pro
				WHERE 
				txt_estado_processo='FINALIZADO'				
			";
			
			if (!String.IsNullOrEmpty(Modalidade)) 
				cmd.CommandText += " AND cod_modalidade_mod IN ("+Modalidade+") ";
			// MF Início. Criação da condição para adicionar o código da instituição como condição do select.
			if (!String.IsNullOrEmpty(Instituicao)) 
				cmd.CommandText += " AND cod_instituicao_ins IN ("+Instituicao+") ";
			// MF fim			
			if (!String.IsNullOrEmpty(Pregoeiro)) 
				cmd.CommandText += " AND cod_presidente_pregoeiro_pes IN ("+Pregoeiro+") ";		
			
			cmd.CommandText += " AND (dat_conclusao_pan >= @DataInicio AND dat_conclusao_pan < @dataFim) ";		
			
			if(auditado)
				cmd.CommandText += " AND pcm.boo_auditado_pro = true";
			

			cmd.CommandText +=" order by txt_descricao_ins, txt_modalidade_mod ";
			cmd.Parameters.Add("@DataInicio",DataInicio);
			cmd.Parameters.Add("@dataFim",DataFim.AddHours(23).AddMinutes(59).AddSeconds(59));					
			
			
			
			
			
			
	  		return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
		}
				
		public DataTable RetornaRemanescentes(DateTime dataInicio,DateTime dataFinal, string idsModalidade )
		{
			NpgsqlCommand cmd = new NpgsqlCommand();
			
			cmd.CommandText = @"				
						select Remanescentes.*,
					Entraram.*, finalizados.*, finalizadossemexito.*, processoemandamento.* from 
					(select count(cod_processo_pro) as countRemanescentes from adm_licitar.vw_tabelao_2009
					where dat_andamento_entrada_pge_pan < @dataInicio::date 
					and txt_estado_processo <> 'FINALIZADO' @modalidade) as Remanescentes,
					
					(select count(cod_processo_pro) as countentraram from adm_licitar.vw_tabelao_2009 where dat_andamento_entrada_pge_pan 
					between @dataInicio::date and @dataFinal::date @modalidade 
					) as Entraram,
					
					(select count(cod_processo_pro) as countFinalizados  from adm_licitar.vw_tabelao_2009 where dat_cadastro_conclusao_pan 
					between @dataInicio::date and @dataFinal::date
					and txt_estado_processo = 'FINALIZADO'
					and num_processo_a_ser_contratado is not null and num_processo_a_ser_contratado > 0 @modalidade) as finalizados,
					
					(select count(cod_processo_pro) as countFinalizadossemexito  from adm_licitar.vw_tabelao_2009 where dat_cadastro_conclusao_pan 
					between @dataInicio::date and @dataFinal::date
					and txt_estado_processo = 'FINALIZADO' and num_nao_contratado_vpr = num_processo_estimado_global
					 and (num_processo_a_ser_contratado is  null or num_processo_a_ser_contratado <= 0) @modalidade
					) as finalizadossemexito,				

					(select 

					(((select count(cod_processo_pro) as countRemanescentes from adm_licitar.vw_tabelao_2009 where dat_andamento_entrada_pge_pan < @dataInicio::date 
					and txt_estado_processo <> 'FINALIZADO' @modalidade)					
					+
					(select count(cod_processo_pro) as countentraram from adm_licitar.vw_tabelao_2009 where dat_andamento_entrada_pge_pan 
					between @dataInicio::date  and @dataFinal::date  @modalidade
					))
					-
					((select count(cod_processo_pro) as countFinalizados  from adm_licitar.vw_tabelao_2009 where dat_cadastro_conclusao_pan 
					between @dataInicio::date and @dataFinal::date
					and txt_estado_processo = 'FINALIZADO'
					and num_processo_a_ser_contratado is not null and num_processo_a_ser_contratado > 0 @modalidade) 
					+					
					(select count(cod_processo_pro) as countFinalizadossemexito  from adm_licitar.vw_tabelao_2009 where dat_cadastro_conclusao_pan 
					between @dataInicio::date and @dataFinal::date
					and txt_estado_processo = 'FINALIZADO' and num_nao_contratado_vpr = num_processo_estimado_global
 					and (num_processo_a_ser_contratado is  null or num_processo_a_ser_contratado <= 0) @modalidade )) 
					
					
					) as countprocessoemandamento) as processoemandamento
			";
			
			if (idsModalidade != null && idsModalidade != String.Empty)
			{
				cmd.CommandText = cmd.CommandText.Replace("@modalidade"," and cod_modalidade_mod in (" + idsModalidade +") ");
			}
			else
			{
				cmd.CommandText = cmd.CommandText.Replace("@modalidade"," ");
			}
			
			cmd.Parameters.Add("@dataInicio",dataInicio);
			cmd.Parameters.Add("@dataFinal",dataFinal);
			
			return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
		}
		
		public DataTable RetornaRelatorioWebMapp( string instituicao, string numeroMapp )			
		{

			NpgsqlCommand cmd = new NpgsqlCommand();
			
			cmd.CommandText = @"	 
			
			select txt_numero_mapp_npr,txt_numero_spu_npr, cod_processo_pro, txt_observacao_pro, txt_estado_processo,
			 txt_ultimo_andamento_pan, txt_numero_mapp_npr, txt_situacao_atual_sit, txt_descricao_ins from adm_licitar.tb_processo_completo_pcm pcm
			 inner join adm_licitar.tb_unidade_administrativa_uad uad on uad.fk_cod_instituicao_ins = pcm.cod_instituicao_ins where 0 =0 
			 
			";
			
			if( !string.IsNullOrEmpty(instituicao))
			{	
				cmd.CommandText+= " and cod_instituicao_ins = "+instituicao;
			}
			if(!string.IsNullOrEmpty(numeroMapp))
			{	
				cmd.CommandText+= " and txt_numero_mapp_npr = @numeroMapp";					
				cmd.Parameters.Add("@numeroMapp",numeroMapp);
			}
			
			return new Licitar.Business.Persistencia.PostgreSqlDatabase().ExecutarConsulta(cmd).Tables[0];
		}
		
		public ProcessoCompleto[] PesquisarProcessosPorInstituicaoMapp(string cnpjInstituicao, string numeroMapp)
		{
			UnidadeAdministrativa oUnidadeAdministrativa;
			oUnidadeAdministrativa = UnidadeAdministrativa.FindFirst(Expression.Eq("Cnpj",cnpjInstituicao)); 
			
			DetachedCriteria pesq = DetachedCriteria.For<ProcessoCompleto>();
			if(cnpjInstituicao != string.Empty)
				pesq.Add(Expression.Eq("StrInstituicao",oUnidadeAdministrativa.Instituicao.Sigla));
			if(numeroMapp != string.Empty)
				pesq.Add(Expression.Eq("NumeroMapp",numeroMapp));
			return ProcessoCompleto.FindAll(pesq);

		}

	}
}
