// srvMotivoAndamento.cs created with MonoDevelop
// User: marcelolima at 08:17 30/6/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Licitar.Business.Entidade;
using NHibernate;
using NHibernate.Expression;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;

namespace Licitar.Business.Servico
{
	
	
	public class SrvMotivoAndamento
	{
		
		public SrvMotivoAndamento()
		{
		}

		public MotivoAndamento[] PopularGridMotivoAndamento(string descricao,int fkAtividade)
		{
			DetachedCriteria pesqMotivoAndamento = DetachedCriteria.For(typeof(MotivoAndamento));
			if(!string.IsNullOrEmpty(descricao))
				pesqMotivoAndamento.Add(Expression.InsensitiveLike("Descricao",descricao));
			if(fkAtividade != 0)
				pesqMotivoAndamento.Add(Expression.Eq("Atividade.Id",fkAtividade));
			pesqMotivoAndamento.AddOrder(Order.Asc("Id"));
			return MotivoAndamento.FindAll(pesqMotivoAndamento);
		}

		public MotivoAndamento RecuperarMotivoAndamento(int idMotivoAndamento)
		{
			return MotivoAndamento.Find(idMotivoAndamento);
		}

		public bool RemoverMotivoAndamento(int idMotivoAndamento)
		{
			bool retorno = false;
			SrvMotivoAndamento srv = new SrvMotivoAndamento();
			MotivoAndamento oMotivoAndamento = RecuperarMotivoAndamento(idMotivoAndamento);
			if(oMotivoAndamento != null)
			{
				oMotivoAndamento.Delete();
				retorno = true;
			}							
			return retorno;
		}
	}
}
