using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace КПО_ЛР3
	{
	struct ImageFile
		{
		public Bitmap Image;
		public string Name;
		public TimeSpan ProceedTime;

		public ImageFile (Bitmap img, string name)
			{
			Image = img;
			Name = name + ".bmp";
			ProceedTime = new TimeSpan(0);
			foreach ( var c in Path.GetInvalidFileNameChars() )
				Name = Name.Replace(c, '_');
			}

		string AddQuotesIfRequired (string path)
			{
			return !string.IsNullOrWhiteSpace(path) ?
				path.Contains(" ") && ( !path.StartsWith("\"") && !path.EndsWith("\"") ) ?
					"\"" + path + "\"" : path :
					string.Empty;
			}

		public void Save (string path)
			{
			string fullPath = Path.Combine(path, Name);
			fullPath = fullPath.Replace(" ", @" ");
			Image.Save(fullPath, ImageFormat.Bmp);
			}
		}

	}
