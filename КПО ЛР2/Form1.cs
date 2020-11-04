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
		ComponentsParser componentsParser = new ComponentsParser();
		Form2 pluginsInfo = new Form2();
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
			textBox1.Text = componentsParser.createArgsString("TESTTEXTBOX1") + "\r\n";
			textBox1.AppendText(componentsParser.createArgsString("TRACKBAR1"));
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			componentsParser.AddPanel("MAIN", flowLayoutPanel1);
			componentsParser.AddPanel("INFOPANEL", pluginsInfo.flowLayoutPanel1);
			vr2 = new VideoReceiver(pictureBox2, "http://92.106.223.122/mjpg/video.mjpg", comboBox2, componentsParser);
		}

		private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
		{

		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			pluginsInfo.Show();
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (vr2 != null) vr2.pluginOn();
		}

		private void button2_Click_1(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				vr2.addVideoFilterFromPath(openFileDialog1.FileName);
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			vr2.Pause();
		}
	}
}
