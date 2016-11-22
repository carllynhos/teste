// SrvFase.cs created with MonoDevelop
// User: guilhermefacanha at 15:25 31/3/2009
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
	
	
	public class SrvFase
	{
		public Fase[] listarFases()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Fase));
			dc.AddOrder(Order.Asc("Descricao"));

			return Fase.FindAll(dc);
		}

		public DataTable listarFasesPorUnidades(List<int> listaUnidades)
		{
			string unidades = "";
			foreach(int i in listaUnidades)
			{
				unidades += i+",";
			}

			if(!string.IsNullOrEmpty(unidades))
			{
				unidades = unidades.Remove(unidades.Length-1);
			}
			else
			{
				return null;
			}	
			
			string select = @"
			SELECT DISTINCT fas.txt_descricao_fas as Descricao, fas.pk_cod_fase_fas as Id
			FROM adm_licitar.tb_fase_unidade_exercicio_fue fue
			INNER JOIN adm_licitar.tb_fase_fas fas ON fas.pk_cod_fase_fas = fue.fk_cod_fase_fas
			WHERE fk_cod_unidade_exercicio_uex IN (@unidades) AND fue.dat_fim_fue = '-infinity' 
			ORDER BY fas.txt_descricao_fas";
			
			select = select.Replace("@unidades", unidades);
		
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
			
		}

		public bool VerificaFaseMarcacaoPorUnidades(List<int> listaUnidades)
		{
			bool retorno = false;
			string unidades = "";
			foreach(int i in listaUnidades)
			{
				unidades += i+",";
			}

			if(!string.IsNullOrEmpty(unidades))
			{
				unidades = unidades.Remove(unidades.Length-1);
			}
			else
			{
				return retorno;
			}	
			
			string select = @"
			SELECT DISTINCT fas.txt_descricao_fas as Descricao, fas.pk_cod_fase_fas as Id
			FROM adm_licitar.tb_fase_unidade_exercicio_fue fue
			INNER JOIN adm_licitar.tb_fase_fas fas ON fas.pk_cod_fase_fas = fue.fk_cod_fase_fas
			WHERE fk_cod_unidade_exercicio_uex IN (@unidades)
			and fas.txt_descricao_fas = 'MARCAÇÃO'
			AND fue.dat_fim_fue = '-infinity'
			ORDER BY fas.txt_descricao_fas";
			
			select = select.Replace("@unidades", unidades);
		
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
            if(dt.Rows.Count >0)
				retorno = true;
			return retorno;
			
		}

		public DataTable listarFasesPorAtividade(string atividade,string workflow)
		{
			string select = @"
			SELECT Distinct fas.pk_cod_fase_fas as id, fas.txt_descricao_fas as descricao
			FROM adm_licitar.tb_fluxo_andamento_fan fan
			INNER JOIN adm_licitar.tb_atividade_ati ati ON ati.pk_cod_atividade_ati = fan.fk_cod_atividade_ati
			INNER JOIN adm_licitar.tb_fase_fas fas ON fas.pk_cod_fase_fas = fan.fk_cod_fase_fas
			WHERE ati.txt_descricao_ati = '@atividade' 
			AND fan.fk_cod_workflow_wor = '@workflow' ";
			
			select = select.Replace("@atividade", atividade);
			select = select.Replace("@workflow", workflow);
		
			DataTable dt = new PostgreSqlDatabase().Consultar(select);
						
			return dt;
			
		}
		
		public SrvFase()
		{
		}
	}
}
