using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows.Forms;

namespace КПО_ЛР3
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			
		}
		VideoReceiver vr1, vr2;
		private void button1_Click(object sender, EventArgs e)
		{			
			vr1.Start();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			vr1.Stop();			
		}

		private void button3_Click(object sender, EventArgs e)
		{
			vr1.Pause();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			vr2.Start();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			vr2.Stop();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
			foreach (string filename in allfiles)
			{
				textBox1.AppendText(filename+"\r\n");
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			vr1 = new VideoReceiver(pictureBox1, "http://95.143.219.190/mjpg/video.mjpg", comboBox1);
			vr2 = new VideoReceiver(pictureBox2, "http://92.106.223.122/mjpg/video.mjpg", comboBox2);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			vr2.Pause();
		}
	}
}
