using System;
using System.Collections;
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
	class VideoFilters
	{
		List<VideoFilter> videoFilters = new List<VideoFilter>();
		ComboBox comboBox;
		ComponentsParser componentsParser;
		int selectedIndex = 0;

		public event Action frameFilterEnd;
		public event Action frameFilterStart;

		public VideoFilters(ComboBox comboBox, ComponentsParser componentsParser)
		{
			this.comboBox = comboBox;
			this.componentsParser = componentsParser;
			VideoFilter videoFilter = new VideoFilter("Отключён", "OFF", componentsParser);
			videoFilters.Add(videoFilter);
			comboBox.Invoke((MethodInvoker)delegate
			{
				comboBox.Items.Add(videoFilter.Name);
				comboBox.SelectedIndex = 0;
			});
			string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
			foreach (string path in allfiles)
			{
				addVideoFilterFromPath(path);
			}			
		}

		public void addVideoFilterFromPath(string path)
		{
			VideoFilter videoFilter = new VideoFilter(path, componentsParser);
			if (videoFilter.IdName != null)
			{
				videoFilters.Add(videoFilter);
				comboBox.Invoke((MethodInvoker)delegate
				{
					comboBox.Items.Add(videoFilter.Name);
				});
				componentsParser.parseComponents("INFOPANEL,LABEL," + videoFilter.IdName + "INFOLABEL,600,400," + videoFilter.PluginInfo);
			}
		}

		public void pluginOn()
		{
			videoFilters[selectedIndex].unsubscribeFunctions(ref frameFilterEnd, ref frameFilterStart);
			comboBox.Invoke((MethodInvoker)delegate
			{
				selectedIndex = comboBox.SelectedIndex;
			});
			videoFilters[selectedIndex].subscribeFunctions(ref frameFilterEnd, ref frameFilterStart);
			componentsParser.pluginOn(videoFilters[selectedIndex].IdName);
		}

		public Bitmap Filter(Bitmap img)
		{
			frameFilterStart?.Invoke();
			var sourceBuffer = new MemoryStream();
			img.Save(sourceBuffer, ImageFormat.Bmp);
			byte[] sourceMap = sourceBuffer.ToArray();
			byte[] resultMap = videoFilters[selectedIndex].filterFunct(sourceMap, sourceMap.Length);
			var resultBuffer = new MemoryStream(resultMap);
			frameFilterEnd?.Invoke();
			return new Bitmap(Image.FromStream(resultBuffer));			 
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
		public delegate IntPtr FilterFunctDLL(byte[] array, int lenth, char[] args);

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
		string version = "";
		string author = "";
		ComponentsParser componentsParser;
		Dictionary<string, StringString> pluginFunctions = new Dictionary<string, StringString>();

		Dictionary<string, Action> triggerFunctionsStart = new Dictionary<string, Action>();
		Dictionary<string, Action> triggerFunctionsEnd = new Dictionary<string, Action>();

		StringBuilder pluginInfoString = new StringBuilder();

		public string Name { get { return name; } }
		public string IdName { get { return idName; } }
		public string PluginInfo { get { return pluginInfoString.ToString(); } }

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
				this.version = callSSFunct("getVersion");
				this.author = callSSFunct("getAuthor");
				createPluginInfoString();
				componentsParser.AddPanel(this.idName + "MAIN");
				parseFunctions();
				pluginInfoString.Append("=====================================================================\r\n");
			}
		}

		void createPluginInfoString()
		{
			pluginInfoString.Append("UUID: " + this.idName + "\r\n");
			pluginInfoString.Append("Название: " + this.name + "\r\n");
			pluginInfoString.Append("Версия: " + this.version + "\r\n");
			pluginInfoString.Append("Автор: " + this.author + "\r\n");
		}

		void parseFunctions()
		{			
			string pluginFunctionsString = callSSFunct("pluginFunctions");
			string[] parsedPluginFunctionsString = pluginFunctionsString.Split(' ');
			foreach (var functionIdName in parsedPluginFunctionsString)
			{				
				componentsParser.parseComponents(callSSFunct("functionsInterfaceCFG", functionIdName));

				StringString function = getSSFunction(functionIdName);
				string functionArgsString = callSSFunct("functionsArgs", functionIdName);
				string functionTypeString = callSSFunct("functionsType", functionIdName);
				string functionTargetString = callSSFunct("functionsTarget", functionIdName);
				pluginFunctions.Add(functionIdName, function);
				if (functionTypeString != "MAIN")
				{
					Action action = () => { componentsParser.setComponentsValue(functionTargetString, function(componentsParser.createArgsString(functionArgsString))); };
					if (functionTypeString == "TRIGGEREND")
					{
						triggerFunctionsEnd.Add(functionIdName, action);
					}
					else if (functionTypeString == "TRIGGERSTART")
					{
						triggerFunctionsStart.Add(functionIdName, action);
					}
					else if (functionTypeString == "NONE")
					{	}
					else
					{
						componentsParser.addOnClickEvent(functionTypeString, action);
					}
				}
				else
				{
					this.filterFunct = getFilterFunctDLL(functionArgsString);
				}
				pluginInfoString.Append(functionIdName + " - " + callSSFunct("functionsDescriptions", functionIdName) + "\r\n");
			}
		}

		FilterFunct getFilterFunctDLL(string argsString)
		{
			IntPtr getProc = GetProcAddress((int)(this.pointerDll), "filterFunct");
			return (byte[] array, int Length) =>
			{
				FilterFunctDLL funct = (FilterFunctDLL)Marshal.GetDelegateForFunctionPointer(getProc, typeof(FilterFunctDLL));
				byte[] tmpByte = new byte[array.Length];
				Marshal.Copy(funct(array, array.Length, (componentsParser.createArgsString(argsString) + "\0").ToCharArray()), tmpByte, 0, array.Length);
				return tmpByte;
			};
		}

		public void subscribeFunctions(ref Action frameFilterEnd, ref Action frameFilterStart)
		{
			foreach (var triggerFunctEnd in triggerFunctionsEnd)
			{
				frameFilterEnd += triggerFunctEnd.Value;
			}
			foreach (var triggerFunctStart in triggerFunctionsStart)
			{
				frameFilterStart += triggerFunctStart.Value;
			}
		}

		public void unsubscribeFunctions(ref Action frameFilterEnd, ref Action frameFilterStart)
		{
			foreach (var triggerFunctEnd in triggerFunctionsEnd)
			{
				frameFilterEnd -= triggerFunctEnd.Value;
			}
			foreach (var triggerFunctStart in triggerFunctionsStart)
			{
				frameFilterStart -= triggerFunctStart.Value;
			}
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
