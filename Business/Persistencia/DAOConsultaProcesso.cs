// DAOConsultaProcesso.cs created with MonoDevelop
// User: marcelolima at 17:19 13/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//


using System;
using Npgsql;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

using Licitar.Business.Dto;
using Licitar.Business.Servico;

namespace Licitar.Business.Dao
{	
	/// <summary>
	/// Acesso a consultas no banco para a consulta de Processo Completo (Tabelão).
	/// </summary>
	public static class DAOConsultaProcesso
	{		
		/// <summary>
		/// Insere um processo na rotina de processo completo.
		/// </summary>
		public static void InserirRotinaProcessoCompleto(DateTime dataUltima,int totalRegistros,string pksProcesso,NpgsqlConnection con)
		{
			string sql=@"INSERT INTO adm_licitar.tb_rotina_processo_completo_rpo (pk_cod_rotina_processo_completo_rpo,
						  		dat_ultima_exec_rpo, num_total_registros_afetados_rpo , txt_processos_alterados_desc) 
						 values (nextval('sq_rotina_processo_completo_rpo'),
						  		@dat_ultima_exec_rpo, @num_total_registros_afetados_rpo , @txt_processos_alterados_desc) ";
			NpgsqlCommand cmd = new NpgsqlCommand(sql,con);
			cmd.Parameters.Add("@dat_ultima_exec_rpo",dataUltima);
			cmd.Parameters.Add("@num_total_registros_afetados_rpo",totalRegistros);
			cmd.Parameters.Add("@txt_processos_alterados_desc",pksProcesso);
			
			cmd.ExecuteNonQuery(); 
		}	
		
		/// <summary>
		/// Remove um registro do tabelão.
		/// </summary>
		public static void RemoverProcessoCompleto(NpgsqlConnection con)
		{
			string sql=@" delete from tb_processo_completo_pcm ";
			NpgsqlCommand cmd = new NpgsqlCommand(sql,con);
			cmd.ExecuteNonQuery(); 
		}

