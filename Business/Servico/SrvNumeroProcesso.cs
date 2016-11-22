// SrvNumeroProcesso.cs created with MonoDevelop
// User: guilhermefacanha at 11:29 3/4/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections.Generic;
using Npgsql;
using NHibernate;
using NHibernate.Expression;
using Castle.ActiveRecord.Queries;
using Licitar.Business.Dao;
using Licitar.Business.Entidade;

namespace Licitar.Business.Servico
{
	public class SrvNumeroProcesso
	{
		public static bool existeNumeroSpu(string spu)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(NumeroProcesso));
			dc.CreateAlias("TipoNumero","tnu");
			dc.Add(Expression.Or(Expression.Eq("tnu.Descricao", "SPU"),Expression.Eq("tnu.Descricao", "VIPROC")));
			dc.Add(Expression.Eq("numeroProcesso", spu));

			return NumeroProcesso.Exists(dc);
		}

		public static bool existeNumeroViproc(string viproc)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(NumeroProcesso));
			dc.CreateAlias("TipoNumero","tnu");
			dc.Add(Expression.Eq("tnu.Descricao", "VIPROC"));
			dc.Add(Expression.Eq("numeroProcesso", viproc));
			
			return NumeroProcesso.Exists(dc);
		}

		public bool existeSpuProcesso(int idPai, int id)
		{
			DetachedCriteria dcSPU = DetachedCriteria.For(typeof(NumeroProcesso));
			dcSPU.CreateAlias("TipoNumero","tnu");

			dcSPU.Add(Expression.Eq("tnu.Descricao", "SPU"));
			dcSPU.Add(Expression.Eq("Processo.Id", idPai));
			dcSPU.Add(Expression.Not(Expression.Eq("Id", id)));
			
			return NumeroProcesso.Exists(dcSPU);
		}

		public bool existeViprocProcesso(int idPai, int id)
		{
			DetachedCriteria dcVIPROC = DetachedCriteria.For(typeof(NumeroProcesso));
			dcVIPROC.CreateAlias("TipoNumero","tnu");
			
			dcVIPROC.Add(Expression.Eq("tnu.Descricao", "VIPROC"));
			dcVIPROC.Add(Expression.Eq("Processo.Id", idPai));
			dcVIPROC.Add(Expression.Not(Expression.Eq("Id", id)));
			
			Console.WriteLine(NumeroProcesso.Exists(dcVIPROC));
			return NumeroProcesso.Exists(dcVIPROC);
		}

		public bool existeOficio(int idPai, int id, string numero)
		{
			DetachedCriteria dcOficio = DetachedCriteria.For(typeof(NumeroProcesso));
			dcOficio.CreateAlias("TipoNumero","tnu");

			dcOficio.Add(Expression.Eq("tnu.Descricao", "OFÍCIO"));
			dcOficio.Add(Expression.Eq("Processo.Id", idPai));
			dcOficio.Add(Expression.Not(Expression.Eq("Id", id)));
			dcOficio.Add(Expression.Eq("numeroProcesso", numero));
			
			return NumeroProcesso.Exists(dcOficio);
		}

		public TipoNumero[] listarTiposComExcecao()
		{
			DetachedCriteria tp = DetachedCriteria.For(typeof(TipoNumero));
			tp.Add(Expression.Sql("txt_descricao_tnu not in ('AUTO DE INFRAÇÃO','CAIXA','DECRETO','JUSTIÇA','PAD','PORTARIA','PARECER')"));
			tp.AddOrder(Order.Asc("Descricao"));
			return TipoNumero.FindAll(tp);
		}

		public bool existeNumeroLicitacaoProcesso(int idPai, int idTipoLicitacao)
		{
			DetachedCriteria dcLicitacaoExist = DetachedCriteria.For(typeof(NumeroProcesso));
			dcLicitacaoExist.Add(Expression.Eq("TipoNumero.id", idTipoLicitacao));
			dcLicitacaoExist.Add(Expression.Eq("Processo.Id", idPai));
				
			return NumeroProcesso.Exists(dcLicitacaoExist);
		}

		public bool existeNumeroLicitacaoModalidade(int modalidade, int instituicao, int tipolicitacao, string numero)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(NumeroProcesso));
            pesquisa.CreateAlias("Processo","pro");
			pesquisa.CreateAlias("pro.Instituicao","ins");
			pesquisa.CreateAlias("pro.Classificacao","cla");
			pesquisa.CreateAlias("cla.Modalidade","mod");
			pesquisa.Add(Expression.Eq("numeroProcesso", numero));			
			pesquisa.Add(Expression.Eq("mod.Id", modalidade));
			pesquisa.Add(Expression.Eq("ins.Id", instituicao));			
            pesquisa.Add(Expression.Eq("TipoNumero.Id", tipolicitacao));
			return NumeroProcesso.Exists(pesquisa);
		}

		public static bool existeNumeroLicitacao(string numLicitacao, int modalidade, int instituicao)
		{
			DetachedCriteria dcLicitacao = DetachedCriteria.For(typeof(NumeroProcesso));
			dcLicitacao.CreateAlias("TipoNumero","tnu");
			dcLicitacao.CreateAlias("Processo","pro");
			dcLicitacao.CreateAlias("pro.Instituicao","ins");
			dcLicitacao.CreateAlias("pro.Classificacao","cla");
			dcLicitacao.CreateAlias("cla.Modalidade","mod");			dcLicitacao.Add(Expression.Eq("numeroProcesso", numLicitacao));								
			dcLicitacao.Add(Expression.Eq("mod.Id", modalidade));
			dcLicitacao.Add(Expression.Eq("ins.Id", instituicao));
			dcLicitacao.Add(Expression.InsensitiveLike("tnu.Descricao", "LICITAÇÃO"));
			
			return NumeroProcesso.Exists(dcLicitacao);
		}
		
		public NumeroProcesso[] listarNumerosProcesso(int _idProcesso)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(NumeroProcesso));
				pesquisa.Add(Expression.Eq("Processo.Id", _idProcesso));
				
            Order[] ordem = new Order[]
			{
				Order.Asc("DataCadastro")
			};
			
			return NumeroProcesso.FindAll(pesquisa, ordem);
		}
		
		
		public NumeroProcesso listarNumeroLoteItem(int _idProcesso)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(NumeroProcesso));
			pesquisa.Add(Expression.Eq("Processo.Id", _idProcesso));
			pesquisa.CreateAlias("TipoNumero","tnu");
			pesquisa.Add(Expression.Eq("tnu.Id",4));
						        
			
			return NumeroProcesso.FindOne(pesquisa);
		}
		
		public SrvNumeroProcesso()
		{
		}
	}
}
