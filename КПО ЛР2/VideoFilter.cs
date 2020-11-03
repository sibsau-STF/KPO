using System;
using System.Collections;
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
		ComponentsParser componentsParser;
		public VideoFilters(ComboBox comboBox, ComponentsParser componentsParser)
		{
			this.comboBox = comboBox;
			this.componentsParser = componentsParser;
			VideoFilter last = new VideoFilter("Отключён", "OFF", componentsParser);
			videoFilters.Add(last);
			comboBox.Invoke((MethodInvoker)delegate
			{
				comboBox.Items.Add(last.Name);
				comboBox.SelectedIndex = 0;
			});
			string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
			foreach (string filename in allfiles)
			{
				last = new VideoFilter(filename, componentsParser);
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

		int selectedIndex = 0;
		public void pluginOn()
		{
			comboBox.Invoke((MethodInvoker)delegate
			{
				selectedIndex = comboBox.SelectedIndex;
			});
			componentsParser.pluginOn(videoFilters[selectedIndex].IdName);
		}

		public byte[] Filter(byte[] array)
		{
			return videoFilters[selectedIndex].filterFunct(array, array.Length);				 
		}
	}
	class VideoFilter
	{
		#region importsDLL
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
		delegate IntPtr DllStringString(byte[] charArray);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate string StringString(string argsString);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate string StringVoid();

		#endregion importsDLL

		string dllPath;
		string name = null;
		string idName = null;
		int? pointerDll = null;
		ComponentsParser componentsParser;
		Dictionary<string, StringString> pluginFunctions = new Dictionary<string, StringString>();
		public string Name { get { return name; } }
		public string IdName { get { return idName; } }

		public FilterFunct filterFunct = (byte[] array, int length) => array;		
		public VideoFilter(string name, string idName, ComponentsParser componentsParser)
		{
			this.name = name;
			this.idName = idName;
			this.componentsParser = componentsParser;
			componentsParser.AddPanel(this.idName + "MAIN");
		}
		public VideoFilter(string dllPath, ComponentsParser componentsParser)
		{
			this.componentsParser = componentsParser;
			this.dllPath = dllPath;
			this.pointerDll = LoadLibrary(dllPath);

			DllStringString getIdName = getSSFunctFromDLL("getIdName");
			DllStringString getName = getSSFunctFromDLL("getName");
			if (getIdName != null && getName != null)
			{
				this.idName = callSSFunct(getIdName);
				this.name = callSSFunct(getName);
				componentsParser.AddPanel(this.idName + "MAIN");
				parseFunctions();
				this.filterFunct = getFilterFunctDLL();
			}
		}

		void parseFunctions()
		{			
			string pluginFunctionsString = callSSFunct("pluginFunctions");
			string[] parsedPluginFunctionsString = pluginFunctionsString.Split(' ');
			foreach (var function in parsedPluginFunctionsString)
			{				
				componentsParser.parseComponentsFromPlugin(this.idName, callSSFunct("functionsInterfaceCFG", function));
				//TODO: pluginFunctions.Add(function, getSSFunction() 
			}
		}

		FilterFunct getFilterFunctDLL()
		{
			IntPtr getProc = GetProcAddress((int)(this.pointerDll), "filterFunct");
			return (byte[] array, int Length) =>
			{
				FilterFunctDLL funct = (FilterFunctDLL)Marshal.GetDelegateForFunctionPointer(getProc, typeof(FilterFunctDLL));
				byte[] tmpByte = new byte[array.Length];
				Marshal.Copy(funct(array, array.Length), tmpByte, 0, array.Length);
				return tmpByte;
			};
			//TODO: Переделать на строковый тип
		}

		string callSSFunct(string functName, string argsString = "")
		{
			return callSSFunct(getSSFunctFromDLL(functName), argsString);
		}
		string callSSFunct(DllStringString funct, string argsString = "")
		{
			return getSSFunction(funct)(argsString);
		}
		StringString getSSFunction(string functName)
		{
			return (string argsString) => { return Marshal.PtrToStringAnsi(getSSFunctFromDLL(functName)(Encoding.ASCII.GetBytes(argsString))); };
		}
		StringString getSSFunction(DllStringString funct)
		{
			return (string argsString) => { return Marshal.PtrToStringAnsi(funct(Encoding.ASCII.GetBytes(argsString))); };
		}
		DllStringString getSSFunctFromDLL(string functName)
		{
			if (this.pointerDll != null)
			{
				IntPtr getProc = GetProcAddress((int)(this.pointerDll), functName);
				if ((int)getProc != 0)
				{
					return (DllStringString)Marshal.GetDelegateForFunctionPointer(getProc, typeof(DllStringString));
				}
			}
			//MessageBox.Show("Ошибка загрузки функции " + this.dllPath + " " + functName);
			return null;
		}
	}
}
