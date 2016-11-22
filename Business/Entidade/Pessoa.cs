//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Pessoa.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 08/11/2008
// DESCRICAO: 
// ALTERADO POR: 
// DATA DA ALTERACAO: 
// MOTIVO DA ALTERACAO:
// OBSERVACOES:
//----------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Expression;
using Licitar.Business.Servico;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_pessoa_pes", Schema="adm_licitar")]
	public class Pessoa : ActiveRecordBase<Pessoa>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_pessoa_pes", SequenceName="adm_licitar.sq_pessoa_pes")]
		public virtual int Id
		{
			get;
			set;
		}	
		
		[Property("boo_pessoa_fisica_pes")]
		public virtual bool PessoaFisica
		{
			get;
			set;
		}
		
		[Property("boo_contato_pes")]
		public virtual bool PessoaContato
		{
			get;
			set;
		}
		
		[Property("txt_cpf_cnpj_pes")]
		public virtual string CpfCnpj
		{
			get;
			set;
		}
		
		[Property("txt_senha_pes")]
		public virtual string Senha
		{
			get;
			set;
		}	
		
		[Property("txt_matricula_pes")]
		public virtual string Matricula
		{
			get;
			set;
		}
		
		[Property("txt_nome_pes")]
		public virtual string Nome
		{
			get;
			set;
		}
		
		[Property("txt_email_pes")]
		public virtual string Email
		{
			get;
			set;
		}
		
		[Property("txt_endereco_pes")]
		public virtual string Endereco
		{
			get;
			set;
		}
		
		[Property("txt_bairro_pes")]
		public virtual string Bairro
		{
			get;
			set;
		}
		
		[Property("txt_cidade_pes")]
		public virtual string Cidade
		{
			get;
			set;
		}
		
		[Property("txt_pais_pes")]
		public virtual string Pais
		{
			get;
			set;
		}
		
		[Property("txt_uf_pes")]
		public virtual string UF
		{
			get;
			set;
		}
		
		[Property("txt_cep_pes")]
		public virtual string Cep
		{
			get;
			set;
		}
		
		[Property("txt_fax_pes")]
		public virtual string Fax
		{
			get;
			set;
		}
		
		[Property("txt_telefone_pes")]
		public virtual string Telefone
		{
			get;
			set;
		}	
		
		[Property("dat_ultimo_login_pes")]
		public virtual DateTime DataHoraUltimoLogin
		{
			get;
			set;
		}
		
		[Property("dat_nascimento_pes")]
		public virtual DateTime DataNascimento
		{
			get;
			set;
		}
		
		[Property("boo_licitante_pes")]
		public virtual bool Licitante			
		{
			get;
			set;
		}
		
		[Property("boo_servidor_pes")]
		public virtual bool Servidor			
		{
			get;
			set;
		}
		
		[Property("boo_ativo_pes")]
		public virtual bool Ativo			
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_unidade_exercicio_funcao_uef")]
		public virtual UnidadeExercicioFuncao UnidadeExercicioFuncao
		{			
			get;
			set;
		}

		[BelongsTo("fk_cod_sub_unidade_exercicio_funcao_uef")]
		public virtual UnidadeExercicioFuncao SubUnidadeExercicioFuncao
		{			
			get;
			set;
		}
		
		[BelongsTo("fk_cod_pessoa_alterada_pes")]
		public virtual Pessoa PessoaAlterada
		{			
			get;
			set;
		}
		
		/// <value>
		/// Se a pessoa esta logada com usuario anonimo.
		/// </value>
		public virtual bool Anonimo
		{
			get;
			set;
		}
		
		/// <value>
		/// Modulo em que a pessoa esta logada.
		/// </value>
		public virtual Modulo Modulo
		{
			get;
			set;
		}
		
		public Pessoa()
		{
		}
		
		public Pessoa(int id)
		{			
			this.Id = id;
		}

        /// <summary>
        /// Lista os processos da pessoa.
        /// </summary>
        /// <returns></returns>
        public virtual ProcessoPapelPessoa[] ListarProcessos()
        {
            return ProcessoPapelPessoa.FindAll(Expression.Eq("Pessoa.Id", this.Id));
        }
		
		/// <summary>
		/// Lista os contatos de uma pessoa juridica.
        /// OBS: Nao pode ser chamado por uma pessoa fisica.
		/// </summary>
		/// <returns></returns>
		public virtual Contato[] ListarContatosDaPessoaJuridica()
		{
			if (!Pessoa.Find(this.Id).PessoaFisica)
				return Contato.FindAll(Expression.Eq("PessoaJuridica.Id", this.Id));
			
			else
				throw new Exception("ATENCAO: O metodo ListarContatosDaPessoaJuridica() nao pode ser chamado por uma PessoaFisica");
		}
		
        /// <summary>
        /// Lista as atividades que a pessoa nao tem acesso.
        /// </summary>
        /// <returns></returns>
		public virtual AtividadePessoaPermissao[] ListarAtividadesComAcessoRestrito()
		{			
            return AtividadePessoaPermissao.FindAll(Expression.Eq("Pessoa.Id", this.Id), Expression.Eq("AcessoPermitido", false));
		}

        /// <summary>
        /// Lista as atividades que a pessoa tem acesso.
        /// </summary>
        /// <returns></returns>
        public virtual AtividadePessoaPermissao[] ListarAtividadesComAcessoPermitido()
        {
            return AtividadePessoaPermissao.FindAll(Expression.Eq("Pessoa.Id", this.Id), Expression.Eq("AcessoPermitido", true));            
        }

        /// <summary>
        /// Lista todas as atividades que a pessoa tem acesso. 
        /// AtividadesDaFuncao + AtividadesExtras - AtividadesBloqueadas.
        /// </summary>
        /// <returns></returns>
        public virtual List<int> ListarTodasAtividades()
        {            
            List<int> listaAtividadesUsuario = new List<int>();
            
            AtividadeUnidadeExercicioPermissao[] arrayAtividadesSubUnidade = null;

			SrvUnidadeExercicioFuncaoPessoa objSrvUEFP = new SrvUnidadeExercicioFuncaoPessoa();
			SrvAtividade objSrvAtividade = new SrvAtividade();

			List<int> listaUnidadeExercicioPessoa = objSrvUEFP.getUnidadesPessoa(this.Id);
			List<int> listaAtividadesPadrao = objSrvAtividade.listarAtividadesPadrao();
			
            if (listaUnidadeExercicioPessoa!=null && listaUnidadeExercicioPessoa.Count>0)
			{
				SrvAtividadeUnidadeExercicioPermissao objSrvAUEP = new SrvAtividadeUnidadeExercicioPermissao();
                arrayAtividadesSubUnidade = objSrvAUEP.listarAtividadesSubUnidades(listaUnidadeExercicioPessoa);
			}
            
            AtividadePessoaPermissao[] arrayAtividadesAcessoLiberado = this.ListarAtividadesComAcessoPermitido();
            AtividadePessoaPermissao[] arrayAtividadesAcessoRestrito = this.ListarAtividadesComAcessoRestrito();
			
			foreach(int i in listaAtividadesPadrao)
			{
				listaAtividadesUsuario.Add(i);				
			}
			
            foreach (AtividadeUnidadeExercicioPermissao oAtividadeFuncao in arrayAtividadesSubUnidade)
			{
				if(!listaAtividadesUsuario.Contains(oAtividadeFuncao.Atividade.Id))
				{
					listaAtividadesUsuario.Add(oAtividadeFuncao.Atividade.Id);
				}
			}
			
			foreach (AtividadePessoaPermissao oAtividadePessoa in arrayAtividadesAcessoLiberado)
            {
                if (oAtividadePessoa.Atividade != null && !listaAtividadesUsuario.Contains(oAtividadePessoa.Atividade.Id))
				{
                    listaAtividadesUsuario.Add(oAtividadePessoa.Atividade.Id);
				}
            }
			
            foreach (AtividadePessoaPermissao oAtividadePessoa in arrayAtividadesAcessoRestrito)
            {
                if (listaAtividadesUsuario.Contains(oAtividadePessoa.Atividade.Id))
				{
					listaAtividadesUsuario.Remove(oAtividadePessoa.Atividade.Id);
				}            
			}

			return listaAtividadesUsuario;
        }
	
		public virtual AtividadePessoaPermissao[] ListarAtividadesComAcessoRestritoMenu(int idModulo)
		{			
            DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dc.Add(Expression.Eq("Pessoa.Id", this.Id));
			dc.Add(Expression.Eq("AcessoPermitido", false));
			dc.CreateAlias("Atividade", "ativ");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.CreateAlias("ativ.Modulo", "mod");
			dc.Add(Expression.Eq("mod.Id", idModulo));
            return AtividadePessoaPermissao.FindAll(dc);
		}
        
        public virtual AtividadePessoaPermissao[] ListarAtividadesComAcessoPermitidoMenu(int idModulo)
        {
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dc.Add(Expression.Eq("Pessoa.Id", this.Id));
			dc.Add(Expression.Eq("AcessoPermitido", true));
			dc.CreateAlias("Atividade", "ativ");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.CreateAlias("ativ.Modulo", "mod");
			dc.Add(Expression.Eq("mod.Id", idModulo));			
            return AtividadePessoaPermissao.FindAll(dc);
        }
		
		public override string ToString()
		{
			return this.Nome;
			
		}
       
        public virtual Atividade[] ListarTodasAtividadesMenu(int idModulo)
        {          
			SrvAtividade  objSrvAtividade = new SrvAtividade();
			SrvUnidadeExercicioFuncaoPessoa objSrvUEFP = new SrvUnidadeExercicioFuncaoPessoa();
			SrvAtividadeUnidadeExercicioPermissao objSrvAtiUniExerPer = new SrvAtividadeUnidadeExercicioPermissao();
			List<Atividade> listaTodasAtividades = objSrvAtividade.listarObjsAtividadesPadraoModulo(idModulo);
			
			List<string> listaUnidadesPessoa = objSrvUEFP.listarSubUnidadesPessoa(this.Id);
			            
			AtividadeUnidadeExercicioPermissao[] arrayAtividadesFuncao = objSrvAtiUniExerPer.ListarAtividadesMenu(idModulo,listaUnidadesPessoa);

            AtividadePessoaPermissao[] arrayAtividadesAcessoLiberado = this.ListarAtividadesComAcessoPermitidoMenu(idModulo);
            AtividadePessoaPermissao[] arrayAtividadesAcessoRestrito = this.ListarAtividadesComAcessoRestritoMenu(idModulo); 
							
			// adiciona as atividades da funcao da pessoa:
            foreach (AtividadeUnidadeExercicioPermissao oAtividadeFuncao in arrayAtividadesFuncao)
			{
                bool contem = false;				
				for (int i = 0; i < listaTodasAtividades.Count; i++)
				{
					if (oAtividadeFuncao.Atividade.Descricao == listaTodasAtividades[i].Descricao)
					{
						contem = true;
						break;
					}
				}
				
				if (!contem)
					listaTodasAtividades.Add(oAtividadeFuncao.Atividade);
			}
			
			// adiciona as atividades com acesso liberado:
            foreach (AtividadePessoaPermissao oAtvLiberada in arrayAtividadesAcessoLiberado)
            {
				bool contem = false;				
				for (int i = 0; i < listaTodasAtividades.Count; i++)
				{
					if (oAtvLiberada.Atividade.Descricao == listaTodasAtividades[i].Descricao)
					{
						contem = true;
						break;
					}
				}
				
				if (!contem)
					listaTodasAtividades.Add(oAtvLiberada.Atividade);
            }
			
			// remove as atividades com acesso restrito:
			foreach (AtividadePessoaPermissao oAtvBloqueada in arrayAtividadesAcessoRestrito)
			{
				for (int i = 0; i < listaTodasAtividades.Count; i++)
				{
					if (oAtvBloqueada.Atividade.Descricao == listaTodasAtividades[i].Descricao)
					{						
						listaTodasAtividades.RemoveAt(i);
					}
				}				
			}

			listaTodasAtividades.Sort( new SrvOrdenacaoAtividade("Descricao",true));

            return listaTodasAtividades.ToArray();
        }	
		
		/// <summary>
		/// TRAGA TODAS AS ATIVIDADES DE TODOS OS METROODOS
		/// </summary>
		/// <returns>
		/// A <see cref="Pessoa"/>
		/// </returns>				
		public virtual AtividadePessoaPermissao[] ListarAtividadesComAcessoRestritoMenu()
		{			
            DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dc.Add(Expression.Eq("Pessoa.Id", this.Id));
			dc.Add(Expression.Eq("AcessoPermitido", false));
			dc.CreateAlias("Atividade", "ativ");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.CreateAlias("ativ.Modulo", "mod");
					
            return AtividadePessoaPermissao.FindAll(dc);
		}
        
        public virtual AtividadePessoaPermissao[] ListarAtividadesComAcessoPermitidoMenu()
        {
			DetachedCriteria dc = DetachedCriteria.For(typeof(AtividadePessoaPermissao));
			dc.Add(Expression.Eq("Pessoa.Id", this.Id));
			dc.Add(Expression.Eq("AcessoPermitido", true));
			dc.CreateAlias("Atividade", "ativ");
			dc.Add(Expression.Eq("ativ.ExibirNoMenu", true));
			dc.CreateAlias("ativ.Modulo", "mod");
				
            return AtividadePessoaPermissao.FindAll(dc);
        }		

        public virtual Atividade[] ListarTodasAtividadesMenu()
        {  			
           	SrvUnidadeExercicioFuncaoPessoa objSrvUEFP = new SrvUnidadeExercicioFuncaoPessoa();
			SrvAtividadeUnidadeExercicioPermissao objSrvAtiUniExerPer = new SrvAtividadeUnidadeExercicioPermissao();
			SrvAtividade  objSrvAtividade = new SrvAtividade();

			List<Atividade> listaTodasAtividades = objSrvAtividade.listarObjsAtividadesPadrao();
						
			List<string> listaUnidadesPessoa = objSrvUEFP.listarSubUnidadesPessoa(this.Id);
			
            AtividadeUnidadeExercicioPermissao[] arrayAtividadesFuncao = objSrvAtiUniExerPer.ListarAtividadesMenu(listaUnidadesPessoa);
           
            AtividadePessoaPermissao[] arrayAtividadesAcessoLiberado = this.ListarAtividadesComAcessoPermitidoMenu();
            AtividadePessoaPermissao[] arrayAtividadesAcessoRestrito = this.ListarAtividadesComAcessoRestritoMenu();
							
			// adiciona as atividades da funcao da pessoa:
            foreach (AtividadeUnidadeExercicioPermissao oAtividadeFuncao in arrayAtividadesFuncao)
			{
				bool contem = false;				
				for (int i = 0; i < listaTodasAtividades.Count; i++)
				{
					if (oAtividadeFuncao.Atividade.Descricao == listaTodasAtividades[i].Descricao)
					{
						contem = true;
						break;
					}						

				}
				
				if (!contem)
					listaTodasAtividades.Add(oAtividadeFuncao.Atividade);
			}
			
			// adiciona as atividades com acesso liberado:
            foreach (AtividadePessoaPermissao oAtvLiberada in arrayAtividadesAcessoLiberado)
            {
				bool contem = false;				
				for (int i = 0; i < listaTodasAtividades.Count; i++)
				{
					if (oAtvLiberada.Atividade.Descricao == listaTodasAtividades[i].Descricao)
					{
						contem = true;
						break;
					}
				}
				
				if (!contem)
				{
					listaTodasAtividades.Add(oAtvLiberada.Atividade);					
				}
            }
			
			// remove as atividades com acesso restrito:
			foreach (AtividadePessoaPermissao oAtvBloqueada in arrayAtividadesAcessoRestrito)
			{
				for (int i = 0; i < listaTodasAtividades.Count; i++)
				{
					if (oAtvBloqueada.Atividade.Descricao == listaTodasAtividades[i].Descricao)
					{						
						listaTodasAtividades.RemoveAt(i);
					}					
				}				
			}
			
            listaTodasAtividades.Sort( new SrvOrdenacaoAtividade("Descricao",true));
		

			return listaTodasAtividades.ToArray();
        }	

		public static Pessoa[] ListarPessoasFisicas()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Pessoa));
			dc.Add(Expression.Eq("PessoaFisica", true));
			dc.AddOrder(Order.Asc("Nome"));
			return Pessoa.FindAll(dc);
		}
		
		public static Pessoa[] ListarPessoasJuridicas()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Pessoa));
			dc.Add(Expression.Eq("PessoaFisica", false));
			dc.AddOrder(Order.Asc("Nome"));
			return Pessoa.FindAll(dc);
		}	
		
		public static Pessoa[] Pesquisar(bool pessoaFisica, string nome, string cpfCnpj)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Pessoa));
			dc.AddOrder(Order.Asc("Nome"));
			dc.Add(Expression.Eq("PessoaFisica", pessoaFisica));
			dc.Add(Expression.InsensitiveLike("Nome", "%" + nome + "%"));
			dc.Add(Expression.InsensitiveLike("CpfCnpj", "%" + cpfCnpj + "%"));
			return Pessoa.FindAll(dc);
		}
				
		#region Realizacao de Auditoria
		public override void Delete()
		{			
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Delete();
		}
		
		public override void Save()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);		
			base.Save();
		}
		
		public override void Update()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Update();
		}
		
		public override void Create()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.Create();
		}
		
		public override void DeleteAndFlush()
		{			
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.DeleteAndFlush();
		}
		
		public override void SaveAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.SaveAndFlush();
		}
		
		public override void UpdateAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.UpdateAndFlush();
		}
		
		public override void CreateAndFlush()
		{
			new AuditoriaSistema().RealizarAuditoria(this.Id);
			base.CreateAndFlush();
		}		
		
		
		#endregion
	}
}