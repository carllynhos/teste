// AgendaLicitacao.cs created with MonoDevelop
// User: vitoralves at 14:12 26/9/2008
// PGE - Procuradoria Geral do Estado do Ceará
//

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using NHibernate.Expression;

using Licitar.Business.Dao;
using Licitar.Business.Entidade;

using Licitar.Business.Persistencia;

namespace Licitar.Business.Dto 
{
	/// <summary>
	/// View da Agenda de Licitações
	/// </summary>
	public class DTOAgendaLicitacao : PostgreSqlDatabase
	{	
		public virtual int NumeroProcesso { get; set; }
		
		public virtual int ProcessoAndamento { get; set; }
		
		public virtual string NumeroSpu { get; set; }
		
		public virtual string NumeroLicitacao { get; set; }
		
		public virtual string Instituicao { get; set; }
		
		//public virtual string Andamento { get; set; }
		
		public virtual string Objeto { get; set; }
		
		public virtual string Natureza { get; set; }
		
		public virtual string Modalidade { get; set; }
		
		public virtual DateTime Data { get; set; }
		
		public virtual int PkAuditorio { get; set; }
		
		public virtual string Auditorio { get; set; }
		
		public virtual string DataInicio { get; set; }
		
		public virtual string DataTermino { get; set; }
				
		public virtual string DataProrrogacao { get; set; }
		
		public virtual int Cancelada { get; set; }
		
		public virtual string ObsCancelada { get; set; }
		
		public static DTOAgendaLicitacao[] listarAgenda(DateTime data)
		{
			return new DTOAgendaLicitacao().listarAgenda(data, null);
		}
		
		public static Agenda[] ListarAgendaAR(DateTime dataMarcacao, int status, List<int> listIdModalidades)
		{				
			DetachedCriteria dc = DetachedCriteria.For(typeof(Agenda));
			dc.CreateAlias("ProcessoAndamento", "pa");
			dc.CreateAlias("pa.Processo", "pro");
			dc.CreateAlias("pro.Instituicao", "ins");
			dc.AddOrder(Order.Asc("ins.Descricao"));			
			dc.Add(Expression.Between("pa.DataAndamento", DateTime.Parse(dataMarcacao.ToShortDateString() + " 00:00:00"), DateTime.Parse(dataMarcacao.ToShortDateString()+" 23:59:59")));
			
				
			
			if (listIdModalidades.Count > 0)
			{
				dc.CreateAlias("pro.Classificacao", "cla");
				dc.CreateAlias("cla.Modalidade", "mod");
				dc.Add(Expression.In("mod.Id", listIdModalidades));
			}
			
			// inativo:
			if (status == 0)
				dc.Add(Expression.Eq("Ativo", false));
			
			// ativo:
			else if (status == 1)
				dc.Add(Expression.Eq("Ativo", true));
			
			Console.WriteLine("-----{");			
			Agenda[] agendas = Agenda.FindAll(dc);
			Console.WriteLine("-----}");
			Console.WriteLine("TOTAL:" + agendas.Length);
			return agendas;
		}
				