		/// <summary>
		/// Realiza a inserção de um processo no tabelão.
		/// </summary>
		public static void InserirProcessoCompleto(ProcessoCompleto processo,NpgsqlConnection con)
		{
			string sql=@"INSERT INTO adm_licitar.tb_processo_completo_pcm 
							(pk_cod_processo_pro,pk_cod_instituicao_ins,txt_descricao_ins,pk_cod_area_are,txt_descricao_are,
							cod_unidade_exercicio_uex,txt_descricao_uex,txt_observacao_pro,pk_cod_natureza,txt_natureza_nat,
							pk_cod_modalidade,txt_modalidade_mod,pk_cod_tipo_licitacao,txt_tipo_licitacao_tli,
							txt_numero_spu_npr,txt_numero_licitacao_npr,txt_numero_processo_comprasnet,txt_numero_processo_sisbb,
							txt_numero_processo_ig,vlr_processo_estimado_global,num_processo_a_ser_contratado,num_processo_fracassado,
							num_processo_deserto,num_processo_anulado,num_processo_revogado,num_processo_cancelado,
							pk_cod_presidente_pregoeiro,txt_presidente_pregoeiro_pes,pk_cod_papel,txt_papel_pap,dt_cadastro,dt_entrada_na_pge,
							dat_realizacao_pan,dt_adjudicacao,dt_abertura_propostas,dt_homologado,dt_conclusao,dt_devolucao,dt_deserto,dt_revogado,
							dt_fracassado,dt_aprovacao,dt_anulacao,dt_sessao_abertura_proposta_comercial,dt_sessao_abertura_proposta_tecnica,
							dt_sessao_resultado_proposta_comercial,dt_sessao_resultado_proposta_tecnica,dt_sessao_resultado_habilitacao,
							txt_situacao_atual_desc,cod_ultimo_andamento_pan,txt_ultimo_andamento_pan,pk_cod_ultima_fase,txt_ultima_fase_fas,
							txt_estado_processo,txt_motivo_concluido,txt_andamento_pan,pk_cod_vencedor,txt_vencedor_pes,
							vl_nao_contratado,num_estimado_real_vpr,vl_economia,vl_economia_porcent,txt_ano_mes_inicio_processo,
							txt_ano_mes_final_processo,dt_ultimo_andamento) 
							values (@pk_cod_processo_pro,@pk_cod_instituicao_ins,@txt_descricao_ins,@pk_cod_area_are,@txt_descricao_are,
							@cod_unidade_exercicio_uex,@txt_descricao_uex,@txt_observacao_pro,@pk_cod_natureza,@txt_natureza_nat,
							@pk_cod_modalidade,@txt_modalidade_mod,@pk_cod_tipo_licitacao,@txt_tipo_licitacao_tli,@txt_numero_spu_npr,
							@txt_numero_licitacao_npr,@txt_numero_processo_comprasnet,@txt_numero_processo_sisbb,@txt_numero_processo_ig,
							@vlr_processo_estimado_global,@num_processo_a_ser_contratado,@num_processo_fracassado,@num_processo_deserto
							,@num_processo_anulado,@num_processo_revogado,@num_processo_cancelado,@pk_cod_presidente_pregoeiro,
							@txt_presidente_pregoeiro_pes,@pk_cod_papel,@txt_papel_pap,@dt_cadastro,@dt_entrada_na_pge,@dat_realizacao_pan,
							@dt_adjudicacao,@dt_abertura_propostas,@dt_homologado,@dt_conclusao,@dt_devolucao,@dt_deserto,@dt_revogado,
							@dt_fracassado,@dt_aprovacao,@dt_anulacao,@dt_sessao_abertura_proposta_comercial,@dt_sessao_abertura_proposta_tecnica,
							@dt_sessao_resultado_proposta_comercial,@dt_sessao_resultado_proposta_tecnica,@dt_sessao_resultado_habilitacao
							,@txt_situacao_atual_desc,@cod_ultimo_andamento_pan,@txt_ultimo_andamento_pan,@pk_cod_ultima_fase,@txt_ultima_fase_fas,
							@txt_estado_processo,@txt_motivo_concluido,@txt_andamento_pan,@pk_cod_vencedor,@txt_vencedor_pes,
							@vl_nao_contratado,@num_estimado_real_vpr,@vl_economia,@vl_economia_porcent,@txt_ano_mes_inicio_processo,
							@txt_ano_mes_final_processo,@dt_ultimo_andamento) ";

			NpgsqlCommand cmd = new NpgsqlCommand(sql,con);
			cmd.Parameters.Add("@pk_cod_processo_pro",processo.IdProcesso);
			cmd.Parameters.Add("@pk_cod_instituicao_ins",processo.IdInstituicao);
			cmd.Parameters.Add("@txt_descricao_ins",processo.Instituicao);
			cmd.Parameters.Add("@pk_cod_area_are",processo.IdArea);
			cmd.Parameters.Add("@txt_descricao_are",processo.Area);
			cmd.Parameters.Add("@cod_unidade_exercicio_uex",processo.IdUnidExercicio);
			cmd.Parameters.Add("@txt_descricao_uex",processo.UnidExercicio);
			cmd.Parameters.Add("@txt_observacao_pro",processo.ResumoObjeto);
			cmd.Parameters.Add("@pk_cod_natureza",processo.IdNatureza);
			cmd.Parameters.Add("@txt_natureza_nat",processo.Natureza);						
			cmd.Parameters.Add("@pk_cod_modalidade",processo.IdModalidade);
			cmd.Parameters.Add("@txt_modalidade_mod",processo.Modalidade);
			cmd.Parameters.Add("@pk_cod_tipo_licitacao",processo.IdTipoLicitacao);
			cmd.Parameters.Add("@txt_tipo_licitacao_tli",processo.TipoLicitacao);
			cmd.Parameters.Add("@txt_numero_spu_npr",processo.NumeroSpu);
			cmd.Parameters.Add("@txt_numero_licitacao_npr",processo.NumeroLicitacao);
			cmd.Parameters.Add("@txt_numero_processo_comprasnet",processo.NumeroComprasNet);
			cmd.Parameters.Add("@txt_numero_processo_sisbb",processo.NumeroSisBB);
			cmd.Parameters.Add("@txt_numero_processo_ig",processo.NumeroIG);
			cmd.Parameters.Add("@vlr_processo_estimado_global",processo.ValorEstimadoGlobal);
			cmd.Parameters.Add("@num_processo_a_ser_contratado",processo.ValorASerContratado);
			cmd.Parameters.Add("@num_processo_fracassado",processo.ValorFracassado);
			cmd.Parameters.Add("@num_processo_deserto",processo.ValorDeserto);
			cmd.Parameters.Add("@num_processo_anulado",processo.ValorAnulado);
			cmd.Parameters.Add("@num_processo_revogado",processo.ValorRevogado);
			cmd.Parameters.Add("@num_processo_cancelado",processo.ValorCancelado);
			cmd.Parameters.Add("@pk_cod_presidente_pregoeiro",processo.IdPresidentePregoeiro);
			cmd.Parameters.Add("@txt_presidente_pregoeiro_pes",processo.PresidentePregoeiro);
			cmd.Parameters.Add("@pk_cod_papel",processo.IdPapel);
			cmd.Parameters.Add("@txt_papel_pap",processo.Papel);
			cmd.Parameters.Add("@dt_cadastro",string.IsNullOrEmpty(processo.DataCadastro)? null : processo.DataCadastro);
			cmd.Parameters.Add("@dt_entrada_na_pge",string.IsNullOrEmpty(processo.DataEntradaPge)? null : processo.DataEntradaPge);
			cmd.Parameters.Add("@dat_realizacao_pan",string.IsNullOrEmpty(processo.DataRealizacao) ? null :processo.DataRealizacao);
			cmd.Parameters.Add("@dt_adjudicacao",string.IsNullOrEmpty(processo.DataAdjudicacao)? null : processo.DataAdjudicacao);
			cmd.Parameters.Add("@dt_abertura_propostas",string.IsNullOrEmpty(processo.DataAberturaPropostas) ? null :processo.DataAberturaPropostas );			
			cmd.Parameters.Add("@dt_homologado",string.IsNullOrEmpty(processo.DataHomologado) ? null : processo.DataHomologado);
			cmd.Parameters.Add("@dt_conclusao",string.IsNullOrEmpty(processo.DataConclusao)? null : processo.DataConclusao);
			cmd.Parameters.Add("@dt_devolucao",string.IsNullOrEmpty(processo.DataDevolucao) ? null :processo.DataDevolucao);			
			cmd.Parameters.Add("@dt_deserto",string.IsNullOrEmpty(processo.DataDeserto) ? null : processo.DataDeserto);			
			cmd.Parameters.Add("@dt_revogado",string.IsNullOrEmpty(processo.DataRevogado) ? null : processo.DataRevogado);
			cmd.Parameters.Add("@dt_fracassado",string.IsNullOrEmpty(processo.DataFracassado) ? null : processo.DataFracassado );
			cmd.Parameters.Add("@dt_aprovacao",string.IsNullOrEmpty(processo.DataAprovacao) ? null : processo.DataAprovacao);
			cmd.Parameters.Add("@dt_anulacao",string.IsNullOrEmpty(processo.DataAnulacao)? null : processo.DataAnulacao);
			cmd.Parameters.Add("@dt_sessao_abertura_proposta_comercial",string.IsNullOrEmpty(processo.DataSessaoAberturaPropostaComercial) ? null : processo.DataSessaoAberturaPropostaComercial);
			cmd.Parameters.Add("@dt_sessao_abertura_proposta_tecnica",string.IsNullOrEmpty(processo.DataSessaoAberturaPropostaTecnica) ? null : processo.DataSessaoAberturaPropostaTecnica);
			cmd.Parameters.Add("@dt_sessao_resultado_proposta_comercial",string.IsNullOrEmpty(processo.DataSessaoResultadoPropostaComercial) ? null : processo.DataSessaoResultadoPropostaComercial);
			cmd.Parameters.Add("@dt_sessao_resultado_proposta_tecnica",string.IsNullOrEmpty(processo.DataSessaoResultadoPropostaTecnica) ? null : processo.DataSessaoResultadoPropostaTecnica);
			cmd.Parameters.Add("@dt_sessao_resultado_habilitacao",string.IsNullOrEmpty(processo.DataSessaoResultadoHabilitacao)? null : processo.DataSessaoResultadoHabilitacao);
			cmd.Parameters.Add("@dt_ultimo_andamento",string.IsNullOrEmpty(processo.DataSessaoResultadoHabilitacao)? null : processo.DataUltimoAndamento); 
			cmd.Parameters.Add("@txt_situacao_atual_desc",processo.SituacaoAtual);
			cmd.Parameters.Add("@cod_ultimo_andamento_pan",processo.idUltimoAndamento);
			cmd.Parameters.Add("@txt_ultimo_andamento_pan",processo.UltimoAndamento);
			cmd.Parameters.Add("@pk_cod_ultima_fase",processo.idUltimaFase);
			cmd.Parameters.Add("@txt_ultima_fase_fas",processo.UltimaFase);
			cmd.Parameters.Add("@txt_estado_processo",processo.EstadoProcesso);
			cmd.Parameters.Add("@txt_motivo_concluido",processo.MotivoConcluido);
			cmd.Parameters.Add("@txt_andamento_pan",processo.Observacao);
			cmd.Parameters.Add("@pk_cod_vencedor",processo.IdVencedor);		
			cmd.Parameters.Add("@txt_vencedor_pes",processo.Vencedor);		
			cmd.Parameters.Add("@vl_nao_contratado",processo.VlNaoContratado);
			cmd.Parameters.Add("@num_estimado_real_vpr",processo.VlEstimadoReal);
            cmd.Parameters.Add("@vl_economia",processo.VlEconomia);
			cmd.Parameters.Add("@vl_economia_porcent",processo.VlEconomiaPorcent);
			cmd.Parameters.Add("@txt_ano_mes_inicio_processo",((string.IsNullOrEmpty(processo.MesAnoInicio) || processo.MesAnoInicio == "-infinity") ? null : processo.MesAnoInicio));
			cmd.Parameters.Add("@txt_ano_mes_final_processo",((string.IsNullOrEmpty(processo.MesAnoFim ) || processo.MesAnoFim == "-infinity") ? null :  processo.MesAnoFim));
		   
			cmd.ExecuteNonQuery(); 
		}

