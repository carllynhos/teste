// Mail.cs created with MonoDevelop
// User: wanialdo at 09:17 3/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Configuration;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Text;

namespace Licitar.Business.Utilidade
{
	/// <class>eMail</class>
	/// <summary>
	/// Classe para envio de email pelo .Net Framework
	/// </summary>
	public class eMail
	{
	    #region Campos
	    //Remetente
	    protected string _remetente;        //Remetente do e-mail
	    protected string _senha;            //Senha da caixa de e-mail
	    protected string _smtpserver;       //Endereço FTP da caixa de e-mail
	    protected string _smtp_aut = "1";   //Tipo de autenticação

	    //Destinatário e Mensagem
	    protected string _para;
	    protected string _comcopia;
	    protected string _copiaoculta;
	    protected string _assunto;
	    protected string _mensagem;

		/// <value>
		/// Remetente do e-mail.
		/// </value>
		public string Remetente
		{
			get { return _remetente; }
			set { _remetente = value; }
		}

		/// <value>
		/// Senha da caixa de e-mail.
		/// </value>
	    public string Senha
		{
			get { return _senha; }
			set { _senha = value; }
		}

		/// <value>
		/// Servidor SMTP.
		/// </value>
	    public string SmtpServer
		{
			get { return _smtpserver; }
			set { _smtpserver = value; }
		}

		/// <value>
		/// Tipo de autenticação do servidor SMTP.
		/// </value>
	    public string Smtp_Aut
		{
			get { return _smtp_aut; }
	    }

		/// <value>
		/// Destinatário do e-mail.
		/// </value>
	    public string Para
		{
			get { return _para; }
			set { _para = value; }
		}

		/// <value>
		/// Destinatários 'com cópia'.
		/// </value>
	    public string ComCopia
		{
			get { return _comcopia; }
			set { _comcopia = value; }
		}

		/// <value>
		/// Destinatários 'com cópia oculta'.
		/// </value>
	    public string CopiaOculta
		{
			get { return _copiaoculta; }
			set { _copiaoculta = value; }
		}

		/// <value>
		/// Assunto do e-mail.
		/// </value>
	    public string Assunto
		{
			get { return _assunto; }
			set { _assunto = value; }
		}

		/// <value>
		/// Corpo da mensagem.
		/// </value>
	    public string Mensagem
		{
			get { return _mensagem; }
			set { _mensagem = value; }
		}
	    #endregion

	    #region Construtores
		/// <summary>
		/// Construtor padrão. Configura o servidor e o remetente padrão de e-mail do sistema.
		/// </summary>
	    public eMail() {
	        _smtpserver = "172.18.27.4"; //ConfigurationSettings.AppSettings["smtpserver"];
	        //_remetente = "sisaf@pge.ce.gov.br"; //ConfigurationSettings.AppSettings["remetente"];  
	    }
	    #endregion

	    #region Métodos
		/// <summary>
		/// Envia o e-mail usando System.Net.Mail
		/// </summary>
	    public void Enviar()
	    {
			SmtpClient SmtpMail = new SmtpClient(); //Cria o client SMTP

			MailMessage Email = new MailMessage(_remetente, _para, _assunto, _mensagem);

			//Configurações regionais, para evitar problemas com acentos e afins. Aqui está como Western Ocidental
			Email.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
			Email.IsBodyHtml = true;

			//SmtpMail.Credentials = New System.Net.NetworkCredential("usuario", "senha")
			SmtpMail.Host = "172.18.27.4";

			try
			{
				SmtpMail.Send(Email);
			} catch { throw new Exception("Não foi possível enviar o eMail."); }
		}
	    #endregion
		
	}
}