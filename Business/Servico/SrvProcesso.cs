// SrvProcesso.cs created with MonoDevelop
// User: diogolima at 17:41 13/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;

using NHibernate;
using Licitar.Business.Entidade;
using NHibernate.Expression;

using Licitar.Business.Dto;
using Licitar.Business.Dao;
using System.Data;
using Licitar.Business.Persistencia;
using System.Text;

namespace Licitar.Business.Servico
{	
	[DataObjectAttribute]
	public class SrvProcesso
	{

		/// <summary>
		/// metodo para retorno do valor estimado real.(utilizado em relatorios)
		/// </summary>		
		public decimal ValorEstimadoReal(decimal valorEstimado,decimal valorNaoContratado)
		{
			decimal valorEstimadoReal;
			if(valorEstimado.Equals(valorNaoContratado))
				valorEstimadoReal = valorEstimado;
			else
			{
				valorEstimadoReal = decimal.Round((valorEstimado - valorNaoContratado),2);
			}
			return valorEstimadoReal;
		}
		/// <summary>
		/// metodo para retorno do valor nao contratado.(utilizado em relatorios)
		/// </summary>	
		public decimal ValorNaoContratado(decimal fracassado,decimal valorDeserto,decimal valorAnulado,decimal valorRevogado,decimal valorCancelado)
		{
			decimal valorNaoContratado = fracassado + valorDeserto + valorAnulado + valorRevogado + valorCancelado;
			return valorNaoContratado;
		}
		
		/// <summary>
		/// valorNaoContratado é a soma dos valores que nao foram contratados.(utilizado em relatorios)
		/// </summary>	
		public decimal RegraEconomia(decimal? valorContratado,decimal? valorEstimadoReal)
		{					               
			// início MF			
			if (valorContratado.HasValue && valorEstimadoReal.HasValue) 
			{
				return valorEstimadoReal.Value - valorContratado.Value; 
			}
			// fim MF
			
			return 0;
				
		}

		public decimal RegraEconomiaPorcentagem(decimal economia ,decimal valorEstimadoReal)
		{
			decimal economiaPorcentagem = 0;
			
			if((economia != 0M) && (valorEstimadoReal != 0M))	
			{				
				economiaPorcentagem = decimal.Round(((economia * 100 ) / valorEstimadoReal ),2);
			}
			
			return economiaPorcentagem;
		}
		
		public string RetiraZero(string valor)
		{
			string retorno = string.Empty;
			if(valor != " 0,00" & valor != "0" )
				retorno = valor;
			
			return retorno;
		}
		
		public string FormataMoeda(string valor)
		{
			
			string retorno = string.Empty;
			if(!string.IsNullOrEmpty(valor) && valor != "&nbsp;")
			{
				
				decimal retornodec = decimal.Parse(valor);
				retorno = retornodec.ToString("c");
				retorno = retorno.Replace("R$",string.Empty).Replace("(","-").Replace(")",string.Empty);
			}
			return retorno;
		}
		
		public ConsultaReceberTramitar[] BuscarProcessoTramitadoDoUsuario(int idPessoaFisica)
		{
			SrvUnidadeExercicioFuncaoPessoa srvUefp = new SrvUnidadeExercicioFuncaoPessoa();
			List<string> listaSubUnidades = srvUefp.listarSubUnidadesPessoa(idPessoaFisica);
			
			Order[] ordenar = {Order.Desc("DataUltimoTramitarReceber"), Order.Desc("DataUltimoAndamento")};
		
			DetachedCriteria dcIgualTRAMITAR = DetachedCriteria.For(typeof(ConsultaReceberTramitar));
			dcIgualTRAMITAR.Add(Expression.Eq("NomeTipoAndamento", "TRAMITAR"));//TRAMITAR (9 = TRAMITAR)
			dcIgualTRAMITAR.Add(
			       Expression.Or(
			                       Expression.And(Expression.IsNull("IdPessoaDestinoAndamento"), Expression.In("UnidadeExDestino", listaSubUnidades)), 
			                       Expression.And(Expression.IsNull("UnidadeExDestino"), Expression.Eq("IdPessoaDestinoAndamento", idPessoaFisica))));
		
			return ConsultaReceberTramitar.FindAll(dcIgualTRAMITAR,ordenar);
		}
		
