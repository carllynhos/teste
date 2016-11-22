// SrvPessoa.cs created with MonoDevelop
// User: guilhermefacanha at 11:15 3/4/2009
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

using Licitar.Business.Dto;
using Licitar.Business.Persistencia;
using Castle.ActiveRecord;
using Licitar.Business.Utilidade;
using System.Text;

namespace Licitar.Business.Servico
{
	
	
	public class SrvPessoa
	{

		public Pessoa[] listarAnalistas()
		{
			DetachedCriteria dcAnalista = DetachedCriteria.For(typeof(Pessoa));
			dcAnalista.CreateAlias("UnidadeExercicioFuncao","unidfun").CreateAlias("unidfun.Funcao","fun");
			string[] func = new string[3];
			func[0]= "PRESIDENTE";
			func[1] = "VICE-PRESIDENTE";
			func[2] = "ANALISTA DE PROCESSO";
			dcAnalista.Add(Expression.In("fun.Descricao",func));

			return Pessoa.FindAll(dcAnalista);
		}

		public Pessoa[] listarVencedor()
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Pessoa));
			pesquisa.Add(Expression.Eq("Licitante",true));
			pesquisa.Add(Expression.Sql(" this_.fk_cod_pessoa_alterada_pes is null "));
			return Pessoa.FindAll(pesquisa, new Order("Nome", true));
		}

		public Pessoa[] listarPessoas()
		{
			return Pessoa.FindAll(new Order("Nome", true));
		}
		
		public Pessoa listarPessoaPorNome(string nomePes)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Pessoa));
				dc.Add(Expression.Like("Nome", nomePes.ToUpper()));
				
				dc.Add(Expression.Sql(" this_.fk_cod_pessoa_alterada_pes is null "));
			
			return Pessoa.FindFirst(dc);
		}

		public Pessoa getPessoaPorCpf(string cpf)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Pessoa));
			dc.Add(Expression.Eq("CpfCnpj",cpf));
			dc.Add(Expression.Eq("PessoaFisica",true));
			dc.Add(Expression.Sql(" this_.fk_cod_pessoa_alterada_pes is null "));


			return Pessoa.FindFirst(dc);
		}

		public Pessoa[] listarLicitante()
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Pessoa));
			pesquisa.Add(Expression.Eq("Licitante",true));
			pesquisa.Add(Expression.Sql(" this_.fk_cod_pessoa_alterada_pes is null "));

			return Pessoa.FindAll(pesquisa, new Order("Nome", true));
		}

		public Pessoa[] listarPessoasNaoFisicas()
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Pessoa));			
            pesquisa.Add(Expression.Eq("PessoaFisica",false));
			pesquisa.Add(Expression.Sql(" this_.fk_cod_pessoa_alterada_pes is null "));

			return Pessoa.FindAll(pesquisa, new Order("Nome", true));
		}

		public Pessoa[] listarPessoasComStringFiltro(string pesq)
		{
			DetachedCriteria pesquisa = DetachedCriteria.For(typeof(Pessoa));
				
			if (pesq != string.Empty)
				pesquisa.Add(Expression.InsensitiveLike("Nome","%" + pesq + "%"));
			
			pesquisa.Add(Expression.Sql(" this_.fk_cod_pessoa_alterada_pes is null "));

				
			Order[] ordem = new Order[]
			{
				Order.Asc("Nome")
			};

			return Pessoa.FindAll(pesquisa, ordem);
		}


		public int CriptografarSenhas()
		{
			Criptografia cript = new Criptografia();
			int registros = 0;
			using(TransactionScope scopo = new TransactionScope())
			{
				try
				{
					Pessoa[] pessoas = Pessoa.FindAll();
					
					foreach ( Pessoa pes in pessoas ) {
						if(!string.IsNullOrEmpty(pes.Senha))
						{
							pes.Senha = cript.criptografarSenhaSHA1(pes.Senha);
							pes.Save();
						}
			
					}
					registros = pessoas.Length;
					scopo.VoteCommit();
				}
				catch (Exception ex)
				{
					scopo.VoteRollBack();
				}
			}

			return registros;			
		}

		public string GerarSenhas()
		{		
			string caracteres = "abcdefghijmnpqrstuvxz123456789@#%&";
		
			int totalCaracteres = caracteres.Length;		
			Random randon = new Random(DateTime.Now.Millisecond);		
			StringBuilder novasenha = new StringBuilder(8);
		
			for (int i = 0; i < 8; i++)			
				novasenha.Append(caracteres[randon.Next(0, totalCaracteres)]);			
			return Convert.ToString(novasenha);			
		}

		public Area[] ListarAreas(string instValor)
		{
			DetachedCriteria pesqArea = DetachedCriteria.For(typeof(Area));
				pesqArea.Add(Expression.Eq("Instituicao.Id", int.Parse(instValor)));
				pesqArea.AddOrder(Order.Asc("Descricao"));				
			return  Area.FindAll(pesqArea);
		}

		public UnidadeExercicioFuncao[] ListarUnidadesExercicioFuncao(string unidadeExercicio)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(UnidadeExercicioFuncao));			
			dc.Add(Expression.Eq("UnidadeExercicio.Id", int.Parse(unidadeExercicio)));
			dc.CreateAlias("Funcao", "fun");
			dc.AddOrder(Order.Asc("fun.Descricao"));
			dc.AddOrder(Order.Asc("Id"));
			
			UnidadeExercicioFuncao[] oUnidadeExercicioFuncao = UnidadeExercicioFuncao.FindAll(dc);
			return oUnidadeExercicioFuncao;
		}

		public string MontaCorpoEmailSenha(string nome, string cpf,string senha, string email )
		{
				
						string corpo = 
							@"<table>
								<tr>
									<td align='right'>Usuário: </td>
									<td align='left'>@nome</td>
								</tr>
								<tr>
									<td align='right'>CPF: </td>
									<td align='left'>@cpf</td>
								</tr>
								<tr>
									<td align='right'>Senha: </td>
									<td align='left'>@senha</td>
								</tr>
							</table>";
	
						corpo = corpo.Replace("@nome",nome);
						corpo = corpo.Replace("@cpf",cpf);
						corpo = corpo.Replace("@senha",senha);			
			return corpo;								
		}

		
		public SrvPessoa()
		{
		}
	}
}
