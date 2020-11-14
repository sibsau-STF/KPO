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

namespace Starter___CSharp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		#region importsDLL
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		[DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
		static extern int LoadLibrary(
			[MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

		[DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
		static extern IntPtr GetProcAddress(int hModule,
			[MarshalAs(UnmanagedType.LPStr)] string lpProcName);

		[DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
		static extern bool FreeLibrary(int hModule);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate double VectorType(double[] array1, double[] array2, int size);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate double MatrixType(IntPtr[] array1, int size1, int size2);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void VoidVoid();
		#endregion importsDLL

		private void button1_Click(object sender, EventArgs e)
		{
			callDll("DLL - C.dll");
			callDll("DLL - RAD C Builder.dll");
			callDll("DLL - RAD C Delphi.dll");
		}

		//int size = 10000000;
		int size = 10000;

		void callDll(string dllPath)
		{
			int pointerDll = LoadLibrary(dllPath);

			VectorType funct1 = getVectorFunctInDll(pointerDll, "getMinRangeOfVector");
			string timesString1 = getTimesStringVector(funct1);
			textBox1.AppendText(timesString1 + "\r\n");

			VectorType funct2 = getVectorFunctInDll(pointerDll, "getStandardDeviation");
			string timesString2 = getTimesStringVector(funct2);
			textBox1.AppendText(timesString2 + "\r\n");

			MatrixType funct3 = getMatrixFunctInDll(pointerDll, "getAvgValue");
			string timesString3 = getTimesStringMatrix(funct3);
			textBox1.AppendText(timesString3 + "\r\n");

			FreeLibrary(pointerDll);
		}

		VectorType getVectorFunctInDll(int pointerDll, string functName)
		{
			IntPtr getProc = GetProcAddress((pointerDll), functName);
			if ((int)getProc != 0)
			{
				return Marshal.GetDelegateForFunctionPointer<VectorType>(getProc);
			}
			else
			{
				return null;
			}
		}

		MatrixType getMatrixFunctInDll(int pointerDll, string functName)
		{
			IntPtr getProc = GetProcAddress((pointerDll), functName);
			if ((int)getProc != 0)
			{
				return (MatrixType)Marshal.GetDelegateForFunctionPointer(getProc, typeof(MatrixType));
			}
			else
			{
				return null;
			}
		}

		double[] getRandomArray(int size)
		{
			double[] array = new double[size];
			Random rnd = new Random();
			for (int i = 0; i < size; i++)
			{
				array[i] = rnd.NextDouble();
			}
			return array;
		}

		string getTimesStringVector(VectorType funct)
		{
			long FbeginCount, FendCount, Ffrequence;
			double minTime = double.MaxValue;
			double maxTime = double.MinValue;
			double summTime = 0;
			int countIterations = 10;
			double[] array1, array2;
			array1 = getRandomArray(size);
			array2 = getRandomArray(size);

			for (int i = 0; i < countIterations; i++)
			{
				QueryPerformanceFrequency(out Ffrequence);
				QueryPerformanceCounter(out FbeginCount);
				double value = funct(array1, array2, size);
				QueryPerformanceCounter(out FendCount);
				double time = (double)((FendCount - FbeginCount)) * 1000 / Ffrequence;
				summTime += time;
				if (minTime > time) minTime = time;
				if (maxTime < time) maxTime = time;
			}
			double avgTime = summTime / countIterations;

			return "Среднее, минимум, максимум\r\n" + avgTime.ToString() + " " + minTime.ToString() + " " + maxTime.ToString() + "\r\n";
		}

		string getTimesStringMatrix(MatrixType funct)
		{
			long FbeginCount, FendCount, Ffrequence;
			double minTime = double.MaxValue;
			double maxTime = double.MinValue;
			double summTime = 0;
			int countIterations = 10;
			IntPtr[] array1;
			int localSize = size / 10000;
			unsafe
			{
				array1 = new IntPtr[localSize];
				for (int k = 0; k < localSize; k++)
				{
					double[] arr = getRandomArray(localSize);
					fixed (void* ptr = arr)
					{
						array1[k] = new IntPtr(ptr);
					}
				}
			}
			for (int i = 0; i < countIterations; i++)
			{
				QueryPerformanceFrequency(out Ffrequence);
				QueryPerformanceCounter(out FbeginCount);
				double value = funct(array1, localSize, localSize);
				QueryPerformanceCounter(out FendCount);

				double time = (double)((FendCount - FbeginCount)) * 1000 / Ffrequence;
				summTime += time;
				if (minTime > time) minTime = time;
				if (maxTime < time) maxTime = time;
			}
			double avgTime = summTime / countIterations;

			return "Среднее, минимум, максимум\r\n" + avgTime.ToString() + " " + minTime.ToString() + " " + maxTime.ToString() + "\r\n";
		}
	}
}
