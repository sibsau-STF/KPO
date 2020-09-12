using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
	{
	public partial class Main : Form
		{

		public Main ()
			{
			InitializeComponent();
			USBObserver.Instance.runAutoUpdate();
			USBObserver.Instance.UpdateDevices += updateDevices;

			USBLogger.Instance.LoadLogs();
			USBLogger.Instance.AddLog($"Application started [{DateTime.Now}]");
			logBox.Text = String.Join("\r\n", USBLogger.Instance.Loggs);
			USBLogger.Instance.AddMessage += newLog;
			}

		private void newLog (string log)
			{
			if ( logBox.InvokeRequired )
				logBox.Invoke(new Action<string>(newLog), log);
			else
				logBox.Text += "\r\n" + log;
			}

		void updateDevices (List<USBFlash> newDevices)
			{
			if ( textBox1.InvokeRequired )
				textBox1.Invoke(new Action<List<USBFlash>>(updateDevices), newDevices);
			else
				textBox1.Text = String.Join("\r\n", newDevices);
			}

		private void createKey_Click (object sender, EventArgs e)
			{
			new CreateKey().Show();
			}

		private void Main_FormClosing (object sender, FormClosingEventArgs e)
			{
			USBObserver.Instance.UpdateDevices -= updateDevices;
			USBObserver.Instance.stopAutoUpdate();

			USBLogger.Instance.AddLog($"Application exited [{DateTime.Now}]");
			USBLogger.Instance.WriteLogs();
			}
		}
	}
