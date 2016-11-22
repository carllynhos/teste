// pgCalendario.aspx.cs created with MonoDevelop
// User: janiojunior at 14:23 12/9/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Calendario_LIcitacao
{
	public partial class pgCalendario : System.Web.UI.Page
	{
		protected override void InitializeCulture()
		{
		    //get the culture information from wherever you want suppose it is a property
		    //with your user class

		    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("PT-BR");
		    Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("PT-BR");

		    base.InitializeCulture();
		}
	
	}
}
