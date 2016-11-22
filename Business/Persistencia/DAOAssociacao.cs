// DAOAssociacao.cs created with MonoDevelop
// User: diogolima at 12:58 15/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

namespace Licitar.Business.Dao
{
	public class DAOAssociacao : BaseDao
	{		
		//Database.Database _db = new Database.Database();
		
		public DataTable ListarTodasAssociacoes()
		{
			string select = @"select
								*
								from
								adm_licitar.tb_acu_pessoa_ape ape
								inner join tah simb_iaa_caso_uso_acu acu on ape.fk_cod_iaa_caso_uso_acu = acu.pk_cod_iaa_caso_uso_acu
								inner join tb_inst_caso_uso_icu icu on acu.fk_cod_inst_caso_uso_icu = icu.pk_cod_inst_caso_uso_icu
								inner join tb_inst_pessoa_ipe ipe on ape.fk_cod_inst_pessoa_ipe = ipe.pk_cod_inst_pessoa_ipe
								inner join tb_pessoa_pes pes on ipe.fk_cod_pessoa_pes = pes.pk_cod_pessoa_pes
								inner join tb_pessoa_fisica_pfi pfi on pes.fk_cod_pessoa_fisica_pfi = pfi.pk_cod_pessoa_fisica_pfi
								inner join tb_iaf_atividade_iaa iaa on acu.fk_cod_iaf_atividade_iaa = iaa.pk_cod_iaf_atividade_iaa
								inner join tb_inst_atividade_iat iat on iaa.fk_cod_inst_atividade_iat = iat.pk_cod_inst_atividade_iat
								inner join tb_iac_funcao_iaf iaf on iaa.fk_cod_iac_funcao_iaf = iaf.pk_cod_iac_funcao_iaf
								inner join tb_iau_cargo_iac iac on iaf.fk_cod_iau_cargo_iac = iac.pk_cod_iau_cargo_iac
								inner join tb_inst_funcao_ifu ifu on iaf.fk_cod_inst_funcao_ifu = ifu.pk_cod_inst_funcao_ifu
								inner join tb_ins_are_uex_iau iau on iac.fk_cod_ins_are_uex_iau = iau.pk_cod_ins_are_uex_iau
								inner join tb_inst_cargo_ica ica on iac.fk_cod_inst_cargo_ica = ica.pk_cod_inst_cargo_ica
								inner join tb_inst_unid_ex_iue iue on iau.fk_cod_inst_unid_ex_iue = iue.pk_cod_inst_unid_ex_iue
								inner join tb_inst_area_iar iar on iau.fk_cod_inst_area_iar = iar.pk_cod_inst_area_iar
								inner join tb_unidade_exerc_uex uex on iue.fk_cod_unidade_exerc_uex = uex.cod_unidade_exercicio_uex
								inner join tb_area_are are on iar.fk_cod_area_are = are.pk_cod_area_are
								inner join tb_cargo_cgo cgo on ica.fk_cod_cargo_cgo = cgo.pk_cod_cargo_cgo
								inner join tb_funcao_fun fun on ifu.fk_cod_funcao_fun = fun.pk_cod_funcao_fun
								inner join tb_atividade_ati ati on iat.fk_cod_atividade_ati = ati.pk_cod_atividade_ati
								inner join tb_caso_uso_cau cau on icu.fk_cod_caso_usu = cau.pk_cod_caso_usu
								inner join tb_instituicao_ins ins on ifu.fk_cod_instituicao_ins = ins.pk_cod_instituicao_ins
								order by pfi.txt_nome_pfi";
			
			return Consultar(select); //_db.ExecutarConsulta(select).Tables[0];
		}
		
