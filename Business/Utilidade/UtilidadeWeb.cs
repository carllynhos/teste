using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Licitar.Business.Utilidade
{
	public class UtilidadeWeb
	{
		public static void  CarregarDropDown(DropDownList ddl, string dataValueField, string dataTextFidld, object dataSource)
		{
			ddl.DataValueField = dataValueField;
			ddl.DataTextField = dataTextFidld;
			ddl.DataSource = dataSource;
			ddl.DataBind();
			ddl.Items.Insert(0, "Selecione um item...");
		}
		
		public static void  CarregarDropDown(DropDownList ddl, string dataValueField, string dataTextFidld, object dataSource, string titulo)
		{
			ddl.DataValueField = dataValueField;
			ddl.DataTextField = dataTextFidld;
			ddl.DataSource = dataSource;
			ddl.DataBind();
			ddl.Items.Insert(0, titulo);
		}
		
		public static void EnviarEmail(List<string> destinatarios, string assunto, string mensagem)
		{
			if (destinatarios.Count > 0)
			{
				MailMessage mailMsg = new MailMessage();
				mailMsg.IsBodyHtml = true;

				foreach (string destinatario in destinatarios)									
					mailMsg.To.Add(destinatario);							

				string topo = 
					@"<table>
						<tr>
							<td>
								<a href='http://intranet/licitar' target='blank'>
									<img border='0' src='http://devintranet/template/imagens/logo_licitar.jpg' />
								</a>
							</td>
						</tr>
					</table> <br/><br/>";

				mailMsg.Body =  topo + mensagem;   
				mailMsg.Subject = assunto;
				mailMsg.From = new MailAddress("danilo.meireles@gmail.com");
				SmtpClient smtp = new SmtpClient();				
				smtp.Send(mailMsg);
			}
		}
		
		public static void AddItemListBox(DropDownList ddl, ListBox lbx)
		{
			if (ddl.SelectedIndex > 0)
			{			
				if (ddl.SelectedIndex > 1)
				{
					if (!lbx.Items.Contains(ddl.SelectedItem))
					{
						lbx.Items.Add(ddl.SelectedItem);	
						lbx.SelectedItem.Selected = false;
					}
				}
				else if (ddl.SelectedItem.Text == "TODAS" || ddl.SelectedItem.Text == "TODOS")
				{
					for (int i = 2; i < ddl.Items.Count; i++)
					{
						if (!lbx.Items.Contains(ddl.Items[i]))
						{
							lbx.Items.Add(ddl.Items[i]);
						}
					}
				}
			}
		}
		
		public static void RemoverItemListBox(ListBox lbx)
		{			
			if (lbx.SelectedIndex != -1)
			{
				lbx.Items.Remove(lbx.SelectedItem);
			}
		}
		
		public static void LimparControlesPainel(Panel painel)
		{
			for (int i = 0; i < painel.Controls.Count; i++)
			{
				if (painel.Controls[i] is TextBox) 
					(painel.Controls[i] as TextBox).Text = string.Empty;
				
				if (painel.Controls[i] is DropDownList) 
					(painel.Controls[i] as DropDownList).SelectedIndex = 0;
				
				if (painel.Controls[i] is ListBox) 
					(painel.Controls[i] as ListBox).Items.Clear();
			}				
		}
		
		public static bool ValidarData(string data)
		{
			DateTime dataConvertida = new DateTime();			
			return DateTime.TryParse(data, out dataConvertida);
		}
		
		public static bool ValidarCPF(string cpf)
		{
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;
			int soma;
			int resto;
			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "");
			if (cpf.Length != 11)
				return false;
			tempCpf = cpf.Substring(0, 9);
			soma = 0;
			for(int i=0; i<9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
			resto = soma % 11;
			if ( resto < 2 )
				resto = 0;
			else
				resto = 11 - resto;
			digito = resto.ToString();
			tempCpf = tempCpf + digito;
			soma = 0;
			for(int i=0; i<10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = digito + resto.ToString();
			return cpf.EndsWith(digito);
		}
		
		public static bool ValidarCNPJ(string cnpj)
		{
			int[] multiplicador1 = new int[12] {5,4,3,2,9,8,7,6,5,4,3,2};
			int[] multiplicador2 = new int[13] {6,5,4,3,2,9,8,7,6,5,4,3,2};
			int soma;
			int resto;
			string digito;
			string tempCnpj;
			cnpj = cnpj.Trim();
			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
			if (cnpj.Length != 14)
				return false;
			tempCnpj = cnpj.Substring(0, 12);
			soma = 0;
			for(int i=0; i<12; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
			resto = (soma % 11);
			if ( resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = resto.ToString();
			tempCnpj = tempCnpj + digito;
			soma = 0;
			for (int i = 0; i < 13; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = digito + resto.ToString();
			return cnpj.EndsWith(digito);
		}
		
		public static bool ValidarPis(string pis)
		{
			int[] multiplicador = new int[10] { 3,2,9,8,7,6,5,4,3,2 };
			int soma;
			int resto;
			if (pis.Trim().Length == 0)
				return false;
			pis = pis.Trim();
			pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');            
			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += int.Parse(pis[i].ToString()) * multiplicador[i];
			resto = soma % 11;
			if ( resto < 2 )
				resto = 0;
			else
				resto = 11 - resto;
			return pis.EndsWith(resto.ToString());
		}
	}
}