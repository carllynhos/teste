------------------------------------------------------------------------------------------------------------------------------------------------
-- CRIADO POR: Danilo Meireles 
-- DATA DA CRIACAO: 12/02/2008
-- DESCRICAO: Script para criacao de todas as funcoes e gatilhos de auditoria do sistema Licitar
-- ALTERADO POR: 
-- DATA DA ALTERACAO: 
-- MOTIVO DA ALTERACAO:
-- OBSERVACOES:
------------------------------------------------------------------------------------------------------------------------------------------------


-- DROPA TODAS AS TRIGGERS:
DROP TRIGGER IF EXISTS tr_aiud_auditoria_pessoa_pes ON adm_licitar.tb_pessoa_pes;
DROP TRIGGER IF EXISTS tr_aiud_auditoria_processo_andamento_pan ON adm_licitar.tb_processo_andamento_pan;
DROP TRIGGER IF EXISTS tr_aiud_auditoria_processo_papel_pessoa_ppp ON adm_licitar.tb_processo_papel_pessoa_ppp;
DROP TRIGGER IF EXISTS tr_aiud_auditoria_processo_pro ON adm_licitar.tb_processo_pro;
DROP TRIGGER IF EXISTS tr_aiud_auditoria_valor_processo_vpr ON adm_licitar.tb_valor_processo_vpr;
DROP TRIGGER IF EXISTS tr_aiud_auditoria_numero_processo_anp ON adm_licitar.tb_numero_processo_npr;


-- DROPA TODAS AS FUNCOES:
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_pessoa_pes();
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan();
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp();
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_processo_pro();
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr();
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp();


-- CRIA TODAS AS FUNCOES:
CREATE OR REPLACE FUNCTION adm_licitar.fn_tr_aiud_auditoria_pessoa_pes()
  RETURNS trigger AS
