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
using MJPEG;

namespace КПО_ЛР3
{
	public partial class Form1 : Form
	{
		List<VideoFilter> Filters;

		StreamDecoder stream;
		VideoReceiver reciever;
		VideoRecorder recorder;
		ImagesDB DB;

		VideoFilter Filter;
		ImageResolution Resolution;
		string savePath;
		int MaxWorkers => (int)workersUpDown.Value;
		bool isRecording = false;

		public Form1 ()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
			{
			// http://103.137.83.115:8090/mjpg/video.mjpg
			// http://188.170.32.93:82/mjpg/1/video.mjpg
			stream = new StreamDecoder("http://103.137.83.115:8090/mjpg/video.mjpg");
			reciever = new VideoReceiver(pictureBox1, stream);
			reciever.ImageFiltered += PreviewImage;

			recorder = new VideoRecorder(reciever, pictureBox1);
			recorder.RecordingStart += lockUI;
			recorder.SavingEnd += releaseUI;

			recorder.InsertDB += savingProgress;
			recorder.FileProceeded += proceededProgress;

			recorder.InsertDB += displayWork;
			recorder.Logs.ForEach(displayWork);
			recorder.StartDB();

			pathBox.Text = Path.Combine(Environment.CurrentDirectory, "captured");
			savePath = pathBox.Text;

			resCombo.Items.Add(new ImageResolution(800, 600));
			resCombo.Items.Add(new ImageResolution(1280, 720));
			resCombo.Items.Add(new ImageResolution(1440, 960));
			resCombo.SelectedIndex = 0;

			LoadFilters();
			}

		private void savingProgress (DBRow obj)
			{
			savedBar.PerformStep();
			}

		private void proceededProgress (ImageFile obj)
			{
			filteredBar.PerformStep();
			}

		private void displayWork (DBRow row)
			{
			dbBox.Text += row.ToString() + "\r\n";
			}

		private void lockUI ()
			{
			toggleLock(true);
			}

		private void releaseUI ()
			{
			toggleLock(false);
			recBtn.Text = "Записать";
			}

		private void PreviewImage (Image img)
			{
			pictureBox1.Image = img;
			}

		private void button1_Click(object sender, EventArgs e)
		{			
			reciever.Start();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			reciever.Stop();			
		}

		private void button3_Click(object sender, EventArgs e)
		{
			reciever.Pause();
		}

		private void openBtn_Click (object sender, EventArgs e)
			{
			if ( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
				{
				savePath = folderBrowserDialog1.SelectedPath;
				pathBox.Text = savePath;
				}
			}

		private void comboBox1_SelectedValueChanged (object sender, EventArgs e)
			{
			reciever.filter = (VideoFilter)comboBox1.SelectedItem;
			}

		private void filterCombo_SelectedValueChanged (object sender, EventArgs e)
			{
			Filter = (VideoFilter)filterCombo.SelectedItem;
			}

		private void resCombo_SelectedValueChanged (object sender, EventArgs e)
			{
			Resolution = (ImageResolution)resCombo.SelectedItem;
			}

		private void recBtn_Click (object sender, EventArgs e)
			{
			if ( recorder.isStopped )
				recorder.Record(Filter, Resolution, MaxWorkers - 2, pathBox.Text);

			if ( isRecording )
				recorder.Pause();
			else
				recorder.Resume();

			isRecording = !isRecording;
			recBtn.Text = isRecording ? "Пауза" : "Записать";
			}

		private void recStopBtn_Click (object sender, EventArgs e)
			{
			if ( isRecording )
				{
				int filesCount = recorder.Stop();
				SetupProgress(filesCount);
				isRecording = false;
				recBtn.Text = "Записать";
				}
			}

		private void SetupProgress(int count)
			{
			filteredBar.Value = 0;
			savedBar.Value = 0;
			filteredBar.Maximum = count;
			savedBar.Maximum = count;
			filteredBar.Step = 1;
			filteredBar.Step = 1;
			}

		private void LoadFilters ()
			{
			VideoFilter last;
			Filters = new List<VideoFilter>();
			Filters.Add(new VideoFilter("Отключён", (byte[] array, int length) => array));
			string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
			foreach ( string filename in allfiles )
				{
				last = new VideoFilter(filename);
				if ( last.Name != null )
					Filters.Add(last);
				}
			comboBox1.Items.AddRange(Filters.ToArray());
			comboBox1.SelectedIndex = 0;

			filterCombo.Items.AddRange(Filters.ToArray());
			filterCombo.SelectedIndex = 0;
			}

		private void toggleLock(bool locked)
			{
			resCombo.Enabled = !locked;
			workersUpDown.Enabled = !locked;
			filterCombo.Enabled = !locked;
			openBtn.Enabled = !locked;
			// плей, стоп, пауза
			button1.Enabled = !locked;
			button2.Enabled = !locked;
			button3.Enabled = !locked;
			}
		}
}