		public void ReceberProcessoIncluindoPessoa(int idUsuario, bool UnidExercicio, string fase, string obs, params int[] idProcessos)
		{
			if (idUsuario <= 0)
				throw new ArgumentNullException("idUsuario");
			
			if ((idProcessos == null) || (idProcessos.Length == 0))
				throw new ArgumentNullException("idProcesso");
			
			for (int index = 0; index < idProcessos.Length; index++)
			{
				if (idProcessos[index] <= 0)
					throw new ArgumentException("idProcesso deve ser maior do que zero", string.Format("idProcessos[{0}]", index));
			}
			Pessoa usuario = Pessoa.Find(idUsuario);
			
			if (usuario == null)
			{
				string message = String.Format("PessoaFisica Id:{0} não encontrada.", idUsuario);
				System.Diagnostics.Trace.TraceError(message);				
				throw new ApplicationException(message);
			}
			int processedId = -1;
			
			using(TransactionScope transaction = new TransactionScope())
			{
				System.Diagnostics.Trace.TraceInformation("Processando recebimento de processos pelo usuário PessoaFisica Id:{0}", idUsuario);
				
				try
				{
					System.Diagnostics.Trace.Indent();
					
					foreach(int id in idProcessos)
					{
						processedId = id;
						
						Processo processo = Processo.Find(id);
																		
	                    ProcessoAndamento andamento = new ProcessoAndamento();												
				
						Atividade tipoAndamento = Atividade.BuscarPorDescricao("Receber");
						DetachedCriteria dcFluxo = DetachedCriteria.For(typeof(FluxoAndamento));				
								dcFluxo.CreateAlias("Fase","fas");
						    	dcFluxo.CreateAlias("Atividade","ati");
								dcFluxo.Add(Expression.Eq("fas.Id",Convert.ToInt32(fase)));
							dcFluxo.Add(Expression.Eq("ati.Descricao",tipoAndamento.Descricao));
								dcFluxo.Add(Expression.Sql("this_.fk_cod_workflow_wor in (select wor.pk_cod_workflow_wor from adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu inner join adm_licitar.tb_workflow_wor wor on wmu.fk_cod_workflow_wor = wor.pk_cod_workflow_wor where wmu.fk_cod_modalidade_mod="+processo.Classificacao.Modalidade.Id+")"));
							FluxoAndamento objFluxo = FluxoAndamento.FindFirst(dcFluxo);	
				
							if(objFluxo != null)
							{	                
								//INSERIR ANDAMENTO
								andamento.Cadastrante = usuario;				
								andamento.Processo = processo;
								andamento.FluxoAndamento = objFluxo;
								andamento.Pessoa = null;
								andamento.Andamento = obs;
							    andamento.DataAndamento = DateTime.Now;	
								andamento.DataCadastro = DateTime.Now;								
								andamento.SaveAndFlush();					
								
								System.Diagnostics.Trace.TraceInformation("Processado recebimento do Processo Id:{0}", id);
							}
					}
					
					transaction.VoteCommit();
					
					System.Diagnostics.Trace.TraceInformation("Processos recebidos com sucesso.");
				}
				catch(ObjectNotFoundException ex1)
				{
					System.Diagnostics.Trace.TraceError("Erro ao processar recebimento do processo Id:{0} - '{1}'", processedId, ex1.Message);
					transaction.VoteRollBack();
					
					throw new ApplicationException("Erro ao processar recebimento de processos. Não foi possível localizar um dos processos envolvidos no procedimento. Operação cancelada.");
				}
				catch(Exception ex2)
				{
					System.Diagnostics.Trace.TraceError("Erro ao processar recebimento do processo Id:{0} - '{1}'", processedId, ex2.Message);
					transaction.VoteRollBack();
					
					string msg = string.Format("Ocorreu um erro não tratado ao tentar receber o processo id:{0}. Operação cancelada.", processedId); 

					throw new ApplicationException(msg, ex2);
				}
				finally
				{
					System.Diagnostics.Trace.Unindent();
				}
				
				System.Diagnostics.Trace.TraceInformation("Recebimento de processos pelo usuário PessoaFisica Id:{0} finalizado", idUsuario);
			}
		}		
		
		public ConsultaReceberTramitar[] BuscarProcessoRecebidoDoUsuario(int idUsuario)
		{
			Order[] ordenar = {Order.Desc("DataUltimoTramitarReceber"), Order.Desc("DataUltimoAndamento")};
			int idPessoa = Pessoa.FindOne(Expression.Eq("Id", idUsuario)).Id;			
			string[] andamentosExcluidosDaPesquisa = {"TRAMITAR","CONCLUÍDO"}; 
			DetachedCriteria dcDiferenteTRAMITAR = DetachedCriteria.For(typeof(ConsultaReceberTramitar));									
			dcDiferenteTRAMITAR.Add(Expression.Not(Expression.In("NomeTipoAndamento",andamentosExcluidosDaPesquisa)));
			dcDiferenteTRAMITAR.Add(Expression.Or(Expression.Eq("IdPessoaUltAndReceber",idPessoa),Expression.And(Expression.Eq("IdPessoaCadastro",idPessoa),Expression.IsNull("IdPessoaUltAndReceber"))));
			return ConsultaReceberTramitar.FindAll(dcDiferenteTRAMITAR,ordenar);
			
		}
		