		public DataTable ListarTodasAssociacoesDeUmaPessoa(string nomePessoa)
		{
			string select = @"select
								*
								from
								adm_licitar.tb_acu_pessoa_ape ape
								inner join tb_iaa_caso_uso_acu acu on ape.fk_cod_iaa_caso_uso_acu = acu.pk_cod_iaa_caso_uso_acu
								inner join tb_inst_caso_uso_icu icu on acu.fk_cod_inst_caso_uso_icu = icu.pk_cod_inst_caso_uso_icu
								inner join tb_inst_pessoa_ipe ipe on ape.fk_cod_inst_pessoa_ipe = ipe.pk_cod_inst_pessoa_ipe
								inner join tb_pessoa_pes pes on ipe.fk_cod_pessoa_pes = pes.pk_cod_pessoa_pes
								inner join tb_pessoa_fisica_pfi pfi on pes.fk_cod_pessoa_fisica_pfi = pfi.pk_cod_pessoa_fisica_pfi
								inner join tb_iaf_atividade_iaa iaa on acu.fk_cod_iaf_atividade_iaa = iaa.pk_cod_iaf_atividade_iaa
								inner join tb_inst_atividade_iat iat on iaa.fk_cod_inst_atividade_iat = iat.pk_cod_inst_atividade_iat
								inner join tb_iac_funcao_iaf iaf on iaa.fk_cod_iac_funcao_iaf = iaf.pk_cod_iac_funcao_iaf
								inner join tb_iau_cargo_iac iac on iaf.fk_cod_iau_cargo_iac = iac.pk_cod_iau_cargo_iac
								inner join tb_inst_funcao_ifu ifu on iaf.fk_cod_inst_funcao_ifu = ifu.pk_cod_inst_funcao_ifu
								inner join tb_ins_are_uex_iau iau on iac.fk_cod_ins_are_uex_iau = iau.pk_cod_ins_are_uex_iau
								inner join tb_inst_cargo_ica ica on iac.fk_cod_inst_cargo_ica = ica.pk_cod_inst_cargo_ica
								inner join tb_inst_unid_ex_iue iue on iau.fk_cod_inst_unid_ex_iue = iue.pk_cod_inst_unid_ex_iue
								inner join tb_inst_area_iar iar on iau.fk_cod_inst_area_iar = iar.pk_cod_inst_area_iar
								inner join tb_unidade_exerc_uex uex on iue.fk_cod_unidade_exerc_uex = uex.cod_unidade_exercicio_uex
								inner join tb_area_are are on iar.fk_cod_area_are = are.pk_cod_area_are
								inner join tb_cargo_cgo cgo on ica.fk_cod_cargo_cgo = cgo.pk_cod_cargo_cgo
								inner join tb_funcao_fun fun on ifu.fk_cod_funcao_fun = fun.pk_cod_funcao_fun
								inner join tb_atividade_ati ati on iat.fk_cod_atividade_ati = ati.pk_cod_atividade_ati
								inner join tb_caso_uso_cau cau on icu.fk_cod_caso_usu = cau.pk_cod_caso_usu
								inner join tb_instituicao_ins ins on ifu.fk_cod_instituicao_ins = ins.pk_cod_instituicao_ins
								where upper(pfi.txt_nome_pfi) ilike '@nome' 
								order by pfi.txt_nome_pfi";			
			select = select.Replace("@nome", "%" + nomePessoa + "%");
			return Consultar(select); //_db.ExecutarConsulta(select).Tables[0];	
		}
		
		public DataTable ListarUnidadeExercicio(int idPessoa)
		{
			string select = @"select
								uex.txt_descricao_uex
								from
								adm_licitar.tb_acu_pessoa_ape ape
								inner join tb_iaa_caso_uso_acu acu on ape.fk_cod_iaa_caso_uso_acu = acu.pk_cod_iaa_caso_uso_acu
								inner join tb_inst_caso_uso_icu icu on acu.fk_cod_inst_caso_uso_icu = icu.pk_cod_inst_caso_uso_icu
								inner join tb_inst_pessoa_ipe ipe on ape.fk_cod_inst_pessoa_ipe = ipe.pk_cod_inst_pessoa_ipe
								inner join tb_pessoa_pes pes on ipe.fk_cod_pessoa_pes = pes.pk_cod_pessoa_pes
								inner join tb_pessoa_fisica_pfi pfi on pes.fk_cod_pessoa_fisica_pfi = pfi.pk_cod_pessoa_fisica_pfi
								inner join tb_iaf_atividade_iaa iaa on acu.fk_cod_iaf_atividade_iaa = iaa.pk_cod_iaf_atividade_iaa
								inner join tb_inst_atividade_iat iat on iaa.fk_cod_inst_atividade_iat = iat.pk_cod_inst_atividade_iat
								inner join tb_iac_funcao_iaf iaf on iaa.fk_cod_iac_funcao_iaf = iaf.pk_cod_iac_funcao_iaf
								inner join tb_iau_cargo_iac iac on iaf.fk_cod_iau_cargo_iac = iac.pk_cod_iau_cargo_iac
								inner join tb_inst_funcao_ifu ifu on iaf.fk_cod_inst_funcao_ifu = ifu.pk_cod_inst_funcao_ifu
								inner join tb_ins_are_uex_iau iau on iac.fk_cod_ins_are_uex_iau = iau.pk_cod_ins_are_uex_iau
								inner join tb_inst_cargo_ica ica on iac.fk_cod_inst_cargo_ica = ica.pk_cod_inst_cargo_ica
								inner join tb_inst_unid_ex_iue iue on iau.fk_cod_inst_unid_ex_iue = iue.pk_cod_inst_unid_ex_iue
								inner join tb_inst_area_iar iar on iau.fk_cod_inst_area_iar = iar.pk_cod_inst_area_iar
								inner join tb_unidade_exerc_uex uex on iue.fk_cod_unidade_exerc_uex = uex.cod_unidade_exercicio_uex
								inner join tb_area_are are on iar.fk_cod_area_are = are.pk_cod_area_are
								inner join tb_cargo_cgo cgo on ica.fk_cod_cargo_cgo = cgo.pk_cod_cargo_cgo
								inner join tb_funcao_fun fun on ifu.fk_cod_funcao_fun = fun.pk_cod_funcao_fun
								inner join tb_atividade_ati ati on iat.fk_cod_atividade_ati = ati.pk_cod_atividade_ati
								inner join tb_caso_uso_cau cau on icu.fk_cod_caso_usu = cau.pk_cod_caso_usu
								inner join tb_instituicao_ins ins on ifu.fk_cod_instituicao_ins = ins.pk_cod_instituicao_ins
								where pes.pk_cod_pessoa_pes = @id
								order by pfi.txt_nome_pfi";			
			select = select.Replace("@id", "" + idPessoa + "");
			return Consultar(select); //_db.ExecutarConsulta(select).Tables[0];	
		}	
	}
}
