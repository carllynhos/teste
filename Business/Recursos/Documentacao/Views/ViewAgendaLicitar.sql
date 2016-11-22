CREATE OR REPLACE VIEW adm_licitar.vw_agenda_transmissao AS
select 
pan.fk_cod_processo_pro as pk_cod_processo_pro ,
pan.pk_cod_processo_andamento_pan,
txt_descricao_nat as natureza,
txt_descricao_mod as modalidade,
txt_descricao_tli as tipolicitacao,
(select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr 
inner join adm_licitar.tb_tipo_numero_tnu tnu on tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu 
where txt_descricao_tnu = 'SPU' and npr.fk_cod_processo_pro = pan.fk_cod_processo_pro) AS numero_spu,
(select txt_numero_processo_npr from adm_licitar.tb_numero_processo_npr npr 
inner join adm_licitar.tb_tipo_numero_tnu tnu on tnu.pk_cod_tipo_numero_tnu = npr.fk_cod_tipo_numero_tnu 
where txt_descricao_tnu = 'LICITAÇÃO' and npr.fk_cod_processo_pro = pan.fk_cod_processo_pro) AS numero_pregao,
txt_sigla_ins as instituicao,
txt_resumo_objeto_pro,
fk_cod_auditorio_aud,
txt_descricao_aud as auditorio,
dat_cadastro_pan,
dat_inicio_age as dat_inicio,
dat_fim_age as dat_termino,
dat_prorrogacao_age as dat_prorrogacao,
(case when boo_ativo_age = false then 0 else 1 end) as cancelada,
txt_observacao_age as obs_cancelada
from 
adm_licitar.tb_agenda_age age
inner join adm_licitar.tb_processo_andamento_pan pan on pan.pk_cod_processo_andamento_pan = age.fk_cod_processo_andamento_pan
inner join adm_licitar.tb_processo_pro pro on pro.pk_cod_processo_pro = pan.fk_cod_processo_pro
inner join adm_licitar.tb_classificacao_cla cla on cla.pk_cod_classificacao_cla = pro.fk_cod_classificacao_cla
inner join adm_licitar.tb_modalidade_mod mod on mod.pk_cod_modalidade_mod = cla.fk_cod_modalidade_mod
inner join adm_licitar.tb_natureza_nat nat on nat.pk_cod_natureza_nat = cla.fk_cod_natureza_nat
inner join adm_licitar.tb_tipo_licitacao_tli tli on tli.pk_cod_tipo_licitacao_tli = cla.fk_cod_tipo_licitacao_tli 
inner join adm_licitar.tb_instituicao_ins ins on ins.pk_cod_instituicao_ins = pro.fk_cod_instituicao_ins
inner join adm_licitar.tb_auditorio_aud aud on aud.pk_cod_auditorio_aud = age.fk_cod_auditorio_aud