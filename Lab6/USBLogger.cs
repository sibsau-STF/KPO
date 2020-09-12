using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab6
	{
	public class USBLogger : IDisposable
		{
		string fileName = "loggs.txt";
		public List<string> Loggs;
		public event Action<string> AddMessage;

		static USBLogger instance;
		public static USBLogger Instance { get
				{
				if ( instance == null )
					instance = new USBLogger();
				return instance;
				} }

		USBLogger ()
			{
			Loggs = new List<string>();
			}

		public void LoadLogs ()
			{
			if (File.Exists(fileName))
				Loggs.AddRange(File.ReadAllLines(fileName));
			}

		public void AddLog(string message)
			{
			Loggs.Add(message);
			AddMessage?.Invoke(message);
			}

		public void ClearLogs ()
			{
			Loggs.Clear();
			WriteLogs();
			}

		public void WriteLogs ()
			{
			File.WriteAllLines(fileName, Loggs);
			}

		public void Dispose ()
			{
			File.WriteAllLines(fileName, Loggs);
			}
		}
	}
