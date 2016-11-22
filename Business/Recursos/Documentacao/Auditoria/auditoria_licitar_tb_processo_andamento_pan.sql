DROP TRIGGER IF EXISTS tr_aiud_auditoria_processo_andamento_pan ON adm_licitar.tb_processo_andamento_pan;
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan();

CREATE OR REPLACE FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan()
  RETURNS trigger AS
$BODY$
	BEGIN  
		IF (TG_OP = 'DELETE') THEN 

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'D',
			txt_tabela_asi = 'tb_processo_andamento_pan',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU' LIMIT 1), ''),	
			txt_registro_asi = 
			'Id Processo Andamento: ' || 
				coalesce(OLD.pk_cod_processo_andamento_pan, 0) ||
			' | Processo: ' || 
				coalesce((select txt_andamento_pan from adm_licitar.tb_processo_andamento_pan pan 
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = pan.fk_cod_processo_pro)
				where pk_cod_processo_andamento_pan = OLD.pk_cod_processo_andamento_pan LIMIT 1), '') ||
			' | Número SPU: ' || 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU' LIMIT 1), '') || 
			' | Unidade de Exercicio: ' ||
				coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_unidade_exercicio_uex = uex.pk_cod_unidade_exercicio_uex)
				where pan.fk_cod_unidade_exercicio_uex = OLD.fk_cod_unidade_exercicio_uex LIMIT 1), '') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
				inner join adm_licitar.tb_processo_andamento_pan pan on (pes.pk_cod_pessoa_pes = pan.fk_cod_pessoa_pes)
				where fk_cod_pessoa_pes = OLD.fk_cod_pessoa_pes LIMIT 1), '') ||
			' | Data do Andamento: ' || 
				coalesce(OLD.dat_andamento_pan, '-infinity') ||			
			' | Data do Cadastro: ' ||
				coalesce(OLD.dat_cadastro_pan, '-infinity') ||
			' | Data do Término: ' ||
				coalesce(OLD.dat_termino_pan, '-infinity') ||
			' | Descricao do Andamento: ' ||
				coalesce(OLD.txt_andamento_pan, '-infinity') ||
			' | Tipo de Andamento: ' || 
				coalesce((select txt_descricao_ati from adm_licitar.tb_atividade_ati ati
				inner join adm_licitar.tb_fluxo_andamento_fan fan on (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati)
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_fluxo_andamento_fan = fan.pk_cod_fluxo_andamento_fan)
				where fk_cod_fluxo_andamento_fan = OLD.fk_cod_fluxo_andamento_fan LIMIT 1), '')
				
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
						
		    RETURN OLD;          

		ELSIF (TG_OP = 'UPDATE') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'U',
			txt_tabela_asi = 'tb_processo_andamento_pan',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU' LIMIT 1), ''),	

			txt_registro_asi = 
			'Id Processo Andamento: ' || 
				coalesce(NEW.pk_cod_processo_andamento_pan, 0) ||
			' | Processo: ' || 
				coalesce((select txt_andamento_pan from adm_licitar.tb_processo_andamento_pan pan 
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = pan.fk_cod_processo_pro)
				where pk_cod_processo_andamento_pan = NEW.pk_cod_processo_andamento_pan LIMIT 1), '') ||
			' | Número SPU: ' || 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU' LIMIT 1), '')	|| 
			' | Unidade de Exercicio: ' ||
				coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_unidade_exercicio_uex = uex.pk_cod_unidade_exercicio_uex)
				where pan.fk_cod_unidade_exercicio_uex = NEW.fk_cod_unidade_exercicio_uex LIMIT 1), '') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
				inner join adm_licitar.tb_processo_andamento_pan pan on (pes.pk_cod_pessoa_pes = pan.fk_cod_pessoa_pes)
				where fk_cod_pessoa_pes = NEW.fk_cod_pessoa_pes LIMIT 1), '') ||
			' | Data do Andamento: ' || 
				coalesce(NEW.dat_andamento_pan, '-infinity') ||			
			' | Data do Cadastro: ' ||
				coalesce(NEW.dat_cadastro_pan, '-infinity') ||
			' | Data do Término: ' ||
				coalesce(NEW.dat_termino_pan, '-infinity') ||
			' | Descricao do Andamento: ' ||
				coalesce(NEW.txt_andamento_pan, '-infinity') ||
			' | Tipo de Andamento: ' || 
				coalesce((select txt_descricao_ati from adm_licitar.tb_atividade_ati ati
				inner join adm_licitar.tb_fluxo_andamento_fan fan on (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati)
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_fluxo_andamento_fan = fan.pk_cod_fluxo_andamento_fan)
				where fk_cod_fluxo_andamento_fan = NEW.fk_cod_fluxo_andamento_fan LIMIT 1), ''),

			txt_registro_update_asi = 
			'Id Processo Andamento: ' || 
				coalesce(OLD.pk_cod_processo_andamento_pan, 0) ||
			' | Processo: ' || 
				coalesce((select txt_andamento_pan from adm_licitar.tb_processo_andamento_pan pan 
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = pan.fk_cod_processo_pro)
				where pk_cod_processo_andamento_pan = OLD.pk_cod_processo_andamento_pan LIMIT 1), '') ||
			' | Número SPU: ' || 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = OLD.fk_cod_processo_pro and txt_descricao_tnu = 'SPU' LIMIT 1), '')	|| 
			' | Unidade de Exercicio: ' ||
				coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_unidade_exercicio_uex = uex.pk_cod_unidade_exercicio_uex)
				where pan.fk_cod_unidade_exercicio_uex = OLD.fk_cod_unidade_exercicio_uex LIMIT 1), '') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
				inner join adm_licitar.tb_processo_andamento_pan pan on (pes.pk_cod_pessoa_pes = pan.fk_cod_pessoa_pes)
				where fk_cod_pessoa_pes = OLD.fk_cod_pessoa_pes LIMIT 1), '') ||
			' | Data do Andamento: ' || 
				coalesce(OLD.dat_andamento_pan, '-infinity') ||			
			' | Data do Cadastro: ' ||
				coalesce(OLD.dat_cadastro_pan, '-infinity') ||
			' | Data do Término: ' ||
				coalesce(OLD.dat_termino_pan, '-infinity') ||
			' | Descricao do Andamento: ' ||
				coalesce(OLD.txt_andamento_pan, '-infinity') ||
			' | Tipo de Andamento: ' || 
				coalesce((select txt_descricao_ati from adm_licitar.tb_atividade_ati ati
				inner join adm_licitar.tb_fluxo_andamento_fan fan on (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati)
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_fluxo_andamento_fan = fan.pk_cod_fluxo_andamento_fan)
				where fk_cod_fluxo_andamento_fan = OLD.fk_cod_fluxo_andamento_fan LIMIT 1), '')		
				
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;
			
		    RETURN NEW;

		ELSIF (TG_OP = 'INSERT') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'I',
			txt_tabela_asi = 'tb_processo_andamento_pan',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU' LIMIT 1), ''),	
			txt_registro_asi = 
			'Id Processo Andamento: ' || 
				coalesce(NEW.pk_cod_processo_andamento_pan, 0) ||
			' | Processo: ' || 
				coalesce((select txt_andamento_pan from adm_licitar.tb_processo_andamento_pan pan 
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = pan.fk_cod_processo_pro)
				where pk_cod_processo_andamento_pan = NEW.pk_cod_processo_andamento_pan LIMIT 1), '') ||
			' | Número SPU: ' || 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.fk_cod_processo_pro and txt_descricao_tnu = 'SPU' LIMIT 1), '') || 
			' | Unidade de Exercicio: ' ||
				coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_unidade_exercicio_uex = uex.pk_cod_unidade_exercicio_uex)
				where pan.fk_cod_unidade_exercicio_uex = NEW.fk_cod_unidade_exercicio_uex LIMIT 1), '') ||
			' | Pessoa: ' ||
				coalesce((select txt_nome_pes from adm_licitar.tb_pessoa_pes pes 
				inner join adm_licitar.tb_processo_andamento_pan pan on (pes.pk_cod_pessoa_pes = pan.fk_cod_pessoa_pes)
				where fk_cod_pessoa_pes = NEW.fk_cod_pessoa_pes LIMIT 1), '') ||
			' | Data do Andamento: ' || 
				coalesce(NEW.dat_andamento_pan, '-infinity') ||			
			' | Data do Cadastro: ' ||
				coalesce(NEW.dat_cadastro_pan, '-infinity') ||
			' | Data do Término: ' ||
				coalesce(NEW.dat_termino_pan, '-infinity') ||
			' | Descricao do Andamento: ' ||
				coalesce(NEW.txt_andamento_pan, '-infinity') ||
			' | Tipo de Andamento: ' || 
				coalesce((select txt_descricao_ati from adm_licitar.tb_atividade_ati ati
				inner join adm_licitar.tb_fluxo_andamento_fan fan on (ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati)
				inner join adm_licitar.tb_processo_andamento_pan pan on (pan.fk_cod_fluxo_andamento_fan = fan.pk_cod_fluxo_andamento_fan)
				where fk_cod_fluxo_andamento_fan = NEW.fk_cod_fluxo_andamento_fan LIMIT 1), '')
				
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;
		
		    RETURN NEW;
		    
		END IF;
		RETURN NULL; -- result is ignored since this is an AFTER trigger

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan() OWNER TO adm_licitar;

CREATE TRIGGER tr_aiud_auditoria_processo_andamento_pan
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_processo_andamento_pan
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan();
