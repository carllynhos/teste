/*
 * 
 * Criador: Danilo Meireles
 * Data da Criação: 15/03/2008
 * 
 * 
 */ 

using System;

namespace Licitar.Business.Utilidade
{
	/// <class>Validador</class>
	/// <summary>
	/// Classe para validação de valores diversos.
	/// </summary>
	public class Validador
	{
		/// <summary>
		/// Verifica se uma data é válida, e se está dentro de um período.
		/// </summary>
		public static bool ValidarData(string data, DateTime dataMinima, DateTime dataMaxima)
		{	
			try
			{
				string dia = data.Substring(0, 2);
				string barra1 = data.Substring(2, 1);
				string mes = data.Substring(3, 2);
				string barra2 = data.Substring(5, 1);
				string ano = data.Substring(6, 4);				
				
				//verifica se a data possui 10 caracteres:			
				if ((data.Length < 10) || (data.Length > 10))
				{
					return false;
				}	
				//verifica se a data está no formato dd/mm/aaaa:
				if ((!barra1.Equals("/")) || (!barra2.Equals("/")))
				{
					return false;
				}			
				//verifica o intervalo do dia:
				if ((Convert.ToInt32(dia) < 1) || (Convert.ToInt32(dia) > 31))
				{
					return false;
				}
				//verifica o intervalo do mês:
				if ((Convert.ToInt32(mes) < 1) || (Convert.ToInt32(mes) > 12))
				{
					return false;
				}				
				//verifica se a data é menor que a data mínima ou maior que a data máxima: 
				DateTime oData = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), Convert.ToInt32(dia));
				if ((oData < dataMinima) || (oData > dataMaxima))
				{
					return false;
				}
			}
			catch(Exception)
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Verifica se um determinado CPF é válido.
		/// </summary>
		public static bool ValidarCPF(string cpf)
		{
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;
			int soma;
			int resto;
			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");
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
		
		/// <summary>
		/// Verifica se um determinado CNPJ é válido.
		/// </summary>
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
		
		/// <summary>
		/// Verifica se um determinado PIS é válido.
		/// </summary>
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
