// SrvFraseAndamento.cs created with MonoDevelop
// User: guilhermefacanha at 16:21 20/5/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;

using NHibernate;
using Licitar.Business.Entidade;
using NHibernate.Expression;

using Licitar.Business.Dto;
using Licitar.Business.Dao;
using System.Data;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Servico
{
	
	
	public class SrvFraseAndamento
	{
		public bool existeFraseAndamentoPorFluxo(string fluxo, string frase)
		{
			string select = @"
			SELECT pk_cod_frase_fluxo_andamento_ffa
			FROM adm_licitar.tb_frase_fluxo_andamento_ffa
			WHERE fk_cod_fluxo_andamento_fan = @fluxo
			AND fk_cod_frase_fra = @frase";
			
			select = select.Replace("@fluxo", fluxo);
			select = select.Replace("@frase", frase);		
		
			DataTable dt = new PostgreSqlDatabase().Consultar(select);

			if(dt.Rows.Count>0)
				return true;
			else
				return false;
		}

		public DataTable listarFraseAndamento(string fase, string andamento, string modalidade)
		{
			string select = @"
			SELECT ffa.pk_cod_frase_fluxo_andamento_ffa as id, fra.txt_descricao_fra as frase, 
			ati.txt_descricao_ati as andamento, fas.txt_descricao_fas as fase, mod.txt_descricao_mod as modalidade
			FROM adm_licitar.tb_frase_fluxo_andamento_ffa ffa
			INNER JOIN adm_licitar.tb_frase_fra fra ON fra.pk_cod_frase_fra = ffa.fk_cod_frase_fra
			INNER JOIN adm_licitar.tb_fluxo_andamento_fan fan ON fan.pk_cod_fluxo_andamento_fan = ffa.fk_cod_fluxo_andamento_fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_fase_fas fas ON fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
			INNER JOIN adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu ON wmu.fk_cod_workflow_wor = fan.fk_cod_workflow_wor
			INNER JOIN adm_licitar.tb_modalidade_mod mod ON mod.pk_cod_modalidade_mod = wmu.fk_cod_modalidade_mod
			WHERE 0=0 
			@andamento 
			@fase
			@modalidade 
			ORDER BY fas.txt_descricao_fas, ati.txt_descricao_ati, fra.txt_descricao_fra
			";

			if(!string.IsNullOrEmpty(andamento))
				select = select.Replace("@andamento", " AND ati.pk_cod_atividade_ati = "+andamento);
			else
				select = select.Replace("@andamento", "");

			if(!string.IsNullOrEmpty(fase))
				select = select.Replace("@fase", " AND fas.pk_cod_fase_fas = "+fase);
			else
				select = select.Replace("@fase", "");

			if(!string.IsNullOrEmpty(modalidade))
				select = select.Replace("@modalidade", " AND mod.pk_cod_modalidade_mod = "+modalidade);
			else
				select = select.Replace("@modalidade", "");
		
			DataTable dt = new PostgreSqlDatabase().Consultar(select);

			return dt;
		}
		
		public SrvFraseAndamento()
		{
		}
	}
}
