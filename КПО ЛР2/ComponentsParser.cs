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

		void AddPluginComponent(string componentName, Control component)
		{
			components.Add(componentName, component);
		}

		public void parseComponents(string pluginComponentsInfo)
		{
			string[] splitedPluginComponentsInfo = pluginComponentsInfo.Split(';');
			foreach (var componentInfo in splitedPluginComponentsInfo)
			{
				Control tmpControl = parseComponentInfo(componentInfo);
				AddPluginComponent(tmpControl.Name, tmpControl);
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
					break;
				case "TEXTBOX":
					component = new TextBox();
					break;
				case "TRACKBAR":
					var trackBar = new TrackBar();
					trackBar.Minimum = Convert.ToInt32(splitedComponentInfo[6]);
					trackBar.Maximum = Convert.ToInt32(splitedComponentInfo[7]);
					component = trackBar;
					break;
				default:
					component = new Label();
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
			if (componentNames == "") return "";
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

		public void setComponentsValue(string componentName, string value)
		{
			components[componentName].Text = value;
		}

		public void pluginOn(string pluginName)
		{
			panels["MAIN"].Controls.Clear();
			panels["MAIN"].Controls.Add(panels[pluginName + "MAIN"]);
		}

		public void addOnClickEvent(string componentName, Action funct)
		{
			components[componentName].Click += (object sender, EventArgs e) => { funct(); } ;
		}
	}
}
