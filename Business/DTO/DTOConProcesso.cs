using System;

namespace Licitar.Business.Dto
{		
	public class DTOConProcesso
	{
		public int IdInstituicao { get; set; }
		public int IdModalidade { get; set; }
		public string NumeroSpu { get; set; }
		public string NumeroViproc { get; set; }
		public string NumeroLicitacao { get; set; }
		public string DataInicio { get; set; }
		public string DataFim { get; set; }
		public string NomePessoa { get; set; }
		public string ResumoObjeto { get; set; }
		public string ReceberTramitar { get; set; }
		public bool ProcessosAssociados { get;set;}
		public string NumeroComprasnet	{ get;set;}
		public string NumeroSisBB {	get;set;}
		public string NumeroMAPP {	get;set;}
		public string NumeroPortalDigital {	get;set;}
		
	}
}
