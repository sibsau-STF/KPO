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
		public event Action<Bitmap> ImageRecieved;
		public event Action<Bitmap> ImageFiltered;

		Control actor;
		StreamDecoder decoder = null;
		public VideoFilter filter;

		public VideoReceiver(Control actor, StreamDecoder decoder, VideoFilter filter=null)
		{
			this.actor = actor;
			this.filter = filter;
			this.decoder = decoder;
			this.decoder.OnFrameReceived += OnFrameReceived;
		}

		private void OnFrameReceived(object sender, FrameReceivedEventArgs e)
		{
			using (var ms = new MemoryStream(e.Frame))
			{
				if (decoder.isWorking)
				{
					Bitmap bm = new Bitmap(Image.FromStream(ms));

					//получаю кадр
					if ( actor.InvokeRequired )
						actor.Invoke(new Action<Bitmap>((img) => ImageRecieved?.Invoke(img)), bm);

					if ( filter != null )
						bm = filter.Filter(bm);

					if ( actor.InvokeRequired )
						actor.Invoke(new Action<Bitmap>((img) => ImageFiltered?.Invoke(img)), bm);
				}
			}
		}

		public void Start()
		{
			if ( decoder.isStopped )
				{
				decoder.Stream();
				decoder.StartDecodingAsync();
				}
			decoder.Stream();
		}

		public void Stop()
		{
			if (!decoder.isStopped)
				decoder.Stop();
			else
				MessageBox.Show("Просмотр выключен, невозможно выключить просмотр");			
		}

		public void Pause()
		{
			if (decoder.isWorking)
				decoder.Pause();
			else
				MessageBox.Show("Просмотр выключен, невозможно поставить на паузу");
		}
	}
}
