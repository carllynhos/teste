// SrvLayout.cs created with MonoDevelop
// User: guilhermefacanha at 12:12 24/4/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using Licitar.Business.Dao;

namespace Licitar.Business.Servico
{	
	public class SrvLayout
	{
		public struct Links
		{
			public string linkImagemSair{get;set;}
			public string linkImagemLogoLicitar{get;set;}
			public string linkImagemLogoPqn{get;set;}
			public string linkCssEstilo{get;set;}
			public string linkCssImpressao{get;set;}				
		}
		
		public struct LinkJavaScript
		{
			public string linkJs01{get;set;}
			public string linkJs02{get;set;}
			public string linkJs03{get;set;}
			public string linkJs04{get;set;}
			public string linkJs05{get;set;}
		}
		
		public static string GetLinkServidorBuscador(string servidor)
		{
			string link = string.Empty;	
			if(servidor == "127.0.0.1")
				servidor = "devlicitar";
			else if(servidor != "localhost" && servidor != "127.0.0.1" && servidor != "devlicitar" && servidor != "hglicitar")
				servidor = "licitar";
			link = ConsultaXML.retornarLinks("Buscador|"+servidor).Trim();
			
			return link;
		}

		public Links getLinksPorServidor(string servidor)
		{
			Links obj = new Links();

			/*if(servidor.Equals("10.27.0.7") || servidor.Equals("10.27.0.13"))
			{
				obj.linkImagemSair ="~/template/imagens/botoes/btn_sair.png";
				obj.linkImagemLogoLicitar = "~/template/imagens/logo_licitar.jpg";
				obj.linkImagemLogoPqn = "~/template/imagens/logos/pge_mono_pqn.gif";
				obj.linkCssEstilo = "~/template/css/estilo.css";
			}
			else if(servidor.Equals("10.27.0.9") || servidor.Equals("10.27.0.12"))
			{
				obj.linkImagemSair = "~/template/imagens/botoes/btn_sair.png";
				obj.linkImagemLogoLicitar = "~/template/imagens/logo_licitar.jpg";
				obj.linkImagemLogoPqn = "~/template/imagens/logos/pge_mono_pqn.gif";
				obj.linkCssEstilo = "~/template/css/estilo.css";
			}
			else if(servidor.Equals("10.27.0.8") || servidor.Equals("10.27.0.22") || servidor.Equals("10.27.0.11") || servidor.Equals("10.27.0.14"))
			{
				obj.linkImagemSair ="~/template/imagens/botoes/btn_sair.png";
				obj.linkImagemLogoLicitar = "~/template/imagens/logo_licitar.jpg";
				obj.linkImagemLogoPqn = "~/template/imagens/logos/pge_mono_pqn.gif";
				obj.linkCssEstilo = "~/template/css/estilo.css";
			}
			else if(servidor.Equals("127.0.0.1"))
			{
				obj.linkImagemSair ="http://devlicitar/licitar/template/imagens/botoes/btn_sair.png";
				obj.linkImagemLogoLicitar = "http://devlicitar/licitar/template/imagens/logo_licitar.jpg";
				obj.linkImagemLogoPqn = "http://devlicitar/licitar/template/imagens/logos/pge_mono_pqn.gif";
				obj.linkCssEstilo = "http://devlicitar/licitar/template/css/estilo.css";
				obj.linkCssImpressao = "http://devlicitar/licitar/template/css/impressao.css";			
			}*/
			//else
			//{
				obj.linkImagemSair = "~/template/imagens/botoes/btn_sair.png";
				obj.linkImagemLogoLicitar = "~/template/imagens/logo_licitar.jpg";
				obj.linkImagemLogoPqn = "~/template/imagens/logos/pge_mono_pqn.gif";
				obj.linkCssEstilo = "~/template/css/estilo.css";
			//}

			return obj;
		}
		
		public LinkJavaScript GetLinksJavaScriptPorServidor(string servidor)
		{
			LinkJavaScript linkJs = new LinkJavaScript();			
				linkJs.linkJs01 = "/Jquery/Calendario/js/jquery-1.3.2.min.js";
				linkJs.linkJs02 = "/Jquery/Calendario/js/jquery-ui-1.7.2.custom.min.js";
				linkJs.linkJs03 = "/Jquery/Calendario/js/calendario.js";
				linkJs.linkJs04 = "/Jquery/Calendario/css/jquery-ui-1.7.2.custom.css";
				linkJs.linkJs05 = "/js/ScriptsMasterPage.js";			
			return linkJs;
		}
	}
}