		public DTOAgendaLicitacao[] listarAgenda(DateTime data, string Cancelada)
		{
			string sql = @"
			
				SELECT
				pro.pk_cod_processo_pro, 
				ins.pk_cod_instituicao_ins, 
				ins.txt_descricao_ins,
				(
					SELECT txt_numero_processo_npr FROM adm_licitar.tb_numero_processo_npr npr 
					WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro AND npr.fk_cod_tipo_numero_tnu = 1
				) As txt_numero_spu, 
				(
					SELECT txt_numero_processo_npr FROM adm_licitar.tb_numero_processo_npr npr 
					WHERE npr.fk_cod_processo_pro = pro.pk_cod_processo_pro AND npr.fk_cod_tipo_numero_tnu = 20
				) As txt_numero_licitacao,
				pro.txt_resumo_objeto_pro, 
				nat.txt_descricao_nat, 
				mod.txt_descricao_mod,
				age.dat_inicio_age, 
				age.dat_fim_age, 
				aud.pk_cod_auditorio_aud, 
				aud.txt_descricao_aud,
				age.boo_ativo_age, 
				age.txt_observacao_age
				FROM 
				adm_licitar.tb_processo_pro pro, 
				adm_licitar.tb_instituicao_ins ins,
				adm_licitar.tb_natureza_nat nat, 
				adm_licitar.tb_modalidade_mod mod, 
				adm_licitar.tb_classificacao_cla cla,
				adm_licitar.tb_agenda_age age, 
				adm_licitar.tb_auditorio_aud aud
				WHERE 
				pro.fk_cod_instituicao_ins = ins.pk_cod_instituicao_ins
				AND pro.fk_cod_classificacao_cla = cla.pk_cod_classificacao_cla
				AND cla.fk_cod_natureza_nat = nat.pk_cod_natureza_nat
				AND cla.fk_cod_modalidade_mod = mod.pk_cod_modalidade_mod
				AND age.fk_cod_processo_pro = pro.pk_cod_processo_pro
				AND age.fk_cod_auditorio_aud = aud.pk_cod_auditorio_aud 
			";							
			
			if (!String.IsNullOrEmpty(data.ToString()))
			    sql += " AND to_char(age.dat_inicio_age, 'DD/MM/YYYY') = '" + data.ToString("dd/MM/yyyy") + "' ";
			
			if (!String.IsNullOrEmpty(Cancelada))
			{
				if (Cancelada == "0")				
					sql += " AND age.boo_ativo_age = true ";
				
				else				
					sql += " AND age.boo_ativo_age = false ";				
			}
			
			Console.WriteLine(sql);
				
			DataTable dt = Consultar(sql);

			DTOAgendaLicitacao[] lista = null;
			DTOAgendaLicitacao al = null;
			
			if (dt.Rows.Count > 0)
			{
				lista = new DTOAgendaLicitacao[dt.Rows.Count];
				//string[] propriedades = {"NumeroProcesso","NumeroSpu","NumeroLicitacao","Instituicao","Objeto","Natureza",
				//	"Modalidade","Data","PkAuditorio","Auditorio","DataInicio","DataTermino","Cancelada","obsCancelada"};
				
				for (int i = 0; i <= dt.Rows.Count - 1; i++)
				{
					al = new DTOAgendaLicitacao();
					if (!String.IsNullOrEmpty(dt.Rows[i][0].ToString()))
						al.NumeroProcesso = Convert.ToInt32(dt.Rows[i][0]);
					if (!String.IsNullOrEmpty(dt.Rows[i][2].ToString()))
						al.Instituicao = dt.Rows[i][2].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][3].ToString()))
						al.NumeroSpu = dt.Rows[i][3].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][4].ToString()))
						al.NumeroLicitacao = dt.Rows[i][4].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][5].ToString()))
						al.Objeto = dt.Rows[i][5].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][6].ToString()))
						al.Natureza = dt.Rows[i][6].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][7].ToString()))
						al.Modalidade = dt.Rows[i][7].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][8].ToString()))
						al.DataInicio = dt.Rows[i][8].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][9].ToString()))
						al.DataTermino = dt.Rows[i][9].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][10].ToString()))
						al.PkAuditorio = Convert.ToInt32(dt.Rows[i][10]);
					if (!String.IsNullOrEmpty(dt.Rows[i][11].ToString()))
						al.Auditorio = dt.Rows[i][11].ToString();
					if (!String.IsNullOrEmpty(dt.Rows[i][12].ToString()))
						al.Cancelada = Convert.ToInt32(dt.Rows[i][12]);
					if (!String.IsNullOrEmpty(dt.Rows[i][13].ToString()))
						al.ObsCancelada = dt.Rows[i][13].ToString();
					
					lista[i] = al;
				}
			}
			
			return lista;
		}
		
		public static Licitar.Business.Entidade.ProcessoAndamento AndamentoProcesso(int idProcesso)
		{
			return Licitar.Business.Entidade.ProcessoAndamento.FindFirst(Order.Desc("DataCadastro"), Expression.Eq("Processo.Id", idProcesso));
		}
	}
}
