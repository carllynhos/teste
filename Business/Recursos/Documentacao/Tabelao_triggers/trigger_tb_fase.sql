// trigger_tb_fase.cs created with MonoDevelop
// User: marcelolima at 14:39 6/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

//ALTERAR PARA FASE ESSA TRIGGER !!
//
//-- Trigger: tg_processo_completo_fase_pcf on adm_licitar.tb_fase_fas
//
//-- DROP TRIGGER tg_processo_completo_fase_pcf ON adm_licitar.tb_fase_fas;
//
//CREATE TRIGGER tg_processo_completo_fase_pcf
//  AFTER INSERT OR UPDATE OR DELETE
//  ON adm_licitar.tb_fase_fas
//  FOR EACH ROW
//  EXECUTE PROCEDURE adm_licitar.fn_processo_completo_papel_pcp();
//
//-- Function: adm_licitar.fn_processo_completo_papel_pcp()
//
//-- DROP FUNCTION adm_licitar.fn_processo_completo_papel_pcp();
//
//CREATE OR REPLACE FUNCTION adm_licitar.fn_processo_completo_papel_pcp()
//  RETURNS trigger AS
//$BODY$
//	BEGIN
//		IF (TG_OP = 'DELETE') THEN 
//		
//			UPDATE adm_licitar.tb_processo_completo_pcm SET
//			cod_papel_pap = null, 
//			txt_descricao_pap = null
//			WHERE cod_papel_pap = OLD.pk_cod_papel_pap;
//						
//			END IF;
//			
//		IF (TG_OP = 'UPDATE') THEN
//			
//			UPDATE adm_licitar.tb_processo_completo_pcm SET
//			txt_descricao_pap = NEW.txt_descricao_pap
//			WHERE cod_papel_pap = OLD.pk_cod_papel_pap;
//			
//			END IF;		
//				
//		RETURN NULL;
//	END
//$BODY$
//  LANGUAGE 'plpgsql' VOLATILE
//  COST 100;
//ALTER FUNCTION adm_licitar.fn_processo_completo_papel_pcp() OWNER TO adm_licitar;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_papel_pcp() TO public;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_papel_pcp() TO adm_licitar;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_papel_pcp() TO licitar;
