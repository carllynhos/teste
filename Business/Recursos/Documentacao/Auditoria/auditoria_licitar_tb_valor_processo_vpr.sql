DROP TRIGGER IF EXISTS tr_aiud_auditoria_valor_processo_vpr ON adm_licitar.tb_valor_processo_vpr;
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr();

CREATE OR REPLACE FUNCTION adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr()
  RETURNS trigger AS
$BODY$
	BEGIN
		IF (TG_OP = 'DELETE') THEN 
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'D',
			txt_tabela_asi = 'tb_valor_processo_vpr',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),
			txt_registro_asi = 
			'Id Valor Processo: ' ||
				coalesce(OLD.pk_cod_valor_processo_vpr, 0) ||
			' | Número SPU: ' ||
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') || 
			' | Valor: ' ||
				coalesce(OLD.vlr_processo_vpr, 0.000) ||
			' | Tipo de Valor: ' || 
				coalesce((select txt_descricao_tva from adm_licitar.tb_tipo_valor_tva tva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_tipo_valor_tva = tva.pk_cod_tipo_valor_tva)
				where pk_cod_valor_processo_vpr = OLD.pk_cod_valor_processo_vpr), '') ||
			' | Moeda ' ||
				coalesce((select txt_descricao_moe from adm_licitar.tb_moeda_moe moe 
				inner join adm_licitar.tb_valor_processo_vpr vpr on (moe.pk_cod_moeda_moe = vpr.fk_cod_moeda_moe)
				where pk_cod_valor_processo_vpr = OLD.pk_cod_valor_processo_vpr), '') ||
			' | Fonte do Valor: ' ||
				coalesce((select txt_descricao_fva from adm_licitar.tb_fonte_valor_fva fva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_fonte_valor_fva = fva.pk_cod_fonte_valor_fva)
				where pk_cod_valor_processo_vpr = OLD.pk_cod_valor_processo_vpr), '') ||
			' | Descrição: ' ||
				coalesce(OLD.txt_descricao_vpr, '') ||
			' | Data do Cadastro: ' || 
				coalesce(OLD.dat_cadastro_vpr, '-infinity') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes
				inner join adm_licitar.tb_valor_processo_vpr vpr on (pes.pk_cod_pessoa_pes = vpr.fk_cod_pessoa_pes)
				where pk_cod_valor_processo_vpr = OLd.pk_cod_valor_processo_vpr), '')

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
						
		    RETURN OLD;          

		ELSIF (TG_OP = 'UPDATE') THEN
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'U',
			txt_tabela_asi = 'tb_valor_processo_vpr',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),
			txt_registro_asi = 
			'Id Valor Processo: ' ||
				coalesce(NEW.pk_cod_valor_processo_vpr, 0) ||
			' | Número SPU: ' ||
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') || 
			' | Valor: ' ||
				coalesce(NEW.vlr_processo_vpr, 0.000) ||
			' | Tipo de Valor: ' || 
				coalesce((select txt_descricao_tva from adm_licitar.tb_tipo_valor_tva tva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_tipo_valor_tva = tva.pk_cod_tipo_valor_tva)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), '') ||
			' | Moeda ' ||
				coalesce((select txt_descricao_moe from adm_licitar.tb_moeda_moe moe 
				inner join adm_licitar.tb_valor_processo_vpr vpr on (moe.pk_cod_moeda_moe = vpr.fk_cod_moeda_moe)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), '') ||
			' | Fonte do Valor: ' ||
				coalesce((select txt_descricao_fva from adm_licitar.tb_fonte_valor_fva fva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_fonte_valor_fva = fva.pk_cod_fonte_valor_fva)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), '') ||
			' | Descrição: ' ||
				coalesce(NEW.txt_descricao_vpr, '') ||
			' | Data do Cadastro: ' || 
				coalesce(NEW.dat_cadastro_vpr, '-infinity') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes
				inner join adm_licitar.tb_valor_processo_vpr vpr on (pes.pk_cod_pessoa_pes = vpr.fk_cod_pessoa_pes)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), ''),

			txt_registro_update_asi = 
			'Id Valor Processo: ' ||
				coalesce(OLD.pk_cod_valor_processo_vpr, 0) ||
			' | Número SPU: ' ||
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') || 
			' | Valor: ' ||
				coalesce(OLD.vlr_processo_vpr, 0.000) ||
			' | Tipo de Valor: ' || 
				coalesce((select txt_descricao_tva from adm_licitar.tb_tipo_valor_tva tva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_tipo_valor_tva = tva.pk_cod_tipo_valor_tva)
				where pk_cod_valor_processo_vpr = OLD.pk_cod_valor_processo_vpr), '') ||
			' | Moeda ' ||
				coalesce((select txt_descricao_moe from adm_licitar.tb_moeda_moe moe 
				inner join adm_licitar.tb_valor_processo_vpr vpr on (moe.pk_cod_moeda_moe = vpr.fk_cod_moeda_moe)
				where pk_cod_valor_processo_vpr = OLD.pk_cod_valor_processo_vpr), '') ||
			' | Fonte do Valor: ' ||
				coalesce((select txt_descricao_fva from adm_licitar.tb_fonte_valor_fva fva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_fonte_valor_fva = fva.pk_cod_fonte_valor_fva)
				where pk_cod_valor_processo_vpr = OLD.pk_cod_valor_processo_vpr), '') ||
			' | Descrição: ' ||
				coalesce(OLD.txt_descricao_vpr, '') ||
			' | Data do Cadastro: ' || 
				coalesce(OLD.dat_cadastro_vpr, '-infinity') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes
				inner join adm_licitar.tb_valor_processo_vpr vpr on (pes.pk_cod_pessoa_pes = vpr.fk_cod_pessoa_pes)
				where pk_cod_valor_processo_vpr = OLd.pk_cod_valor_processo_vpr), '')

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
			
		    RETURN NEW;

		ELSIF (TG_OP = 'INSERT') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'I',
			txt_tabela_asi = 'tb_valor_processo_vpr',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),
			txt_registro_asi = 
			'Id Valor Processo: ' ||
				coalesce(NEW.pk_cod_valor_processo_vpr, 0) ||
			' | Número SPU: ' ||
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') || 
			' | Valor: ' ||
				coalesce(NEW.vlr_processo_vpr, 0.000) ||
			' | Tipo de Valor: ' || 
				coalesce((select txt_descricao_tva from adm_licitar.tb_tipo_valor_tva tva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_tipo_valor_tva = tva.pk_cod_tipo_valor_tva)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), '') ||
			' | Moeda ' ||
				coalesce((select txt_descricao_moe from adm_licitar.tb_moeda_moe moe 
				inner join adm_licitar.tb_valor_processo_vpr vpr on (moe.pk_cod_moeda_moe = vpr.fk_cod_moeda_moe)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), '') ||
			' | Fonte do Valor: ' ||
				coalesce((select txt_descricao_fva from adm_licitar.tb_fonte_valor_fva fva
				inner join adm_licitar.tb_valor_processo_vpr vpr on (vpr.fk_cod_fonte_valor_fva = fva.pk_cod_fonte_valor_fva)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), '') ||
			' | Descrição: ' ||
				coalesce(NEW.txt_descricao_vpr, '') ||
			' | Data do Cadastro: ' || 
				coalesce(NEW.dat_cadastro_vpr, '-infinity') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes
				inner join adm_licitar.tb_valor_processo_vpr vpr on (pes.pk_cod_pessoa_pes = vpr.fk_cod_pessoa_pes)
				where pk_cod_valor_processo_vpr = NEW.pk_cod_valor_processo_vpr), '')

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
		
		    RETURN NEW;
		    
		END IF;
		RETURN NULL; -- result is ignored since this is an AFTER trigger

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr() OWNER TO adm_licitar;

CREATE TRIGGER tr_aiud_auditoria_valor_processo_vpr
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_valor_processo_vpr
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr();
