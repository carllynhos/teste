// pgTeste.aspx.cs created with MonoDevelop
// User: janiojunior at 14:00 12/9/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Web;
using System.Web.UI;

namespace Calendario_LIcitacao
{
	
	
	public partial class pgTeste : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(Request.QueryString["Date"] != null)
				Response.Write(Request.QueryString["Date"]);
			
			
		}
	}
}
