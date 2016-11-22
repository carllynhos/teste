
using System;
using System.Collections;
using Licitar.Business.Entidade;
using Licitar.Business.Servico;
using System.IO;
using System.Diagnostics;

namespace Licitar.Business
{


	public class SrvDigitalizacao
	{

		//Método para listar os arquivos digitalizados de um processo
		public static ArrayList listarArquivosDigitalizados(int idProcesso)
		{
			
			ArrayList array = SrvProcessoAndamento.ListarArquivosDigitalizados(idProcesso);
			ArrayList arrayGrid = new ArrayList();
			foreach (ProcessoAndamento andamento in array)
			{
				DTOArquivoVirtual dto = new DTOArquivoVirtual();
				dto.DescricaoAndamento = andamento.FluxoAndamento.Atividade.Descricao;
				dto.IdAndamento = andamento.Id.ToString();
				dto.ObservacaoAndamento = andamento.Andamento;
				arrayGrid.Add(dto);
			}
			
			return arrayGrid;
			
		}
		
		//Método para listar os arquivos digitalizados de um processo.
		public static ArrayList listarArquivosDigitalizados(ProcessoAndamento processoAndamento)
		{
			int idProcesso = processoAndamento.Processo.Id;
			
			return listarArquivosDigitalizados(idProcesso);
		}
		
		//Método que verifica se já foi criado arquivo no repositório
		public static bool JaCriouArquivoRepositorio(string nomeArquivo, string mapPath )
		{
			string enderecoRepositorio = mapPath;
			string[] files = Directory.GetFiles(enderecoRepositorio);
			foreach (string file in files)
			{
				if (file.Contains(nomeArquivo))
					return true;
			}
			return false;
		}
		
		//Método que convert arquivo pdf em SWF mapPathCaminhoPDF[Server.MapPath("." + @"\Arquivo\Digitalizacao\idAndamento" + @"\").Replace("idAndamento",idAndamento)] [Server.MapPath("../Flexpaper/repositorio/")]
		public static void convertPDFtoSWF(string mapPathCaminhoPDF, string mapPathPastaRepositorioSWF)
		{
			DirectoryInfo DirInfo = new DirectoryInfo(mapPathCaminhoPDF);
            FileInfo[] AllFiles = DirInfo.GetFiles();
			string nome_arq = "";
			FileInfo arquivo = null;
            foreach (FileInfo file in AllFiles)
			{
            	arquivo = file;
			}
			
			jaCriouPasta(mapPathPastaRepositorioSWF);
			Process myProcess = new Process();
			myProcess.StartInfo.FileName = "pdf2swf " + arquivo.FullName +" -o "+ mapPathPastaRepositorioSWF + arquivo.Name.Replace("pdf","swf");
			myProcess.Start();
			myProcess.WaitForExit();
			
		}
		
		//Gera Endereço da Regra 500 apartir do idProcesso e idAndamento
		public static string getEnderecoRegraRepositorio(int idProcesso, int idAndamento)
		{
			int regra500 = idProcesso/500;
            string caminho = regra500 + @"/" + idProcesso + @"/" + idAndamento + @"/";
			return caminho;
		}
		
		//Gera Endereço da Regra 500 apartir do idProcesso e idAndamento
		public static string getEnderecoRegraRepositorio( int idAndamento)
		{
			ProcessoAndamento processoAndamento = ProcessoAndamento.Find(idAndamento);
			return getEnderecoRegraRepositorio(processoAndamento.Processo.Id,processoAndamento.Id);
		}
		
		
		//Retorna o nome do arquivo do mapPath com endereço completo ou não
		public static string getNomeArquivoEnderecoCompleto(string mapPath, bool enderecoCompleto)
		{
			DirectoryInfo DirInfo = new DirectoryInfo(mapPath);
            FileInfo[] AllFiles = DirInfo.GetFiles();
			string nome_arq = "";
            foreach (FileInfo FileTxt in AllFiles)
			{
            	nome_arq = FileTxt.Name;
			}
			
			string[] caminhoArquivo= null;
			if (!enderecoCompleto)
			{
				caminhoArquivo = nome_arq.Split('/');
				nome_arq = caminhoArquivo[caminhoArquivo.Length-1].ToString();
			}
				
			return nome_arq;
		}
		
		//Retorna o arquivo FILE INFO
		public static FileInfo getArquivoFileInfo(string mapPath)
		{
			DirectoryInfo DirInfo = new DirectoryInfo(mapPath);
            FileInfo[] AllFiles = DirInfo.GetFiles();
			FileInfo retorno = null;
			
            foreach (FileInfo FileTxt in AllFiles)
			{
            	retorno = FileTxt;
			}
			return retorno;
		}
		
		//Verifica se já foi criada a pasta em um diretorio, caso não tenha sido ele cria
		public static void jaCriouPasta(string mapPath)
		{
			if (!Directory.Exists(mapPath))
				Directory.CreateDirectory(mapPath);
		}
	}
}
