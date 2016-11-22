
using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections;
using System.Collections.Generic;

namespace Licitar.Business.Entidade
{
	[Serializable]
	[ActiveRecord(Table="tb_configuracao_con", Schema="adm_licitar")]

	
	public class Configuracao: ActiveRecordBase<Configuracao>
	{
		
		public Configuracao()
		{
		}
	
		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_configuracao_con", SequenceName="adm_licitar.sq_configuracao_con")]
		public virtual int Id
		{
			get;
			set;
		}
		
		[Property("txt_descricao_con")]
		public virtual string Descricao
		{
			get;
			set;
		}

		[Property("boo_permissao_con")]
		public virtual bool Permissao
		{
			get;
			set;
		}
		
	}
	
}
