// SrvTramitacaoUnidadeExercicio.cs created with MonoDevelop
// User: guilhermefacanha at 14:22 5/5/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using NHibernate.Expression;

using Licitar.Business.Dao;
using Licitar.Business.Entidade;
using System.Collections.Generic;
using Licitar.Business.Dto;
using Licitar.Business.Persistencia;
using Castle.ActiveRecord;

namespace Licitar.Business.Servico
{
	public struct structTUEAndamento
	{
		public string processo;
		public string unidade;
		public string datatramitacao;
	}	
	
	public class SrvTramitacaoUnidadeExercicio : PostgreSqlDatabase
	{
		public string listarUltimaUnidadeExercicioTramitacaoProcesso(int idProcesso)
		{
			string select =@"
			SELECT fk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
			WHERE fk_cod_processo_pro = @idProcesso
			ORDER BY dat_tramitacao_tue DESC, pk_cod_tramitacao_unidade_exercicio_tue DESC 
			LIMIT 1
			";

			select = select.Replace("@idProcesso",idProcesso.ToString());

			DataTable dt = Consultar(select);

			string unidade="";
			foreach(DataRow row in dt.Rows)
			{
				unidade = row["fk_cod_unidade_exercicio_uex"].ToString();
			}

			return unidade;
		}

		public TramitacaoUnidadeExercicio getTramitacaoUnidadeExercicioProcesso(int idProcesso)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(TramitacaoUnidadeExercicio));
			dc.Add(Expression.Eq("Processo.Id",idProcesso));
			dc.AddOrder(Order.Desc("DataEntradaUnidade"));
			dc.AddOrder(Order.Desc("Id"));

