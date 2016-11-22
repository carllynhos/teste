using System;
using System.Collections;
using System.Collections.Generic;
using Licitar.Business.Entidade;

namespace Licitar.Business.Servico
{	
	public class SrvOrdenacaoAtividade : IComparer<Atividade>
	{	
	    OrdenadorGenerico ordenador;
	
	    public SrvOrdenacaoAtividade(string colunaOrdenada, bool ascendente)
	    {
	        ordenador = new OrdenadorGenerico(typeof(Atividade), colunaOrdenada, ascendente);
	    }
	
	    public int Compare(Atividade x, Atividade y)
	    {
	        return ordenador.Compare(x, y);
	    }
	}
	
}