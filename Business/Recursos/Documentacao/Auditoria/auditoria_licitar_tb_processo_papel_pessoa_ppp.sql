DROP TRIGGER IF EXISTS tr_aiud_auditoria_processo_papel_pessoa_ppp ON adm_licitar.tb_processo_papel_pessoa_ppp;
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp();

CREATE OR REPLACE FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp()
  RETURNS trigger AS
$BODY$
	BEGIN   

		IF (TG_OP = 'DELETE') THEN 
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'D',
			txt_tabela_asi = 'tb_processo_papel_pessoa_ppp',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),
				
			txt_registro_asi = 
				'Id Processo Papel Pessoa: ' ||
					coalesce(OLD.pk_cod_processo_papel_pessoa_ppp, 0) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pro.pk_cod_processo_pro = ppp.fk_cod_processo_pro) 
					where pk_cod_processo_papel_pessoa_ppp = OLD.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Papel: ' ||
					coalesce((select txt_descricao_pap from adm_licitar.tb_papel_pap pap 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pap.pk_cod_papel_pap = ppp.fk_cod_papel_pap)
					where pk_cod_processo_papel_pessoa_ppp = OLD.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Pessoa: ' ||
					coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pes.pk_cod_pessoa_pes = ppp.fk_cod_pessoa_pes)
					where pk_cod_processo_papel_pessoa_ppp = OLD.pk_cod_processo_papel_pessoa_ppp), '')				
				

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
						
		    RETURN OLD;          

		ELSIF (TG_OP = 'UPDATE') THEN
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'U',
			txt_tabela_asi = 'tb_processo_papel_pessoa_ppp',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),
			txt_registro_asi = 
				'Id Processo Papel Pessoa: ' ||
					coalesce(NEW.pk_cod_processo_papel_pessoa_ppp, 0) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pro.pk_cod_processo_pro = ppp.fk_cod_processo_pro) 
					where pk_cod_processo_papel_pessoa_ppp = NEW.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Papel: ' ||
					coalesce((select txt_descricao_pap from adm_licitar.tb_papel_pap pap 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pap.pk_cod_papel_pap = ppp.fk_cod_papel_pap)
					where pk_cod_processo_papel_pessoa_ppp = NEW.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Pessoa: ' ||
					coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pes.pk_cod_pessoa_pes = ppp.fk_cod_pessoa_pes)
					where pk_cod_processo_papel_pessoa_ppp = NEW.pk_cod_processo_papel_pessoa_ppp), ''),						
			txt_registro_update_asi = 
				'Id Processo Papel Pessoa: ' ||
					coalesce(OLD.pk_cod_processo_papel_pessoa_ppp, 0) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pro.pk_cod_processo_pro = ppp.fk_cod_processo_pro) 
					where pk_cod_processo_papel_pessoa_ppp = OLD.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Papel: ' ||
					coalesce((select txt_descricao_pap from adm_licitar.tb_papel_pap pap 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pap.pk_cod_papel_pap = ppp.fk_cod_papel_pap)
					where pk_cod_processo_papel_pessoa_ppp = OLD.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Pessoa: ' ||
					coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pes.pk_cod_pessoa_pes = ppp.fk_cod_pessoa_pes)
					where pk_cod_processo_papel_pessoa_ppp = OLD.pk_cod_processo_papel_pessoa_ppp), '')
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
			
		    RETURN NEW;

		ELSIF (TG_OP = 'INSERT') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'I',
			txt_tabela_asi = 'tb_processo_papel_pessoa_ppp',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),		
			txt_registro_asi = 
				'Id Processo Papel Pessoa: ' ||
					coalesce(NEW.pk_cod_processo_papel_pessoa_ppp, 0) || 
				' | Processo: ' || 
					coalesce((select txt_resumo_objeto_pro from adm_licitar.tb_processo_pro pro 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pro.pk_cod_processo_pro = ppp.fk_cod_processo_pro) 
					where pk_cod_processo_papel_pessoa_ppp = NEW.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Papel: ' ||
					coalesce((select txt_descricao_pap from adm_licitar.tb_papel_pap pap 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pap.pk_cod_papel_pap = ppp.fk_cod_papel_pap)
					where pk_cod_processo_papel_pessoa_ppp = NEW.pk_cod_processo_papel_pessoa_ppp), '') ||
				' | Pessoa: ' ||
					coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
					inner join adm_licitar.tb_processo_papel_pessoa_ppp ppp on (pes.pk_cod_pessoa_pes = ppp.fk_cod_pessoa_pes)
					where pk_cod_processo_papel_pessoa_ppp = NEW.pk_cod_processo_papel_pessoa_ppp), '')
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
		
		    RETURN NEW;
		    
		END IF;
		RETURN NULL; -- result is ignored since this is an AFTER trigger

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp() OWNER TO adm_licitar;

CREATE TRIGGER tr_aiud_auditoria_processo_papel_pessoa_ppp
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_processo_papel_pessoa_ppp
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp();
