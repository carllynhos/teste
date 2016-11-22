// SrvModalidade.cs created with MonoDevelop
// User: guilhermefacanha at 11:12 3/4/2009
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
	
	
	public class SrvModalidade
	{
		public Modalidade[] listarModalidades()
		{
			return Modalidade.FindAll(Order.Asc("Descricao"));
		}

		public DataTable listarModalidadesPorFaseAndamento(string fase, string andamento)
		{
			string select = @"
			SELECT distinct mod.txt_descricao_mod as descricao, mod.pk_cod_modalidade_mod as id
			FROM adm_licitar.tb_fluxo_andamento_fan fan
			INNER JOIN adm_licitar.tb_workflow_modalidade_unidade_exercicio_wmu wmu ON wmu.fk_cod_workflow_wor = fan.fk_cod_workflow_wor
			INNER JOIN adm_licitar.tb_modalidade_mod mod ON mod.pk_cod_modalidade_mod = wmu.fk_cod_modalidade_mod
			WHERE fan.fk_cod_fase_fas = @fase
			AND fan.fk_cod_atividade_ati = @andamento";
			
			select = select.Replace("@fase", fase);
			select = select.Replace("@andamento", andamento);
		
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
		}
		
		public SrvModalidade()
		{
		}
	}
}
