// DtoExibirFiltros.cs created with MonoDevelop
// User: marcelolima at 08:58 15/5/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace  Licitar.Business.Dto
{
	
	
	public class DtoExibirFiltros
	{
		
		public DtoExibirFiltros()
		{
		}

        public bool	TodosFiltros
		{
			get;
			set;
		}	

		public bool FiltroFaseAndamento
		{
			get;
			set;
		}
		
		public bool	FiltroInstituicao
		{
			get;
			set;
		}

		public bool FiltroNatureza
		{
			get;
			set;
		}

		public bool FiltroModalidade
		{
			get;
			set;			
		}

		public bool FiltroNumero
		{
			get;
			set;
		}

		public bool FiltroPeriodo
		{
			get;
			set;
		}

		public bool FiltroPregoeiro
		{
			get;
			set;
		}

		public bool FiltroResumoObjeto
		{
			get;
			set;
		}

		public bool	FiltroTipoLicitacao
		{
			get;
			set;
		}	
	}
}
