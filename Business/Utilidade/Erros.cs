/* Envio de mensagem com erros disparados pelas p�ginas
 * Respons�vel: Wanialdo Lima
 * �ltima Atualiza��o: 26/06/2008
 */

using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Text;

namespace Licitar.Business.Utilidade
{
	/// <class>Erro</class>
	/// <summary>
	/// Classe para captura de erros na aplica��o e alerta � equipe de desenvolvimento.
	/// </summary>
	public class Erro
	{
		eMail oEmail;	
		
	    #region Propriedades
		
		protected string _usuario;
		/// <value>
	    /// Usu�rio
	    /// </value>
		public string Usuario 
		{
			get { return _usuario; }
			set { _usuario = value; }
		}

		protected string _browser;
		/// <value>
		/// Browser
		/// </value>
	    public string Browser
		{
			get { return _browser; }
			set { _browser = value; }
		}

		protected string _versaobrowser;
		/// <value>
		/// Vers�o do Browser
		/// </value>
	    public string VersaoBrowser
		{
			get { return _versaobrowser; }
			set { _versaobrowser = value; }
		}

		protected string _sistemaoperacional;
		/// <value>
		/// Sistema Operacional
		/// </value>
	    public string SistemaOperacional
		{
			get { return _sistemaoperacional; }
			set { _sistemaoperacional = value; }
		}

		protected string _paginaerro;
		/// <value>
		/// P�gina
		/// </value>
	    public string PaginaErro
		{
			get { return _paginaerro; }
			set { _paginaerro = value; }
		}

		protected string _mensagemerro;
		/// <value>
		/// Mensagem de Erro
		/// </value>
	    public string MensagemErro
		{
			get { return _mensagemerro; }
			set { _mensagemerro = value; }
		}

		protected string _data;
		/// <value>
		/// Data da Ocorr�ncia
		/// </value>
		public string Data
		{
			get { return _data; }
			set { _data = value; }
		}

		protected string _mensagemapresentada;
		/// <value>
		/// Mensagem Apresentada
		/// </value>
	    public string MensagemApresentada
		{
			get { return _mensagemapresentada; }
			set { _mensagemapresentada = value; }
		}

		protected string _aplicacao;
		/// <value>
		/// Aplica��o
		/// </value>
	    public string Aplicacao
		{
			get { return _aplicacao; }
			set { _aplicacao = value; }
		}

		protected string _mensagem;
		/// <value>
		/// Mensagem do email
		/// </value>
		public string Mensagem
		{
			get { return _mensagem; }
			set { _mensagem = value; }
		}
	    #endregion

	    #region Construtores
		/// <summary>
		/// Construtor Padr�o
		/// </summary>
	    public Erro() {
			oEmail = new eMail();
			Data = DateTime.Now.ToString();
	    }
	    #endregion

	    #region M�todos
		/// <summary>
		/// Envia um e-mail informando o erro ocorrido na aplica��o.
		/// </summary>
		public void InformarErro() 
		{
			string err = Mensagem;
			
			Mensagem = "Ocorreu um erro na Aplica��o: " + this.Aplicacao + "."
	                + "\n As informa��es sobre o erro encontram-se abaixo. Favor conferir o site. \n "
	                + "\n Data e Hora da Ocorr�ncia: " + Data + "\n "
	                + "\n Mensagem p�blica ..: " + MensagemApresentada + "\n"
					+ "\n P�gina ............: " + PaginaErro
	                + "\n Browser............: " + Browser + " " + VersaoBrowser + " " + SistemaOperacional
	                + "\n Usu�rio ...........: " + Usuario;

            Mensagem += "\n Descri��o do Erro .: " + MensagemErro + "\n \n" + err + "\n \n"; 

            oEmail.Para = "desenvolvimento@pge.ce.gov.br";
            oEmail.Assunto = "ERRO: " + Aplicacao;
            oEmail.Mensagem = Mensagem;

			try
			{
	            oEmail.Enviar();
			}
			catch { }
	    }	    #endregion
	}
}