			return TramitacaoUnidadeExercicio.FindFirst(dc);
		}
		
		public SrvTramitacaoUnidadeExercicio()
		{
		}

		public bool deletarRegistrosUnidadeExercicioTramitacaoProcesso()
		{
			string select =@"
			DELETE FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
			";
			
			ExecutarComando(select);

			return true;
		}

		public bool inserirProcessosSemAndamentoTramitar()
		{
			string select =@"
			SELECT pan.fk_cod_processo_pro, pan.dat_cadastro_pan,
			(
			SELECT uef.fk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			WHERE efp.fk_cod_pessoa_pes = pan.fk_cod_pessoa_cadastrante_pes
			AND (dat_fim_efp = '-infinity' OR dat_fim_efp is null)
			ORDER BY dat_inicio_efp DESC
			LIMIT 1
			)  as unidade_cadastrante
			FROM adm_licitar.tb_processo_andamento_pan pan
			WHERE pan.fk_cod_processo_pro not in
			(
			SELECT DISTINCT pan.fk_cod_processo_pro
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE ati.txt_descricao_ati = 'TRAMITAR'
			ORDER BY pan.fk_cod_processo_pro
			)
			AND pan.pk_cod_processo_andamento_pan = 
			(
			SELECT pan2.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan pan2
			WHERE pan2.fk_cod_processo_pro = pan.fk_cod_processo_pro
			ORDER BY pan2.dat_cadastro_pan DESC, pan2.pk_cod_processo_andamento_pan DESC
			LIMIT 1
			)
			AND (pan.dat_cadastro_pan is not null  AND pan.dat_cadastro_pan <> '-infinity')
			";
			
			DataTable dt = Consultar(select);
			TramitacaoUnidadeExercicio objTUE;
			
			foreach(DataRow row in dt.Rows)
			{
				int unidade = Convert.ToInt32(row["unidade_cadastrante"].ToString());
				int processo = Convert.ToInt32(row["fk_cod_processo_pro"].ToString());
				string data = row["dat_cadastro_pan"].ToString();
				
				objTUE = new TramitacaoUnidadeExercicio();
				objTUE.DataEntradaUnidade = Convert.ToDateTime(data);
				objTUE.UnidadeExercicio = UnidadeExercicio.Find(unidade);
				objTUE.Processo = Processo.Find(processo);
				objTUE.Save();
			}

			return true;
		}

		public List<string> getProcessosComAndamentoTramitar()
		{
			List<string> lista = new List<string>();
			string select =@"
			SELECT DISTINCT pan.fk_cod_processo_pro
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE ati.txt_descricao_ati = 'TRAMITAR'
			ORDER BY pan.fk_cod_processo_pro
			";
			
			DataTable dt = Consultar(select);
					
			foreach(DataRow row in dt.Rows)
			{
				lista.Add(row["fk_cod_processo_pro"].ToString());
			}

			return lista;
		}		
		
		public List<structTUEAndamento> getAndamentosTramitarProcesso(string id)
		{
			List<structTUEAndamento> lista = new List<structTUEAndamento>();

			string select =@"
			SELECT distinct pan.pk_cod_processo_andamento_pan ,pan.dat_cadastro_pan as data, pan.fk_cod_processo_pro as processo, pan.fk_cod_unidade_exercicio_uex as unidade,
			(
			SELECT uef.fk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			WHERE efp.fk_cod_pessoa_pes = pan.fk_cod_pessoa_pes
			AND (dat_fim_efp = '-infinity' OR dat_fim_efp is null)
			ORDER BY dat_inicio_efp DESC
			LIMIT 1
			) as unidade_pessoa,
			(
			SELECT uef.fk_cod_unidade_exercicio_uex
			FROM adm_licitar.tb_unidade_exercicio_funcao_pessoa_efp efp
			INNER JOIN adm_licitar.tb_unidade_exercicio_funcao_uef uef ON uef.pk_cod_unidade_exercicio_funcao_uef = efp.fk_cod_unidade_exercicio_funcao_uef
			WHERE efp.fk_cod_pessoa_pes = pan.fk_cod_pessoa_cadastrante_pes
			AND (dat_fim_efp = '-infinity' OR dat_fim_efp is null)
			ORDER BY dat_inicio_efp DESC
			LIMIT 1
			)  as unidade_cadastrante
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE ati.txt_descricao_ati = 'TRAMITAR'
			AND pan.fk_cod_processo_pro = @id
			ORDER BY pan.dat_cadastro_pan, pan.pk_cod_processo_andamento_pan
			";

			select = select.Replace("@id",id);
			
			DataTable dt = Consultar(select);

			string unidadeAtual = "";
			string unidadeProx = "";
			structTUEAndamento objStruct;
			
			foreach(DataRow row in dt.Rows)
			{
				if(!string.IsNullOrEmpty(row["unidade"].ToString()))
				{
					unidadeProx = row["unidade"].ToString();
				}
				else if(!string.IsNullOrEmpty(row["unidade_pessoa"].ToString()))
				{
					unidadeProx = row["unidade_pessoa"].ToString();
				}
				else
				{
					unidadeProx = row["unidade_cadastrante"].ToString();
				}
				
				unidadeAtual = unidadeProx;
				objStruct = new structTUEAndamento();
				objStruct.unidade = unidadeProx;
				objStruct.datatramitacao = row["data"].ToString();
				objStruct.processo = row["processo"].ToString();
				lista.Add(objStruct);				
			}

			return lista;
		}

		public int popularTabelaTramitacaoUnidade()
		{
			List<string> listaProcessos = getProcessosComAndamentoTramitar();
			int registros = 0;		
			
			using(TransactionScope scopo = new TransactionScope())
			{
				try
				{
					inserirProcessosSemAndamentoTramitar();
					foreach(string id in listaProcessos)
					{
						List<structTUEAndamento> listaAndamentosProcesso = getAndamentosTramitarProcesso(id);
						registros+=listaAndamentosProcesso.Count;
						TramitacaoUnidadeExercicio objTUE;
						foreach(structTUEAndamento obj in listaAndamentosProcesso)
						{
							objTUE = new TramitacaoUnidadeExercicio();
							objTUE.UnidadeExercicio = UnidadeExercicio.Find(Convert.ToInt32(obj.unidade));
							objTUE.Processo = Processo.Find(Convert.ToInt32(obj.processo));
							objTUE.DataEntradaUnidade = Convert.ToDateTime(obj.datatramitacao);
							objTUE.Save();
						}
					}			
					
					scopo.VoteCommit();
				}
				catch
				{
					scopo.VoteRollBack();
				}
			}

			return registros;
		}


		public void atualizarEncaminhadoSetorial()
		{
			string select =@"
			SELECT pan.dat_cadastro_pan as data, pan.fk_cod_processo_pro as cod
			FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue tue
			INNER JOIN adm_licitar.tb_processo_andamento_pan pan ON pan.fk_cod_processo_pro = tue.fk_cod_processo_pro
			WHERE pan.pk_cod_processo_andamento_pan = 
			(
			SELECT pan.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE pan.fk_cod_processo_pro = tue.fk_cod_processo_pro
			AND ati.txt_descricao_ati = 'ENCAMINHADO PARA SETORIAL'
			AND pan.dat_cadastro_pan > tue.dat_tramitacao_tue
			ORDER BY dat_cadastro_pan DESC, pk_cod_processo_andamento_pan DESC
			LIMIT 1
			)
			AND pk_cod_tramitacao_unidade_exercicio_tue = 
			(
			SELECT pk_cod_tramitacao_unidade_exercicio_tue
			FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
			WHERE fk_cod_processo_pro = tue.fk_cod_processo_pro
			ORDER BY dat_tramitacao_tue DESC, pk_cod_tramitacao_unidade_exercicio_tue DESC
			LIMIT 1
			)
			";
			
			DataTable dt = Consultar(select);
			TramitacaoUnidadeExercicio obj = null;
			int idProcesso = 0;
			
			foreach(DataRow row in dt.Rows)
			{
				idProcesso = Convert.ToInt32(row["cod"].ToString());
				
				obj = getTramitacaoUnidadeExercicioProcesso(idProcesso);
				
				if(obj!=null)
				{
					obj.DataEncaminhadoSetorial = Convert.ToDateTime(row["data"].ToString());
					obj.Update();
				}
				
			}

		}

		public void atualizarRecebidoSetorial()
		{
			string select =@"
			SELECT pan.dat_cadastro_pan as data, pan.fk_cod_processo_pro as cod
			FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue tue
			INNER JOIN adm_licitar.tb_processo_andamento_pan pan ON pan.fk_cod_processo_pro = tue.fk_cod_processo_pro
			WHERE pan.pk_cod_processo_andamento_pan = 
			(
			SELECT pan.pk_cod_processo_andamento_pan
			FROM adm_licitar.tb_processo_andamento_pan pan
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE pan.fk_cod_processo_pro = tue.fk_cod_processo_pro
			AND ati.txt_descricao_ati = 'RECEBIDO DA SETORIAL'
			AND pan.dat_cadastro_pan > tue.dat_tramitacao_tue
			ORDER BY dat_cadastro_pan DESC, pk_cod_processo_andamento_pan DESC
			LIMIT 1
			)
			AND pk_cod_tramitacao_unidade_exercicio_tue = 
			(
			SELECT pk_cod_tramitacao_unidade_exercicio_tue
			FROM adm_licitar.tb_tramitacao_unidade_exercicio_tue
			WHERE fk_cod_processo_pro = tue.fk_cod_processo_pro
			ORDER BY dat_tramitacao_tue DESC, pk_cod_tramitacao_unidade_exercicio_tue DESC
			LIMIT 1
			)
			";
			
			DataTable dt = Consultar(select);
			TramitacaoUnidadeExercicio obj = null;
			int idProcesso = 0;
			
			foreach(DataRow row in dt.Rows)
			{
				idProcesso = Convert.ToInt32(row["cod"].ToString());
				
				obj = getTramitacaoUnidadeExercicioProcesso(idProcesso);
				
				if(obj!=null)
				{
					obj.DataRecebidoSetorial = Convert.ToDateTime(row["data"].ToString());
					obj.Update();
				}
				
			}

		}
	}
}
