using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static КПО_ЛР3.VideoFilter;

namespace КПО_ЛР3
{
	class ComponentsParser
	{
		Dictionary<string, FlowLayoutPanel> panels = new Dictionary<string, FlowLayoutPanel>();
		Dictionary<string, Control> components = new Dictionary<string, Control>();
		//TODO: Удалить, если не пригодится
		//Dictionary<string, Dictionary<string, Control>> pluginsComponents = new Dictionary<string, Dictionary<string, Control>>();

		public void AddPanel(string name, FlowLayoutPanel flowLayoutPanel)
		{
			panels.Add(name, flowLayoutPanel);
		}
		public void AddPanel(string name)
		{
			var newPanel = new FlowLayoutPanel();
			newPanel.AutoSize = true;
			newPanel.Name = name;
			panels.Add(name, newPanel);
		}

		//public void AddPlugin(string pluginName)
		//{
		//	pluginsComponents.Add(pluginName, new Dictionary<string, Control>());
		//}

		void AddPluginComponent(string pluginName, string componentName, Control component)
		{
			//pluginsComponents[pluginName].Add(componentName, component);
			components.Add(componentName, component);
		}

		public void parseComponentsFromPlugin(string pluginName, string pluginComponentsInfo)
		{
			string[] splitedPluginComponentsInfo = pluginComponentsInfo.Split(';');
			foreach (var componentInfo in splitedPluginComponentsInfo)
			{
				Control tmpControl = parseComponentInfo(componentInfo);
				AddPluginComponent(pluginName, tmpControl.Name, tmpControl);
			}
		}

		//    "PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,...;PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,..."
		Control parseComponentInfo(string componentInfo)
		{
			Control component;
			string[] splitedComponentInfo = componentInfo.Split(',');
			switch (splitedComponentInfo[1])
			{
				case "LABEL":
					component = new Label();
					//TODO: добавление персональных параметров
					break;
				case "TEXTBOX":
					component = new TextBox();
					//TODO: добавление персональных параметров
					break;
				case "TRACKBAR":
					var trackBar = new TrackBar();
					trackBar.Minimum = Convert.ToInt32(splitedComponentInfo[6]);
					trackBar.Maximum = Convert.ToInt32(splitedComponentInfo[7]);
					component = trackBar;
					break;
				default:
					component = new Label();
					//TODO: добавление персональных параметров
					break;
			}
			component.Name = splitedComponentInfo[2];
			component.Size = new Size(Convert.ToInt32(splitedComponentInfo[3]), Convert.ToInt32(splitedComponentInfo[4]));
			component.Text = splitedComponentInfo[5];
			panels[splitedComponentInfo[0]].Controls.Add(component);
			return component;
		}

		public string createArgsString(string componentNames)
		{
			string[] parsedComponentNames = componentNames.Split(' ');
			StringBuilder argsString = new StringBuilder();
			foreach (var componentName in parsedComponentNames)
			{
				string typeOfComponent = components[componentName].GetType().ToString();
				switch (typeOfComponent)
				{
					case "System.Windows.Forms.TextBox":
						argsString.Append(";" + components[componentName].Text);
						break;
					case "System.Windows.Forms.Label":
						argsString.Append(";" + components[componentName].Text);
						break;
					case "System.Windows.Forms.TrackBar":
						argsString.Append(";" + ((TrackBar)components[componentName]).Value);
						break;
					default:
						argsString.Append(";" + components[componentName].Text);
						break;
				}
			}
			return argsString.ToString().Trim(';');
		}

		public void pluginOn(string pluginName)
		{
			panels["MAIN"].Controls.Clear();
			panels["MAIN"].Controls.Add(panels[pluginName + "MAIN"]);
		}

		public void addOnClickEvent(string componentName, StringString funct, StringVoid argsFunct)
		{
			components[componentName].Click += (object sender, EventArgs e) => { funct(argsFunct()); } ;
		}
	}
}
