// DTORelLicitacao.cs created with MonoDevelop
// User: marcelolima at 15:22 9/2/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Licitar.Business.Dto
{
	
	
	public class DTORelLicitacao
	{
		
		public DTORelLicitacao()
		{
			
		}
		
		public string sql
		{
			get;
			set;
		}
		
		public string Instituicao
		{
			get;
			set;
		}

		public DtoExibirFiltros DtoExibirFiltros
		{
			get;
			set;
		}

		public DTOExibirColunas DTOExibirColunas
		{
			get;
			set;
		}

		
		public string FaseAndamento
		{
			get;
			set;
		}
		
		public string TipoNumero
		{
			get;
			set;
		}
		
		public string Numero
		{
			get;
			set;
		}
		
		public bool UltimoAndamento
		{
			get;
			set;
		}
		
		public string Modalidade
		{
			get;
			set;
		}
		
		public string Natureza
		{
			get;
			set;
		}
		
		public string TipoLicitacao
		{
			get;
			set;
		}
		
		public string ResumoObjeto
		{
			get;
			set;
		}
		
		public string Pregoeiro
		{
			get;set;
		}
		
		public string DataInicio
		{
			get;
			set;
		}
		
		public string DataFim
		{
			get;
			set;
		}
		
		public string TotalValorEstimado
		{
			get;set;
		}
		
		public string TotalValorContratado
		{
			get;set;
		}
		
		public string TotalValorFracassado
		{
			get;set;
		}
		
		public string TotalValorEconomia
		{
			get;set;
		}
		
		public string TotalValorEconomiaPorcent
		{
			get;set;
		}
		
		public string TotalValorEstimadoReal
		{
			get;set;
		}
	}
}
