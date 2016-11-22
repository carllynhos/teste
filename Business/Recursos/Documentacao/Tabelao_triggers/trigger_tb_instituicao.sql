// trigger_tb_instituicao.cs created with MonoDevelop
// User: marcelolima at 14:41 6/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
//
//
//-- Trigger: tg_processo_completo_instituicao_pci on adm_licitar.tb_instituicao_ins
//
//-- DROP TRIGGER tg_processo_completo_instituicao_pci ON adm_licitar.tb_instituicao_ins;
//
//CREATE TRIGGER tg_processo_completo_instituicao_pci
//  AFTER INSERT OR UPDATE OR DELETE
//  ON adm_licitar.tb_instituicao_ins
//  FOR EACH ROW
//  EXECUTE PROCEDURE adm_licitar.fn_processo_completo_instituicao_pci();
//
//
//-- Function: adm_licitar.fn_processo_completo_instituicao_pci()
//
//-- DROP FUNCTION adm_licitar.fn_processo_completo_instituicao_pci();
//
//CREATE OR REPLACE FUNCTION adm_licitar.fn_processo_completo_instituicao_pci()
//  RETURNS trigger AS
//$BODY$
//	BEGIN
//		IF (TG_OP = 'DELETE') THEN 
//		
//			UPDATE adm_licitar.tb_processo_completo_pcm SET
//			cod_instituicao_ins = null, 
//			txt_descricao_ins = null
//			WHERE cod_instituicao_ins = OLD.pk_cod_instituicao_ins;
//						
//			END IF;
//			
//		IF (TG_OP = 'UPDATE') THEN
//			
//			UPDATE adm_licitar.tb_processo_completo_pcm SET
//			txt_descricao_ins = NEW.txt_sigla_ins
//			WHERE cod_instituicao_ins = OLD.pk_cod_instituicao_ins;
//			
//			END IF;		
//				
//		RETURN NULL;
//	END
//$BODY$
//  LANGUAGE 'plpgsql' VOLATILE
//  COST 100;
//ALTER FUNCTION adm_licitar.fn_processo_completo_instituicao_pci() OWNER TO adm_licitar;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_instituicao_pci() TO public;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_instituicao_pci() TO adm_licitar;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_instituicao_pci() TO licitar;
