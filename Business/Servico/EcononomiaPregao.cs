// EcononomiaPregao.cs created with MonoDevelop
// User: guilhermefacanha at 15:38 12/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Licitar.Business.Servico
{
	/// <summary>
	/// Classe de consulta para armazenar dados de economia do pregão.
	/// </summary>
	public class EcononomiaPregao
	{
		public DateTime dataInicio
		{
			get;set;
		}
		public DateTime dataFim
		{
			get;set;
		}
		public string ano
		{
			get;set;
		}
		public string instituicao
		{
			get;set;
		}
		public string modalidade
		{
			get;set;
		}
		public string natureza
		{
			get;set;
		}
		
		/// <summary>
		/// Construtor.
		/// </summary>
		public EcononomiaPregao()
		{
		}
	}
}
