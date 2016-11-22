using System;

namespace Licitar.Business.Utilidade
{	
	public class UtdString
	{		
		public static string TrocarAcentosPorPorcento(string texto)
        {
            string comCaracteresIndesejados = "!@#$%š&*()-?:{}][ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semCaracteresIndesejados = "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%";
            
			for (int i = 0; i < comCaracteresIndesejados.Length; i++)
                texto = texto.Replace(comCaracteresIndesejados[i].ToString(), semCaracteresIndesejados[i].ToString()).Trim();          
			
            return texto;
        }
		
		public static string SubstituirCaracteresIndesejados(string texto)
        {
			Console.WriteLine("SubstituirCaracteresIndesejados");
			string letraA = "[AÄÅÁÂÀÃaäáâàã]";
			string letraE = "[EÉÊËÈeéêëè]";
			string letraI = "[IÍÎÏÌiíîïì]";
			string letraO = "[OÖÓÔÒÕoöóôòõ]";
			string letraU = "[UÜÚÛuüúûù]";
			string cedilha = "[CÇcç]";
			bool encontrouLetra = false;
			
			for (int i = 0; i < texto.Length; i++)
			{								
				Console.WriteLine("FOR TEXTO");
				
				if (!encontrouLetra)
				{
					for (int j = 0; j < letraA.Length; j++)
					{						
						if (texto[i] == letraA[j])
						{
							Console.WriteLine("FOR letraA");
							
							texto = texto.Replace(texto[i].ToString(), letraA);
							encontrouLetra = true;
							break;
						}
					}
				}
				
				if (!encontrouLetra)
				{
					for (int j = 0; j < letraE.Length; j++)
					{
						if (texto[i] == letraE[j])
						{
							Console.WriteLine("FOR letraE");
							
							texto = texto.Replace(texto[i].ToString(), letraE);
							encontrouLetra = true;
							break;
						}
					}
				}
				
				if (!encontrouLetra)					
				{
					for (int j = 0; j < letraI.Length; j++)
					{
						if (texto[i] == letraI[j])
						{
							Console.WriteLine("FOR letraI");
							
							texto = texto.Replace(texto[i].ToString(), letraI);
							encontrouLetra = true;
							break;
						}
					}
				}
				
				if (!encontrouLetra)
				{
					for (int j = 0; j < letraO.Length; j++)
					{
						if (texto[i] == letraO[j])
						{
							Console.WriteLine("FOR letraO");
							
							texto = texto.Replace(texto[i].ToString(), letraO);
							encontrouLetra = true;
							break;
						}
					}
				}
				
				if (!encontrouLetra)
				{
					for (int j = 0; j < letraU.Length; j++)
					{
						if (texto[i] == letraU[j])
						{
							Console.WriteLine("FOR letraU");
							
							texto = texto.Replace(texto[i].ToString(), letraU);
							encontrouLetra = true;
							break;
						}
					}
				}
				
				if (!encontrouLetra)
				{
					for (int j = 0; j < cedilha.Length; j++)
					{
						if (texto[i] == cedilha[j])
						{
							Console.WriteLine("FOR cedilha");
							
							texto = texto.Replace(texto[i].ToString(), cedilha);
							encontrouLetra = true;
							break;
						}
					}
				}
				
				encontrouLetra = false;
			}
			
			texto = texto.Replace(" ", "%");
			texto = "%" + texto + "%";
		
            return texto;
        }
	}
}
