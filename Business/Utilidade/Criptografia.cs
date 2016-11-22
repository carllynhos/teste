// Criptografia.cs created with MonoDevelop
// User: marcelolima at 11:15 9/7/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//



using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Licitar.Business.Utilidade
{
	public class Criptografia : NameValueCollection
	{
		private string sTimeStampKey = "__ExpirationTime__";
		private DateTime dExpireTime = DateTime.MaxValue;
        private string sCryptoKey = "!Cr1Pt0Gr4F4r3sP4nkjkdsuC0sk" + string.Format("{0:dMdMyyyyMdMd}",DateTime.Now) + "nkjkdsuC0skl3EsPfteggh4";
		private byte[] IV = new byte[8] {240, 3, 45, 29, 0, 76, 173, 59}; 
		public Criptografia() : base() {
			this.ExpireTime = DateTime.Now.AddMinutes(2000);
		}

        public Criptografia(string encryptedString) 
		{
			Deserialize(Decrypt(encryptedString));
			// Compara a data da querystring com a data atual pra ver se j� expirou
			if (DateTime.Compare(ExpireTime, DateTime.Now) < 0) 
			{
				//throw new Exception("Autentica��o expirada.");
			}
		}

		public string criptografarSenhaSHA1(string senha) 
   		{ 
        	string senhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(senha, "sha1"); 
        	return senhaCriptografada; 
    	}

		/// <summary>
		/// Encripta a querystring.
		/// </summary>
		public string EncryptedString 
		{
			get 
			{
				return HttpUtility.UrlEncode(Encrypt(Serialize()));
			}
		}

		/// <summary>
		/// Data e hora que a querystring dever� expirar
		/// </summary>
		public DateTime ExpireTime 
		{
			get 
			{
				return dExpireTime;
			}
			set 
			{
				dExpireTime = value;
			}
		}

		/// <summary>
		/// Retorna a querystring encriptada.
		/// </summary>
		public override string ToString()
		{
			return EncryptedString;
		}

		/// <summary>
		/// Encripta e serializa a querystring 
		/// </summary>
		public string Encrypt(string serializedQueryString) 
		{
			byte[] buffer = Encoding.ASCII.GetBytes(serializedQueryString);
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
			des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(sCryptoKey));
			des.IV = IV;
			return Convert.ToBase64String(
				des.CreateEncryptor().TransformFinalBlock(
					buffer,
					0,
					buffer.Length
				)
			);
		}

		/// <summary>
		/// desencripta e serializa a querystring
		/// </summary>
		public string Decrypt(string encryptedQueryString) 
		{
			byte[] buffer = Convert.FromBase64String(HttpUtility.UrlDecode(encryptedQueryString));
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
			des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(sCryptoKey));
			des.IV = IV;
			return Encoding.ASCII.GetString(
				des.CreateDecryptor().TransformFinalBlock(
					buffer,
					0,
					buffer.Length
				)
			);
		}

		/// <summary>
		/// Deserializa a querystring.
		/// </summary>
		private void Deserialize(string decryptedQueryString) 
		{
			string[] nameValuePairs = decryptedQueryString.Split('&');
			for (int i=0; i<nameValuePairs.Length; i++) 
			{
				string[] nameValue = nameValuePairs[i].Split('=');
				if (nameValue.Length == 2) 
				{
					base.Add(nameValue[0], nameValue[1]);
				}
			}
			// Ensure that timeStampKey exists and update the expiration time.
			if (base[sTimeStampKey] != null) 
				dExpireTime = DateTime.Parse(base[sTimeStampKey]);
		}

		
		private string Serialize()
		{
			StringBuilder sb = new StringBuilder();
			foreach (string key in base.AllKeys) 
			{
				sb.Append(key);
				sb.Append('=');
				sb.Append(base[key]);
				sb.Append('&');
			}

			// adiciona a data/hora
			sb.Append(sTimeStampKey);
			sb.Append('=');
			sb.Append(dExpireTime);

			return sb.ToString();
		}


	}
}
