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
using Licitar.Business.Persistencia;
using System.Data;
using Npgsql;

namespace Licitar.Business.Servico
{	
	public class SrvAgenda
	{
		public bool VerificarSeJaExisteTransmissao(DateTime dataInicio, DateTime dataFim, int idAuditorio)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Agenda));
			dc.Add(Expression.Between("DataInicio", dataInicio, dataFim.AddSeconds(59)));
			dc.Add(Expression.Eq("Auditorio.Id", idAuditorio));
			
			if (Agenda.FindAll(dc).Length > 0)
				return true;
			
			return false;
		}
	    
		public static bool VerificarSeJaExisteTransmissaoTerminarSalvar(DateTime dataInicio,DateTime dataFim,int idAuditorio,int idProcessoAndamento)
		{
			PostgreSqlDatabase db = new PostgreSqlDatabase();
			bool horarioFechado = false;
			DetachedCriteria dc = DetachedCriteria.For(typeof(Agenda));
			dc.Add(Expression.Sql(" to_char(this_.dat_inicio_age,'dd/MM/yyyy') = '"+dataInicio.ToString("dd/MM/yyyy")+"' and to_char(this_.dat_fim_age,'dd/MM/yyyy') = '"
			     +dataFim.ToString("dd/MM/yyyy")+"'"+@" and (SELECT (timestamp '"
			     +dataInicio+"' ,timestamp '"+dataFim+@"' ) OVERLAPS (this_.dat_inicio_age, this_.dat_fim_age)) = 't' and this_.fk_cod_processo_andamento_pan != "+idProcessoAndamento +
			                      " and this_.fk_cod_processo_andamento_remarcado_pan is null  "			                      
			     ));  			
			dc.Add(Expression.Eq("Auditorio.Id", idAuditorio));
			
			Agenda[] ag = Agenda.FindAll(dc);
			if(ag.Length > 0)
				horarioFechado = true;
				
			return horarioFechado;
		}

		public Agenda existeAgenda(int idAndamento)
		{
			DetachedCriteria dcAgenda = DetachedCriteria.For(typeof(Agenda));
			dcAgenda.CreateAlias("ProcessoAndamento","pan");
			dcAgenda.Add(Expression.Eq("pan.Id",idAndamento));
			return Agenda.FindFirst(dcAgenda);
		}

		public bool existeAgendaDataCadastrada(DateTime dataAndamento, int idProcesso)
		{			
			string select = @"
			SELECT age.pk_cod_agenda_age
			FROM adm_licitar.tb_agenda_age age
			INNER JOIN adm_licitar.tb_processo_andamento_pan pan ON pan.pk_cod_processo_andamento_pan = age.fk_cod_processo_andamento_pan
			WHERE pan.dat_andamento_pan = '@data' 
			and pan.fk_cod_processo_pro = @idProcesso ";

			select = select.Replace("@data",dataAndamento.ToString());
			select = select.Replace("@idProcesso",idProcesso.ToString());
						
			if (new PostgreSqlDatabase().Consultar(select).Rows.Count > 0)
				return true;
			
			return false;
		}

		
		public string existeAgendaHoraCadastrada(DateTime dataAndamento, int idProcesso)
		{
			DateTime horaFinal = dataAndamento.AddMinutes(59);
			string resultado = "";
			
			string select = @"
			SELECT age.pk_cod_agenda_age, pan.dat_andamento_pan as data
			FROM adm_licitar.tb_agenda_age age
			INNER JOIN adm_licitar.tb_processo_andamento_pan pan ON pan.pk_cod_processo_andamento_pan = age.fk_cod_processo_andamento_pan
			WHERE pan.dat_andamento_pan between '@data' and '@final' 
			and pan.fk_cod_processo_pro = @idProcessso ";

			select = select.Replace("@data",dataAndamento.ToString("dd/MM/yyyy HH")+":00");
			select = select.Replace("@final",horaFinal.ToString());
			select = select.Replace("@idProcesso",idProcesso.ToString());
						
			DataTable dt = new PostgreSqlDatabase().Consultar(select);

			foreach(DataRow row in dt.Rows)
			{
				string data = row["data"].ToString();
				if(!string.IsNullOrEmpty(data))
				{
					DateTime time = Convert.ToDateTime(row["data"].ToString());
					resultado += time.ToString("dd/MM/yyyy HH:mm")+", ";
				}
			}

			if(!string.IsNullOrEmpty(resultado))
			{
				resultado = resultado.Remove(resultado.Length-1);
				resultado = resultado.Remove(resultado.Length-1);
			}
			
			return resultado;
		}
		
		public static List<string> GetHorariosMarcados(int idAuditorio, DateTime dataSelecionada)
		{			
			string select = @"
				select 
				date_part('hour', dat_inicio_age) as hora_inicio, 
				date_part('minute', dat_inicio_age) as minuto_inicio,
				date_part('hour', dat_fim_age) as hora_fim, 
				date_part('minute', dat_fim_age) as minuto_fim
				from adm_licitar.tb_agenda_age 
				where 
				date_part('year',dat_inicio_age) = @ano and date_part('month',dat_inicio_age) = @mes and date_part('day',dat_inicio_age) = @dia and fk_cod_auditorio_aud = @auditorio
				and fk_cod_processo_andamento_remarcado_pan is null
				order by hora_inicio
			";
			
			select = select.Replace("@ano", dataSelecionada.Year.ToString());
			select = select.Replace("@mes", dataSelecionada.Month.ToString());
			select = select.Replace("@dia", dataSelecionada.Day.ToString());
			select = select.Replace("@auditorio", idAuditorio.ToString());

			DataSet dataSet = new PostgreSqlDatabase().ExecutarConsulta(select);
			
			DateTime horaInicio;
			DateTime horaFim;
			
			List<string> listaHorarios = new List<string>();
			for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)	
			{
				horaInicio = new DateTime(2000, 01, 01, Convert.ToInt32(dataSet.Tables[0].Rows[i]["hora_inicio"]), Convert.ToInt32(dataSet.Tables[0].Rows[i]["minuto_inicio"]), 0);								
				horaFim = new DateTime(2000, 01, 01, Convert.ToInt32(dataSet.Tables[0].Rows[i]["hora_fim"]), Convert.ToInt32(dataSet.Tables[0].Rows[i]["minuto_fim"]), 0);								
				listaHorarios.Add(horaInicio.ToString("HH") + ":" + horaInicio.ToString("mm") + " - " + horaFim.ToString("HH") + ":" + horaFim.ToString("mm"));
			}
			
			return listaHorarios;
		}	
		
		public void DescartarAgenda(int idProcessoAndamento, int idProcessoAndamentoRemarcado)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Agenda));
			dc.Add(Expression.Eq("ProcessoAndamento.Id", idProcessoAndamento));
			Agenda[] agendas = Agenda.FindAll(dc);
			for (int i = 0; i < agendas.Length; i++)
			{
				ProcessoAndamento procAnd = ProcessoAndamento.Find(idProcessoAndamentoRemarcado);
				agendas[i].ProcessoAndamentoRemarcado = new ProcessoAndamento(idProcessoAndamentoRemarcado); 				
				agendas[i].UpdateAndFlush();
			}
		}

		public void RetirarDescartarAgenda(int idProcessoAndamentoRemarcado)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(Agenda));
			dc.Add(Expression.Eq("ProcessoAndamentoRemarcado.Id", idProcessoAndamentoRemarcado));
			Agenda[] agendas = Agenda.FindAll(dc);
			
			for (int i = 0; i < agendas.Length; i++)
			{
				agendas[i].ProcessoAndamentoRemarcado = null;
				agendas[i].Update();
			}
		}

	}
}