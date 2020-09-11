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
			//USBObserver.Instance.refreshDevices();

			}

		private void createKey_Click (object sender, EventArgs e)
			{
			new CreateKey().Show();
			}

		}
	}
