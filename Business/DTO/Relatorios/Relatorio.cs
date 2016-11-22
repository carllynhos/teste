using System;
using System.Collections.Generic;

namespace Licitar.Business.DTO.Relatorios
{
	public class Relatorio
	{		
		public string Titulo { get; set; }
		public List<string> Filtros { get; set; }
		public DateTime DataEmissao { get; set; }
		public int TotalRegistrosEncontrados { get; set; }
		public object DataSource  { get; set; } 		
	}
}
