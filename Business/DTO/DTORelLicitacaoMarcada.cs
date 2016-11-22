using System;
using System.Data;
using System.Web;

namespace Licitar.Business.Dto
{
	public class DTORelLicitacaoMarcada
	{	
		public DataSet dataSetResultado { get; set; }
		
		public string TotalRegistrosEncontrados { get; set; }
		
		public string DataEmissao { get { return DateTime.Now.ToString("dd/MM/yyyy"); } }		
		
		public string Filtros { get; set; }
	}
}
