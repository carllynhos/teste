// AtividadeUnidadeExercicioPermissao.cs created with MonoDevelop
// User: guilhermefacanha at 10:25 25/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{
	
	[ActiveRecord(Table="tb_atividade_unidade_exercicio_permissao_aup", Schema="adm_licitar")]
	public class AtividadeUnidadeExercicioPermissao:ActiveRecordBase<AtividadeUnidadeExercicioPermissao>
	{

		[PrimaryKey(PrimaryKeyType.Sequence, "pk_cod_atividade_unidade_exercicio_permissao_aup", SequenceName="adm_licitar.sq_atividade_unidade_exercicio_permissao_aup")]
		public virtual int Id
		{
			get;
			set;
		}
		
		 [BelongsTo("fk_cod_permissao_per")]
        public virtual Permissao Permissao
        {
            get;
            set;
        }

        [BelongsTo("fk_cod_atividade_ati")]
        public virtual Atividade Atividade
        {
            get;
            set;
        }

		[BelongsTo("fk_cod_unidade_exercicio_uex")]
        public virtual UnidadeExercicio UnidadeExercicio
        {
            get;
            set;
        }
		
		public AtividadeUnidadeExercicioPermissao()
		{
			this.UnidadeExercicio = new UnidadeExercicio();
			this.Atividade = new Atividade();
			this.Permissao = new Permissao();
		}
	}
}
