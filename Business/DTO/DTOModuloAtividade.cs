// ModuloFase.cs created with MonoDevelop
// User: marcelolima at 13:34 19/2/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Licitar.Business.Dto
{
	
	
	public class DTOModuloAtividade
	{
		
		public DTOModuloAtividade(int Id,string Descricao)
		{
			this.Id = Id;
			this.Descricao = Descricao;
		}

		public DTOModuloAtividade()
		{
			
		}
		
		public int Id
		{
			get;
			set;
		}
		
		public string Descricao
		{
			get;
			set;
		}
	}
}
