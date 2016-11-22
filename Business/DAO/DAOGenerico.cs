// DAOGenerico.cs created with MonoDevelop
// User: marcelolima at 17:46 17/8/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Dao
{
	
	
	public class DAOGenerico
	{
		
		public DAOGenerico()
		{
		}

		public bool AtualizarTabelaoComPublicacaoEdital(int idProcesso, string idPublicacaoEdital)
		{
			bool retorno = false;
			string query = "update adm_licitar.tb_processo_completo_pcm set txt_publicacao_edital_pcm = '"
				+idPublicacaoEdital+"' where cod_processo_pro = "+idProcesso;
			Console.WriteLine("Query: "+query);
			PostgreSqlDatabase postgres = new PostgreSqlDatabase();
			postgres.ExecutarComando(query);
			retorno = true;
			return retorno;
		}
	}
}
