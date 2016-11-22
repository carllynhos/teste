// SrvTransmissao.cs created with MonoDevelop
// User: diogolima at 14:30 15/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using Castle.ActiveRecord.Queries;

using Licitar.Business.Entidade;
using Licitar.Business.Dao;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Serviço da transmissão da licitação.
	/// </summary>
	public class SrvTransmissao
	{
		public SrvTransmissao()
		{
		}

		/// <summary>
		/// Ativa uma transmissão.
		/// </summary>
		public static void AtivarTransmissao(int idProcesso, DateTime dtInicio, DateTime dtTermino,int idAuditorio, int idUsuario)
		{
			using(new Castle.ActiveRecord.SessionScope())
			{
				try
				{
					if (VerificaExistencia(idAuditorio,dtInicio,dtTermino))
						throw new InvalidOperationException("Já existe uma Transmissão agendada neste Horário e Auditório");
					
					//Processo oProcesso = Processo.Find(idProcesso);
					
					Agenda oAgenda = new Agenda();					
					oAgenda.Auditorio = Auditorio.Find(idAuditorio);
					oAgenda.DataInicio = dtInicio;
					oAgenda.DataFim = dtTermino;
					oAgenda.Ativo = true;
					oAgenda.Save();
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException("Não existe o andamento TRANSMISSÃO ONLINE para esta classificação do Processo");
				}
			}
		}
		
		/// <summary>
		/// Desativa uma transmissão. 
		/// </summary>
		public static void DesativarTransmissao(int idProcesso, string obs, int idUsuario)
		{
			DetachedCriteria pesqTrans = DetachedCriteria.For(typeof(Agenda));
			pesqTrans.Add(Expression.Eq("Processo.Id", idProcesso));
			Agenda oAgenda = Agenda.FindOne(pesqTrans);

			oAgenda.Ativo = false;
			oAgenda.Observacao = obs;
			oAgenda.Save();
		}
		
		/// <summary>
		/// Atualiza o término de uma transmissão. 
		/// </summary>
		public static void AtualizarTransmissao(int idAgenda,string dtTermino)
		{
			Agenda oAgenda = Agenda.Find(idAgenda);
			DateTime novaData = DateTime.Parse(DateTime.Now.ToShortDateString() +" "+ dtTermino + ":00");
			
			if (VerificaExistencia(oAgenda.Auditorio.Id, oAgenda.DataInicio, oAgenda.DataFim))
			{
				throw new InvalidOperationException("Não é possível Terminar neste horário pois conflita com outra Transmissão");
			}
			
			oAgenda.DataFim = novaData;
			oAgenda.Update();
		}
		
		/// <summary>
		/// Verifica a existência de uma transmissão.
		/// </summary>
		public static bool VerificaExistencia(int idAuditorio, DateTime dtInicio, DateTime dtTermino)
		{			
			DetachedCriteria pesqTrans = DetachedCriteria.For(typeof(Agenda));
			pesqTrans.Add(Expression.Eq("Auditorio.Id", idAuditorio));
			pesqTrans.Add(Expression.Or(Expression.Between("DataInicio", dtInicio, dtTermino),
				                   Expression.Between("DataProrrogacao", dtInicio, dtTermino)));
			pesqTrans.Add(Expression.Eq("Ativo", true));
			
			return Agenda.Exists(pesqTrans);
		}
	}
}
