
using System;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using Business.wsgestaomaterial.seplag.ce.gov.br;

namespace Licitar.Business.Servico
{
	public class SrvItem
	{
		private WebProxy myProxy;
		private GestaoMaterialFacadeService gestaoMaterial = new GestaoMaterialFacadeService();
		
		public SrvItem(HttpRequest request)
		{
			if (request.UserHostName == "127.0.0.1" || request.UserHostName == "devlicitar") {
				myProxy = new WebProxy("http://proxy.pge.ce.gov.br:3128",true);
				myProxy.Credentials = new NetworkCredential("edivansoares","set1m0ssever0");
				gestaoMaterial.Proxy = myProxy;
			}
		}
		
		public List<VwDadosWebService> consultarItem(string consulta, string tipo) {
			
			List<VwDadosWebService> lista = new List<VwDadosWebService>();
			
			if (tipo == "codigo"){
				object[] itens_codigo = gestaoMaterial.consultaItemPorCodigo(Convert.ToInt32(consulta));		
				for(int i=0;i<itens_codigo.Length;i++){
					VwDadosWebService a = (VwDadosWebService)itens_codigo.GetValue(i);
					lista.Add(a);		
				}
			}else{
				object[] itens_descricao = gestaoMaterial.consultaItemPorQualquerPalavra(consulta);
				for(int i=0;i<itens_descricao.Length;i++){
					VwDadosWebService a = (VwDadosWebService)itens_descricao.GetValue(i);
					lista.Add(a);		
				}
			}
			
			return lista;
		}
				
	}
}
