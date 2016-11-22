// ucCalendarioPregao.ascx.cs created with MonoDevelop
// User: janiojunior at 09:57 12/9/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Configuration;

namespace Calendario_LIcitacao
{
	public partial class ucCalendarioPregao : System.Web.UI.UserControl
	{
		DateTime[] datas = null;
		
		protected bool getDiaPregao(DateTime data)
		{
			Console.WriteLine("Pegando Data");
			
			if (datas != null)
			{
				for (int i = 0; i <= datas.Length - 1; i++)
				{
					if (datas[i].Date == data.Date) return true;
				}
			}
			else
			{
				Console.WriteLine("retorno nulo");
			}
			
			return false;
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if(!IsPostBack)
			{
				datas = clDBCalendario.GetPregoesDia(System.DateTime.Now);
				callicitacao.SelectedDayStyle.BackColor = Color.LightGray;
				callicitacao.SelectedDate = DateTime.Now;
			}
			else
			{
				datas = clDBCalendario.GetPregoesDia(callicitacao.SelectedDate);
			}
		}
				
		protected void callicitacao_SelectionChanged(object sender, EventArgs e)
		{
		}
		
		protected void callicitacao_DayRender(object sender, DayRenderEventArgs e)
		{
			if(!e.Day.IsOtherMonth)
			{
				HyperLink lkn = new HyperLink();
				
				if(getDiaPregao(e.Day.Date))
					lkn.ForeColor = Color.Red;
				
				e.Cell.Controls.Clear();
				string atributo = ConfigurationManager.AppSettings["PageName"].ToString() +"?Date=" +e.Day.Date.Day.ToString() + "/" + e.Day.Date.Month.ToString() + "/" + e.Day.Date.Year.ToString();
				
				if(e.Day.IsToday)
					atributo += "&Atual=true";
				lkn.Attributes.Add("href", atributo);
				
				lkn.Target = ConfigurationManager.AppSettings["IframeName"].ToString();
				lkn.Text = e.Day.DayNumberText;	
				lkn.Font.Underline = false;
				e.Cell.Controls.Add(lkn);
				
//				if(e.Day.IsToday && !IsPostBack)
//					e.Cell.BackColor = Color.LightGray;
			}
			else
				e.Cell.Controls.Clear();
		}
	}
}
