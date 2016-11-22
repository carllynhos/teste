
using System;
using System.Diagnostics;

namespace Licitar.Business.Servico
{
	
	
	public class SrvLog
	{
		
		public SrvLog(Exception ex, string controlName)
		{
			Console.WriteLine("aaaaaaaaaa");
			var trace = new StackTrace(ex,true);
			var frame = trace.GetFrame(0);
			var line = frame.GetFileLineNumber();
			
			string msg = "================[Erro no Sistema Licitar na Data de " + DateTime.Now.ToString() + "]================";
			msg += "\nExceção: " + ex.Message;
			msg += "\nDescrição: " + ex.ToString();
			msg += "\nPágina: " + controlName;
			msg += "\nMétodo: " + ex.TargetSite;
			msg += "\nLinha: " + line;
			msg += "\n============================================================================";
			Console.WriteLine(msg);
			
			string[] destinatario = { "desenvolvimento@pge.ce.gov.br" };
			SrvEmail.EnviarEmail(destinatario,"ERRO REPORTADO DO LICITAR - LICITAR", msg);
		}
	}
}
