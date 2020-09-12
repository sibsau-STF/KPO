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

		public CreateKey ()
			{
			InitializeComponent();
			source = new BindingSource();
			devicesBox.DataSource = source;
			USBObserver.Instance.UpdateDevices += updateDevices;
			updateDevices(USBObserver.Instance.Devices);

			functionsListBox1.Items.AddRange(new object[] { new Function(1), new Function(2), new Function(3) });
			saveFileDialog1.AddExtension = true;
			saveFileDialog1.DefaultExt = "key";
			}

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


		private void button1_Click (object sender, EventArgs e)
			{
			if ( dateTimePicker1.Value > DateTime.Now 
				&& nameBox.Text.Length > 0 
				&& devicesBox.SelectedValue != null 
				&& functionsListBox1.SelectedItems.Count > 0 )
				{
				var result = saveFileDialog1.ShowDialog();
				if (result == DialogResult.OK)
					{
					var path = saveFileDialog1.FileName;
					var device = (USBFlash)devicesBox.SelectedValue;
					List<Function> selected = new List<Function>();
					foreach ( Function item in functionsListBox1.SelectedItems )
						selected.Add(item);

					// сохраняю ключ
					KeyFactory.CreateKey(path, new Key(selected, device.SerialNumber, nameBox.Text, dateTimePicker1.Value));
					}
				}
			}

		private void CreateKey_FormClosing (object sender, FormClosingEventArgs e)
			{
			USBObserver.Instance.UpdateDevices -= updateDevices;
			}
		}
	}
