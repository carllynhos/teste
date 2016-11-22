// DTOExibirColunas.cs created with MonoDevelop
// User: marcelolima at 11:41 15/5/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Licitar.Business.Dto
{
	
	
	public class DTOExibirColunas
	{
		
		public DTOExibirColunas()
		{
			
		}

		public bool Instituicao
		{
			get;set;
		}

		public bool UnidadeAdministrativa
		{
			get;set;
		}

		public bool Numero
		{
			get;set;
		}

		public bool Modalidade
		{
			get;set;
		}

		public bool Natureza
		{
			get;set;
		}

		public bool TipoLicitacao
		{
			get;set;
		}

		public bool ResumoObjeto
		{
			get;set;		
		}
		
		public bool Pregoeiro
		{
			get;set;
		}
		
		public bool Presidente
		{
			get;set;
		}
	}
}
