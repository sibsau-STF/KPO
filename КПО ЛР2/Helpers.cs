using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КПО_ЛР3
{
	static class Helpers
	{
		public static string byteArrayToString(byte[] byteArray)
		{
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < byteArray.Length; i++)
			{
				result.Append((char)byteArray[i]);
			}
			return result.ToString();
		}

		public static byte[] stringToByteArray(string str)
		{
			byte[] result = new byte[str.Length];
			for (int i = 0; i < str.Length; i++)
			{
				result[i] = (byte)str[i];
			}
			return result;
		}
	}
}
