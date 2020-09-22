using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab5_entry.IPC;


// Entry --(COPYDATA)[Color]--> ImagePreview
// Entry --(COPYDATA)[Color]--> DB
// Entry --(Socket)[Image]----> ImagePreview
// Entry --(ClipBoard)[Text]--> DB

namespace Lab5_entry
	{
	public partial class Entry : Form
		{
		Socket socket;
		public Entry ()
			{
			InitializeComponent();
			}

		private void Entry_Load (object sender, EventArgs e)
			{

			}

		private void colorButton_Click (object sender, EventArgs e)
			{
			if ( colorDialog1.ShowDialog() == DialogResult.OK )
				{
				var Selected = colorDialog1.Color;
				this.BackColor = Selected;
				sendColor(Selected, "Storage");
				sendColor(Selected, "Preview");
				}
			}

		private void sendColor (Color clr, string windowTitle)
			{
			IntPtr ptrWnd = NativeMethods.FindWindow(null, windowTitle);
			IntPtr ptrCopyData = IntPtr.Zero;
			try
				{
				// Create the data structure and fill with data
				NativeMethods.COPYDATASTRUCT copyData = new NativeMethods.COPYDATASTRUCT();
				string strColor = clr.ToArgb().ToString();
				copyData.dwData = new IntPtr(2);        //type of data
				copyData.cbData = strColor.Length + 1;  // bytes for string + \0 character
				copyData.lpData = Marshal.StringToHGlobalAnsi(strColor);

				// Allocate memory for the data and copy
				ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
				Marshal.StructureToPtr(copyData, ptrCopyData, false);

				// Send the message
				NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
				}
			catch ( Exception ex )
				{
				MessageBox.Show(ex.ToString(), "Entry WM_COPYDATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			finally
				{
				// Free the allocated memory after the contol has been returned
				if ( ptrCopyData != IntPtr.Zero )
					Marshal.FreeCoTaskMem(ptrCopyData);
				}
			}

		private void previewButton_Click (object sender, EventArgs e)
			{
			if ( openFileDialog1.ShowDialog() == DialogResult.OK )
				new Task(() =>
				{
					try
						{
						socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
						socket.Connect("localhost", 7878);
						socket.SendFile(openFileDialog1.FileName);
						socket.Disconnect(false);
						}
					catch ( SocketException )
						{
						}
				}).Start();
			}
		}
	}
