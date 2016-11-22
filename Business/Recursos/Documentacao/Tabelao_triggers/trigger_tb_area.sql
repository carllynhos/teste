// trigger_tb_area.cs created with MonoDevelop
// User: marcelolima at 14:36 6/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
//
//-- Trigger: tg_processo_completo_area_pca on adm_licitar.tb_area_are
//
//-- DROP TRIGGER tg_processo_completo_area_pca ON adm_licitar.tb_area_are;
//
//CREATE TRIGGER tg_processo_completo_area_pca
//  AFTER INSERT OR UPDATE OR DELETE
//  ON adm_licitar.tb_area_are
//  FOR EACH ROW
//  EXECUTE PROCEDURE adm_licitar.fn_processo_completo_area_pca();
//
//-- Function: adm_licitar.fn_processo_completo_area_pca()
//
//-- DROP FUNCTION adm_licitar.fn_processo_completo_area_pca();
//
//CREATE OR REPLACE FUNCTION adm_licitar.fn_processo_completo_area_pca()
//  RETURNS trigger AS
//$BODY$
//	BEGIN
//		IF (TG_OP = 'DELETE') THEN 
//		
//			UPDATE adm_licitar.tb_processo_completo_pcm SET
//			pk_cod_area_are = null, 
//			txt_descricao_are = null
//			WHERE cod_area_are = OLD.pk_cod_area_are;
//						
//			END IF;
//			
//		IF (TG_OP = 'UPDATE') THEN
//			
//			UPDATE adm_licitar.tb_processo_completo_pcm SET
//			txt_descricao_are = NEW.txt_descricao_are
//			WHERE cod_area_are = OLD.pk_cod_area_are;
//			
//			END IF;		
//				
//		RETURN NULL;
//	END
//$BODY$
//  LANGUAGE 'plpgsql' VOLATILE
//  COST 100;
//ALTER FUNCTION adm_licitar.fn_processo_completo_area_pca() OWNER TO adm_licitar;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_area_pca() TO public;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_area_pca() TO adm_licitar;
//GRANT EXECUTE ON FUNCTION adm_licitar.fn_processo_completo_area_pca() TO licitar;
//
