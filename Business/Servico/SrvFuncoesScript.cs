// SrvFuncoesScript.cs created with MonoDevelop
// User: guilhermefacanha at 13:48 26/6/2009
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
	public class SrvFuncoesScript: PostgreSqlDatabase
	{
		private List<string> listarProcessosSemPregoeiro(string dataInicio, string dataFim)
		{
			List<string> lista = new List<string>();
			string select =@"
			SELECT pcm.cod_processo_pro as cod
			FROM adm_licitar.tb_processo_completo_pcm pcm
			WHERE pcm.txt_estado_processo='FINALIZADO'
			AND pcm.dat_conclusao_pan between '@di' and '@df'
			AND pcm.cod_processo_pro NOT IN
			(
			SELECT distinct pcm2.cod_processo_pro
			FROM adm_licitar.tb_processo_completo_pcm pcm2
			INNER JOIN adm_licitar.tb_processo_papel_pessoa_ppp ppp ON ppp.fk_cod_processo_pro = pcm2.cod_processo_pro
			INNER JOIN adm_licitar.tb_pessoa_pes pes ON pes.pk_cod_pessoa_pes = ppp.fk_cod_pessoa_pes
			INNER JOIN adm_licitar.tb_papel_pap pap ON pap.pk_cod_papel_pap = ppp.fk_cod_papel_pap
			WHERE pcm2.txt_estado_processo='FINALIZADO'
			AND txt_descricao_pap IN ('PREGOEIRO')
			)
			";

			select = select.Replace("@di",dataInicio);
			select = select.Replace("@df",dataFim);

			DataTable dt = Consultar(select);
			
			foreach(DataRow row in dt.Rows)
			{
				lista.Add(row["cod"].ToString());
			}

			return lista;
		}

		private List<string> listarProcessosComDataConclusao(string dataInicio, string dataFim)
		{
			List<string> lista = new List<string>();
			string select =@"
			SELECT pcm.cod_processo_pro as cod
			FROM adm_licitar.tb_processo_completo_pcm pcm
			WHERE pcm.txt_estado_processo='FINALIZADO'
			AND pcm.dat_conclusao_pan between '@di' and '@df'			
			";

			select = select.Replace("@di",dataInicio);
			select = select.Replace("@df",dataFim);

			DataTable dt = Consultar(select);
			
			foreach(DataRow row in dt.Rows)
			{
				lista.Add(row["cod"].ToString());
			}

			return lista;
		}

		public string getCodigoPapelPorDescricao(string descricao)
		{
			string select =@"
			SELECT pap.pk_cod_papel_pap as id, pap.txt_descricao_pap as desc
			FROM adm_licitar.tb_papel_pap pap
			WHERE pap.txt_descricao_pap = '@descricao'
			";

			select = select.Replace("@descricao",descricao);

			DataTable dt = Consultar(select);

			string cod = "";
			
			foreach(DataRow row in dt.Rows)
			{
				cod = row["id"].ToString();
			}

			return cod;
		}

		public DataTable executarScript(string select)
		{
			string ver = select.ToUpper();
			if(ver.Contains("INSERT") || ver.Contains("DELETE") || ver.Contains("UPDATE"))
			{
				return null;
			}
			else
			{			
				DataTable dt = Consultar(select);
				return dt;
			}
		}

		public string executarFuncaoCorrigirPregoeirosProcesso(string di, string df)
		{
			SrvUnidadeExercicioFuncaoPessoa objSrvUEFP = new SrvUnidadeExercicioFuncaoPessoa();
			SrvProcessoPapelPessoa objSrvPPP = new SrvProcessoPapelPessoa();
			List<string> lista = this.listarProcessosComDataConclusao(di,df);
			string papel = this.getCodigoPapelPorDescricao("PREGOEIRO");
			ProcessoPapelPessoa objPPP = null;
			string resultado = "Lista de SPU's: ";

			foreach(string cod in lista)
			{
				string select =@"
				SELECT uex.pk_cod_unidade_exercicio_uex as unidade, pan.fk_cod_processo_pro as processo
				FROM adm_licitar.tb_processo_andamento_pan pan
				INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = pan.fk_cod_fluxo_andamento_fan
				INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
				INNER JOIN adm_licitar.tb_unidade_exercicio_uex uex ON uex.pk_cod_unidade_exercicio_uex = pan.fk_cod_unidade_exercicio_uex
				WHERE pan.fk_cod_processo_pro = '@processo'
				AND ati.txt_descricao_ati = 'TRAMITAR'
				AND uex.txt_descricao_uex ILIKE 'PREGOEIRO%'
				ORDER BY pan.dat_cadastro_pan DESC
				LIMIT 1
				";

				select = select.Replace("@processo",cod);

				DataTable dt = Consultar(select);

				bool existePregoeiroProcesso = false;

				using(TransactionScope scope = new TransactionScope())
				{
					try
					{						
						foreach(DataRow row in dt.Rows)
						{
							string unidade = row["unidade"].ToString();
							int idUnidade = 0;
							if(int.TryParse(unidade,out idUnidade))
							{
								UnidadeExercicioFuncaoPessoa[] listaPregoeiros = objSrvUEFP.ListarPregoeirosDaSubUnidade(idUnidade);
								if(listaPregoeiros.Length>0)
								{
									ProcessoPapelPessoa[] listaPPP = objSrvPPP.listarPessoasPorProcessoPapelPrincipal(Convert.ToInt32(cod), Convert.ToInt32(papel));

									if(listaPPP.Length>0)
									{
										foreach(ProcessoPapelPessoa p in listaPPP)
										{
											if(p.Pessoa == listaPregoeiros[0].Pessoa)
											{
												if(!p.PregoeiroPrincipal)
												{
													p.PregoeiroPrincipal = true;
													p.Update();
													resultado += Processo.numSPU(p.Processo.Id)+"; ";
												}
												existePregoeiroProcesso = true;
											}
											else
											{
												p.PregoeiroPrincipal = false;
												p.Update();
											}
										}
									}

									if(!existePregoeiroProcesso)
									{									
										objPPP = new ProcessoPapelPessoa();
										objPPP.Papel = new Papel(Convert.ToInt32(papel));
										objPPP.Pessoa = listaPregoeiros[0].Pessoa;
										objPPP.Processo = new Processo(Convert.ToInt32(cod));
										objPPP.PregoeiroPrincipal = true;		
										
										int idProcesso = objPPP.Processo.Id;
										resultado += Processo.numSPU(idProcesso)+"; ";
										objPPP.Save();
									}
								}
							}						
						}
						scope.VoteCommit();
					}
					catch
					{
						scope.VoteRollBack();
						resultado = "Ocorreu um erro!";
					}
				}
			}			
			resultado += "<br/> Processos Corrigidos com Sucesso!";
			return resultado;
		}
		
		public SrvFuncoesScript()
		{
		}
	}
}