$BODY$
	BEGIN   
		IF (TG_OP = 'DELETE') THEN  
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'D',
			txt_tabela_asi = 'tb_pessoa_pes',
				
			txt_registro_asi = 
				'Id Pessoa: ' ||
					coalesce(OLD.pk_cod_pessoa_pes, 0) ||
				' | Função: ' ||
					coalesce((select txt_descricao_fun from adm_licitar.tb_funcao_fun fun
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (fun.pk_cod_funcao_fun = uef.fk_cod_funcao_fun)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
				' | Unidade de Exercício: ' ||
					coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
				' | Pessoa Fisica ' ||
					coalesce(OLD.boo_pessoa_fisica_pes, false) ||
				' | CPF/CNPJ: ' || 
					coalesce(OLD.txt_cpf_cnpj_pes, '') ||
				' | Nome: ' ||
					coalesce(OLD.txt_nome_pes, '') ||
				' | E-mail: ' || 
					coalesce(OLD.txt_email_pes, '') ||
				' | Endereço: ' || 
					coalesce(OLD.txt_endereco_pes, '') ||
				' | Bairro: ' || 
					coalesce(OLD.txt_bairro_pes, '') ||
				' | Cidade: ' || 
					coalesce(OLD.txt_cidade_pes, '') ||
				' | UF: ' ||
					coalesce(OLD.txt_uf_pes, '') ||
				' | Cep: ' ||
					coalesce(OLD.txt_cep_pes, '') ||
				' | Fax: ' ||
					coalesce(OLD.txt_fax_pes, '') ||
				' | Telefone: ' ||
					coalesce(OLD.txt_telefone_pes, '') ||
				' | Data do Ultimo Login: ' ||
					coalesce(OLD.dat_ultimo_login_pes, '-infinity') ||
				' | Data de Nascimento: ' ||
					coalesce(OLD.dat_nascimento_pes, '-infinity') ||
				' | Licitante: ' ||
					coalesce(OLD.boo_licitante_pes, false) 	
		
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
						
		    RETURN OLD;          

		ELSIF (TG_OP = 'UPDATE') THEN
			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'U',
			txt_tabela_asi = 'tb_pessoa_pes',			
			txt_registro_asi = 'Id Pessoa: ' ||
					coalesce(NEW.pk_cod_pessoa_pes, 0) ||
				' | Função: ' ||
					coalesce((select txt_descricao_fun from adm_licitar.tb_funcao_fun fun
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (fun.pk_cod_funcao_fun = uef.fk_cod_funcao_fun)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = NEW.pk_cod_pessoa_pes), '') ||
				' | Unidade de Exercício: ' ||
					coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = NEW.pk_cod_pessoa_pes), '') ||
				' | Pessoa Fisica ' ||
					coalesce(NEW.boo_pessoa_fisica_pes, false) ||
				' | CPF/CNPJ: ' || 
					coalesce(NEW.txt_cpf_cnpj_pes, '') ||
				' | Nome: ' ||
					coalesce(NEW.txt_nome_pes, '') ||
				' | E-mail: ' || 
					coalesce(NEW.txt_email_pes, '') ||
				' | Endereço: ' || 
					coalesce(NEW.txt_endereco_pes, '') ||
				' | Bairro: ' || 
					coalesce(NEW.txt_bairro_pes, '') ||
				' | Cidade: ' || 
					coalesce(NEW.txt_cidade_pes, '') ||
				' | UF: ' ||
					coalesce(NEW.txt_uf_pes, '') ||
				' | Cep: ' ||
					coalesce(NEW.txt_cep_pes, '') ||
				' | Fax: ' ||
					coalesce(NEW.txt_fax_pes, '') ||
				' | Telefone: ' ||
					coalesce(NEW.txt_telefone_pes, '') ||
				' | Data do Ultimo Login: ' ||
					coalesce(NEW.dat_ultimo_login_pes, '-infinity') ||
				' | Data de Nascimento: ' ||
					coalesce(NEW.dat_nascimento_pes, '-infinity') ||
				' | Licitante: ' ||
					coalesce(NEW.boo_licitante_pes, false),
			txt_registro_update_asi = 				
				'Id Pessoa: ' ||
					coalesce(OLD.pk_cod_pessoa_pes, 0) ||
				' | Função: ' ||
					coalesce((select txt_descricao_fun from adm_licitar.tb_funcao_fun fun
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (fun.pk_cod_funcao_fun = uef.fk_cod_funcao_fun)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
				' | Unidade de Exercício: ' ||
					coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
				' | Pessoa Fisica ' ||
					coalesce(OLD.boo_pessoa_fisica_pes, false) ||
				' | CPF/CNPJ: ' || 
					coalesce(OLD.txt_cpf_cnpj_pes, '') ||
				' | Nome: ' ||
					coalesce(OLD.txt_nome_pes, '') ||
				' | E-mail: ' || 
					coalesce(OLD.txt_email_pes, '') ||
				' | Endereço: ' || 
					coalesce(OLD.txt_endereco_pes, '') ||
				' | Bairro: ' || 
					coalesce(OLD.txt_bairro_pes, '') ||
				' | Cidade: ' || 
					coalesce(OLD.txt_cidade_pes, '') ||
				' | UF: ' ||
					coalesce(OLD.txt_uf_pes, '') ||
				' | Cep: ' ||
					coalesce(OLD.txt_cep_pes, '') ||
				' | Fax: ' ||
					coalesce(OLD.txt_fax_pes, '') ||
				' | Telefone: ' ||
					coalesce(OLD.txt_telefone_pes, '') ||
				' | Data do Ultimo Login: ' ||
					coalesce(OLD.dat_ultimo_login_pes, '-infinity') ||
				' | Data de Nascimento: ' ||
					coalesce(OLD.dat_nascimento_pes, '-infinity') ||
				' | Licitante: ' ||
					coalesce(OLD.boo_licitante_pes, false)
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
			
		    RETURN NEW;

		ELSIF (TG_OP = 'INSERT') THEN

			UPDATE adm_licitar.tb_auditoria_sistema_asi SET
			dat_acao_asi = now(),
			txt_tipo_acao_asi = 'I',
			txt_tabela_asi = 'tb_pessoa_pes',			
			txt_registro_asi = 
				'Id Pessoa: ' ||
					coalesce(NEW.pk_cod_pessoa_pes, 0) ||
				' | Função: ' ||
					coalesce((select txt_descricao_fun from adm_licitar.tb_funcao_fun fun
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (fun.pk_cod_funcao_fun = uef.fk_cod_funcao_fun)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = NEW.pk_cod_pessoa_pes), '') ||
				' | Unidade de Exercício: ' ||
					coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = NEW.pk_cod_pessoa_pes), '') ||
				' | Pessoa Fisica ' ||
					coalesce(NEW.boo_pessoa_fisica_pes, false) ||
				' | CPF/CNPJ: ' || 
					coalesce(NEW.txt_cpf_cnpj_pes, '') ||
				' | Nome: ' ||
					coalesce(NEW.txt_nome_pes, '') ||
				' | E-mail: ' || 
					coalesce(NEW.txt_email_pes, '') ||
				' | Endereço: ' || 
					coalesce(NEW.txt_endereco_pes, '') ||
				' | Bairro: ' || 
					coalesce(NEW.txt_bairro_pes, '') ||
				' | Cidade: ' || 
					coalesce(NEW.txt_cidade_pes, '') ||
				' | UF: ' ||
					coalesce(NEW.txt_uf_pes, '') ||
				' | Cep: ' ||
					coalesce(NEW.txt_cep_pes, '') ||
				' | Fax: ' ||
					coalesce(NEW.txt_fax_pes, '') ||
				' | Telefone: ' ||
					coalesce(NEW.txt_telefone_pes, '') ||
				' | Data do Ultimo Login: ' ||
					coalesce(NEW.dat_ultimo_login_pes, '-infinity') ||
				' | Data de Nascimento: ' ||
					coalesce(NEW.dat_nascimento_pes, '-infinity') ||
				' | Licitante: ' || 
					coalesce(NEW.boo_licitante_pes, false)
			where pk_cod_auditoria_sistema_asi in (select max(pk_cod_auditoria_sistema_asi) from adm_licitar.tb_auditoria_sistema_asi) and
			txt_tipo_acao_asi is null;	
		
		    RETURN NEW;
		    
		END IF;
		RETURN NULL; 
	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_pessoa_pes() OWNER TO adm_licitar;


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
		RETURN NULL; 

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan() OWNER TO adm_licitar;


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
		RETURN NULL; 

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp() OWNER TO adm_licitar;


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
				where pk_cod_processo_pro = OLD.pk_cod_processo_pro and txt_descricao_tnu = 'SPU'), ''),

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
		RETURN NULL; 

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_pro() OWNER TO adm_licitar;


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
		RETURN NULL; 

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr() OWNER TO adm_licitar;


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
		RETURN NULL; 

	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp() OWNER TO adm_licitar;