		public static List<int> ConsultarPksProcesso(NpgsqlConnection con)
		{
			List<int> ListaPks = new List<int>();
		    NpgsqlDataReader reader = null;
			try
			{					
				string sql = " select distinct fk_cod_processo_pro from tb_processo_andamento_pan ";			
				NpgsqlCommand command = new NpgsqlCommand(sql, con);
				reader = command.ExecuteReader();
			} 
			catch (NpgsqlException ex)
			{
				//TODO: Esse procedimento tinha catch vazio. Motivo?
				throw ex;
			}
	
			while (reader.Read())
			{
				ListaPks.Add(int.Parse(reader["fk_cod_processo_pro"].ToString()));
			}
			reader.Close();				   
			return ListaPks;			
		}
		
		public static List<ProcessoCompleto> ConsultarProcessoCompleto(int processo,NpgsqlConnection con)
		{
			List<ProcessoCompleto> ConsultarProcessoCompleto = new List<ProcessoCompleto>();
			NpgsqlDataReader reader = null;
				
			string sql = " select * from vw_consulta_processo_tabelao where pk_cod_processo_pro = @processo ";
			NpgsqlCommand command = new NpgsqlCommand(sql, con);
			command.Parameters.Add("@processo",processo);
			reader = command.ExecuteReader();
			while (reader.Read())
			{
				ProcessoCompleto proc = new ProcessoCompleto();				
				srvProcessoCompleto srvProc = new srvProcessoCompleto();
				proc.IdProcesso = int.Parse(reader["pk_cod_processo_pro"].ToString());
				proc.IdInstituicao = int.Parse(reader["pk_instituicao_ins"].ToString()); 
				Console.WriteLine("proc.IdProcesso = "+proc.IdProcesso);	  
				proc.Instituicao = reader["instituicao"].ToString();
				proc.IdArea = int.Parse(reader["pkArea"].ToString());
				proc.Area = string.IsNullOrEmpty(reader["txt_descricao_are"].ToString()) ? string.Empty : reader["txt_descricao_are"].ToString();
				Console.WriteLine("Teste unid = "+reader["cod_unidade_exercicio_uex"].ToString());
				if(reader["cod_unidade_exercicio_uex"].ToString() != string.Empty) proc.IdUnidExercicio = int.Parse(reader["cod_unidade_exercicio_uex"].ToString());
				proc.UnidExercicio = reader["txt_unidade_exercicio_desc"].ToString();
				proc.ResumoObjeto = reader["objeto"].ToString();
				proc.IdNatureza = int.Parse(reader["pk_natureza"].ToString());
				proc.Natureza = reader["natureza"].ToString();
				proc.IdModalidade = int.Parse(reader["pk_modalidade"].ToString());
				proc.Modalidade = reader["modalidade"].ToString();
				proc.IdTipoLicitacao = int.Parse(reader["pk_tipo"].ToString());
				proc.TipoLicitacao = reader["tipo"].ToString();
				proc.NumeroSpu = reader["numero_spu"].ToString();
				proc.NumeroLicitacao = reader["numero_licitacao"].ToString();
				proc.NumeroComprasNet = reader["numero_comprasnet"].ToString();
				proc.NumeroSisBB = reader["numero_sisbb"].ToString();
				proc.NumeroIG = reader["numero_ig"].ToString();				
				proc.ValorEstimadoGlobal = string.IsNullOrEmpty(reader["valor_estimado"].ToString()) ? decimal.Zero : decimal.Parse(reader["valor_estimado"].ToString());
				proc.ValorASerContratado = string.IsNullOrEmpty(reader["valor_contratado"].ToString()) ? decimal.Zero : decimal.Parse(reader["valor_contratado"].ToString());								
				proc.ValorFracassado = string.IsNullOrEmpty(reader["valor_fracassado"].ToString())  ? decimal.Zero : decimal.Parse(reader["valor_fracassado"].ToString());
				proc.ValorDeserto = string.IsNullOrEmpty(reader["valor_deserto"].ToString()) ? decimal.Zero : decimal.Parse(reader["valor_deserto"].ToString());
				proc.ValorAnulado = string.IsNullOrEmpty(reader["valor_anulado"].ToString()) ? decimal.Zero : decimal.Parse(reader["valor_anulado"].ToString());
				proc.ValorRevogado = string.IsNullOrEmpty(reader["valor_revogado"].ToString()) ? decimal.Zero : decimal.Parse(reader["valor_revogado"].ToString());
				proc.ValorCancelado = string.IsNullOrEmpty( reader["valor_cancelado"].ToString()) ? decimal.Zero : decimal.Parse(reader["valor_cancelado"].ToString());
				
				if(!string.IsNullOrEmpty(reader["pk_presidente_pregoeiro"].ToString())) proc.IdPresidentePregoeiro = int.Parse(reader["pk_presidente_pregoeiro"].ToString()); 		
				proc.PresidentePregoeiro = string.IsNullOrEmpty(reader["presidente_pregoeiro"].ToString()) ? string.Empty : reader["presidente_pregoeiro"].ToString();
				if(!(reader["pk_cod_papel_pap"].ToString() == string.Empty)) proc.IdPapel =  int.Parse(reader["pk_cod_papel_pap"].ToString());
				proc.Papel = reader["txt_papel_pap"].ToString();
				proc.DataCadastro = reader["data_cadastro"].ToString();
				proc.DataEntradaPge = (reader["data_entrada_pge"].ToString() == "-infinity") ? null : reader["data_entrada_pge"].ToString(); 
				proc.DataAberturaPropostas = reader["data_abertura_propostas"].ToString();
				proc.DataAdjudicacao = reader["data_adjudicado"].ToString();
				proc.DataHomologado = reader["data_homologado"].ToString();
				proc.DataConclusao =(reader["data_concluido"].ToString() == "-infinity") ? null : reader["data_concluido"].ToString();
				proc.DataDevolucao = reader["data_devolvido_para_setorial"].ToString();
				proc.DataDeserto = reader["data_deserto"].ToString();
				proc.DataRevogado = reader["data_revogado"].ToString();
				proc.DataFracassado = reader["data_fracassado"].ToString();
				proc.DataAprovacao = reader["data_aprovado"].ToString();
				proc.DataAnulacao = reader["data_anulado"].ToString();
				proc.DataSessaoAberturaPropostaComercial = reader["data_sessao_abertura_proposta_comercial"].ToString();
				proc.DataSessaoAberturaPropostaTecnica = reader["Data_sessao_abertura_proposta_tecnica"].ToString();
				proc.DataSessaoResultadoPropostaComercial = reader["data_sessao_resultado_proposta_comercial"].ToString();
				proc.DataSessaoResultadoPropostaTecnica = reader["data_sessao_resultado_proposta_tecnica"].ToString();
				proc.DataSessaoResultadoHabilitacao = reader["data_sessao_resultado_habilitacao"].ToString();
				proc.DataRealizacao = reader["licitacao_marcada_disputa"].ToString();
				proc.SituacaoAtual = reader["ultima_situacao"].ToString();
				proc.DataUltimoAndamento = reader["data_ultimo_andamento"].ToString();
				proc.UltimoAndamento = reader["txt_ultimo_andamento"].ToString();
				proc.Observacao = reader["observacao"].ToString();
				proc.idUltimoAndamento = int.Parse(reader["pk_ultimo_andamento"].ToString());
			
				if(reader["pk_ult_fase"].ToString()!= string.Empty)
					proc.idUltimaFase =  int.Parse(reader["pk_ult_fase"].ToString());
				proc.UltimaFase = reader["txt_descricao_fase"].ToString();
				if(proc.idUltimaFase == 5) 
					proc.MotivoConcluido = srvProc.VerificarMotivoConcluido(proc.ValorEstimadoGlobal,proc.ValorAnulado,
				proc.ValorDeserto,proc.ValorCancelado,proc.ValorFracassado,proc.ValorRevogado,proc.ValorASerContratado);

                //vencedor
				//foi feito esse trecho de codigo por que os vencedores vinham repetidos entao foi refeito para que nao se repetissem mais.
				System.Collections.Generic.List<string> listaVenc = new List<string>();
				System.Collections.Generic.List<string> listaVencpk = new List<string>(); 
				string[] str =  reader["vencedor"].ToString().Split('|');
				string[] strpk = reader["pkvencedor"].ToString().Split('|');
				for (int i=0;i<str.Length ;i++ ) {
					if(!listaVenc.Contains(str[i].ToString()))
					{
						listaVenc.Add(str[i].ToString());
						listaVencpk.Add(strpk[i].ToString());
					}
				}	
				   
				for(int i=0;i<listaVenc.Count;i++)
				{
					proc.Vencedor += listaVenc[i].ToString();
					proc.IdVencedor += listaVencpk[i].ToString();
					if(i<listaVenc.Count-1)
					{
						proc.Vencedor+=",";
						proc.IdVencedor+=","; 
					}
				}

				//final vencedor
				proc.EstadoProcesso = srvProc.VerificarEstadoProcesso((proc.DataRealizacao == string.Empty) ? null : (DateTime ?)DateTime.Parse(proc.DataRealizacao),(string.IsNullOrEmpty(proc.DataConclusao)) ? null : (DateTime ?) DateTime.Parse(proc.DataConclusao),proc.idUltimoAndamento);
				proc.VlNaoContratado = srvProc.ValorNaoContratado(proc.ValorDeserto,proc.ValorAnulado,proc.ValorFracassado,proc.ValorRevogado,proc.ValorCancelado);
				proc.VlEstimadoReal = srvProc.ValorEstimadoReal(proc.ValorEstimadoGlobal,proc.VlNaoContratado);
				proc.VlEconomia = srvProc.Economia(proc.VlEstimadoReal,proc.ValorASerContratado);
				proc.VlEconomiaPorcent = ((proc.VlEstimadoReal !=0) ? srvProc.EconomiaPorcentagem(proc.VlEconomia,proc.VlEstimadoReal) : decimal.Zero);
				proc.MesAnoInicio = reader["mes_ano_inicio"].ToString();
				proc.MesAnoFim = reader["mes_ano_fim"].ToString();
				
				ConsultarProcessoCompleto.Add(proc);											
			}

            reader.Close();			
			return ConsultarProcessoCompleto;		
		}
	}
}
