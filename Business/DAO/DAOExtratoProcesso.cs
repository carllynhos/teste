using System;
using System.Data;
using Licitar.Business.Persistencia;

namespace Licitar.Business.Dao
{
	public class DAOExtratoProcesso
	{
		public DataSet GetDatas(int idProcesso)
		{
			string select = @"

					select dat_cadastro_pan as cadastrado, dat_abertura_propostas_pan as aberturapropostas,
					dat_realizacao_pan as disputa, dat_adjudicacao_pan as adjudicado, dat_homologado_pan as homologado,
					dat_conclusao_pan as concluido, dat_encaminhado_setorial_tue as devolucao from adm_licitar.tb_processo_completo_pcm
					where cod_processo_pro = @idProcesso
							";
			select = select.Replace("@idProcesso", idProcesso.ToString());
			return new PostgreSqlDatabase().ExecutarConsulta(select);
				
		}
	}
}
