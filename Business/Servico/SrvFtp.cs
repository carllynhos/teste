// SrvFtp.cs created with MonoDevelop
// User: guilhermefacanha at 17:03 3/9/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace Licitar.Business.Servico
{	
	public class SrvFtp
	{
		static string uri = "ftp://intsrv022.etice.ce.gov.br/site/arquivos/licita/atas/concorrencias/cel03";
		static string uri2 = "ftp://intsrv022.etice.ce.gov.br/site/";
		
		public static bool uploadArquivoParaSite()
		{
			try
			{
				string caminhoUpload = "/home/cti/guilhermefacanha/Desktop/apostila C.pdf";
				string targetFtp = "ftp://intsrv022.etice.ce.gov.br/site/arquivos/licita/atas/concorrencias/cel03/apostila C.pdf";
				
				FileInfo toUpload = new FileInfo(caminhoUpload);

				FtpWebRequest req = (FtpWebRequest)WebRequest.Create(targetFtp);
				req.Method = WebRequestMethods.Ftp.UploadFile;
				req.Credentials = new NetworkCredential("pge","Peg572m");

				Stream ftpStream = req.GetRequestStream();

				FileStream file = File.OpenRead(caminhoUpload);

				int length = 1024;
				byte[] buffer = new byte[length];
				int bytesRead = 0;

				do
				{
					bytesRead = file.Read(buffer,0,length);
					ftpStream.Write(buffer,0,bytesRead);
				}
				while(bytesRead!=0);

				file.Close();
				ftpStream.Close();		
				
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public static void Upload(string filename)

		{
			filename = "/home/cti/guilhermefacanha/Desktop/apostila C.pdf";
		    FileInfo fileInf = new FileInfo(filename);
		
		    uri = "ftp://intsrv022.etice.ce.gov.br/site/arquivos/licita/atas/concorrencias/cel03" + fileInf.Name;
		
		    FtpWebRequest reqFTP; 
		
		    // Create FtpWebRequest object from the Uri provided
		
		    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://intsrv022.etice.ce.gov.br/site/arquivos/licita/atas/concorrencias/cel03/" + fileInf.Name));
		
		    // Provide the WebPermission Credintials
		
		    reqFTP.Credentials = new NetworkCredential("pge","Peg572m");
			
		    // By default KeepAlive is true, where the control connection is not closed
		
		    // after a command is executed.
		
		    reqFTP.KeepAlive = false; 
		
		    // Specify the command to be executed.
		
		    reqFTP.Method = WebRequestMethods.Ftp.UploadFile; 
		
		    // Specify the data transfer type.
		
		    reqFTP.UseBinary = true;
		
		    // Notify the server about the size of the uploaded file
		
		    reqFTP.ContentLength = fileInf.Length; 
		
		    // The buffer size is set to 2kb
		
		    int buffLength = 2048;
		
		    byte[] buff = new byte[buffLength];
		
		    int contentLen; 
		
		    // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
		
		    FileStream fs = fileInf.OpenRead(); 
		
		    try
		
		    {
		
		        // Stream to which the file to be upload is written
		
		        Stream strm = reqFTP.GetRequestStream(); 
		
		        // Read from the file stream 2kb at a time
		
		        contentLen = fs.Read(buff, 0, buffLength); 
		
		        // Till Stream content ends
		
		        while (contentLen != 0)
		
		        {
		
		            // Write Content from the file stream to the FTP Upload Stream
		
		            strm.Write(buff, 0, contentLen);
		
		            contentLen = fs.Read(buff, 0, buffLength);
		
		        } 
		
		        // Close the file stream and the Request Stream
		
		        strm.Close();
		
		        fs.Close();
		
		    }
		
		    catch (Exception ex)		
		    {		
		        Console.WriteLine(ex.Message);	
		    }
		
		}

		public static void ftpfile()  
	    {
			 try
			 {
				string filename = "/home/cti/guilhermefacanha/Desktop/apostila C.pdf";
		        string ftpfullpath = @"ftp://intsrv022.etice.ce.gov.br/"; 
		        FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);  
		        ftp.Credentials = new NetworkCredential("pge","Peg572m");
		        //userid and password for the ftp server to given  
		     
		        ftp.KeepAlive = true;  
		        ftp.UseBinary = true;  
		        ftp.Method = WebRequestMethods.Ftp.UploadFile;  
		        FileStream fs = File.OpenRead(filename);  
		        byte[] buffer = new byte[fs.Length];  
		        fs.Read(buffer, 0, buffer.Length);  
		        fs.Close();  
		        Stream ftpstream = ftp.GetRequestStream();  
		        //ftpstream.Write(buffer, 0, buffer.Length);  
		        ftpstream.Close();
		    }
			catch (Exception ex)		
		    {		
		        Console.WriteLine(ex.Message);	
		    }
		
		}

		public static void GetFileList()

		{
		
		    string[] downloadFiles;
		
		    StringBuilder result = new StringBuilder();
		
		    FtpWebRequest reqFTP;
		
		    try
		
		    {
		
		        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri2));
		
		        reqFTP.Credentials = new NetworkCredential("pge","Peg572m");
		
		        reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
		
		        WebResponse response = reqFTP.GetResponse();
		
		        StreamReader reader = new StreamReader(response.GetResponseStream());
		
		        string line = reader.ReadLine();
		
		        while (line != null)
		
		        {
					Console.WriteLine(line);
		
		            line = reader.ReadLine();
		
		        }
				
		        reader.Close();
		
		        response.Close();
		
		    }
		
		    catch (Exception ex)
		
		    {
		
		       Console.WriteLine(ex.Message);
		
		      		
		    }
	
		}
		
		public SrvFtp()
		{
		}
	}
}
