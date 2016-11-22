// ProcessarPermissoes.cs created with MonoDevelop
// User: wanialdo at 15:20 29/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;

using NHibernate.Expression;

using Licitar.Business.Entidade;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Classe para reprocessamento completo das permissões de acesso ao sistema.
	/// </summary>
	public class ProcessarPermissoes : PostgreSqlDatabase
	{
		protected string local = "/licitar";
		
		/// <summary>
		/// Construtor padrão.
		/// </summary>
		public ProcessarPermissoes()
		{
		}
		
		/// <summary>
		/// Limpa os módulos cadastrados.
		/// </summary>
		public void LimparModulos()
		{
			Processar("delete from adm_licitar.tb_modulo_mod");
		}
		
		/// <summary>
		/// Limpa os níveis de permissão cadastrados.
		/// </summary>
		public void LimparPermissoes()
		{
			Processar("delete from adm_licitar.tb_permissao_per");
		}
		
		/// <summary>
		/// Limpa as páginas cadastradas.
		/// </summary>
		public void LimparPaginas()
		{
			Processar("delete from adm_licitar.tb_atividade_ati where boo_tipo_andamento_ati = false");
		}
		
		/// <summary>
		/// Limpa as associações de permissão de acesso. Se for limpeza geral do banco, deve ser processado primeiro.
		/// </summary>
		public void LimparAssociacoes()
		{
			Processar("delete from adm_licitar.tb_atividade_pessoa_permissao_app");
			Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
				values ((select pk_cod_permissao_per from adm_licitar.tb_permissao_per where txt_descricao_per = 'Master'), 
					(select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = 'MARCELO LIMA DE ALMEIDA'),
					(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '/licitar/Admin/pgFormProcessarPermissoes.aspx'), true)");
			Processar("delete from adm_licitar.tb_atividade_funcao_permissao_afp");
		}
		
		/// <summary>
		/// Inclui no banco os módulos do sistema.
		/// </summary>
		public void IncluirModulos()
		{
			string tabela = "tb_modulo_mod";
			string campos = "txt_descricao_mod";
			string[] modulos = {"'Administração'", "'Relatórios'", "'Processo'"};
			
			for (int i = 0; i < modulos.Length; i++)
			{
				ProcessarIncluir(tabela, campos, modulos[i]);
			}
		}
		
		/// <summary>
		/// Inclui os níveis de permissão de acesso nas tabelas.
		/// </summary>
		public void IncluirPermissoes()
		{
			string tabela = "tb_permissao_per";
			string campos = "txt_descricao_per, txt_sigla_per";
			string[] permissoes = {
				"'Master','Master'",
				"'Incluir','Incluir'",
				"'Alterar','Alterar'",
				"'Excluir','Excluir'",
				"'Consultar','Consultar'"
			};
			
			for (int i = 0; i < permissoes.Length; i++)
			{
				ProcessarIncluir(tabela, campos, permissoes[i]);
			}
		}
		
		/// <summary>
		/// Inclui as páginas no tabela de atividades.
		/// </summary>
		public void IncluirPaginas()
		{
			IncluirBlocos();
			IncluirBlocoAdministracao();
			IncluirBlocoRelatorios();
			IncluirBlocoProcesso();
		}
		
		/// <summary>
		/// Inclui as páginas principais dos módulos do sistema.
		/// </summary>
		public void IncluirBlocos()
		{
			string tabela = "tb_atividade_ati";
			string campos = "fk_cod_modulo_mod, txt_nivel_ati, txt_descricao_ati, txt_url_ati, boo_tipo_andamento_ati, boo_exibir_no_menu_ati";
			string[] confere = {
				"/licitar/Admin/Default.aspx",
				"/licitar/Relatorios/Default.aspx",
				"/licitar/Operacional/Default.aspx"
			};
			string[] paginas = {
				"(select pk_cod_modulo_mod from adm_licitar.tb_modulo_mod where txt_descricao_mod = 'Administração'),'1', 'Administração', '/licitar/Admin/Default.aspx', false, false",
				"(select pk_cod_modulo_mod from adm_licitar.tb_modulo_mod where txt_descricao_mod = 'Relatórios'),'1', 'Relatórios', '/licitar/Relatorios/Default.aspx', false, false",
				"(select pk_cod_modulo_mod from adm_licitar.tb_modulo_mod where txt_descricao_mod = 'Processo'),'1', 'Processo', '/licitar/Operacional/Default.aspx', false, false"
			};
			
			for (int i = 0; i < paginas.Length; i++)
			{
				if (Consultar("select * from adm_licitar.tb_atividade_ati where txt_descricao_ati = '" + confere[i] + "'").Rows.Count <= 0)
					ProcessarIncluir(tabela, campos, paginas[i]);
			}
			
		}
		
		/// <summary>
		/// Inclui as páginas do bloco Administrativo.
		/// </summary>
		public void IncluirBlocoAdministracao()
		{
			string tabela = "tb_atividade_ati";
			string campos = "fk_cod_atividade_ati, fk_cod_modulo_mod, txt_nivel_ati, txt_descricao_ati, txt_url_ati, boo_tipo_andamento_ati, boo_exibir_no_menu_ati";
			string[] confere = {
				"/licitar/Admin/pgConProcessoCompleto.aspx",
				"/licitar/Admin/pgConProcessoCompletoStatus.aspx",
				"/licitar/Admin/pgFormAreas.aspx",
				"/licitar/Admin/pgFormAtualizarDataEntrada.aspx",
				"/licitar/Admin/pgFormFluxoAndamento.aspx",
				"/licitar/Admin/pgFormFraseProntas.aspx",
				"/licitar/Admin/pgFormGrupoRelatorio.aspx",
				"/licitar/Admin/pgFormInstituicao.aspx",
				"/licitar/Admin/pgFormModalidadeUnidadeExercicio.aspx"
			};
			
			string[] paginas = {
				"'1.1', 'Processamento do Tabelão', '/licitar/Admin/pgConProcessoCompleto.aspx', false, true",
				"'1.2', 'Processamento do Tabelão - Auxiliar', '/licitar/Admin/pgConProcessoCompletoStatus.aspx', false, false",
				"'1.3', 'Cadastro de Áreas', '/licitar/Admin/pgFormAreas.aspx', false, true",
				"'1.4', 'Atualizar Datas de Entrada na PGE', '/licitar/Admin/pgFormAtualizarDataEntrada.aspx', false, true",
				"'1.5', 'Fluxo do Andamento', '/licitar/Admin/pgFormFluxoAndamento.aspx', false, true",
				"'1.6', 'Frases Prontas', '/licitar/Admin/pgFormFraseProntas.aspx', false, true",
				"'1.7', 'Permissões de Grupo', '/licitar/Admin/pgFormGrupoRelatorio.aspx', false, true",
				"'1.8', 'Cadastro de Instituição', '/licitar/Admin/pgFormInstituicao.aspx', false, true",
				"'1.9', 'Associação Modalidade x Unidade de Exercício', '/licitar/Admin/pgFormModalidadeUnidadeExercicio.aspx', false, true"
			};
			
			for (int i = 0; i < paginas.Length; i++)
			{
				if (Consultar("select * from adm_licitar.tb_atividade_ati where txt_descricao_ati = '" + confere[i] + "'").Rows.Count <= 0)
					ProcessarIncluir(tabela, campos, @"(Select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_descricao_ati = 'Administração'), 
					(select pk_cod_modulo_mod from adm_licitar.tb_modulo_mod where txt_descricao_mod = 'Administração')," + paginas[i]);
			}
		}
		
		/// <summary>
		/// Inclui as páginas do bloco Relatórios.
		/// </summary>
		public void IncluirBlocoRelatorios()
		{
			string tabela = "tb_atividade_ati";
			string campos = "fk_cod_atividade_ati, fk_cod_modulo_mod, txt_nivel_ati, txt_descricao_ati, txt_url_ati, boo_tipo_andamento_ati, boo_exibir_no_menu_ati";
			string[] confere = {"/licitar/Relatorios/pgConExtratoProcesso.aspx",
				"/licitar/Relatorios/pgConHistExtratoProcesso.aspx",
				"/licitar/Relatorios/pgRelAgenda de Licitações.aspx",
				"/licitar/Relatorios/pgRelAndamentoLicitacoes.aspx",
				"/licitar/Relatorios/pgRelContagemConcluidos.aspx",
				"/licitar/Relatorios/pgRelEconomiaPregao.aspx",
				"/licitar/Relatorios/pgRelEconomiaPregaoGeral.aspx",
				"/licitar/Relatorios/pgRelGerencial.aspx",
				"/licitar/Relatorios/pgRelLicitacao.aspx",
				"/licitar/Relatorios/pgRelLicitacoesAndamentoPorPregoeiro.aspx",
				"/licitar/Relatorios/pgRelLicitacoesConcluidas.aspx",
				"/licitar/Relatorios/pgRelLicitacoesTipoConclusao.aspx",
				"/licitar/Relatorios/pgRelTotaisConcluidos.aspx",
				"/licitar/Relatorios/pgRelLicitacaoGeral.aspx" 
			};
			string[] paginas = {"'2.1', 'Extrato do Processo', '/licitar/Relatorios/pgConExtratoProcesso.aspx', false, false",
				"'2.13', 'Histórico do Processo', '/licitar/Relatorios/pgConHistExtratoProcesso.aspx', false, true",
				"'2.2', 'Agenda de Licitações', '/licitar/Operacional/pgRelAgendaLicitações.aspx', false, true",
				"'2.3', 'Andamento das Licitações', '/licitar/Relatorios/pgRelAndamentoLicitacoes.aspx', false, true",
				"'2.4', 'Contagem de Licitações Concluídas', '/licitar/Relatorios/pgRelContagemConcluidos.aspx', false, true",
				"'2.5', 'Relatório de Economia do Pregão', '/licitar/Relatorios/pgRelEconomiaPregao.aspx', false, true",
				"'2.6', 'Relatorio de Economia do Pregão - Sub', '/licitar/Relatorios/pgRelEconomiaPregaoGeral.aspx', false, false",
				"'2.7', 'Gerador de Consultas', '/licitar/Relatorios/pgRelGerencial.aspx', false, true",
				"'2.8', 'Consulta de Licitações', '/licitar/Relatorios/pgRelLicitacao.aspx', false, true",
				"'2.9', 'Licitações em Andamento por Pregoeiro/Presidente/Vice-Presidente', '/licitar/Relatorios/pgRelLicitacoesAndamentoPorPregoeiro.aspx', false, true",
				"'2.10', 'Relatório de Licitações Concluídas', '/licitar/Relatorios/pgRelLicitacoesConcluidas.aspx', false, true",
				"'2.11', 'Licitações por Tipo de Conclusão', '/licitar/Relatorios/pgRelLicitacoesTipoConclusao.aspx', false, false",
				"'2.12', 'Totais das Licitações Concluídas', '/licitar/Relatorios/pgRelTotaisConcluidos.aspx', false, true",
				"'2.13', 'Impressão Consulta Licitações', '/licitar/Relatorios/pgRelLicitacaoGeral.aspx', false, false"
			};
			
			for (int i = 0; i < paginas.Length; i++)
			{
				if (Consultar("select * from adm_licitar.tb_atividade_ati where txt_descricao_ati = '" + confere[i] + "'").Rows.Count <= 0)
					ProcessarIncluir(tabela, campos, @"(Select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_descricao_ati = 'Relatórios'), 
					(select pk_cod_modulo_mod from adm_licitar.tb_modulo_mod where txt_descricao_mod = 'Relatórios')," + paginas[i]);
			}
		}
		
		/// <summary>
		/// Inclui as páginas do bloco Processo.
		/// </summary>
		public void IncluirBlocoProcesso()
		{
			string tabela = "tb_atividade_ati";
			string campos = "fk_cod_atividade_ati, fk_cod_modulo_mod, txt_nivel_ati, txt_descricao_ati, txt_url_ati, boo_tipo_andamento_ati, boo_exibir_no_menu_ati";
			string[] confere = {
				"/licitar/Operacional/pgConAgendaLicitacao.aspx",
				"/licitar/Operacional/pgConProcesso.aspx",
				"/licitar/Operacional/pgFormProcesso.aspx",
				"/licitar/Operacional/pgConPessoa.aspx",
				"/licitar/Operacional/pgFormNumeroProcesso.aspx",
				"/licitar/Operacional/pgFormValorProcesso.aspx",
				"/licitar/Operacional/pgFormProcessoAndamento.aspx",
				"/licitar/Operacional/pgFormPessoaProcesso.aspx",
				"/licitar/Operacional/pgPopupAssociar.aspx",
				"/licitar/Operacional/pgFormPessoa.aspx"
			};
			string[] paginas = {
				"'3.1', 'Agenda de Licitações', '/licitar/Operacional/pgConAgendaLicitacao.aspx', false, true",
				"'3.2', 'Consulta de Processos', '/licitar/Operacional/pgConProcesso.aspx', false, true",
				"'3.3', 'Cadastro de Processos', '/licitar/Operacional/pgFormProcesso.aspx', false, true",
				"'3.4', 'Consulta de Pessoas', '/licitar/Operacional/pgConPessoa.aspx', false, true",
				"'3.5', 'Números do Processo', '/licitar/Operacional/pgFormNumeroProcesso.aspx', false, false",
				"'3.6', 'Valores do Processo', '/licitar/Operacional/pgFormValorProcesso.aspx', false, false",
				"'3.7', 'Andamentos do Processo', '/licitar/Operacional/pgFormProcessoAndamento.aspx', false, false",
				"'3.8', 'Pessoas do Processo', '/licitar/Operacional/pgFormPessoaProcesso.aspx', false, false",
				"'3.9', 'Cadastro de Pessoas', '/licitar/Operacional/pgFormPessoa.aspx', false, true",
				"'3.10', 'Upload de arquivos', '/licitar/Operacional/pgPopupAssociar.aspx', false, false"
			};
			
			for (int i = 0; i < paginas.Length; i++)
			{
				if (Consultar("select * from adm_licitar.tb_atividade_ati where txt_descricao_ati = '" + confere[i] + "'").Rows.Count <= 0)
					ProcessarIncluir(tabela, campos, @"(Select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_descricao_ati = 'Processo'), 
					(select pk_cod_modulo_mod from adm_licitar.tb_modulo_mod where txt_descricao_mod = 'Processo')," + paginas[i]);
			}
		}
		
		/// <summary>
		/// Inclui as associações de acesso às páginas do sistema.
		/// </summary>
		public void IncluirAssociacoes()
		{
			IncluirAssociacoesBlocos();
			IncluirAssociacoesProcesso();
			IncluirAssociacoesRelatorios();
			IncluirAssociacoesAdministracao();
		}
		
		public void IncluirAssociacoesBlocos()
		{
			Permissao oPermissao = Permissao.FindOne(Expression.Eq("Descricao","Master"));
			Funcao[] oFuncao = Funcao.FindAll();
			string[] paginas = {"/licitar/Admin/Default.aspx",
				"/licitar/Relatorios/Default.aspx",
				"/licitar/Operacional/Default.aspx"
				};

			for (int i = 0; i < oFuncao.Length; i++)
			{
				for (int j = 0; j < paginas.Length; j++)
				{
					if (Consultar("select * from adm_licitar.tb_atividade_funcao_permissao_afp where fk_cod_permissao_per=" 
					              + oPermissao.Id.ToString() + " and fk_cod_funcao_fun=" + oFuncao[i].Id.ToString() 
					              + " and fk_cod_atividade_ati=(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '" + paginas[j] + "')").Rows.Count <= 0)
							Processar(@"insert into adm_licitar.tb_atividade_funcao_permissao_afp (fk_cod_permissao_per, fk_cod_funcao_fun, fk_cod_atividade_ati) 
								values (" + oPermissao.Id.ToString() + ", " + oFuncao[i].Id.ToString() + @",
									(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '" + paginas[j] + "'))");
				}
			}
		}
		
		/// <summary>
		/// Inclui as associações de acesso às páginas do bloco Processo.
		/// </summary>
		public void IncluirAssociacoesProcesso()
		{
			Permissao oPermissao = Permissao.FindOne(Expression.Eq("Descricao","Consultar"));
			Funcao[] oFuncao = Funcao.FindAll();
			string[] paginas = {
				"/licitar/Operacional/pgConAgendaLicitacao.aspx",
				"/licitar/Operacional/pgConProcesso.aspx",
				"/licitar/Operacional/pgFormProcesso.aspx",
				"/licitar/Operacional/pgConPessoa.aspx",
				"/licitar/Operacional/pgFormNumeroProcesso.aspx",
				"/licitar/Operacional/pgFormValorProcesso.aspx",
				"/licitar/Operacional/pgFormProcessoAndamento.aspx",
				"/licitar/Operacional/pgFormPessoaProcesso.aspx",
				"/licitar/Operacional/pgPopupAssociar.aspx",
				"/licitar/Operacional/pgFormPessoa.aspx"
			};

			//Permissão Padrão: Consultar [Não necessariamente quer dizer "Consultar Apenas", mas que é a permissão é padrão para todos].
			for (int i = 0; i < oFuncao.Length; i++)
			{
				for (int j = 0; j < paginas.Length; j++)
				{
					if (Consultar("select * from adm_licitar.tb_atividade_funcao_permissao_afp where fk_cod_permissao_per=" 
					              + oPermissao.Id.ToString() + " and fk_cod_funcao_fun=" + oFuncao[i].Id.ToString() 
					              + " and fk_cod_atividade_ati=(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '" + paginas[j] + "')").Rows.Count <= 0)
							Processar(@"insert into adm_licitar.tb_atividade_funcao_permissao_afp (fk_cod_permissao_per, fk_cod_funcao_fun, fk_cod_atividade_ati) 
								values (" + oPermissao.Id.ToString() + ", " + oFuncao[i].Id.ToString() + @",
									(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '" + paginas[j] + "'))");
				}
			}
			
			//Agenda de Licitação: Colunas de Marcação só para os usuários Master
			string[] oPessoaAgenda = {
				"TARSO PINHEIRO BORGES",
				"ANTONIA TANIA TRAJANO BESERRA",
				"ISAURA CARDOSO CAVALCANTE DE CASTRO",
				"MARCELO LIMA DE ALMEIDA",
				"DANIELLE MOURÃO PINTO"};
			
			for (int i = 0; i < oPessoaAgenda.Length; i++)
			{
				Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
					values ((select pk_cod_permissao_per from adm_licitar.tb_permissao_per where txt_descricao_per = 'Master'), 
						(select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = '" 
				    + oPessoaAgenda[i].ToString() + "'),(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '/licitar/Operacional/pgConAgendaLicitacao.aspx'), true)");
			}
			
			//Consulta de Processo: Coluna Editar/Excluir somente para os usuários Master
			string[] oPessoaConProcesso = {
				"TARSO PINHEIRO BORGES",
				"RITA DE CASSIA HOLLANDA MATOS",
				"ANTONIA TANIA TRAJANO BESERRA",
				"EDMAR MENDES DA SILVA",
				"ISAURA CARDOSO CAVALCANTE DE CASTRO",
				"MARCELO LIMA DE ALMEIDA",
				"DANIELLE MOURÃO PINTO"};

			for (int i = 0; i < oPessoaConProcesso.Length; i++)
			{
				Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
					values ((select pk_cod_permissao_per from adm_licitar.tb_permissao_per where txt_descricao_per = 'Master'), 
						(select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = '" 
				    + oPessoaConProcesso[i].ToString() + "'),(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '/licitar/Operacional/pgConProcesso.aspx'), true)");
			}
			
			//Formulário de Processo: Permissão de Editar/Excluir somente para o grupo Master.
			for (int i = 0; i < oPessoaConProcesso.Length; i++)
			{
				Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
					values ((select pk_cod_permissao_per from adm_licitar.tb_permissao_per where txt_descricao_per = 'Master'), 
						(select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = '" 
				    + oPessoaConProcesso[i].ToString() + "'),(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '/licitar/Operacional/pgFormProcesso.aspx'), true)");
			}
			
			//Números do Processo: Todos têm permissão de inclusão/alteração/exclusão.
			
			//Valores do Processo: Permissão de Editar/Excluir apenas para o grupo Master.
			string[] oPessoaValores = {
				"TARSO PINHEIRO BORGES",
				"RITA DE CASSIA HOLLANDA MATOS",
				"ANTONIA TANIA TRAJANO BESERRA",
				"MAGNO FARNEY PINHEIRO HOLANDA",
				"EDMAR MENDES DA SILVA",
				"LUIS CLAUDIO PONTES MASCARENHAS",
				"FRANCISCO IRISNALDO DE OLIVEIRA",
				"ISAURA CARDOSO CAVALCANTE DE CASTRO",
				"MARCELO LIMA DE ALMEIDA",
				"DANIELLE MOURÃO PINTO"};

			for (int i = 0; i < oPessoaValores.Length; i++)
			{
				Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
					values ((select pk_cod_permissao_per from adm_licitar.tb_permissao_per where txt_descricao_per = 'Master'), 
						(select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = '" 
				    + oPessoaValores[i].ToString() + "'),(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '/licitar/Operacional/pgFormValorProcesso.aspx'), true)");
			}
			
			//Pessoas do Processo: Todos têm permissão de inclusão/alteração/exclusão, menos para Excluir Pregoeiro/Presidente/Vice, que é apenas pelos Masters.
			string[] oPessoaProcesso = {
				"TARSO PINHEIRO BORGES",
				"RITA DE CASSIA HOLLANDA MATOS",
				"ANTONIA TANIA TRAJANO BESERRA",
				"EDMAR MENDES DA SILVA",
				"ISAURA CARDOSO CAVALCANTE DE CASTRO",
				"MARCELO LIMA DE ALMEIDA",
				"DANIELLE MOURÃO PINTO"};

			for (int i = 0; i < oPessoaProcesso.Length; i++)
			{
				Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
					values ((select pk_cod_permissao_per from adm_licitar.tb_permissao_per where txt_descricao_per = 'Master'), 
						(select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = '" 
				    + oPessoaProcesso[i].ToString() + "'),(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '/licitar/Operacional/pgFormPessoaProcesso.aspx'), true)");
			}
			
			//Andamento do Processo: Edição/Exclusão apenas pelos Masters.
			string[] oPessoaAndamentos = {
				"TARSO PINHEIRO BORGES",
				"RITA DE CASSIA HOLLANDA MATOS",
				"ANTONIA TANIA TRAJANO BESERRA",
				"MAGNO FARNEY PINHEIRO HOLANDA",
				"EDMAR MENDES DA SILVA",
				"LUIS CLAUDIO PONTES MASCARENHAS",
				"FRANCISCO IRISNALDO DE OLIVEIRA",
				"ISAURA CARDOSO CAVALCANTE DE CASTRO",
				"MARCELO LIMA DE ALMEIDA",
				"DANIELLE MOURÃO PINTO"};
			
			for (int i = 0; i < oPessoaAndamentos.Length; i++)
			{
				Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
					values ((select pk_cod_permissao_per from adm_licitar.tb_permissao_per where txt_descricao_per = 'Master'), 
						(select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = '" 
				    + oPessoaAndamentos[i].ToString() + "'),(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '/licitar/Operacional/pgFormProcessoAndamento.aspx'), true)");
			}
			
		}
		
		/// <summary>
		/// Inclui as associações de acesso às páginas do bloco Administracao.
		/// </summary>
		public void IncluirAssociacoesAdministracao()
		{
			Permissao oPermissao = Permissao.FindOne(Expression.Eq("Descricao","Master"));
			string[] oPessoa = {"ISAURA CARDOSO CAVALCANTE DE CASTRO","MARCELO LIMA DE ALMEIDA","DANIELLE MOURÃO PINTO"};
			string[] paginas = {"/licitar/Admin/pgConProcessoCompleto.aspx",
				"/licitar/Admin/pgConProcessoCompletoStatus.aspx",
				"/licitar/Admin/pgFormAreas.aspx",
				"/licitar/Admin/pgFormAtualizarDataEntrada.aspx",
				"/licitar/Admin/pgFormFluxoAndamento.aspx",
				"/licitar/Admin/pgFormFraseProntas.aspx",
				"/licitar/Admin/pgFormGrupoRelatorio.aspx",
				"/licitar/Admin/pgFormInstituicao.aspx",
				"/licitar/Admin/pgFormModalidadeUnidadeExercicio.aspx"};

			for (int i = 0; i < oPessoa.Length; i++)
			{
				for (int j = 0; j < paginas.Length; j++)
				{
					Processar(@"insert into adm_licitar.tb_atividade_pessoa_permissao_app (fk_cod_permissao_per, fk_cod_pessoa_pes, fk_cod_atividade_ati, boo_acesso_permitido_ape) 
						values (" + oPermissao.Id.ToString() + ", (select pk_cod_pessoa_pes from adm_licitar.tb_pessoa_pes where txt_nome_pes = '" 
					    + oPessoa[i].ToString() + "'),(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '" + paginas[j] + "'), true)");
				}
			}
		}
		
		/// <summary>
		/// Inclui as associações de acesso às páginas do bloco Relatórios.
		/// </summary>
		public void IncluirAssociacoesRelatorios()
		{
			Permissao oPermissao = Permissao.FindOne(Expression.Eq("Descricao","Master"));
			Funcao[] oFuncao = Funcao.FindAll();
			string[] paginas = {"/licitar/Relatorios/pgConExtratoProcesso.aspx",
				"/licitar/Relatorios/pgRelAgenda de Licitações.aspx",
				"/licitar/Relatorios/pgRelAndamentoLicitacoes.aspx",
				"/licitar/Relatorios/pgRelContagemConcluidos.aspx",
				"/licitar/Relatorios/pgRelEconomiaPregao.aspx",
				"/licitar/Relatorios/pgRelEconomiaPregaoGeral.aspx",
				"/licitar/Relatorios/pgRelGerencial.aspx",
				"/licitar/Relatorios/pgRelLicitacao.aspx",
				"/licitar/Relatorios/pgRelLicitacoesAndamentoPorPregoeiro.aspx",
				"/licitar/Relatorios/pgRelLicitacoesConcluidas.aspx",
				"/licitar/Relatorios/pgRelLicitacoesTipoConclusao.aspx",
				"/licitar/Relatorios/pgRelTotaisConcluidos.aspx",
			    "/licitar/Relatorios/pgRelLicitacaoGeral.aspx"};

			for (int i = 0; i < oFuncao.Length; i++)
			{
				for (int j = 0; j < paginas.Length; j++)
				{
					if (Consultar("select * from adm_licitar.tb_atividade_funcao_permissao_afp where fk_cod_permissao_per=" 
					              + oPermissao.Id.ToString() + " and fk_cod_funcao_fun=" + oFuncao[i].Id.ToString() 
					              + " and fk_cod_atividade_ati=(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '" + paginas[j] + "')").Rows.Count <= 0)
							Processar(@"insert into adm_licitar.tb_atividade_funcao_permissao_afp (fk_cod_permissao_per, fk_cod_funcao_fun, fk_cod_atividade_ati) 
								values (" + oPermissao.Id.ToString() + ", " + oFuncao[i].Id.ToString() + @",
									(select pk_cod_atividade_ati from adm_licitar.tb_atividade_ati where txt_url_ati = '" + paginas[j] + "'))");
				}
			}
		}
		
		/// <summary>
		/// Método de inclusão genérico. 
		/// </summary>
		protected void ProcessarIncluir(string tabela, string campos, string valores)
		{
			Processar("insert into adm_licitar." + tabela + " (" + campos + ") values (" + valores + ")");
		}

		/// <summary>
		/// Processa um comando SQL no banco. 
		/// </summary>
		protected void Processar(string sql)
		{
			try 
			{
				ExecutarComando(sql);
			}
			catch (Exception ex) 
			{ 
				throw new Exception("Ocorreu um erro na execução do comando: " + sql + ". O erro é: " + ex.Message);
			}
		}
	}
}
