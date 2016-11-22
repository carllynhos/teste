DROP TRIGGER IF EXISTS tr_aiud_auditoria_numero_processo_anp ON adm_licitar.tb_numero_processo_npr;
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp();

CREATE OR REPLACE FUNCTION adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp()
  RETURNS trigger AS
$BODY$
	BEGIN   

		IF (TG_OP = 'DELETE') THEN 
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'D',
			txt_tabela_asi = 'tb_numero_processo_npr',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),				
			txt_registro_asi = 
				'Id Número Processo: ' ||
					coalesce(OLD.pk_cod_numero_processo_npr, 0) || 
				' | Tipo de Número: ' ||
					coalesce((select txt_descricao_tnu from adm_licitar.tb_tipo_numero_tnu tnu
					inner join adm_licitar.tb_numero_processo_npr npr on (tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu)
					where pk_cod_numero_processo_npr = OLD.pk_cod_numero_processo_npr), '') || 
				' | Data do Cadastro: ' ||
					coalesce(OLD.dat_cadastro_npr, '-infinity') || 
				' | Número: ' || 
					coalesce(OLD.txt_numero_processo_npr, '') || 
				' | Principal: ' || 
					coalesce(OLD.boo_principal_npr, false) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro
					inner join adm_licitar.tb_numero_processo_npr npr on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_numero_processo_npr = OLD.pk_cod_numero_processo_npr), '')	||
				' | Spu: ' ||
				        coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '')

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
						
		    RETURN OLD;          

		ELSIF (TG_OP = 'UPDATE') THEN
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'U',
			txt_tabela_asi = 'tb_numero_processo_npr',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),				
			txt_registro_asi = 
				'Id Número Processo: ' ||
					coalesce(NEW.pk_cod_numero_processo_npr, 0) || 
				' | Tipo de Número: ' ||
					coalesce((select txt_descricao_tnu from adm_licitar.tb_tipo_numero_tnu tnu
					inner join adm_licitar.tb_numero_processo_npr npr on (tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu)
					where pk_cod_numero_processo_npr = NEW.pk_cod_numero_processo_npr), '') || 
				' | Data do Cadastro: ' ||
					coalesce(NEW.dat_cadastro_npr, '-infinity') || 
				' | Número: ' || 
					coalesce(NEW.txt_numero_processo_npr, '') || 
				' | Principal: ' || 
					coalesce(NEW.boo_principal_npr, false) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro
					inner join adm_licitar.tb_numero_processo_npr npr on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_numero_processo_npr = NEW.pk_cod_numero_processo_npr), '')	||
				' | Spu: ' ||
				        coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),
						
			txt_registro_update_asi = 				
				'Id Número Processo: ' ||
					coalesce(OLD.pk_cod_numero_processo_npr, 0) || 
				' | Tipo de Número: ' ||
					coalesce((select txt_descricao_tnu from adm_licitar.tb_tipo_numero_tnu tnu
					inner join adm_licitar.tb_numero_processo_npr npr on (tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu)
					where pk_cod_numero_processo_npr = OLD.pk_cod_numero_processo_npr), '') || 
				' | Data do Cadastro: ' ||
					coalesce(OLD.dat_cadastro_npr, '-infinity') || 
				' | Número: ' || 
					coalesce(OLD.txt_numero_processo_npr, '') || 
				' | Principal: ' || 
					coalesce(OLD.boo_principal_npr, false) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro
					inner join adm_licitar.tb_numero_processo_npr npr on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_numero_processo_npr = OLD.pk_cod_numero_processo_npr), '')	||
				' | Spu: ' ||
				        coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '')
				

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
			
		    RETURN NEW;

		ELSIF (TG_OP = 'INSERT') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'I',
			txt_tabela_asi = 'tb_numero_processo_npr',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),				
			txt_registro_asi = 
				'Id Número Processo: ' ||
					coalesce(NEW.pk_cod_numero_processo_npr, 0) || 
				' | Tipo de Número: ' ||
					coalesce((select txt_descricao_tnu from adm_licitar.tb_tipo_numero_tnu tnu
					inner join adm_licitar.tb_numero_processo_npr npr on (tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu)
					where pk_cod_numero_processo_npr = NEW.pk_cod_numero_processo_npr), '') || 
				' | Data do Cadastro: ' ||
					coalesce(NEW.dat_cadastro_npr, '-infinity') || 
				' | Número: ' || 
					coalesce(NEW.txt_numero_processo_npr, '') || 
				' | Principal: ' || 
					coalesce(NEW.boo_principal_npr, false) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro
					inner join adm_licitar.tb_numero_processo_npr npr on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_numero_processo_npr = NEW.pk_cod_numero_processo_npr), '')	||
				' | Spu: ' ||
				        coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '')

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
		
		    RETURN NEW;
		    
		END IF;
		RETURN NULL; -- result is ignored since this is an AFTER trigger

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp() OWNER TO adm_licitar;

CREATE TRIGGER tr_aiud_auditoria_numero_processo_anp
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_numero_processo_npr
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp();
