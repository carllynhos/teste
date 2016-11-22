// SrvTipoAndamento.cs created with MonoDevelop
// User: guilhermefacanha at 15:05 20/5/2009
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
	
	
	public class SrvTipoAndamento
	{

		public DataTable listarAndamentosPorFase(string fase)
		{
			string select = @"
			SELECT distinct ati.txt_descricao_ati as descricao, ati.pk_cod_atividade_ati as id
			FROM adm_licitar.tb_fluxo_andamento_fan fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			WHERE fan.fk_cod_fase_fas = @fase";
			
			select = select.Replace("@fase", fase);
		
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
		}
		
		public DataTable listarAndamentosDigitalizacaoTrue()
		{
			string select = @"
			SELECT distinct ati.txt_descricao_ati as descricao, ati.pk_cod_atividade_ati as id
			FROM adm_licitar.tb_atividade_ati ati
			WHERE ati.boo_andamento_digitalizacao_ati = true
			order by descricao";
			
								
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
		}
		
		public SrvTipoAndamento()
		{
		}
	}
}
