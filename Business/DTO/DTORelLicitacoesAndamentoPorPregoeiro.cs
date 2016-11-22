using System;

namespace Licitar.Business.Dto
{	
	/// <summary>
	/// DTO para andamento das licitações por pregoeiro.
	/// </summary>
	public class DTORelLicitacoesAndamentoPorPregoeiro
	{		
		public string IdUnidadeExercicioNivel1 { get; set; }	
		public string Estados { get; set; }
		public string idPessoa { get; set; }
		public string DataInicio { get; set; }
		public string DataFim { get; set; }
		
		/// <summary>
		/// Construtor padrão.
		/// </summary>
		public DTORelLicitacoesAndamentoPorPregoeiro()
		{
		}
	}
}
