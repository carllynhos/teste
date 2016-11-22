// Ordenador.cs created with MonoDevelop
// User: diogolima at 14:11 14/1/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;

namespace Licitar.Business.Utilidade
{
public class Ordenador
	{
		public static void SortByValue(ListControl combo)
		{
			SortCombo(combo, new ComboValueComparer());
		}
		
		public static void SortByText(ListControl combo)
		{
		    SortCombo(combo, new ComboTextComparer());
		}		

		private static void SortCombo(ListControl combo, IComparer comparer)
		{
		    int i;
		    if (combo.Items.Count <= 1)
		        return;

		    ArrayList arrItems=new ArrayList();
		    for (i=0; i<combo.Items.Count; i++)
		    {
		        ListItem item=combo.Items[i];
		        arrItems.Add(item);
		    }

		    arrItems.Sort(comparer);

		    combo.Items.Clear();

		    for (i=0; i<arrItems.Count; i++)

		    {
				combo.Items.Add((ListItem) arrItems[i]);

			}
		}		

		private class ComboValueComparer : IComparer
		{
		    public enum SortOrder
		    {
		        Ascending=1,
		        Descending=-1
		    } 

		    private int _modifier; 

		    public ComboValueComparer()
		    {
				_modifier = (int) SortOrder.Ascending;
		    }

		    public ComboValueComparer(SortOrder order)
		    {
		        _modifier = (int) order;
		    } 

		    //sort by value
		    public int Compare(Object o1, Object o2)
		    {
		        ListItem cb1=(ListItem) o1;
		        ListItem cb2=(ListItem) o2;
		        return cb1.Value.CompareTo(cb2.Value)*_modifier;
		    }

		} //end class ComboValueComparer

		
		private class ComboTextComparer : IComparer
		{
		    public enum SortOrder
		    {
		        Ascending=1,
		        Descending=-1
		    }

		    private int _modifier; 

		    public ComboTextComparer()
		    {
		        _modifier = (int) SortOrder.Ascending;
		    } 

		    public ComboTextComparer(SortOrder order)
		    {
		        _modifier = (int) order;
		    } 

		    //sort by value
		    public int Compare(Object o1, Object o2)
		    {
		        ListItem cb1=(ListItem) o1;
		        ListItem cb2=(ListItem) o2;
		        return cb1.Text.CompareTo(cb2.Text)*_modifier;
		    }

		} //end class ComboTextComparer
		
	}
}
