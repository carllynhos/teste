
using System;
using NHibernate.Expression;
using Licitar.Business.Persistencia;
using Licitar.Business.Entidade;

namespace Licitar.Business.Servico
{
	
	
	public class SrvProcessoCompleto
	{
		
		public string SalvarProcessoWebService(string numeroPortalDigital, string numeroSpu, string numeroLicitacao, string numeroComprasNet, 
		                   string numeroSisBb,
		                   string strInstituicao, string strResumoObjeto, string strModalidade, string strTipoLicitacao, string strNatureza, 
		                   double? vlrEstimadoGlobal, double? vlrNaoContratado, double? vlrASerContratado,double? vlrFracassado, 
		                   double? vlrDeserto, double? vlrAnulado, double? vlrRevogado, double? vlrCancelado, double? vlrEstimadoReal, string numeroMapp, string moeda,
		                   string fonteValor, string strUsuarioCadastrante, string strAnalista, DateTime? dataEntradaPge, DateTime? dataConclusao,
		                   DateTime? dataRealizacao, DateTime? dataAberturaPropostas, DateTime? dataAcolhimentoPropostas,
		                   string strVencedor, string strEstado, string strPresidentePregoeiro, string strPapel,
		                                       string strSituacaoProcesso, bool? processoAuditado, string strPublicacaoEdital )
		{
			
			try {
			
			
			DetachedCriteria pesqProcessoCompleto =  DetachedCriteria.For(typeof(ProcessoCompleto));
				pesqProcessoCompleto.Add(Expression.Sql(" txt_numero_portaldigital_npr = '"+numeroPortalDigital+"'"));	
	
				
			string retorno = string.Empty;
				
			ProcessoCompleto procComp = ProcessoCompleto.FindFirst(pesqProcessoCompleto);
    		if(procComp == null)
				procComp = new ProcessoCompleto();
	
			if(!string.IsNullOrEmpty(numeroPortalDigital))
				procComp.NumeroDigital = numeroPortalDigital;
			if(!string.IsNullOrEmpty(numeroSpu))
				procComp.NumeroSpu = numeroSpu;
			if(!string.IsNullOrEmpty(numeroLicitacao))
				procComp.NumeroLicitacao = numeroLicitacao;
			if(!string.IsNullOrEmpty(numeroComprasNet))
				procComp.NumeroComprasNet = numeroComprasNet;
			if(!string.IsNullOrEmpty(numeroSisBb))
				procComp.NumeroSisBB = numeroSisBb;
			if(!string.IsNullOrEmpty(numeroMapp))
				procComp.NumeroMapp = numeroMapp;
				
			//Preparação das consultas no activeRecord:					

				
			DetachedCriteria pesqUnidadeAdministrativa = DetachedCriteria.For(typeof(UnidadeAdministrativa));
				pesqUnidadeAdministrativa.Add(Expression.Eq("Cnpj",strInstituicao));
			
			DetachedCriteria pesqTipoLicitacao = DetachedCriteria.For(typeof(TipoLicitacao));
				pesqTipoLicitacao.Add(Expression.Sql(" upper(txt_descricao_tli) = '"+strTipoLicitacao.ToUpper()+"'"));

			DetachedCriteria pesqNatureza = DetachedCriteria.For(typeof(Natureza));
				pesqNatureza.Add(Expression.Sql(" upper(txt_descricao_nat) = '"+strNatureza.ToUpper()+"'"));
				
			DetachedCriteria pesqModalidade = DetachedCriteria.For(typeof(Modalidade));
				pesqModalidade.Add(Expression.Sql(" upper(txt_descricao_mod) = '"+ strModalidade.ToUpper()+"'"));
				
			DetachedCriteria pesqPessoa = DetachedCriteria.For(typeof(Pessoa));
				pesqPessoa.Add(Expression.Eq("CpfCnpj",strPresidentePregoeiro));
				

			//Criação dos Objetos:
			//Instituicao oInstituicao;
			Modalidade oModalidade;
			Natureza oNatureza;
			UnidadeExercicio oUnidadeExercicio;
			TipoLicitacao oTipoLicitacao;
			Pessoa PregPresidente;
			UnidadeAdministrativa oUnidadeAdministrativa;
				
			
//			//Verificação das consultas.

			
			if(!string.IsNullOrEmpty(strInstituicao))
			{
				if( !UnidadeAdministrativa.Exists(pesqUnidadeAdministrativa))
					retorno = " Esta instituicao não existe ";
				else
				{
					oUnidadeAdministrativa = UnidadeAdministrativa.FindFirst(Expression.Eq("Cnpj",strInstituicao));
					procComp.Instituicao = oUnidadeAdministrativa.Instituicao;
					procComp.StrInstituicao = oUnidadeAdministrativa.Instituicao.Sigla.ToUpper();
					procComp.CodUnidadeAdministrativa = oUnidadeAdministrativa.Id;
				}
			}
			
				
			if(!string.IsNullOrEmpty(strModalidade))
			{		
				if(!Modalidade.Exists(pesqModalidade))
					retorno += " Esta Modalidade não existe ";
				else
				{
					oModalidade = Modalidade.FindOne(Expression.Sql(" upper(txt_descricao_mod) = '"+ strModalidade.ToUpper()+"'"));
					procComp.Modalidade = oModalidade;
					procComp.StrModalidade = oModalidade.Descricao.ToUpper();
				}
			}
				
			if(!string.IsNullOrEmpty(strNatureza))
			{				
				if(!Natureza.Exists(pesqNatureza))
					retorno += " Esta Natureza não existe ";
				else
				{
					oNatureza = Natureza.FindOne(Expression.Sql(" upper(txt_descricao_nat) = '"+strNatureza.ToUpper()+"'"));	
					procComp.StrNatureza = oNatureza.Descricao.ToUpper();
					procComp.CodNatureza = oNatureza.Id;
				}
			}
			
			
			if(!string.IsNullOrEmpty(strTipoLicitacao))	
			{
				if(!TipoLicitacao.Exists(pesqTipoLicitacao))
					retorno += " Este Tipo Licitação não existe ";
				else
				{
					oTipoLicitacao = TipoLicitacao.FindOne(Expression.Sql(" upper(txt_descricao_tli) = '"+strTipoLicitacao.ToUpper()+"'"));	
					procComp.StrTipoLicitacao = oTipoLicitacao.Descricao.ToUpper();
					procComp.CodTipoLicitacao = oTipoLicitacao.Id;
				}
			}
				
			Console.WriteLine("preg = "+strPresidentePregoeiro);
			
			if(!string.IsNullOrEmpty(strPresidentePregoeiro))
			{
				if(!Pessoa.Exists(pesqPessoa))
					retorno += " Este Pregoeiro/Presidente não existe ";
				else
				{
					PregPresidente = Pessoa.FindFirst(Expression.Eq("CpfCnpj",strPresidentePregoeiro));
					procComp.CodPresidentePregoeiro = PregPresidente.Id;
					procComp.StrPresidentePregoeiro = PregPresidente.Nome.ToUpper();
				}	
			}
		
			if(!string.IsNullOrEmpty(strResumoObjeto))				
				procComp.ResumoObjeto = strResumoObjeto.ToUpper();
			
				
			if(!string.IsNullOrEmpty(strPapel))
				procComp.StrPapel = strPapel.ToUpper();
			if(!string.IsNullOrEmpty(strUsuarioCadastrante))
				procComp.CadastranteProcesso = strUsuarioCadastrante;
			if(!string.IsNullOrEmpty(strPublicacaoEdital))
				procComp.StrPublicacaoEdital = strPublicacaoEdital;
					
			
			//Valores:	
			if(vlrEstimadoGlobal != null)
				procComp.ValorEstimadoGlobal = vlrEstimadoGlobal;
			if(vlrASerContratado != null)
				procComp.ValorASerContratado = vlrASerContratado; 
			if(vlrAnulado != null)
				procComp.ValorAnulado = vlrAnulado; 
			if(vlrCancelado != null)
				procComp.ValorCancelado = vlrCancelado; 
			if(vlrDeserto != null)
				procComp.ValorDeserto = vlrDeserto; 
			if(vlrEstimadoReal != null)				
				procComp.ValorEstimadoReal = vlrEstimadoReal;
			if(vlrFracassado != null)							
				procComp.ValorFracassado = vlrFracassado;	
			if(vlrRevogado != null)											
				procComp.ValorRevogado = vlrRevogado;
			if(vlrNaoContratado != null)											
				procComp.ValorNaoContratado = vlrNaoContratado;
			if(!string.IsNullOrEmpty(strSituacaoProcesso))														
				procComp.SituacaoProcesso = strSituacaoProcesso.ToUpper();
			if(!string.IsNullOrEmpty(strEstado))			
				procComp.EstadoProcesso = strEstado;
			if(!string.IsNullOrEmpty(strVencedor))			
				procComp.StrVencedor = strVencedor;
			
				

			//Datas:
			if(dataEntradaPge != null)
			{
				procComp.DataEntradaPGE = dataEntradaPge;
				procComp.DataCadEntradaPGE = dataEntradaPge;
				procComp.DataAndEntradaPGE = dataEntradaPge;	
			}
			if(dataConclusao != null)
			{
				procComp.DataConclusao = dataConclusao;
				procComp.DataAndConclusao = dataConclusao;
				procComp.DataCadConclusao = dataConclusao;
			}
			
			if(dataRealizacao != null)
			{
				procComp.DataRealizacao = dataRealizacao;
				procComp.DataAndRealizacao = dataRealizacao;
				procComp.DataCadRealizacao = dataRealizacao;
				procComp.DataAndMarcacao = dataRealizacao;
				procComp.DataCadMarcacao = dataRealizacao;
			}
				
			if(dataAberturaPropostas != null)
			{
				procComp.DataAberturaPropostas = dataAberturaPropostas;
				procComp.DataAndAberturaPropostas = dataAberturaPropostas;
				procComp.DataCadAberturaPropostas = dataAberturaPropostas;
			}
				
			if(dataAcolhimentoPropostas != null)
			{
				procComp.DataAcolhimentoPropostas = dataAcolhimentoPropostas;
				procComp.DataAndAcolhimentoPropostas = dataAcolhimentoPropostas;
				procComp.DataCadAcolhimentoPropostas = dataAcolhimentoPropostas;
			}
			
			if(processoAuditado != null)
			{
				procComp.ProcessoAuditado = processoAuditado;
				procComp.ProcessoAuditando = processoAuditado;
			}
			
			procComp.ProcessoDigital = true;
			if(retorno == string.Empty)
			{
				Console.WriteLine("");
				procComp.Save();	
				retorno = "OPERAÇÃO REALIZADA COM SUCESSO!"+procComp.Id;		
			}	
			else
				throw new Exception(retorno);
				
			return retorno;
			} catch (Exception ex) {
				throw new Exception(ex.Message+" "+ex.StackTrace);
			}
	
		}
		
	}
}
