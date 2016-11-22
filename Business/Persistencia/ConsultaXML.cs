using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Licitar.Business.Dao
{	
    public class ConsultaXML
    {			
		public static string retornarSQL(string idComando)
		{
			string comando = null;            
            XPathDocument poDocumentoXML = new XPathDocument(AppDomain.CurrentDomain.BaseDirectory + "/xml/Consultas.xml");
            XPathNavigator poConsultaXML = poDocumentoXML.CreateNavigator();
            XPathNodeIterator poListaInstrucao = poConsultaXML.Select("/Instrucoes/Instrucao[@id='" + idComando + "']");
            
            if (poListaInstrucao.MoveNext())
                comando = poListaInstrucao.Current.Value;

            return comando;
		}
		public static string retornarLinks(string idComando)
		{
			string comando = null;            
            XPathDocument poDocumentoXML = new XPathDocument(AppDomain.CurrentDomain.BaseDirectory + "/xml/LinksVariados.xml");
            XPathNavigator poConsultaXML = poDocumentoXML.CreateNavigator();
            XPathNodeIterator poListaInstrucao = poConsultaXML.Select("/Instrucoes/Instrucao[@id='" + idComando + "']");
            
            if (poListaInstrucao.MoveNext())
                comando = poListaInstrucao.Current.Value;

            return comando;
		}
    }
}
