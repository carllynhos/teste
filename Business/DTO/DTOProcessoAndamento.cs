// DTOProcessoAndamento.cs created with MonoDevelop
// User: marcelolima at 16:24 20/2/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Licitar.Business.Dto
{
	
	
	public class DTOProcessoAndamento
	{
		
		public DTOProcessoAndamento()
		{
			
		}
		public int Id
		{
			get;
			set;
		}
		
		public string Cadastrante
		{
			get;
			set;
		}
		
		public DateTime DataCadastro
		{
			get;
			set;
		}
		
		public string Fase
		{
			get;
			set;
		}
		
		public string TipoAndamento
		{
			get;
			set;
		}
		
		public string Observacao
		{
			get;
			set;
		}
		
		public string PesTramitou
		{
			get;
			set;
		}
		
		public DateTime DataAndamento
		{
			get;
			set;
		}
		
		public int AndamentoCorridigo
		{
			get;
			set;
		}
	}
}
