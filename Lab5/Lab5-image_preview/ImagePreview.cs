using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab5_entry.IPC;

namespace Lab5_image_preview
	{
	public partial class ImagePreview : Form
		{
		public ImagePreview ()
			{
			InitializeComponent();
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
					if (int.TryParse((String)Marshal.PtrToStringAnsi(copyData.lpData), out argbColor) )
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

		}
	}
