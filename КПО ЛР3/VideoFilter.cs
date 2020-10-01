using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КПО_ЛР3
{
	

	class VideoFilters
	{
		List<VideoFilter> videoFilters = new List<VideoFilter>();
		ComboBox comboBox;
		public VideoFilters(ComboBox comboBox)
		{
			this.comboBox = comboBox;
			VideoFilter last;
			last = new VideoFilter("Отключён", (byte[] array, int length) => array);
			videoFilters.Add(last);
			comboBox.Invoke((MethodInvoker)delegate
			{
				comboBox.Items.Add(last.Name);
				comboBox.SelectedIndex = 0;
			});
			string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
			foreach (string filename in allfiles)
			{
				last = new VideoFilter(filename);
				if (last.Name != null)
				{
					videoFilters.Add(last);
					comboBox.Invoke((MethodInvoker)delegate
					{
						comboBox.Items.Add(last.Name);
					});
				}
			}			
		}

		public byte[] Filter(byte[] array)
		{
			int selectedIndex = 0;
			comboBox.Invoke((MethodInvoker)delegate
			{
				selectedIndex = comboBox.SelectedIndex;				
			});
			return videoFilters[selectedIndex].filterFunct(array, array.Length);				 
		}
	}
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

		public FilterFunct filterFunct;		
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
	}
}
