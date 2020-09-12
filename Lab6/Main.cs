﻿using System;
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
		Key loadedKey;
		public Key LoadedKey { get { return loadedKey; } set
				{
				loadedKey = value;
				displayKey(loadedKey);
				}
			}

		public Main ()
			{
			InitializeComponent();
			// настройка диалога
			openFileDialog1.DefaultExt = "key";
			openFileDialog1.Filter = "USB Key (*.key)|*.key";

			// запуск обсервера
			USBObserver.Instance.runAutoUpdate();
			USBObserver.Instance.UpdateDevices += updateDevices;

			// запуск логирования
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

		void displayKey (Key key)
			{
			userBox.Text = key.User;
			serialBox.Text = key.SerialNumber;
			vendorBox.Text = key.KeyCreator;
			createdBox.Text = key.CreatedDate.ToString();
			untillBox.Text = key.UntilDate.ToString();
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

		private void button1_Click (object sender, EventArgs e)
			{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
				var path = openFileDialog1.FileName;
				var key = KeyFactory.ReadKey(path);
				if ( key != null )
					LoadedKey = (Key)key;
				}
			
			}
		}
	}
