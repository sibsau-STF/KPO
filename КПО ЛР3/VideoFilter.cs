using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КПО_ЛР3
{
	class VideoFilter
	{
		[DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
		static extern int LoadLibrary(
			[MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

		[DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
		static extern IntPtr GetProcAddress(int hModule,
			[MarshalAs(UnmanagedType.LPStr)] string lpProcName);

		[DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
		static extern bool FreeLibrary(int hModule);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr FilterFunctDLL(byte[] array, int lenth);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate byte[] FilterFunct(byte[] array, int lenth);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate IntPtr DllGetInfo();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate int Calc();

		string dllPath;
		string name = null;
		public string Name { get { return name; } }


		public Bitmap Filter(Bitmap img)
			{
			var sourceBuffer = new MemoryStream();
			img.Save(sourceBuffer, ImageFormat.Bmp);

			byte[] sourceMap = sourceBuffer.ToArray();
			byte[] resultMap = filterFunct(sourceMap, sourceMap.Length);

			var resultBuffer = new MemoryStream(resultMap);
			return new Bitmap(Image.FromStream(resultBuffer));
			}

		FilterFunct filterFunct;	
			
		public VideoFilter(string name, FilterFunct filterFunct)
		{
			this.name = name;
			this.filterFunct = filterFunct;
		}

		public VideoFilter(string dllPath)
		{
			
			this.dllPath = dllPath;
			int pDll = LoadLibrary(dllPath);
			IntPtr getProc;
			if (pDll != null)
			{
				getProc = GetProcAddress(pDll, "getInfo");
				if ((int)getProc != 0)
				{
					DllGetInfo GetInfo = (DllGetInfo)Marshal.GetDelegateForFunctionPointer(getProc, typeof(DllGetInfo));
					var tmpResult = Marshal.PtrToStringAnsi(GetInfo());
					if (tmpResult != "" && tmpResult != null)
					{
						name = tmpResult;
						getProc = GetProcAddress(pDll, "filterFunct");
						if ((int)getProc != 0)
						{
							
							filterFunct = (byte[] array, int Length) => {
								var funct = (FilterFunctDLL)Marshal.GetDelegateForFunctionPointer(getProc, typeof(FilterFunctDLL));
								byte[] tmpByte = new byte[array.Length];
								Marshal.Copy(funct(array, array.Length), tmpByte, 0, array.Length);								
								return tmpByte;
							};						
						}
					}
				}
			}
		}

		public override string ToString ()
			{
			return name;
			}
		}
}
