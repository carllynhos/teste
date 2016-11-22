/*
 * 
 * Criador: Danilo Meireles
 * Data da Cria��o: 15/03/2008
 * 
 */

using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Licitar.Business.Utilidade
{
	public class JavaScript
	{
	    /// <summary>
	    /// Exibe uma caixa de confirmacao. (Ex: Exclus�o de um registro).
	    /// Deve ser aplicado a propriedade OnClientClick de um Button, ImageButton ou LinkButton antes do controle ser clicado. Ex: Page_Load
	    /// Exemplo de uso: Button1.OnClientClick = JavaScript.Confirmar("Confirma a exclus�o do registro?");
	    /// </summary>
	    /// <param name="pergunta">A pergunta de confirma��o</param>
	    public static string ConfirmarOnClientClick(string pergunta)
	    {
	        return "JavaScript: return confirm('" + pergunta + "')";
	    } 

	    /// <summary>
	    /// Exibe um Alert JavaScript com uma mensagem de sucesso. Deve ser chamado ap�s a realiza��o de opera��es.
	    /// </summary>
	    /// <param name="pagina">A p�gina da qual o m�todo est� sendo chamado</param>
	    /// <param name="mensagem">A mensagem de sucesso</param>
	    public static void ExibirMensagem(Page pagina, string mensagem)
	    {        
	        pagina.ClientScript.RegisterStartupScript(pagina.GetType(), "Mensagem", "window.alert('" + mensagem + "');", true);
	    }

	    public static string AbrirPopupOnClientClick
	    (Page pagina, string url, string titulo, bool redimensionavel, bool barraDeMenu, bool barraDeRolagem, int largura, int altura)
	    {
	        string resizable = "yes";
	        if (!redimensionavel)
	        {
	            resizable = "no";
	        }
	        string menuBar = "yes";
	        if (!barraDeMenu)
	        {
	            menuBar = "no";
	        }
	        string scrollBar = "yes";
	        if (!barraDeRolagem)
	        {
	            scrollBar = "no";
	        }              
	        string script = "javascript:window.open('@url', '@titulo', 'resizable=@resizable, menubar=@menubar, scrollbars=@scrollbar, width=@width, height=@height')";
	        script = script.Replace("@url", url);
	        script = script.Replace("@titulo", titulo);
	        script = script.Replace("@resizable", resizable);
	        script = script.Replace("@menubar", menuBar);
	        script = script.Replace("@scrollbar", scrollBar);        
	        script = script.Replace("@width", largura.ToString());
	        script = script.Replace("@height", altura.ToString());
	        return script;       
	    }

	    /// <summary>
	    /// Abre um popup. Pode ser chamado dentro do evento OnClick do Button, ImageButton ou LinkButton
	    /// </summary>
	    /// <param name="pagina"></param>
	    /// <param name="url"></param>
	    /// <param name="titulo"></param>
	    /// <param name="redimensionavel"></param>
	    /// <param name="barraDeMenu"></param>
	    /// <param name="barraDeRolagem"></param>
	    /// <param name="largura"></param>
	    /// <param name="altura"></param>
	    public static void AbrirPopup(Page pagina, string url, string titulo, bool redimensionavel, bool barraDeMenu, bool barraDeRolagem, int largura, int altura)
	    {
	        string resizable = "yes";
	        if (!redimensionavel)
	        {
	            resizable = "no";
	        }
	        string menuBar = "yes";
	        if (!barraDeMenu)
	        {
	            menuBar = "no";
	        }
	        string scrollBar = "yes";
	        if (!barraDeRolagem)
	        {
	            scrollBar = "no";
	        }
	        string script = "javascript:window.open('@url', '@titulo', 'resizable=@resizable, menubar=@menubar, scrollbars=@scrollbar, width=@width, height=@height')";
	        script = script.Replace("@url", url);
	        script = script.Replace("@titulo", titulo);
	        script = script.Replace("@resizable", resizable);
	        script = script.Replace("@menubar", menuBar);
	        script = script.Replace("@scrollbar", scrollBar);
	        script = script.Replace("@width", largura.ToString());
	        script = script.Replace("@height", altura.ToString());
	        pagina.ClientScript.RegisterStartupScript(pagina.GetType(), "OK", script, true);
	    }
		
		/// <summary>
		/// Registra uma fun��o JavaScript na p�gina em que � chamado. Normalmente � chamado dentro do Page_Load
		/// Importante: � necess�rio inserir a seguinte propriedade no TextBox: (OnKeyPress="return MascararData(this, event)") sem parent�ses
		/// </summary>
		/// <param name="pagina">
		/// this.Page
		/// A <see cref="Page"/>
		/// </param>
		public static void RegistrarMascaraDataTextBox(Page pagina)
		{
			string script = @"javascript:
				function MascararData(campo, evt)
				{   
					var key = (window.event)?evt.keyCode:evt.which;		
					if(key > 47 &&  key < 58)
					{
						switch(campo.value.length)
						{
							case 2:
								campo.value = campo.value + '/';
								break;
							case 5:
								campo.value = campo.value + '/';
								break;
						}
						return true;
					}	
					else if((key == 13 || key == 0) && campo.value.length == 2)
						return true;
					else if(key == 8 || key == 0)
						return true;
					else
						return false;		
				}


			";
			pagina.ClientScript.RegisterStartupScript(pagina.GetType(), "OK", script, true);
		}
		
		public static void AbrirModal(Page pagina)
	    {
	        
	        string script = @"	$(document).ready(function(){
				$('.modal').colorbox({width:'900', height:'95%', iframe:true, href:'pgFormBalcaoVisualizarArquivoDigital.aspx'})
			});alert('Alerta!')";
			
	        pagina.ClientScript.RegisterStartupScript(pagina.GetType(), "OK", script, true);
	    }
		
		
	}
}