
using System;

namespace Licitar.Business.Utilidade
{
	
	
	public class Data
	{
		public static DateTime RetornaDataNow()			
        {			
             string strData ="";
             strData = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
             return Convert.ToDateTime(strData);
         }

		
		public static DateTime RetornaData(DateTime data)			
        {            
			 DateTime dt = new DateTime(data.Year,data.Month,data.Day,data.Hour,data.Minute,data.Second);			
             return dt;
        }		
	}
}
