// AssociacaoGeral.cs created with MonoDevelop
// User: bruno at 16:51 12/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Classe para permissão de acesso aos relatórios.
	/// </summary>
	public class SrvAssociacaoGeral : PostgreSqlDatabase
	{	
		public void CadastrarRelatorio(string nome, string pagina)
		{
		}
		
		public void CadastrarGrupo(string nome, string icone)
		{
		}
		
		public void AssociarRelatorioGrupo(int idRelatorio, int idGrupo)
		{
		}
		
		public void AssociarUnidadeExercicioGrupo(int idUnidadeExercicio, int idGrupo)
		{
		}
		
		public void AssociarPessoaGrupo(int idPessoa, int idGrupo)
		{
		}
		
		public DataTable ListarGrupo(int inicio)
		{
			return ListarGrupo(inicio, 0);
		}
		
		public DataTable ListarGrupo(int inicio, int fim)
		{
			string sql = string.Format(@"select * from adm_licitar.tb_grupo_gru 
										where pk_cod_grupo_gru >= {0}",inicio);
			if(fim != 0) sql += string.Format(@" and pk_cod_grupo_gru <= {0}", fim);   
			sql += " order by pk_cod_grupo_gru";
			return Consultar(sql);
		}
		
		public DataTable ListarRelatorio()
		{
			string sql = string.Format(@"select * from adm_licitar.tb_relatorio_rel order by txt_descricao_rel");   
			return Consultar(sql);
		}
		
		public DataTable ListarUnidadeExercicio()
		{
			string sql = string.Format(@"select 
										iau.pk_cod_ins_are_uex_iau, uex.txt_descricao_uex
										from tb_ins_are_uex_iau iau
										inner join tb_inst_unid_ex_iue iue on iau.fk_cod_inst_unid_ex_iue = iue.pk_cod_inst_unid_ex_iue
										inner join tb_unidade_exerc_uex uex on iue.fk_cod_unidade_exerc_uex = uex.cod_unidade_exercicio_uex
										order by uex.txt_descricao_uex");   
			return Consultar(sql);
		}
		
		public DataTable ListarPessoa()
		{
			string sql = string.Format(@"select 
										pes.pk_cod_pessoa_pes, pfi.txt_nome_pfi
										from tb_pessoa_pes pes
										inner join tb_pessoa_fisica_pfi pfi on pes.fk_cod_pessoa_fisica_pfi = pfi.pk_cod_pessoa_fisica_pfi
										order by pfi.txt_nome_pfi");   
			return Consultar(sql);
		}
		
		public DataTable ListarRelatorioGrupo()
		{
			string sql = string.Format(@"select distinct rel.*
										from tb_relatorio_grupo_reg reg
										inner join tb_relatorio_rel rel on reg.fk_cod_relatorio_rel = rel.pk_cod_relatorio_rel
										where txt_descricao_rel <> 'lista'");
			return Consultar(sql);
		}
		
		public DataTable ListarRelatorioGrupo(int idUnidExerc, int idPessoa)
		{
			string sql = string.Format(@"select distinct rel.* 
										from tb_relatorio_grupo_reg reg
										inner join tb_relatorio_rel rel on reg.fk_cod_relatorio_rel = rel.pk_cod_relatorio_rel
										where txt_descricao_rel <> 'lista' and reg.fk_cod_grupo_gru in (select gip.fk_cod_grupo_gru 
											from tb_grupo_iau_pes_gip gip
											where gip.fk_cod_ins_are_uex_iau in
												(SELECT distinct iau.pk_cod_ins_are_uex_iau
												  FROM tb_ins_are_uex_iau iau, tb_inst_unid_ex_iue iue, tb_unidade_exerc_uex uex
												 WHERE iau.fk_cod_inst_unid_ex_iue = iue.pk_cod_inst_unid_ex_iue
												   AND iue.fk_cod_unidade_exerc_uex = {0})
											   or gip.fk_cod_pessoa_pes = {1}
											group by gip.fk_cod_grupo_gru)", idUnidExerc, idPessoa);
			return Consultar(sql);
		}
		
		public DataTable ListarRelatorioGrupo(int idGrupo)
		{
			string sql = string.Format(@"select 
										rel.* 
										from tb_relatorio_grupo_reg reg
										inner join tb_relatorio_rel rel on reg.fk_cod_relatorio_rel = rel.pk_cod_relatorio_rel
										where reg.fk_cod_grupo_gru  = {0}", idGrupo);
			return Consultar(sql);
		}
		
		public void ListaUnidadeExercicioPessoaGrupo(int Grupo)
		{
		}
	}
}
