// Licitacao.cs created with MonoDevelop
// User: alberto at 17:34 7/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using NHibernate.Expression;

namespace Licitar.Business.Entidade
{	
	public class Licitacao
	{		
		public Licitacao()
		{
		}
		
		private int _processo;
		
		public int Processo
		{
			get { return _processo;}
			set { _processo = value;}
		}
		
		private string _instituicao;
		
		public string Instituicao
		{
			get { return _instituicao;}
			set { _instituicao = value;}
		}
		
		private string _situacao;
		
		public string Situacao
		{
			get { return _situacao;}
			set { _situacao = value;}
		}
		
//		public string UltimaSituacao()
//		{
//			get;
//			set;
//		}
//		
		private string _numeroSPU;
		
		public string NumeroSPU
		{
			get { return _numeroSPU;}
			set { _numeroSPU = value;}
		}
		
		private string _numeroEdital;
		
		public string NumeroEdital
		{
			get { return _numeroEdital;}
			set { _numeroEdital = value;}
		}
		
		private string _numeroLicitacao;
		
		public string NumeroLicitacao
		{
			get { return _numeroLicitacao;}
			set { _numeroLicitacao = value;}
		}
		
		private string _numeroPregao;
		
		public string NumeroPregao
		{
			get { return _numeroPregao;}
			set { _numeroPregao = value;}
		}
		
		private string _objeto;
				
		public string Objeto
		{
			get { return _objeto;}
			set { _objeto = value;}
		}
		
		private decimal _valor;
		
		public decimal Valor
		{
			get { return _valor;}
			set { _valor = value;}
		}
		
		private string _natureza;
				
		public string Natureza
		{
			get { return _natureza;}
			set { _natureza = value;}
		}
		
		private string _modalidade;
				
		public string Modalidade
		{
			get { return _modalidade;}
			set { _modalidade = value;}
		}
		
		private decimal? _valorEstimado;
			
		public decimal? ValorEstimado
		{
			get { return _valorEstimado;}
			set { _valorEstimado = value;}
		}		
		
		private decimal? _valorFracassado;
			
		public decimal? ValorFracassado
		{
			get { return _valorFracassado;}
			set { _valorFracassado = value;}
		}
		
		private decimal? _valorEstimadoReal;
			
		public decimal? ValorEstimadoReal
		{
			get { return _valorEstimadoReal;}
			set { _valorEstimadoReal = value;}
		}
		
		private decimal? _valorDeserto;
			
		public decimal? ValorDeserto
		{
			get { return _valorDeserto;}
			set { _valorDeserto = value;}
		}
		
		private decimal? _valorAnulado;
			
		public decimal? ValorAnulado
		{
			get { return _valorAnulado;}
			set { _valorAnulado = value;}
		}
		
		private decimal? _valorRevogado;
		public decimal? ValorRevogado
		{
			get {return _valorRevogado;}
			set {_valorRevogado = value; }
		}
		
		private decimal? _valorCancelado;
		public decimal? ValorCancelado
		{
			get{return _valorCancelado;}
			set{_valorCancelado = value;}
		}
		
		private string _dataRecebimento;
			
		public string DataRecebimento
		{
			get { return _dataRecebimento;}
			set { _dataRecebimento = value;}
		}
		
		private string _dataInformada;
			
		public string DataInformada
		{
			get { return _dataInformada;}
			set { _dataInformada = value;}
		}
		
		private string _dataRealizacao;
			
		public string DataRealizacao		
		{
			get { return _dataRealizacao;}
			set { _dataRealizacao = value;}
		}
		
		private decimal? _valorContratado;
			
		public decimal? ValorContratado
		{
			get { return _valorContratado;}
			set { _valorContratado = value;}
		}
		
		private decimal? _valorNaoContratado;
		
		public decimal? ValorNaoContratado
		{
			get{return _valorNaoContratado;}
			set{_valorNaoContratado = value;}
		}
		
		private decimal? _economia;
		
		public decimal? Economia
		{
			get { return _economia;}
			set { _economia = value;}
		}
		
		private string _observacao;
		
		public string Observacao
		{
			get { return _observacao;}
			set { _observacao = value;}
		}	

		private decimal? _economiaPorcentagem;
			
		public decimal? EconomiaPorcentagem
		{
			get { return _economiaPorcentagem;}
			set { _economiaPorcentagem = value;}
		}
		
		private string _dataCadastroAndamento;
		
		public string DataCadastroAndamento
		{
			get{return  this._dataCadastroAndamento ;}
			set{ this._dataCadastroAndamento = value; }
		}
		
		private string _dataAndamento;
		
		public string DataAndamento
		{
			get{return  this._dataAndamento ;}
			set{ this._dataAndamento = value; }
		}
		
		public string _fase;
		
		public string Fase
		{
			get{return _fase;}
			set{_fase = value;}
		}
		
		public string _vencedor;
		
		public string Vencedor
		{
			get{return _vencedor;}
			set{_vencedor = value;}
		}
		
		public string _tpLicitacao;
		
		public string TipoLicitacao
		{
			get{return _tpLicitacao;}
			set{_tpLicitacao = value;}
		}
		
		public string _nmPregoeiro;
		
		public string NmPregoeiro
		{
			get{return _nmPregoeiro;}
			set{_nmPregoeiro = value;}
		}
	}
}