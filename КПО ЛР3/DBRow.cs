using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КПО_ЛР3
	{
	struct DBRow
		{
		public string Name;
		public string Path;
		public DateTime Saved;
		public int ProceedTime;

		public DBRow (string name, string path, TimeSpan time)
			{
			Name = name;
			Path = path;
			Saved = DateTime.Now;
			ProceedTime = (int)time.TotalMilliseconds;
			}

		public override string ToString ()
			{
			return Name + '[' + ProceedTime + ']';
			}
		}
	}
