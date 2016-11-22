using System;

namespace Licitar.Business.Servico
{
	public class SrvData
	{
		public static string FormatarDataComHora(string date)
		{
            string dia="";
			string mes="";

		    if (date.ToString().Length == 17)
			{
				if ((date.ToString().Substring(1, 1) == "/"))
				{
						dia = "0" + date.Substring(0, 2);
				}

				if ((date.ToString().Substring(3, 1) == "/"))
				{	
						mes = "0" + date.Substring(2, 12);
				}
				date = dia + mes;
			 }

		     else if (date.ToString().Length == 18)
			 {

				 if (!(date.ToString().Substring(2, 1) == "/"))
				 {
						dia = "0" + date.Substring(0, 2);
						mes = date.Substring(2, 13);
				 }

				 else
				 {
						dia = date.Substring(0, 3);
						mes = "0" + date.Substring(3, 12);
				 }
				 date = dia + mes;
		     }
			 if (date.ToString().Length == 19)
			 {
				date.Substring(0, 10);						
			 }
			return date;
		}
		
		
	}
}
