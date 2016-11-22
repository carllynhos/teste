DROP TRIGGER IF EXISTS tr_aiud_auditoria_pessoa_pes ON adm_licitar.tb_pessoa_pes;
DROP FUNCTION IF EXISTS adm_licitar.fn_tr_aiud_auditoria_pessoa_pes();

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
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
				' | Unidade de Exercício: ' ||
					coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
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
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
				' | Unidade de Exercício: ' ||
					coalesce((select txt_descricao_uex from adm_licitar.tb_unidade_exercicio_uex uex
					inner join adm_licitar.tb_unidade_exercicio_funcao_uef uef on (uex.pk_cod_unidade_exercicio_uex = uef.fk_cod_unidade_exercicio_uex)
					inner join adm_licitar.tb_pessoa_pes pes on (uef.pk_cod_unidade_exercicio_funcao_uef = pes.fk_cod_unidade_exercicio_funcao_uef)
					where pk_cod_pessoa_pes = OLD.pk_cod_pessoa_pes), '') ||
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
		RETURN NULL; -- result is ignored since this is an AFTER trigger
	END;   
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION adm_licitar.fn_tr_aiud_auditoria_pessoa_pes() OWNER TO adm_licitar;

CREATE TRIGGER tr_aiud_auditoria_pessoa_pes
AFTER INSERT OR UPDATE OR DELETE
ON adm_licitar.tb_pessoa_pes
FOR EACH ROW
EXECUTE PROCEDURE adm_licitar.fn_tr_aiud_auditoria_pessoa_pes();
