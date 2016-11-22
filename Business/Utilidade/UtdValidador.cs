
using System;

namespace Licitar.Business.Utilidade
{
	public class UtdValidador
	{
		public static bool ValidarData(string data)
		{
			DateTime dataConvertida = new DateTime();			
			return DateTime.TryParse(data, out dataConvertida);
		}
	}
}
