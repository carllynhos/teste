// SrvGerais.cs created with MonoDevelop
// User: guilhermefacanha at 15:03 24/3/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections;

namespace Licitar.Business.Servico
{
	
	public class SrvGerais
	{
		public static void CarregarDropDownList(DropDownList ddl,object objConsulta, string texto, string valor, string textoPadrao)
		{
			ddl.DataTextField = texto;
			ddl.DataValueField = valor;
			ddl.DataSource = objConsulta;
			ddl.DataBind();
			ddl.Items.Insert(0, new ListItem(textoPadrao,""));
			
			if(ddl.Items.Count == 2)
			{
				ddl.SelectedIndex = 1;
			}
			
		}

		public static void CarregarDropDownList(DropDownList ddl,object objConsulta, string texto, string valor)
		{
			ddl.DataTextField = texto;
			ddl.DataValueField = valor;
			ddl.DataSource = objConsulta;
			ddl.DataBind();
			ddl.Items.Insert(0,new ListItem("--Selecione um Item--",""));
			
			if(ddl.Items.Count == 2)
			{
				ddl.SelectedIndex = 1;
			}
			
		}

		public static void verificarDdl(DropDownList ddl)
		{
			if(ddl.Items.Count == 2)
			{
				ddl.Enabled = false;
			}
			else
			{
				ddl.Enabled = true;
			}
		}

		public static void AdicionarItemPadraoAcima(DropDownList ddl)
		{
			ddl.Items.Insert(0,new ListItem("--Selecione um Item Acima--",""));
		}

		public static void AdicionarItemPadrao(DropDownList ddl)
		{
			ddl.Items.Insert(0,new ListItem("--Selecione um Item--",""));
		}

		public static void AdicionarItemLista(ListBox ltb, DropDownList ddl)
		{
			if(ddl.SelectedIndex>0)
			{
				ListItem item = new ListItem(ddl.SelectedItem.Text,ddl.SelectedValue);
				
				if(item.Value != "0")
				{
					ListItem itemAux = ltb.Items.FindByValue(item.Value);
					if(itemAux == null)
					{
						ltb.Items.Add(item);
						if(ltb.Items.Count == 1)
						{
							ltb.Items[ltb.Items.IndexOf(item)].Selected = true;
						}
						
					}
				}
			}
		}
		
		public static void DeletarItemLista(ListBox ltb)
		{						
			while(ltb.SelectedIndex>=0)
			{
				ListItem item = new ListItem(ltb.SelectedItem.Text,ltb.SelectedValue);
				
				if(ltb.Items.Contains(item))
				{
					ltb.Items.Remove(item);
				}
			}	
			
		}

		public static string transformarListaEmString(List<string> lista)
		{
			string res = "";
			foreach(string s in lista)
			{
				res += s+",";
			}

			if(!string.IsNullOrEmpty(res))
				res = res.Remove(res.Length-1);

			return res;
		}

		public static string transformarListaEmString(List<int> lista)
		{
			string res = "";
			foreach(int i in lista)
			{
				res += i.ToString()+",";
			}

			if(!string.IsNullOrEmpty(res))
				res = res.Remove(res.Length-1);

			return res;
		}

		public static string transformarListaEmStringValor(ListBox ltb)
		{
			string res = "";
			foreach(ListItem i in ltb.Items)
			{
				res+=i.Value+",";
			}
			if(!string.IsNullOrEmpty(res))
				res = res.Remove(res.Length-1);

			return res;
		}

		public static string transformarListaEmStringTexto(ListBox ltb)
		{
			string res = "";
			foreach(ListItem i in ltb.Items)
			{
				res+=i.Text+",";
			}
			if(!string.IsNullOrEmpty(res))
				res = res.Remove(res.Length-1);

			return res;
		}

		public static void avisar(Label lbl, string msg, bool erro)
		{
			lbl.Visible = true;
			lbl.Text = msg;
			if(erro)
				lbl.CssClass = "aviso_mau";
			else
				lbl.CssClass = "aviso_bom";
		}

		public static ArrayList tranformarItensEmArrayDeValores(ListBox ltb)
		{
			ArrayList array = new ArrayList();
			
			foreach(ListItem i in ltb.Items)
			{
				int num = 0;
				if(int.TryParse(i.Value,out num))
				{
					array.Add(num);
				}
			}
			
			return array;
		}

		public static void adicinarTodosItensPai(DropDownList ddlPai, DropDownList ddlFilho, ListBox ltb)
		{
			if(ddlPai.SelectedIndex>0)
			{
				foreach(ListItem item in ddlFilho.Items)
				{
					ListItem itemAux = ltb.Items.FindByValue(item.Value);
					if(itemAux==null)
					{
						if(!string.IsNullOrEmpty(item.Value))
							ltb.Items.Add(item);
					}
				}
			}
		}

		public static string RemoverCaracteresIndesejados(string texto)
        {
            
			string comCaracteresIndesejados = "!@#$%š&*()-?:{}][ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç ";
            string semCaracteresIndesejados = "_________________AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc_";
            
            for (int i = 0; i < comCaracteresIndesejados.Length; i++)
            {
                texto = texto.Replace(comCaracteresIndesejados[i].ToString(), semCaracteresIndesejados[i].ToString()).Trim();
            }
		
		return texto;
		}
		
		public SrvGerais()
		{
		}
	}
}
