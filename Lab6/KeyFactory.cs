using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab6
	{
	[Serializable]
	public struct Function
		{
		public int ID;
		public string Name;

		public Function (int id)
			{
			ID = id;
			Name = "Function " + id.ToString();
			}

		public override string ToString ()
			{
			return Name;
			}
		}

	[Serializable]
	public struct Key
		{
		public string SerialNumber;
		public string User;
		public DateTime UntilDate;
		public DateTime CreatedDate;
		public string KeyCreator;
		public List<Function> Functions;

		public Key (List<Function> funcs, string serialNum, string user, DateTime untillDate, string vendor = "KeyFactory 1.0")
			{
			SerialNumber = serialNum;
			User = user;
			UntilDate = untillDate;
			CreatedDate = DateTime.Now;
			Functions = funcs;
			KeyCreator = vendor;
			}
		public override string ToString ()
			{
			return $"{User} [{CreatedDate} - {UntilDate}]";
			}
		}

	public static class KeyFactory
		{

		public static void CreateKey(string fileName, Key key)
			{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(File.Open(fileName, FileMode.OpenOrCreate), key);
			}

		public static Key? ReadKey (string fileName)
			{
			Key key;
			BinaryFormatter formatter = new BinaryFormatter();
			try
				{
				key = (Key)formatter.Deserialize(File.Open(fileName, FileMode.OpenOrCreate));
				}
			catch ( System.Runtime.Serialization.SerializationException ex)
				{
				return null;
				}
			return key;
			}
		}
	}
