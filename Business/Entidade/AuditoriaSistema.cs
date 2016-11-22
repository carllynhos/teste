//----------------------------------------------------------------------------------------------------------------------------------------------
// ARQUIVO: AuditoriaSistema.cs
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
using System.Web;
using System.Data;
using Npgsql;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_auditoria_sistema_asi", Schema="adm_licitar")]
	public class AuditoriaSistema : ActiveRecordBase<AuditoriaSistema>
	{		
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_auditoria_sistema_asi", SequenceName="adm_licitar.sq_auditoria_sistema_asi")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_tipo_acao_asi")]
		public virtual string TipoAcao
		{
			get;
			set;
		}
		
		[Property("txt_tabela_asi")]
		public virtual string Tabela
		{
			get;
			set;
		}		
		
		[Property("dat_acao_asi")]
		public virtual DateTime DataAcao 
		{
			get;
			set;
		}
		
		[Property("num_pk_acao_asi")]
		public virtual int IdAcao
		{
			get;
			set;
		}
		
		[Property("txt_cpf_pessoa_asi")]
		public virtual string Cpf 
		{
			get;
			set;
		}
		
		[Property("txt_registro_asi")]
		public virtual string TextoRegistro
		{
			get;
			set;
		}
		
		[Property("txt_registro_update_asi")]
		public virtual string RegistroUpdate
		{
			get;
			set;
		}
		
		[Property("txt_num_spu_asi")]
		public virtual string NumeroSpu
		{
			get;
			set;
		}
		
		[Property("txt_nome_pessoa_asi")]
		public virtual string NomePessoa
		{
			get;
			set;
		}
		
		public AuditoriaSistema()
		{
		}
		
		public AuditoriaSistema(int id)
		{
			this.Id = id;
		}
		
		public void RealizarAuditoria(int idAcao)
		{	
			
			try
			{
				Pessoa usuario = null;
				
		
					if( HttpContext.Current.Session != null && HttpContext.Current.Session["UsrLogado"] != null )				
						usuario = (Pessoa) HttpContext.Current.Session["UsrLogado"];			
					else
					    usuario = Pessoa.Find(810);
					
						string insert = @"
								INSERT INTO adm_licitar.tb_auditoria_sistema_asi 
								(num_pk_acao_asi, txt_cpf_pessoa_asi, txt_nome_pessoa_asi) VALUES 
								(@num_pk_acao_asi, @txt_cpf_pessoa_asi, @txt_nome_pessoa_asi)
						";
						NpgsqlCommand command = new NpgsqlCommand(insert);
						command.Parameters.Add("@num_pk_acao_asi", idAcao.ToString());
						command.Parameters.Add("@txt_cpf_pessoa_asi", usuario.CpfCnpj);
						command.Parameters.Add("@txt_nome_pessoa_asi", usuario.Nome);
						PostgreSqlDatabase db = new PostgreSqlDatabase();
						db.ExecutarComando(command);
				
				
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message.ToString()+ex.StackTrace.ToString());
			}			
		}
		
		public static AuditoriaSistema[] ListarAuditoria(string numeroSpu)
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof(AuditoriaSistema));
			
			if (numeroSpu.Length > 0)
			{
				dc.Add(Expression.Eq("NumeroSpu", numeroSpu));			
				//dc.Add(Expression.InsensitiveLike("TextoRegistro", "%" + numeroSpu + "%"));
				//dc.Add(Expression.InsensitiveLike("RegistroUpdate", "%" + numeroSpu + "%"));
			}
			
			string[] tabelas = {"tb_processo_andamento_pan", "tb_processo_pro", "tb_valor_processo_vpr", "tb_processo_papel_pessoa_ppp", "tb_numero_processo_npr"};
			dc.Add(Expression.In("Tabela", tabelas));			
			dc.AddOrder(Order.Asc("DataAcao"));			
			return AuditoriaSistema.FindAll(dc);
		}
	}
}