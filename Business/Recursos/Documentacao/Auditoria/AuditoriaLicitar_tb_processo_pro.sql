DROP TRIGGER IF EXISTS tr_aiud_auditoria_processo_pro ON adm_licitar.tb_processo_pro;
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_processo_pro();

CREATE OR REPLACE FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_pro()
  RETURNS trigger AS
$BODY$
	BEGIN  
		IF (TG_OP = 'DELETE') THEN 

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET 
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'D',
			txt_tabela_asi = 'tb_processo_pro',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),

			txt_registro_asi =  
				'Id Processo: ' ||
					coalesce(OLD.pk_cod_processo_pro, 0) ||
				' | Instituicao: ' ||
					coalesce((select txt_descricao_ins from adm_licitar.tb_instituicao_ins ins 
					inner join adm_licitar.tb_processo_pro pro on (ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||
				' | Número SPU: ' || 
					coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') ||
				' | Natureza: ' || 
					coalesce((select txt_descricao_nat from adm_licitar.tb_natureza_nat nat
					inner join adm_licitar.tb_classificacao_cla cla on (nat.pk_cod_natureza_nat = cla.fk_cod_natureza_nat)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||			
				' | Modalidade: ' || 
					coalesce((select txt_descricao_mod from adm_licitar.tb_modalidade_mod mod
					inner join adm_licitar.tb_classificacao_cla cla on (mod.pk_cod_modalidade_mod = cla.fk_cod_modalidade_mod)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||				
				' | Tipo de Licitação: ' ||
					coalesce((select txt_descricao_tli from adm_licitar.tb_tipo_licitacao_tli tli
					inner join adm_licitar.tb_classificacao_cla cla on (tli.pk_cod_tipo_licitacao_tli = cla.fk_cod_tipo_licitacao_tli)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||
				' | Finalizado: ' || 
					coalesce(OLD.boo_finalizado_pro, false) ||
				' | Data de Início: ' || 
					coalesce(OLD.dat_inicio_pro, '-infinity') ||
				' | Data Fim: ' ||
					coalesce(OLD.dat_fim_pro, '-infinity') ||
				' | Data do Último Andamento: ' ||
					coalesce(OLD.dat_ultimo_andamento_pro, '-infinity') ||
				' | Resumo do Objeto: ' ||
					coalesce(OLD.txt_resumo_objeto_pro, '') || 
				' | Observacao: ' ||
					coalesce(OLD.txt_observacao_pro, '')
					
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;
						
		    RETURN OLD;          

		ELSIF (TG_OP = 'UPDATE') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'U',
			txt_tabela_asi = 'tb_processo_pro',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),

			txt_registro_asi = 
				'Id Processo: ' ||
					coalesce(NEW.pk_cod_processo_pro, 0) ||
				' | Instituicao: ' ||
					coalesce((select txt_descricao_ins from adm_licitar.tb_instituicao_ins ins 
					inner join adm_licitar.tb_processo_pro pro on (ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||
				' | Número SPU: ' || 
					coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') ||
				' | Natureza: ' || 
					coalesce((select txt_descricao_nat from adm_licitar.tb_natureza_nat nat
					inner join adm_licitar.tb_classificacao_cla cla on (nat.pk_cod_natureza_nat = cla.fk_cod_natureza_nat)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||			
				' | Modalidade: ' || 
					coalesce((select txt_descricao_mod from adm_licitar.tb_modalidade_mod mod
					inner join adm_licitar.tb_classificacao_cla cla on (mod.pk_cod_modalidade_mod = cla.fk_cod_modalidade_mod)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||				
				' | Tipo de Licitação: ' ||
					coalesce((select txt_descricao_tli from adm_licitar.tb_tipo_licitacao_tli tli
					inner join adm_licitar.tb_classificacao_cla cla on (tli.pk_cod_tipo_licitacao_tli = cla.fk_cod_tipo_licitacao_tli)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||
				' | Finalizado: ' || 
					coalesce(NEW.boo_finalizado_pro, false) ||
				' | Data de Início: ' || 
					coalesce(NEW.dat_inicio_pro, '-infinity') ||
				' | Data Fim: ' ||
					coalesce(NEW.dat_fim_pro, '-infinity') ||
				' | Data do Último Andamento: ' ||
					coalesce(NEW.dat_ultimo_andamento_pro, '-infinity') ||
				' | Resumo do Objeto: ' ||
					coalesce(NEW.txt_resumo_objeto_pro, '') ||
				' | Observacao: ' ||
					coalesce(NEW.txt_observacao_pro, ''),
			
			txt_registro_update_asi = 
				'Id Processo: ' ||
					coalesce(OLD.pk_cod_processo_pro, 0) ||
				' | Instituicao: ' ||
					coalesce((select txt_descricao_ins from adm_licitar.tb_instituicao_ins ins 
					inner join adm_licitar.tb_processo_pro pro on (ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||
				' | Número SPU: ' || 
					coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') ||
				' | Natureza: ' || 
					coalesce((select txt_descricao_nat from adm_licitar.tb_natureza_nat nat
					inner join adm_licitar.tb_classificacao_cla cla on (nat.pk_cod_natureza_nat = cla.fk_cod_natureza_nat)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||			
				' | Modalidade: ' || 
					coalesce((select txt_descricao_mod from adm_licitar.tb_modalidade_mod mod
					inner join adm_licitar.tb_classificacao_cla cla on (mod.pk_cod_modalidade_mod = cla.fk_cod_modalidade_mod)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||				
				' | Tipo de Licitação: ' ||
					coalesce((select txt_descricao_tli from adm_licitar.tb_tipo_licitacao_tli tli
					inner join adm_licitar.tb_classificacao_cla cla on (tli.pk_cod_tipo_licitacao_tli = cla.fk_cod_tipo_licitacao_tli)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = OLD.pk_cod_processo_pro), '') ||
				' | Finalizado: ' || 
					coalesce(OLD.boo_finalizado_pro, false) ||
				' | Data de Início: ' || 
					coalesce(OLD.dat_inicio_pro, '-infinity') ||
				' | Data Fim: ' ||
					coalesce(OLD.dat_fim_pro, '-infinity') ||
				' | Data do Último Andamento: ' ||
					coalesce(OLD.dat_ultimo_andamento_pro, '-infinity') ||
				' | Resumo do Objeto: ' ||
					coalesce(OLD.txt_resumo_objeto_pro, '') ||
				' | Observacao: ' ||
					coalesce(OLD.txt_observacao_pro, '')			
				
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;
			
		    RETURN NEW;

		ELSIF (TG_OP = 'INSERT') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'I',
			txt_tabela_asi = 'tb_processo_pro',
			txt_num_spu_asi = 
				coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
				inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
				inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
				where pk_cod_processo_pro = NEW.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),

			txt_registro_asi = 
				'Id Processo: ' ||
					coalesce(NEW.pk_cod_processo_pro, 0) ||
				' | Instituicao: ' ||
					coalesce((select txt_descricao_ins from adm_licitar.tb_instituicao_ins ins 
					inner join adm_licitar.tb_processo_pro pro on (ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||
				' | Número SPU: ' || 
					coalesce((select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr
					inner join adm_licitar.tb_tipo_numero_tnu tnu on (npr.fk_cod_tipo_numero_tnu = tnu.pk_cod_tipo_numero_tnu)
					inner join adm_licitar.tb_processo_pro pro on (pro.pk_cod_processo_pro = npr.fk_cod_processo_pro)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), '') ||
				' | Natureza: ' || 
					coalesce((select txt_descricao_nat from adm_licitar.tb_natureza_nat nat
					inner join adm_licitar.tb_classificacao_cla cla on (nat.pk_cod_natureza_nat = cla.fk_cod_natureza_nat)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||			
				' | Modalidade: ' || 
					coalesce((select txt_descricao_mod from adm_licitar.tb_modalidade_mod mod
					inner join adm_licitar.tb_classificacao_cla cla on (mod.pk_cod_modalidade_mod = cla.fk_cod_modalidade_mod)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||				
				' | Tipo de Licitação: ' ||
					coalesce((select txt_descricao_tli from adm_licitar.tb_tipo_licitacao_tli tli
					inner join adm_licitar.tb_classificacao_cla cla on (tli.pk_cod_tipo_licitacao_tli = cla.fk_cod_tipo_licitacao_tli)
					inner join adm_licitar.tb_processo_pro pro on (cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla)
					where pk_cod_processo_pro = NEW.pk_cod_processo_pro), '') ||
				' | Finalizado: ' || 
					coalesce(NEW.boo_finalizado_pro, false) ||
				' | Data de Início: ' || 
					coalesce(NEW.dat_inicio_pro, '-infinity') ||
				' | Data Fim: ' ||
					coalesce(NEW.dat_fim_pro, '-infinity') ||
				' | Data do Último Andamento: ' ||
					coalesce(NEW.dat_ultimo_andamento_pro, '-infinity') ||
				' | Resumo do Objeto: ' ||
					coalesce(NEW.txt_resumo_objeto_pro, '') ||
				' | Observacao: ' ||
					coalesce(NEW.txt_observacao_pro, '')

			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and 
			txt_tipo_acao_asi is null;			
		
		    RETURN NEW;
		    
		END IF;
		RETURN NULL; -- result is ignored since this is an AFTER trigger

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_pro() OWNER TO adm_licitar;

CREATE TRIGGER tr_aiud_auditoria_processo_pro
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_processo_pro
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_processo_pro();
