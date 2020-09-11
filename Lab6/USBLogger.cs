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
			Loggs.Clear();
			Loggs.AddRange(File.ReadAllLines(fileName));
			}

		public void AddLog(string message)
			{
			Loggs.Add(message);
			}

		public void WriteLogs ()
			{
			Loggs.Clear();
			Loggs.AddRange(File.ReadAllLines(fileName));
			}

		public void Dispose ()
			{
			File.WriteAllLines(fileName, Loggs);
			}
		}
	}
