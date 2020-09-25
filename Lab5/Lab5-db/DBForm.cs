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

namespace Lab5_db
	{
	public partial class DBForm : Form
		{
		IntPtr _ClipboardViewerNext;
		public DBForm ()
			{
			InitializeComponent();
			}

		private void DBForm_Load (object sender, EventArgs e)
			{
			NativeMethods.CHANGEFILTERSTRUCT changeFilter = new NativeMethods.CHANGEFILTERSTRUCT();
			changeFilter.size = (uint)Marshal.SizeOf(changeFilter);
			changeFilter.info = 0;
			if ( !NativeMethods.ChangeWindowMessageFilterEx(this.Handle, NativeMethods.WM_COPYDATA, NativeMethods.ChangeWindowMessageFilterExAction.Allow, ref changeFilter) )
				{
				int error = Marshal.GetLastWin32Error();
				MessageBox.Show(String.Format("The error {0} occured.", error));
				}
			RegisterClipboardViewer();
			}

		/// <summary>
		/// Register this form as a Clipboard Viewer application
		/// </summary>
		private void RegisterClipboardViewer ()
			{
			_ClipboardViewerNext = NativeMethods.SetClipboardViewer(this.Handle);
			}

		/// <summary>
		/// Remove this form from the Clipboard Viewer list
		/// </summary>
		private void UnregisterClipboardViewer ()
			{
			NativeMethods.ChangeClipboardChain(this.Handle, _ClipboardViewerNext);
			}

		private string GetClipboardData ()
			{
			IDataObject iData = new DataObject();
			string strText = "";

			try
				{
				iData = Clipboard.GetDataObject();
				}
			catch ( Exception ex )
				{
				MessageBox.Show(ex.ToString(), "CLIPBOARD", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
				}

			if ( iData.GetDataPresent(DataFormats.Text) )
				strText = (string)iData.GetData(DataFormats.Text);
			else
				strText = "(cannot display this format)";
			return strText;
			}

		protected override void WndProc (ref Message m)
			{

			switch ( (NativeMethods.Msgs)m.Msg )
				{
				case NativeMethods.Msgs.WM_DRAWCLIPBOARD:
					{
					string newMessage = GetClipboardData();
					textBox1.Text += newMessage + "\r\n";
					NativeMethods.SendMessage(_ClipboardViewerNext, (uint)m.Msg, m.WParam, m.LParam);
					break;
					}

				case NativeMethods.Msgs.WM_COPYDATA:
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
						MessageBox.Show(String.Format("Unrecognized data type = {0}.", dataType), "WM_COPYDATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					break;
					}
				default:
				base.WndProc(ref m);
				break;
				}

			}

		private void DBForm_FormClosing (object sender, FormClosingEventArgs e)
			{
			UnregisterClipboardViewer();
			}
		}
	}
