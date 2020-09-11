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
	public partial class CreateKey : Form
		{
		List<USBFlash> devices;
		BindingSource source;

		void updateDevices(List<USBFlash> newDevices)
			{
			if ( devicesBox.InvokeRequired )
				devicesBox.Invoke(new Action<List<USBFlash>>(updateDevices), newDevices);
			else
				{
				devices = newDevices;
				source.DataSource = devices;
				source.ResetBindings(true);
				}
			}

		public CreateKey ()
			{
			InitializeComponent();
			source = new BindingSource();
			devicesBox.DataSource = source;
			USBObserver.Instance.UpdateDevices += updateDevices;
			updateDevices(USBObserver.Instance.Devices);
			}

		private void button1_Click (object sender, EventArgs e)
			{

			}

		private void CreateKey_FormClosing (object sender, FormClosingEventArgs e)
			{
			USBObserver.Instance.UpdateDevices -= updateDevices;
			}
		}
	}
