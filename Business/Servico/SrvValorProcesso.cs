// srvValorProcesso.cs created with MonoDevelop
// User: diogolima at 18:19 14/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using Licitar.Business.Entidade;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Servico
{
	public class SrvValorProcesso
	{
		public bool existeProcesso(string tipoValor, string fonteValor, string idPai)
		{
			DetachedCriteria pesqValor = DetachedCriteria.For(typeof(ValorProcesso));
			pesqValor.CreateAlias("TipoValor","tva");
			pesqValor.Add(Expression.Eq("tva.Id",int.Parse(tipoValor)))
				.Add(Expression.Eq("FonteValor.Id",int.Parse(fonteValor)))
					.Add(Expression.Eq("Processo.Id",int.Parse(idPai)));

			return ValorProcesso.Exists(pesqValor);			
		}

		public bool existeProcesso(string fonteValor, string idPai)
		{
			DetachedCriteria pesqValor = DetachedCriteria.For(typeof(ValorProcesso));
			pesqValor.CreateAlias("TipoValor","tva");
			pesqValor.Add(Expression.Eq("tva.Id",int.Parse(fonteValor))).Add(Expression.Eq("Processo.Id",int.Parse(idPai)));

			return ValorProcesso.Exists(pesqValor);
			
		}

		public bool existeValorProcessoPessoa(int idPessoa)
		{
			DetachedCriteria pesqVl = DetachedCriteria.For(typeof(ValorProcesso));
                        pesqVl.Add(Expression.Eq("Pessoa.Id",idPessoa));
			return ValorProcesso.Exists(pesqVl);
		}
		
		public ValorProcesso[] getValorSerContratado(int idPai)
		{
			DetachedCriteria pesqVlAserContratado = DetachedCriteria.For(typeof(ValorProcesso),"vl");
			pesqVlAserContratado.CreateAlias("TipoValor","tva").Add(Expression.Eq("tva.Descricao","A SER CONTRATADO")).Add(Expression.Eq("Processo.Id",idPai));
			return ValorProcesso.FindAll(pesqVlAserContratado);
		}
		
		public static bool existeValorProcesso(int processo)
		{		
			DetachedCriteria pesqValor = DetachedCriteria.For(typeof(ValorProcesso));
            pesqValor.Add(Expression.Eq("Processo.Id", processo));
            pesqValor.CreateAlias("TipoValor", "tv")
                .Add(Expression.Or(Expression.Eq("tv.Descricao", "ITEM A SER CONTRATADO"), Expression.Eq("tv.Descricao", "LOTE A SER CONTRATADO")));
            if(ValorProcesso.Exists(pesqValor))
			{
				return true;	
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Lista os valores do processo.
		/// </summary>
		/// <param name="idPai">
		/// Processo Licitatório
		/// </param>
		/// <returns>
		/// Um vetor de informações de valores
		/// </returns>
		public static itemValorProcesso[] listarValoresProcesso(int idPai)
		{
			DetachedCriteria dc = DetachedCriteria.For<ValorProcesso>();
			dc.Add(Expression.Eq("Processo.Id", idPai));
			
			Processo oProcesso = new Processo(idPai);
				
			ValorProcesso[] oValores =  oProcesso.ListarValores();
			
			ArrayList itensValor = new ArrayList();
			
			for (int i=0; i<oValores.Length;i++)
			{
				itemValorProcesso itemValor = new itemValorProcesso();

				itemValor.Cadastro = oValores[i].DataCadastro;
				itemValor.FonteValor = oValores[i].FonteValor.Descricao;
				itemValor.Id = oValores[i].Id;
				itemValor.InstAreaTipoProcessoTipoValor = oValores[i].TipoValor.Descricao;
				itemValor.MoedaValor = oValores[i].Moeda.Descricao;

				itemValor.Processo = ""; 

				itemValor.Processo = Processo.numSPU(oValores[i].Processo.Id);

				itemValor.Valor = (decimal)oValores[i].Valor;
				itemValor.DescricaoValor = oValores[i].Descricao;
				if (oValores[i].TipoValor != null)
				{
					if (oValores[i].TipoValor.Descricao == "LOTE A SER CONTRATADO" || oValores[i].TipoValor.Descricao == "ITEM A SER CONTRATADO")
					{
						if(oValores[i].Pessoa != null)
						{
							itemValor.DescricaoValor = oValores[i].Descricao.ToUpper();													
							itemValor.PessoaProcesso = oValores[i].Pessoa.Nome.ToUpper();
							itemValor.CNPJ = oValores[i].Pessoa.CpfCnpj;							
						}
					}					
				}
				
				itensValor.Add(itemValor);
			}
		
			return (itemValorProcesso[])itensValor.ToArray(typeof(itemValorProcesso));
		}		
		
		/// <summary>
		/// Confirma se há no processo atual um item valor do tipo 'A Ser Contratado'
		/// </summary>
		/// <param name="idPai">
		/// Processo Licitatório
		/// </param>
		/// <param name="idTipoValorProcesso">
		/// Id do TipoProcessoTipoValor buscado
		/// </param>
		/// <returns>
		/// Retorna booleano
		/// </returns>
		public static bool confirmaItens(int idPai, string TipoValorProcesso)
		{
			bool confirma = false;
			using(new Castle.ActiveRecord.SessionScope())
			{
				DetachedCriteria dc = DetachedCriteria.For<ValorProcesso>();
				dc.Add(Expression.Eq("Processo.Id", idPai));
				
				Processo oProcesso = new Processo(idPai);
								
				ValorProcesso[] oValores =  oProcesso.ListarValores();
								
				for (int i=0; i<oValores.Length;i++)
				{
					if (oValores[i].TipoValor.Descricao == TipoValorProcesso) confirma = true;
				}
			}
			return confirma;
		}		
		
		/// <summary>
		/// Confirma se o processo está todo deserto, fracassado ou anulado
		/// </summary>
		/// <param name="idPai">
		/// O Id Pai do processo licitatório atual
		/// </param>
		/// <param name="idPai">
		/// O Id do TipoProcessoTipoValor buscado
		/// </param>
		/// <returns>
		/// Retorna booleano
		/// </returns>
		public static bool? confirmaCancelamento(int idPai)
		{
			bool? confirma = null;
			
			using(new Castle.ActiveRecord.SessionScope())
			{
				DetachedCriteria dc = DetachedCriteria.For<ValorProcesso>();
				dc.Add(Expression.Eq("Processo.Id", idPai));
				
				Processo oProcesso = new Processo(idPai);
							
				ValorProcesso[] oValores =  oProcesso.ListarValores();
				
				for (int i=0; i<oValores.Length;i++)
				{
					Console.WriteLine("TIPO VALOR: "+oValores[i].TipoValor.Descricao.ToUpper());
					if (oValores[i].TipoValor.Descricao.ToUpper() == "DESERTO" ||
					    oValores[i].TipoValor.Descricao.ToUpper() == "FRACASSADO" ||
					    oValores[i].TipoValor.Descricao.ToUpper() == "ANULADO" ||
					    oValores[i].TipoValor.Descricao.ToUpper() == "CANCELADO" ||
					    oValores[i].TipoValor.Descricao.ToUpper() == "REVOGADO") confirma = true;
				}
				
				for (int i=0; i<oValores.Length;i++)
				{
					if (oValores[i].TipoValor.Descricao.ToUpper() == "A SER CONTRATADO") confirma = false;
				}
			}
			return confirma;
		}

		public ValorProcesso[] getLotesItensProcesso(string idProcesso)
		{
			int id = 0;
			if(int.TryParse(idProcesso,out id))
			{
				DetachedCriteria dc = DetachedCriteria.For(typeof(ValorProcesso));
				dc.CreateAlias("TipoValor","vpr");
				dc.Add(Expression.Eq("Processo.Id",id));
				dc.Add(Expression.Or(Expression.InsensitiveLike("vpr.Descricao","%LOTE%"),Expression.InsensitiveLike("vpr.Descricao","%ITEM%")));
				return ValorProcesso.FindAll(dc);
			}
			else
			{
				return null;
			}
		}

		public ValorProcesso RetornaValorTipoDesejado(string tipo, Processo oProcesso)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(ValorProcesso));
						dc.CreateAlias("TipoValor","tnu");
						dc.Add(Expression.Eq("Processo",oProcesso));
						dc.Add(Expression.InsensitiveLike("tnu.Descricao",tipo));
			return ValorProcesso.FindFirst(dc);
		}
	}
}
namespace Licitar.Business.Servico
{
	public class itemValorProcesso
	{
		#region Construtores

