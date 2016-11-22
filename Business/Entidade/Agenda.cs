//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: Agenda.cs
// CRIADO POR: Danilo Meireles 
// DATA DA CRIACAO: 08/11/2008
// DESCRICAO: 
// ALTERADO POR: 
// DATA DA ALTERACAO: 
// MOTIVO DA ALTERACAO:
// OBSERVACOES:
//----------------------------------------------------------------------------------------------------------------------------------------------

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_agenda_age", Schema="adm_licitar")]
	public class Agenda : ActiveRecordBase<Agenda>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_agenda_age", SequenceName="adm_licitar.sq_agenda_age")]
		public virtual int Id
		{
			get;
			set;
		}		
		
		[BelongsTo("fk_cod_auditorio_aud")]
		public virtual Auditorio Auditorio
		{
			get;
			set;
		}
		
		[Property("dat_inicio_age")]
		public virtual DateTime DataInicio
		{
			get;
			set;
		}
		
		[Property("dat_fim_age")]
		public virtual DateTime DataFim
		{
			get;
			set;
		}
		
		[Property("dat_prorrogacao_age")]
		public virtual DateTime DataProrrogacao
		{
			get;
			set;
		}
		
		[Property("boo_ativo_age")]
		public virtual bool Ativo
		{
			get;
			set;
		}
		
		[Property("txt_observacao_age")]
		public virtual string Observacao
		{
			get;
			set;
		}
		
		[BelongsTo("fk_cod_processo_andamento_pan")]
		public virtual ProcessoAndamento ProcessoAndamento
		{
			get;
			set;
		}
		
		[Property("boo_transmissao_online_age")]	                    
		public virtual bool PossuiTransmissaoOnLine
		{
			get;
			set;
		}	
		
		[BelongsTo("fk_cod_processo_andamento_remarcado_pan")]	                    
		public virtual ProcessoAndamento ProcessoAndamentoRemarcado
		{
			get;
			set;
		}
		 
		public Agenda()
		{
		}
		
		public Agenda(int id)
		{
			this.Id = id;
		}
	}
}