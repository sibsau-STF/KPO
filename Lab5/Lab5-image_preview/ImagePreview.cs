using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab5_entry.IPC;

namespace Lab5_image_preview
	{
	public partial class ImagePreview : Form
		{
		TcpListener server;
		Thread background;
		public Image BgImage;
		public ImagePreview ()
			{
			InitializeComponent();
			server = new TcpListener(IPAddress.Loopback, 7878);
			background = new Thread(RecieveFile);
			background.Start();
			}

		private void ImagePreview_Load (object sender, EventArgs e)
			{
			NativeMethods.CHANGEFILTERSTRUCT changeFilter = new NativeMethods.CHANGEFILTERSTRUCT();
			changeFilter.size = (uint)Marshal.SizeOf(changeFilter);
			changeFilter.info = 0;
			if ( !NativeMethods.ChangeWindowMessageFilterEx(this.Handle, NativeMethods.WM_COPYDATA, NativeMethods.ChangeWindowMessageFilterExAction.Allow, ref changeFilter) )
				{
				int error = Marshal.GetLastWin32Error();
				MessageBox.Show(String.Format("The error {0} occured.", error));
				}
			}

		protected override void WndProc (ref Message m)
			{
			if ( m.Msg == NativeMethods.WM_COPYDATA )
				{
				// Extract the file name
				NativeMethods.COPYDATASTRUCT copyData = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.COPYDATASTRUCT));
				int dataType = (int)copyData.dwData;
				if ( dataType == 2 )
					{
					int argbColor = 0;
					if ( int.TryParse((String)Marshal.PtrToStringAnsi(copyData.lpData), out argbColor) )
						{
						this.BackColor = Color.FromArgb(argbColor);
						}
					}
				else
					{
					MessageBox.Show(String.Format("Unrecognized data type = {0}.", dataType), "SendMessageDemo", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			else
				{
				base.WndProc(ref m);
				}
			}

		void setImage (Image img)
			{
			if ( panel1.InvokeRequired )
				panel1.Invoke(new Action<Image>(setImage), img);
			else
				{
				BgImage = img;
				drawImage();
				}
			}

		void drawImage ()
			{
			if ( BgImage == null )
				return;
			var bg = panel1.CreateGraphics();
			var boundary = getImageBoundary(BgImage, bg);
			bg.Clear(Color.WhiteSmoke);
			bg.DrawImage(BgImage, boundary);
			}

		Rectangle getImageBoundary (Image img, Graphics g)
			{
			var bounds = g.VisibleClipBounds;
			int bgWidth = (int)bounds.Width, bgHeight = (int)bounds.Height;
			int imgWidth = img.Width, imgHeight = img.Height;

			float imgRatio = (float)imgWidth / imgHeight;
			int newWidth;
			int newHeight;
			int xSpan, ySpan;
			if ( imgWidth > imgHeight )
				{
				newWidth = bgWidth;
				newHeight = (int)( newWidth / imgRatio );

				xSpan = 0;
				ySpan = (int)( ( bgHeight - newHeight ) / 2f );
				}
			else
				{
				newHeight = bgHeight;
				newWidth = (int)( newHeight * imgRatio );

				xSpan = (int)( ( bgWidth - newWidth ) / 2f );
				ySpan = 0;
				}
			return new Rectangle(xSpan, ySpan, newWidth, newHeight);
			}

		void RecieveFile ()
			{
			server.Start();
			while ( true )
				{
				using ( var client = server.AcceptTcpClient() )
					{
					int delay = 1;
					while ( client.Available <= 0 && delay < 200 )
						Thread.Sleep(delay += 20);

					MemoryStream memoryStream = new MemoryStream();
					var clientStream = client.GetStream();

					while ( client.Available > 0 )
						memoryStream.WriteByte((byte)clientStream.ReadByte());
					setImage(new Bitmap(memoryStream));
					}
				}
			}

		private void ImagePreview_SizeChanged (object sender, EventArgs e)
			{
			drawImage();
			}

		private void ImagePreview_FormClosing (object sender, FormClosingEventArgs e)
			{
			server.Stop();
			background.Abort();
			}
		}
	}