		public itemValorProcesso()
		{
		}
		#endregion

		#region Propriedades

		/// <value>
		/// Id do Processo Licitatório
		/// </value>
		public virtual int Id
		{
			get; set;
		}

		/// <value>
		/// Tipo do Valor
		/// </value>
		public virtual string InstAreaTipoProcessoTipoValor
		{
			get; set;
		}

		/// <value>
		/// Descrição do Valor
		/// </value>
		public virtual string DescricaoValor
		{
			get; set;
		}

		/// <value>
		/// Fonte do Valor
		/// </value>
		public virtual string FonteValor
		{
			get; set;
		}

		/// <value>
		/// Moeda
		/// </value>
		public virtual string MoedaValor
		{
			get; set;
		}

		/// <value>
		/// Valor praticado
		/// </value>
		public virtual decimal Valor
		{
			get; set;
		}

		/// <value>
		/// Data do Cadastro
		/// </value>
		public virtual DateTime Cadastro
		{
			get; set;
		}

		/// <value>
		/// Descrição do Processo
		/// </value>
		public virtual string Processo
		{
			get; set;
		}

		/// <value>
		/// Nome do Vencedor do Lote/Item
		/// </value>
		public virtual string PessoaProcesso
		{
			get; set;
		}

		/// <value>
		/// CNPJ do vencedor do Lote/Item
		/// </value>
		public virtual string CNPJ
		{
			get; set;
		}

		#endregion
	}
}