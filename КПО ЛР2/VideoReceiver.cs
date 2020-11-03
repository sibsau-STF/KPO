using MJPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
		public VideoReceiver(PictureBox pictureBox, string url, ComboBox comboBox, ComponentsParser componentsParser)
		{
			this.pictureBox = pictureBox;
			this.url = url;
			videoFilters = new VideoFilters(comboBox, componentsParser);
		}
		private void OnFrameReceived(object sender, FrameReceivedEventArgs e)
		{
			using (var ms = new MemoryStream(e.Frame))
			{
				if (isWorked)
				{
					Bitmap bm = new Bitmap(Image.FromStream(ms));
					var ms2 = new MemoryStream();
					bm.Save(ms2, ImageFormat.Bmp);
					byte[] newMap = videoFilters.Filter(ms2.ToArray());
					using (var ms3 = new MemoryStream(newMap))
					{
						pictureBox.Invoke((MethodInvoker)delegate
						{
							pictureBox.Image = Image.FromStream(ms3);
						});
					}						
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
		public void pluginOn()
		{
			videoFilters.pluginOn();
		}
	}
}
