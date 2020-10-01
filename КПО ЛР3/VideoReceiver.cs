using MJPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КПО_ЛР3
{
	class VideoReceiver
	{
		PictureBox pictureBox;
		string url;
		bool isWorked = true;
		StreamDecoder decoder = null;
		VideoFilters videoFilters;
		public VideoReceiver(PictureBox pictureBox, string url, ComboBox comboBox)
		{
			this.pictureBox = pictureBox;
			this.url = url;
			videoFilters = new VideoFilters(comboBox);
		}
		
		private void OnFrameReceived(object sender, FrameReceivedEventArgs e)
		{
			var frame = videoFilters.Filter(e.Frame);
			var tmp = "";
			using (var ms = new MemoryStream(frame))
			{
				if (isWorked)
				{
					pictureBox.Invoke((MethodInvoker)delegate
					{
						pictureBox.Image = Image.FromStream(ms);
					});
				}
			}
		}		
		public void Start()
		{
			if (decoder == null)
			{
				decoder = new StreamDecoder();
				decoder.OnFrameReceived += OnFrameReceived;
				decoder.StartDecodingAsync(url);
			}
			else
			{
				decoder.Resume();
			}
			isWorked = true;
		}
		public void Stop()
		{
			if (decoder != null)
			{
				decoder.Stop();
				isWorked = false;
				decoder = null;
				pictureBox.Image = null;
			}
			else MessageBox.Show("Просмотр выключен, невозможно выключить просмотр");			
		}
		public void Pause()
		{
			if (decoder != null)
			{
				decoder.Pause();
				isWorked = false;
			}					
			else MessageBox.Show("Просмотр выключен, невозможно поставить на паузу");
			
		}



	}
}
