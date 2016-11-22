
using System;
using System.Web;
using System.Net;

namespace Licitar.Business.Servico
{
	
	
	public class SrvFornecedor
	{
		private WebProxy myProxy;
		private FacadeService facade = new FacadeService();
	
		
		public SrvFornecedor(HttpRequest request)
		{
			if (request.UserHostName == "127.0.0.1" || request.UserHostName == "devlicitar") {
				myProxy = new WebProxy("http://proxy.pge.ce.gov.br:3128",true);
				myProxy.Credentials = new NetworkCredential("edivansoares","set1m0ssever0");
				facade.Proxy = myProxy;
			}
		}
		
		public Entidade.Pessoa getFornecedorPorCnpj(string cnpj)
		{
			var fornecedorService = facade.getFornecedor(cnpj);
			Console.WriteLine("FORNECEDOR " + fornecedorService.cnpj_cpf.ToString() );
			Entidade.Pessoa pessoa = new Entidade.Pessoa();
			pessoa.CpfCnpj = fornecedorService.cnpj_cpf;
			pessoa.Nome = fornecedorService.razao_social;
			pessoa.Fax = fornecedorService.num_fax_rep;
			pessoa.Cidade = fornecedorService.municipio;
			pessoa.Endereco = fornecedorService.endereco;
			pessoa.Bairro = fornecedorService.bairro; 
			pessoa.Cep = fornecedorService.cep;
			pessoa.Telefone = fornecedorService.num_fone1;
			pessoa.Email = fornecedorService.email;
			return pessoa;
		}
		
		//Criei esse método para não ter que alterar o nome do método anterior para não quebrar a API.
		public Entidade.Pessoa getFornecedorPorCnpjCpf(string CnpjCpf) {
			return getFornecedorPorCnpj(CnpjCpf);
		}
	}
}