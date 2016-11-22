// SrvEmail.cs created with MonoDevelop
// User: guilhermefacanha at 15:55 26/5/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net.Mail;
using System.Web;
using System.Data;
using System.Collections;

namespace Licitar.Business.Servico
{
	
	
	public class SrvEmail
	{
		
		public SrvEmail()
		{
		}		
	
		public static void EnviarEmail(object[] destino, string Assunto, string corpo)
		{
			if (destino.Length > 0)
			{
				MailMessage mailMsg = new MailMessage();
				mailMsg.IsBodyHtml = true;

				foreach (object email in destino)
				{
					Console.WriteLine((string)email);
					mailMsg.To.Add((string)email);
				}
				
				
				if (HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString() != "127.0.0.8")
				{
					
				}

				string texto = 
					@"<table>
						<tr>
							<td><a href='http://intranet/licitar' target='blank'><img border='0' src='~/template/imagens/logo_licitar.jpg' /></a></td>
						</tr>
					</table> <br/><br/>";

				mailMsg.Body =  texto + corpo;   
				mailMsg.Subject = Assunto;
				mailMsg.From = new MailAddress("licitar@pge.ce.gov.br");
				SmtpClient smtp = new SmtpClient();
				smtp.Send(mailMsg);
				
			}
		}

		public static void EnviarEmailRevalidacaoProposta(object[] destino, string Assunto, string corpo)
		{
			if (destino.Length > 0)
			{
				MailMessage mailMsg = new MailMessage();
				mailMsg.IsBodyHtml = true;

				foreach (object email in destino)
				{
					mailMsg.To.Add((string)email);
				}		

				mailMsg.Body = corpo;   
				mailMsg.Subject = Assunto;
				mailMsg.From = new MailAddress("licitar@pge.ce.gov.br");
				SmtpClient smtp = new SmtpClient();
				smtp.Send(mailMsg);
				
			}
		}
		
	}
}