		//processo completo
		public DataTable BuscarDtProcessosRecebidosDoUsuario(int idUsuario,bool blnProcComp)
		{
			string select = @"
			SELECT 
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pcm.cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'SPU'
			)
			) as NumeroSpu, 
            (
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pcm.cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'VIPROC'
			)
			) as NumeroVIPROC,
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pcm.cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'LICITAÇÃO'
			)
			) as NumeroLicitacao,
			ins.txt_sigla_ins as NomeInstituicaoOrigem, 
			pcm.txt_observacao_pro as ObjetoProcesso, 
			ati.txt_descricao_ati as NomeTipoAndamento, 
			pan.dat_cadastro_pan as DataUltimoAndamento, 
			pcm.cod_processo_pro as Processo
			FROM adm_licitar.tb_processo_completo_pcm pcm
			INNER JOIN adm_licitar.tb_processo_andamento_pan pan ON pcm.cod_processo_pro = pan.fk_cod_processo_pro
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_instituicao_ins ins ON ins.pk_cod_instituicao_ins = pcm.cod_instituicao_ins
			WHERE pan.fk_cod_pessoa_cadastrante_pes = @pessoa
			AND pan.pk_cod_processo_andamento_pan = 
			(
			SELECT pan2.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan pan2
			WHERE pan2.fk_cod_processo_pro = pcm.cod_processo_pro
			ORDER BY pan2.dat_cadastro_pan DESC, pan2.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			)
			AND 
			(
			SELECT ati.txt_descricao_ati
			FROM adm_licitar.tb_processo_andamento_pan pan2
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE pan2.fk_cod_processo_pro = pcm.cod_processo_pro
			ORDER BY pan2.dat_cadastro_pan DESC, pan2.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			) NOT IN ('TRAMITAR','CONCLUÍDO')
			
			ORDER BY NomeInstituicaoOrigem
			";
			
			select = select.Replace("@pessoa", idUsuario.ToString());
			
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
		}

		public DataTable BuscarDtProcessosRecebidosDoUsuario(int idUsuario)
		{
			string select = @"
			SELECT 
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'SPU'
			)
			) as NumeroSpu, 
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'VIPROC'
			)
			) as NumeroVIPROC, 
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'LICITAÇÃO'
			)
			) as NumeroLicitacao,
			ins.txt_sigla_ins as NomeInstituicaoOrigem, 
			pro.txt_resumo_objeto_pro as ObjetoProcesso, 
			ati.txt_descricao_ati as NomeTipoAndamento, 
			pan.dat_cadastro_pan as DataUltimoAndamento, 
			pro.pk_cod_processo_pro as Processo
			FROM adm_licitar.tb_processo_pro pro
			INNER JOIN adm_licitar.tb_processo_andamento_pan pan ON pro.pk_cod_processo_pro = pan.fk_cod_processo_pro
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_instituicao_ins ins ON ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins
			WHERE pan.fk_cod_pessoa_cadastrante_pes = @pessoa
			AND pan.pk_cod_processo_andamento_pan = 
			(
			SELECT pan2.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan pan2
			WHERE pan2.fk_cod_processo_pro = pro.pk_cod_processo_pro
			ORDER BY pan2.dat_cadastro_pan DESC, pan2.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			)
			AND 
			(
			SELECT ati.txt_descricao_ati
			FROM adm_licitar.tb_processo_andamento_pan pan2
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE pan2.fk_cod_processo_pro = pro.pk_cod_processo_pro
			ORDER BY pan2.dat_cadastro_pan DESC, pan2.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			) NOT IN ('TRAMITAR','CONCLUÍDO')
			
			ORDER BY NomeInstituicaoOrigem
			";
			
			select = select.Replace("@pessoa", idUsuario.ToString());
			
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
		}
		
		//do processo completo..
		public DataTable BuscarProcessosAReceberDoUsuario(int idUsuario, bool blnProcComp)
		{
			SrvUnidadeExercicioFuncaoPessoa srvUefp = new SrvUnidadeExercicioFuncaoPessoa();
			
			List<int> listaSubUnidades = srvUefp.listarIdsSubUnidadesPessoa(idUsuario);

			string unidades = "";

			foreach(int s in listaSubUnidades)
				unidades += s.ToString()+",";

			if(!string.IsNullOrEmpty(unidades))
				unidades = unidades.Remove(unidades.Length-1);

			string select = @"
			SELECT 
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pcm.cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'SPU'
			)
			) as NumeroSpu,
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pcm.cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'VIPROC'
			)
			) as NumeroVIPROC,
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pcm.cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'LICITAÇÃO'
			)
			) as NumeroLicitacao,
			ins.txt_sigla_ins as NomeInstituicaoOrigem, 
			pcm.txt_observacao_pro as ObjetoProcesso, 
			ati.txt_descricao_ati as NomeTipoAndamento, 
			pan.dat_cadastro_pan as DataUltimoAndamento, 
			pcm.cod_processo_pro as Processo
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_processo_completo_pcm pcm ON pcm.cod_processo_pro = pan.fk_cod_processo_pro
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_instituicao_ins ins ON ins.pk_cod_instituicao_ins = pcm.cod_instituicao_ins
			WHERE (
			(pan.fk_cod_pessoa_pes = @pessoa)
			OR 
			(pan.fk_cod_unidade_exercicio_uex IN (@unidades) AND pan.fk_cod_pessoa_pes is null)
			)
			AND pan.pk_cod_processo_andamento_pan = 
			(
			SELECT pan2.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan pan2
			WHERE pan2.fk_cod_processo_pro = pcm.cod_processo_pro
			ORDER BY pan2.dat_cadastro_pan DESC, pan2.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			)
			ORDER BY NomeInstituicaoOrigem
			";
			
			select = select.Replace("@unidades", unidades);
			select = select.Replace("@pessoa", idUsuario.ToString());
			
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
		}

		public DataTable BuscarProcessosAReceberDoUsuario(int idUsuario)
		{
			SrvUnidadeExercicioFuncaoPessoa srvUefp = new SrvUnidadeExercicioFuncaoPessoa();
			
			List<int> listaSubUnidades = srvUefp.listarIdsSubUnidadesPessoa(idUsuario);

			string unidades = "";

			foreach(int s in listaSubUnidades)
				unidades += s.ToString()+",";

			if(!string.IsNullOrEmpty(unidades))
				unidades = unidades.Remove(unidades.Length-1);

			string select = @"
			SELECT 
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'SPU'
			)
			) as NumeroSpu,
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'VIPROC'
			)
			) as NumeroVIPROC,
			(
			SELECT npr.txt_numero_processo_npr
			FROM adm_licitar.tb_numero_processo_npr npr
			WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro
			AND npr.fk_cod_tipo_numero_tnu = 
			(
			SELECT pk_cod_tipo_numero_tnu
			FROM adm_licitar.tb_tipo_numero_tnu
			WHERE txt_descricao_tnu = 'LICITAÇÃO'
			)
			) as NumeroLicitacao,
			ins.txt_sigla_ins as NomeInstituicaoOrigem, 
			pro.txt_resumo_objeto_pro as ObjetoProcesso, 
			ati.txt_descricao_ati as NomeTipoAndamento, 
			pan.dat_cadastro_pan as DataUltimoAndamento, 
			pro.pk_cod_processo_pro as Processo
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_processo_pro pro ON pro.pk_cod_processo_pro = pan.fk_cod_processo_pro
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_instituicao_ins ins ON ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins
			WHERE (
			(pan.fk_cod_pessoa_pes = @pessoa)
			OR 
			(pan.fk_cod_unidade_exercicio_uex IN (@unidades) AND pan.fk_cod_pessoa_pes is null)
			)
			AND pan.pk_cod_processo_andamento_pan = 
			(
			SELECT pan2.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan pan2
			WHERE pan2.fk_cod_processo_pro = pro.pk_cod_processo_pro
			ORDER BY pan2.dat_cadastro_pan DESC, pan2.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			)
			ORDER BY NomeInstituicaoOrigem
			";
			
			select = select.Replace("@unidades", unidades);
			select = select.Replace("@pessoa", idUsuario.ToString());
			
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
		}
		
		public int CountRecebido(int idUsuario)
		{
			int idPessoa = Pessoa.Find(idUsuario).Id;
		
			string[] andamentosExcluidosDaPesquisa = {"TRAMITAR","CONCLUÍDO","ENCAMINHADO PARA SETORIAL"}; 
			DetachedCriteria dcDiferenteTRAMITAR = DetachedCriteria.For(typeof(ConsultaReceberTramitar));												
			dcDiferenteTRAMITAR.Add(Expression.Not(Expression.In("NomeTipoAndamento",andamentosExcluidosDaPesquisa)))
				.Add(Expression.Eq("IdPessoaCadastro",idPessoa));
			
			CountQuery max = new CountQuery(typeof(ConsultaReceberTramitar), dcDiferenteTRAMITAR);				
			int count = (int)ActiveRecordMediator.ExecuteQuery(max);								
			return count;	
		}
		
		public FluxoAndamento RetornaFluxoAndamento(string atividade,ProcessoAndamento objProcAndamento)
		{
			
			Atividade tipoAndamento = Atividade.BuscarTipoAndamentoPorDescricao(atividade);
			
			DetachedCriteria dcFluxo = DetachedCriteria.For(typeof(FluxoAndamento));				
			dcFluxo.CreateAlias("Fase","fas");
			dcFluxo.CreateAlias("Atividade","ati");
			if(objProcAndamento.FluxoAndamento !=null && objProcAndamento.FluxoAndamento.Fase != null)
			{
				dcFluxo.Add(Expression.Eq("fas.Descricao",objProcAndamento.FluxoAndamento.Fase.Descricao));
			}
			dcFluxo.Add(Expression.Eq("Atividade",tipoAndamento));
			dcFluxo.Add(Expression.Eq("ati.TipoAndamento",true));
			dcFluxo.Add(Expression.Sql("this_.fk_cod_workflow_wor in (select wor.pk_cod_workflow_wor from adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu inner join adm_licitar.tb_workflow_wor wor on wmu.fk_cod_workflow_wor = wor.pk_cod_workflow_wor where wmu.fk_cod_modalidade_mod="+objProcAndamento.Processo.Classificacao.Modalidade.Id+")"));
			FluxoAndamento objFluxo = FluxoAndamento.FindFirst(dcFluxo);
			return objFluxo;
		}
		
		
		public static Boolean VerificarBloquearTodosOrgaos()
		{
			string bloquear = ConsultaXML.retornarSQL("DataDecretoBOO");
			return Convert.ToBoolean(bloquear);
		}
		
		//caso volte novamente:
		public static DateTime VerificarDecretoPregao(string strInstituicao)
		{
			string DataDecreto = string.Empty;
			//caso volte novamente:
			if(!VerificarBloquearTodosOrgaos())
			 	DataDecreto = ConsultaXML.retornarSQL("DataDecreto"+strInstituicao.ToUpper());	
			else
			 	DataDecreto = ConsultaXML.retornarSQL("DataDecreto");	
			
			DateTime dtDecreto = DateTime.Parse("01/01/0001");				
			if(!string.IsNullOrEmpty(DataDecreto))
				dtDecreto = Convert.ToDateTime(DataDecreto);
		
			return dtDecreto;
		}
		
		//caso volte novamente:
		public static string VerificarDecretoFrase(string strInstituicao)
		{		
			string retorno = string.Empty;
			if(!VerificarBloquearTodosOrgaos())
			 retorno = ConsultaXML.retornarSQL("MsgDecreto").Replace("@data", Convert.ToString(VerificarDecretoPregao(strInstituicao)).Replace("00:00:00",string.Empty)).Replace("@instituicao",strInstituicao.Replace("|"," - "));
			else
			 retorno = ConsultaXML.retornarSQL("MsgDecreto").Replace("@data", Convert.ToString(VerificarDecretoPregao(string.Empty)).Replace("00:00:00",string.Empty)).Replace("@instituicao",strInstituicao.Replace("|"," - "));
			return retorno;
		}

		public bool verificarIsDonoProcesso(string processo, string pessoa)
		{
			//Console.WriteLine("bla1");
			string idPessoaCadastrante = "";
			string select = @"

			SELECT pan.fk_cod_pessoa_cadastrante_pes, ati.txt_descricao_ati
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE pan.fk_cod_processo_pro = @processo
			ORDER BY pan.dat_cadastro_pan DESC, pan.pk_cod_processo_andamento_pan DESC
			LIMIT 1";
			
			select = select.Replace("@processo", processo);
			select = select.Replace("@pessoa", pessoa);
			//Console.WriteLine("bla2");
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			//Console.WriteLine("bla3");
			foreach(DataRow row in dt.Rows)
			{
				//Console.WriteLine("bla4");
				if(!row["txt_descricao_ati"].ToString().Equals("TRAMITAR"))
					idPessoaCadastrante = row["fk_cod_pessoa_cadastrante_pes"].ToString();
				else
					return false;
			}
			
			return idPessoaCadastrante.Equals(pessoa);
			
		}

		public string getNumSPU(int idProcesso)
		{
			string spu = "";
			string select = @"
						SELECT txt_numero_processo_npr
						FROM adm_licitar.tb_numero_processo_npr
						WHERE boo_principal_npr = true
						AND fk_cod_processo_pro = @processo 
						LIMIT 1";
			
			select = select.Replace("@processo", idProcesso.ToString());
						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				spu = row["txt_numero_processo_npr"].ToString();
			}
			
			return spu;
			
		}

		public string ListarDonoProcesso(string processo)
		{
			string pessoa = "";
			string select = @"
			
			SELECT pes.txt_nome_pes as pessoa
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_pessoa_pes pes ON pes.pk_cod_pessoa_pes = pan.fk_cod_pessoa_cadastrante_pes
			WHERE pan.fk_cod_processo_pro = @processo
			ORDER BY pan.dat_cadastro_pan DESC, pan.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			";
			
			select = select.Replace("@processo", processo);
						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				pessoa = row["pessoa"].ToString();
			}

			if(string.IsNullOrEmpty(pessoa))
			{
				return null;
			}
			else
				return pessoa;									
		}

		public string buscarProcessosPessoa(string pessoa)
		{
			string idProcesso = "";
			string select = @"
			SELECT pk_cod_processo_pro
			FROM adm_licitar.vw_consulta_receber_tramitar_crt
			WHERE (cod_pessoa_and_rec IN (@pessoa) OR (cod_pessoa_cadastro IN (@pessoa) AND cod_pessoa_and_rec is null )) 
			";
						
			select = select.Replace("@pessoa", pessoa);
			
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
			foreach(DataRow row in dt.Rows)
			{
				idProcesso += row["pk_cod_processo_pro"].ToString() + ",";
			}

			if(!string.IsNullOrEmpty(idProcesso))
				idProcesso = idProcesso.Remove(idProcesso.Length-1);
							
			return idProcesso;
			
		}

		public UnidadeExercicio GetUnidadeExercicioDoResponsavel(int idProcesso)
		{
			Pessoa responsavel = null;
			
			DetachedCriteria dcAndamentos = DetachedCriteria.For(typeof(ProcessoAndamento));
			dcAndamentos.Add(Expression.Eq("Processo.Id", idProcesso));
			dcAndamentos.CreateAlias("FluxoAndamento", "fan");
			dcAndamentos.CreateAlias("fan.Atividade", "ati");
			string[] tipos = {"RECEBER", "TRAMITAR"};
			dcAndamentos.Add(Expression.In("ati.Descricao", tipos));			
			dcAndamentos.AddOrder(Order.Asc("Id"));			
			ProcessoAndamento[] andamentos = ProcessoAndamento.FindAll(dcAndamentos);

			if (andamentos.Length > 0)
			{
				if (andamentos[andamentos.Length].FluxoAndamento.Atividade.Descricao == "TRAMITAR")
				{
					responsavel = andamentos[andamentos.Length].Pessoa;
				}
				else if (andamentos[andamentos.Length].FluxoAndamento.Atividade.Descricao == "RECEBER")
				{
					responsavel = andamentos[andamentos.Length].Cadastrante;
				}
			}
			else
			{
				DetachedCriteria dcAndamentos2 = DetachedCriteria.For(typeof(ProcessoAndamento));
				dcAndamentos2.Add(Expression.Eq("Processo.Id", idProcesso));
				dcAndamentos2.AddOrder(Order.Asc("Id"));
				andamentos = ProcessoAndamento.FindAll(dcAndamentos2);
				responsavel = andamentos[andamentos.Length].Cadastrante;
			}		

			return responsavel.UnidadeExercicioFuncao.UnidadeExercicio;
		}

		public int GetUnidadeExercicioDoResponsavelSql(int idProcesso)
		{
			int idResponsavel = 0;
			int idUnidadeExercicioResponsavel = 0;
			string select = string.Empty;
			DataTable dt = null;
			
			select = @"
				select
				fk_cod_pessoa_pes, fk_cod_pessoa_cadastrante_pes, txt_descricao_ati
				from adm_licitar.tb_processo_andamento_pan pan 
				join adm_licitar.tb_fluxo_andamento_fan fan on (fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan)
				join adm_licitar.tb_atividade_ati ati on (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati)
				where fk_cod_processo_pro = @fk_cod_processo_pro
				and txt_descricao_ati in ('TRAMITAR', 'RECEBER') order by pk_cod_processo_andamento_pan desc
			";
			select = select.Replace("@fk_cod_processo_pro", idProcesso.ToString());				
			dt = new PostgreSqlDatabase().Consultar(select);

			if (dt.Rows.Count > 0)
			{
				if (dt.Rows[0]["txt_descricao_ati"].ToString() == "TRAMITAR")
				{
					idResponsavel = Convert.ToInt32(dt.Rows[0]["fk_cod_pessoa_pes"]);
				}
				else if (dt.Rows[0]["txt_descricao_ati"].ToString() == "RECEBER")
				{
					idResponsavel = Convert.ToInt32(dt.Rows[0]["fk_cod_pessoa_cadastrante_pes"]);
				}
			}

			if (idResponsavel == 0)
			{
				select = @"
					select
					fk_cod_pessoa_cadastrante_pes
					from adm_licitar.tb_processo_andamento_pan pan 
					join adm_licitar.tb_fluxo_andamento_fan fan on (fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan)
					join adm_licitar.tb_atividade_ati ati on (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati)
					where fk_cod_processo_pro = @fk_cod_processo_pro and txt_descricao_ati not in
					(
						'LICITAÇÃO MARCADA', 
						'LICITAÇÃO MARCADA EM CONTINUIDADE',
						'DISPUTA', 
						'SESSÃO DE ABERTURA - PROPOSTA TÉCNICA', 
						'SESSÃO DE ABERTURA - HABILITAÇÃO', 
						'SESSÃO DE RESULTADO - PROPOSTA COMERCIAL', 
						'SESSÃO DE ABERTURA - PROPOSTA COMERCIAL', 
						'SESSÃO FINAL',
						'SESSÃO DE RESULTADO - PROPOSTA TÉCNICA'
					)
					order by pk_cod_processo_andamento_pan desc limit 1
				";

				select = select.Replace("@fk_cod_processo_pro", idProcesso.ToString());				
				dt = new PostgreSqlDatabase().Consultar(select);

				idResponsavel = Convert.ToInt32(dt.Rows[0]["fk_cod_pessoa_cadastrante_pes"]);
			}

			if (idResponsavel > 0)
			{
				select = @"
					select fk_cod_unidade_exercicio_uex from adm_licitar.tb_pessoa_pes pes
					join adm_licitar.tb_unidade_exercicio_funcao_uef uef on uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef
					where pes.pk_cod_pessoa_pes = @pk_cod_pessoa_pes
				";
				select = select.Replace("@pk_cod_pessoa_pes", idResponsavel.ToString());
				dt = new PostgreSqlDatabase().Consultar(select);
				idUnidadeExercicioResponsavel = Convert.ToInt32(dt.Rows[0]["fk_cod_unidade_exercicio_uex"]);
			}
			else
			{
				idUnidadeExercicioResponsavel = 0;
			}
			
			return idUnidadeExercicioResponsavel;
		}

		public int GetIdResponsavelProcesso(int idProcesso)
		{
			int idResponsavel = 0;
			int idUnidadeExercicioResponsavel = 0;
			string select = string.Empty;
			DataTable dt = null;
			
			select = @"
				select
				fk_cod_pessoa_pes, fk_cod_pessoa_cadastrante_pes, txt_descricao_ati
				from adm_licitar.tb_processo_andamento_pan pan 
				join adm_licitar.tb_fluxo_andamento_fan fan on (fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan)
				join adm_licitar.tb_atividade_ati ati on (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati)
				where fk_cod_processo_pro = @fk_cod_processo_pro
				ORDER BY pan.dat_cadastro_pan DESC, pan.pk_cod_processo_andamento_pan DESC
			";
			select = select.Replace("@fk_cod_processo_pro", idProcesso.ToString());				
			dt = new PostgreSqlDatabase().Consultar(select);

			if (dt.Rows.Count > 0)
			{
				if (dt.Rows[0]["txt_descricao_ati"].ToString() == "TRAMITAR")
				{
					idResponsavel = Convert.ToInt32(dt.Rows[0]["fk_cod_pessoa_pes"]);
				}
				else
				{
					idResponsavel = Convert.ToInt32(dt.Rows[0]["fk_cod_pessoa_cadastrante_pes"]);
				}
			}

			return idResponsavel;
		}

		
		public ProcessoCompleto[] ListarProcessos(DTOConProcesso dtoConProcesso, Pessoa usuarioLogado, bool blnProcComp)
		{

			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoCompleto));
			

		
			//Verifica se no dto de filtro foi selecionado alguma instituição			
			if (dtoConProcesso.IdInstituicao > 0)
			{
				dc.Add(Expression.Eq("Instituicao.Id", dtoConProcesso.IdInstituicao));
			}

			//Verifica se no dto de filtro foi selecionado alguma modalidade
			if (dtoConProcesso.IdModalidade > 0)
			{
				dc.Add(Expression.Eq("Modalidade.Id", dtoConProcesso.IdModalidade));
			}

			//Verifica se no dto de filtro foi selecionado algum número
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroSpu))
			{
					dc.Add(Expression.Eq("NumeroSpu",dtoConProcesso.NumeroSpu));	
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroViproc))
			{
					dc.Add(Expression.Eq("NumeroViproc",dtoConProcesso.NumeroViproc));	
			}

			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroLicitacao))
			{
					dc.Add(Expression.Eq("NumeroLicitacao",dtoConProcesso.NumeroLicitacao));	
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroMAPP))
			{				
					dc.Add(Expression.Eq("NumeroMapp",dtoConProcesso.NumeroMAPP));	
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroComprasnet))
			{
					dc.Add(Expression.Eq("NumeroComprasNet",dtoConProcesso.NumeroComprasnet));	
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroSisBB))
			{									
					dc.Add(Expression.Eq("NumeroSisBB",dtoConProcesso.NumeroSisBB));				
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroPortalDigital))
			{									
					dc.Add(Expression.Eq("NumeroDigital",dtoConProcesso.NumeroPortalDigital));				
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.DataInicio) && !string.IsNullOrEmpty(dtoConProcesso.DataFim))
			{
				dc.Add(Expression.Sql("this_.dat_cadastro_pan between '"+dtoConProcesso.DataInicio+" 00:00:00' and '"+dtoConProcesso.DataFim+" 23:59:59'"));
			}

			if (!string.IsNullOrEmpty(dtoConProcesso.ResumoObjeto))
			{
				dc.Add(Expression.InsensitiveLike("ResumoObjeto", "%"+dtoConProcesso.ResumoObjeto+"%"));
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NomePessoa))
			{
				DetachedCriteria dp = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
				dp.CreateAlias("Pessoa","pes");
				dp.Add(Expression.InsensitiveLike("pes.Nome", '%' + dtoConProcesso.NomePessoa + '%'));
				ProcessoPapelPessoa[] ppp = ProcessoPapelPessoa.FindAll(dp);
			
				string[] pessoa = new string[ppp.Length];
			
				for (int i=0;i<ppp.Length;i++)
				{
					pessoa[i] = ppp[i].Processo.Id.ToString();
				}
			
				dc.Add(Expression.In("Id", pessoa));
			}
			
			return ProcessoCompleto.FindAll(dc);
				
		}
		
		
		public ProcessoPapelPessoa[] ListarPessoasEnvolvidas(int idProcesso)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
			dc.CreateAlias("Papel","pap");
			dc.Add(Expression.Eq("Processo.Id", idProcesso));
			dc.Add(Expression.Not(Expression.Eq("pap.Descricao", "VENCEDOR")));
			return ProcessoPapelPessoa.FindAll(dc);
		}

		/// <summary>
		/// Método que retorna uma lista de DTO do objeto Processo a partir de um dto com os filtros especificados
		/// </summary>
		/// <param name="dtoConProcesso">
		/// A <see cref="DTOConProcesso"/> com os filtros especificados na consulta de processos
		/// </param>
		/// <param name="usuarioLogado">
		/// A <see cref="Pessoa"/> com informações da pessoa logada
		/// </param>
		/// <returns>
		/// A <see cref="Processo"/> com a lista dos processos encontrados
		/// </returns>
		public Processo[] ListarProcessos(DTOConProcesso dtoConProcesso, Pessoa usuarioLogado)
		{
			
			DetachedCriteria dc = DetachedCriteria.For(typeof(Processo));
			//Verifica se no dto de filtro foi selecionado alguma instituição
			if (dtoConProcesso.IdInstituicao > 0)
			{
				dc.Add(Expression.Eq("Instituicao.Id", dtoConProcesso.IdInstituicao));
			}

			//Verifica se no dto de filtro foi selecionado alguma modalidade
			if (dtoConProcesso.IdModalidade > 0)
			{
				dc.CreateAlias("Classificacao", "cla");
				dc.CreateAlias("cla.Modalidade", "mod");				
				dc.Add(Expression.Eq("mod.Id", dtoConProcesso.IdModalidade));
			}

			//Verifica se no dto de filtro foi selecionado algum número
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroSpu))
			{
				DetachedCriteria ds = DetachedCriteria.For(typeof(NumeroProcesso));
				ds.CreateAlias("TipoNumero", "tn");
				//Verifica se existem processos associados através do tipo de numero Manifestação de Interesse
				if(dtoConProcesso.ProcessosAssociados)
				{
					ds.Add(Expression.Or(Expression.Eq("tn.Descricao", "SPU"),Expression.Eq("tn.Descricao", "MANIFESTAÇÃO DE INTERESSE")));
				}
				else
				{
					ds.Add(Expression.Eq("tn.Descricao", "SPU"));
				}
				ds.Add(Expression.Eq("numeroProcesso", dtoConProcesso.NumeroSpu));
				
				NumeroProcesso[] np = NumeroProcesso.FindAll(ds);
				string[] spu = new string[np.Length];			
			
				for(int i=0; i < np.Length; i++)
				{
					spu[i] = np[i].Processo.Id.ToString();
				}
			
				dc.Add(Expression.In("Id", spu));
				
			}

			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroLicitacao))
			{
				
					DetachedCriteria dl = DetachedCriteria.For(typeof(NumeroProcesso));
					dl.CreateAlias("TipoNumero", "tn");
					dl.Add(Expression.Eq("tn.Descricao", "LICITAÇÃO"));
					dl.Add(Expression.Eq("numeroProcesso", dtoConProcesso.NumeroLicitacao));
					NumeroProcesso[] np = NumeroProcesso.FindAll(dl);
				
					string[] licitacao = new string[np.Length];
				
					for(int i=0;i<np.Length;i++)
					{
						licitacao[i] = np[i].Processo.Id.ToString();
					}
				
					dc.Add(Expression.In("Id",licitacao));
				
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroMAPP))
			{
				
					DetachedCriteria dl = DetachedCriteria.For(typeof(NumeroProcesso));
					dl.CreateAlias("TipoNumero", "tn");
					dl.Add(Expression.Eq("tn.Descricao", "MAPP"));
					dl.Add(Expression.Eq("numeroProcesso", dtoConProcesso.NumeroMAPP));
					NumeroProcesso[] np = NumeroProcesso.FindAll(dl);
				
					string[] mapp = new string[np.Length];
				
					for(int i=0;i<np.Length;i++)
					{
						mapp[i] = np[i].Processo.Id.ToString();
					}
				
					dc.Add(Expression.In("Id",mapp));
				
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroComprasnet))
			{
				
					DetachedCriteria dl = DetachedCriteria.For(typeof(NumeroProcesso));
					dl.CreateAlias("TipoNumero", "tn");
					dl.Add(Expression.Eq("tn.Descricao", "COMPRASNET"));
					dl.Add(Expression.Eq("numeroProcesso", dtoConProcesso.NumeroComprasnet));
					NumeroProcesso[] np = NumeroProcesso.FindAll(dl);
				
					string[] licitacao = new string[np.Length];
				
					for(int i=0;i<np.Length;i++)
					{
						licitacao[i] = np[i].Processo.Id.ToString();
					}
				
					dc.Add(Expression.In("Id",licitacao));
				
			}
			
			if (!string.IsNullOrEmpty(dtoConProcesso.NumeroSisBB))
			{
				
					DetachedCriteria dl = DetachedCriteria.For(typeof(NumeroProcesso));
					dl.CreateAlias("TipoNumero", "tn");
					dl.Add(Expression.Eq("tn.Descricao", "SIS-BB"));
					dl.Add(Expression.Eq("numeroProcesso", dtoConProcesso.NumeroSisBB));
					NumeroProcesso[] np = NumeroProcesso.FindAll(dl);
				
					string[] licitacao = new string[np.Length];
				
					for(int i=0;i<np.Length;i++)
					{
						licitacao[i] = np[i].Processo.Id.ToString();
					}
				
					dc.Add(Expression.In("Id",licitacao));
				
			}
			
			

			if (!string.IsNullOrEmpty(dtoConProcesso.DataInicio) && string.IsNullOrEmpty(dtoConProcesso.DataFim))
			{
				DetachedCriteria da = DetachedCriteria.For(typeof(ProcessoAndamento));
				da.CreateAlias("FluxoAndamento","fa");
				da.CreateAlias("fa.Atividade","at");						
				da.Add(Expression.Between("DataCadastro", DateTime.Parse(dtoConProcesso.DataInicio), DateTime.Parse(dtoConProcesso.DataFim).Add(new TimeSpan(23, 59, 59))));
				da.Add(Expression.Eq("at.Descricao","CADASTRADO"));
				ProcessoAndamento[] pa = ProcessoAndamento.FindAll(da);
			
				string[] andamento = new string[pa.Length];
			
				for(int i=0;i<pa.Length;i++)
				{
					andamento[i] = pa[i].Processo.Id.ToString();
				}
			
				dc.Add(Expression.In("Id", andamento));									
			}

			if (!string.IsNullOrEmpty(dtoConProcesso.NomePessoa))
			{
				DetachedCriteria dp = DetachedCriteria.For(typeof(ProcessoPapelPessoa));
				dp.CreateAlias("Pessoa","pes");
				dp.Add(Expression.InsensitiveLike("pes.Nome", '%' + dtoConProcesso.NomePessoa + '%'));
				ProcessoPapelPessoa[] ppp = ProcessoPapelPessoa.FindAll(dp);
			
				string[] pessoa = new string[ppp.Length];
			
				for (int i=0;i<ppp.Length;i++)
				{
					pessoa[i] = ppp[i].Processo.Id.ToString();
				}
			
				dc.Add(Expression.In("Id", pessoa));
			}

			if (!string.IsNullOrEmpty(dtoConProcesso.ResumoObjeto))
			{
				dc.Add(Expression.InsensitiveLike("ResumoObjeto", "%"+dtoConProcesso.ResumoObjeto+"%"));
			}

			if (!string.IsNullOrEmpty(dtoConProcesso.ReceberTramitar))
			{
				if(dtoConProcesso.ReceberTramitar == "TRAMITAR")
				{								
					DetachedCriteria dcAnd = DetachedCriteria.For(typeof(ProcessoAndamento));
					dcAnd.CreateAlias("FluxoAndamento","fan");
					dcAnd.CreateAlias("fan.Atividade","ati");
					dcAnd.Add(Expression.Eq("Pessoa", usuarioLogado));
					dcAnd.Add(Expression.Eq("ati.Descricao","TRAMITAR"));
					
					ProcessoAndamento[] objProcessoaAnd = ProcessoAndamento.FindAll(dcAnd);
					
					int[] filtroAnda = new int[objProcessoaAnd.Length];
					
					for(int i=0;i<objProcessoaAnd.Length;i++)
					{
						filtroAnda[i] = objProcessoaAnd[i].Processo.Id;
					}
					
					dc.Add(Expression.In("Id",filtroAnda));
				}				
			}
			Processo[] proc = Processo.FindAll(dc);
			return Processo.FindAll(dc);
		}

		public string FormataEconomiaPorcentagem(string valor)
		{
			string retorno = string.Empty;
			if(!string.IsNullOrEmpty(valor) && valor != "0")
			{
				if(valor.Contains(","))
				{
					string[] valorArray = valor.Split(',');
				 	if(valorArray[1] == "000")
					{
						retorno = valorArray[0];
					}
				 	else
					{
						retorno = decimal.Round(decimal.Parse(valor),2).ToString();
					}
				}
					else
					{
						retorno = decimal.Round(decimal.Parse(valor),2).ToString();
					}
			}
			return retorno;
		}

		public decimal FormataDecimal(string valor)
		{			
			decimal parse = decimal.Zero;
			if(!string.IsNullOrEmpty(valor))
				parse = (decimal.TryParse(valor,out parse)) ? parse : decimal.Zero;
			return parse;
		}

		public bool VerificarExistenciaPessoa(string cpf_cnpj)
		{
			DetachedCriteria pesqPessoa = DetachedCriteria.For(typeof(Pessoa));
			
			pesqPessoa.Add(Expression.Eq("CpfCnpj",cpf_cnpj));
			return Pessoa.Exists(pesqPessoa);
		}

		public Processo ListarProcesso(int IdProcesso)
		{
			return Processo.Find(IdProcesso);
		}


		public Processo[] ListarProcessos(int idPai)
		{			
				DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Processo));
                pesquisa.Add(Expression.Eq("Id", idPai));

                Order[] ordem = new Order[]
				{
					Order.Desc("Id")
				};

                return Processo.FindAll(pesquisa, ordem);
		}
		
		public Natureza[] ListarNaturezaClassificacao(int idModalidade)
		{
			DetachedCriteria dc2 = DetachedCriteria.For(typeof(Classificacao));
				dc2.CreateAlias("Modalidade","mod");
				dc2.Add(Expression.Eq("mod.Id",Convert.ToInt32(idModalidade)));
				Classificacao[] oC = Classificacao.FindAll(dc2);
				int[] ids = new int[oC.Length];
				for (int i = 0; i < oC.Length; i++)
				{
					ids[i] = oC[i].Natureza.Id; 
				}

               return Natureza.FindAll(Order.Asc("Descricao"),Expression.In("Id",ids));
		}
		
		//Criado por Márcio Fernandes,17/02/2010. Este método facilitará quando precisarmos alterar uma das 03 três frases 
		// pré-definidas, conferencia, conferido e generica.
		// Inicio MF.
		public string FrasesDaConferencia(string nomeFrase)
		{
			string frase = string.Empty;
			
			if (nomeFrase == "Generico")
			{
				frase = "Este processo já foi conferido ou está em conferência, portanto, disponível somente para visualização.";	
			}
			
			if(nomeFrase == "Conferido")
			{
				frase = "Este processo já foi conferido, portanto, disponível somente para visualização.";
			}
			
			if (nomeFrase == "Conferencia")
			{
				frase = "Este processo está em conferência, portanto, disponível somente para visualização.";
			}
			
			return frase;
		}
		//Fim MF
		
		///Metodo consulta processo a partir do numero SPU
		public Processo  getProcesso(string numeroSPU)
		{
			
			DetachedCriteria dcNumProc = DetachedCriteria.For(typeof(NumeroProcesso));
			dcNumProc.CreateAlias("TipoNumero", "tn");
			dcNumProc.Add(Expression.Eq("tn.Descricao", "SPU"));
			dcNumProc.Add(Expression.Eq("numeroProcesso", numeroSPU));
			NumeroProcesso np = NumeroProcesso.FindFirst(dcNumProc);
			if (np != null)
			{
				return np.Processo;
				
			}
			else
				throw new Exception("Número SPU inexistente");
		}
		
	}
		
		
}