-- CRIA TODAS AS TRIGGERS:
CREATE TRIGGER tr_aiud_auditoria_pessoa_pes
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_pessoa_pes
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_pessoa_pes();

CREATE TRIGGER tr_aiud_auditoria_processo_andamento_pan
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_processo_andamento_pan
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan();

CREATE TRIGGER tr_aiud_auditoria_processo_papel_pessoa_ppp
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_processo_papel_pessoa_ppp
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp();

CREATE TRIGGER tr_aiud_auditoria_processo_pro
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_processo_pro
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_processo_pro();

CREATE TRIGGER tr_aiud_auditoria_valor_processo_vpr
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_valor_processo_vpr
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr();

CREATE TRIGGER tr_aiud_auditoria_numero_processo_anp
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_numero_processo_npr
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp();


-- APLICA AS PERMISSOES: 
GRANT EXECUTE ON FUNCTION adm_licitar.fn_tr_aiud_auditoria_pessoa_pes() TO licitar;
GRANT EXECUTE ON FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_andamento_pan() TO licitar;
GRANT EXECUTE ON FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_papel_pessoa_ppp() TO licitar;
GRANT EXECUTE ON FUNCTION adm_licitar.fn_tr_aiud_auditoria_processo_pro() TO licitar;
GRANT EXECUTE ON FUNCTION adm_licitar.fn_tr_aiud_auditoria_valor_processo_vpr() TO licitar;
GRANT EXECUTE ON FUNCTION adm_licitar.fn_tr_aiud_auditoria_numero_processo_anp() TO licitar;





