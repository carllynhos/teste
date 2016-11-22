using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Licitar.Business.Utilidade
{
	public class UtdDropDownList
	{
	    /// <summary>
	    /// Carrega um DropDownList.
	    /// </summary>
	    /// <param name="dropDownList">O DropDownList a ser carregado</param>
	    /// <param name="dataValueField">O nome do campo Id</param>
	    /// <param name="dataTextField">O nome do campo descritivo</param>
	    /// <param name="defaultText">O texto a ser exibido na posição 0. Ex. 'Selecione...'</param>
	    /// <param name="dataSource">A fonte de dados para o DropDownList</param>
	    public static void CarregarDropDownList(DropDownList dropDownList, string dataValueField, string dataTextField, string defaultText, object dataSource)
	    {
	        dropDownList.DataValueField = dataValueField;
	        dropDownList.DataTextField = dataTextField;
	        dropDownList.DataSource = dataSource;
	        dropDownList.DataBind();
	        dropDownList.Items.Insert(0, defaultText);
	    }

	    public static void CarregarDropDownList(DropDownList dropDownList, string dataValueField, string dataTextField, object dataSource)
	    {
	        dropDownList.DataValueField = dataValueField;
	        dropDownList.DataTextField = dataTextField;
	        dropDownList.DataSource = dataSource;
	        dropDownList.DataBind();        
	    }

	    public static void CarregarDropDownList(DropDownList dropDownList, string defaultText, object dataSource)
	    {        
	        dropDownList.DataSource = dataSource;
	        dropDownList.DataBind();
	        dropDownList.Items.Insert(0, defaultText);
	    }

	    public static void CarregarDropDownList(DropDownList dropDownList, object dataSource)
	    {        
	        dropDownList.DataSource = dataSource;
	        dropDownList.DataBind();
	    }
	}
}
