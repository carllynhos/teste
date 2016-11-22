// OrdenadorGenerico.cs created with MonoDevelop
// User: marcelolima at 08:34 14/7/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Reflection;
 
namespace Licitar.Business
{
	
	
	public class OrdenadorGenerico
	{

		   PropertyInfo propriedade;
	        bool ascendente;
	
	        public OrdenadorGenerico(Type getType, string colunaOrdenada, bool ascendente)
	        {
	                this.ascendente = ascendente;
	                propriedade = getType.GetProperty(colunaOrdenada);
	        }
	
	        public int Compare(object x, object y)
	        {
	                if(ascendente)
	                {
	                        return ( (IComparable)propriedade.GetValue(x, null) ).CompareTo(propriedade.GetValue(y, null));
	                }
	                else
	                {
	                        return ( (IComparable)propriedade.GetValue(y, null) ).CompareTo(propriedade.GetValue(x, null));
	                }
	        }
		
		
	}
}


