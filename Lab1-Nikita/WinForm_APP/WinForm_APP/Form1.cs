using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm_APP
{
    public partial class Form1 : Form
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        static extern IntPtr LoadLibrary(
           [MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddress(IntPtr hModule,
            [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]

        static extern bool FreeLibrary(IntPtr hModule);

        //статичное подключение библиотеки Куды
        [DllImport("cuda_dll_v2.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static System.Int32 sumVector(char* s1, char* s2, char* s3, float* a, float* b, float* c, int N);
        [DllImport("cuda_dll_v2.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static System.Int32 mulVector(char* s1, char* s2, char* s3, float* a, float* b, float* c, int N);
        [DllImport("cuda_dll_v2.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static System.Int32 transposeMatrix(char* s1, char* s2, char* s3, float* a, float* b,  int N, int M);
        [DllImport("cuda_dll_v2.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static System.Int32 mulMatrix(char* s1, char* s2, char* s3, float* a, float* b, float* c, int M,int K, int N);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void FMDtype(double[] v1, double[] v2, double[] result, int size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void FMD2type(float[] v1, float[] v2, float[] result, int size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void FMDtype2(double[] m1, double[] m2, int n, int m);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void FMDtype3(double[] m1, double[] m2, double[] m3, int n, int m, int k);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int FIntMAS(IntPtr[] mas, int h, int w);

        //для замера времени
        private long FbeginCount, FendCount, Ffrequence;
        double Ftime;

        //функции вывода векторов и матриц
        public string getVectorElems(double[] a)
        {
            string str = "[";
            foreach (var i in a)
            {
                str += i + ", ";
            }
            return str.Remove(str.Length - 2, 2) + "]";
        }
        public string getFVectorElems(float[] a)
        {
            string str = "[";
            foreach (var i in a)
            {
                str += i + ", ";
            }
            return str.Remove(str.Length - 2, 2) + "]";
        }
        public string get2DElems(double[] a, int M, int N)
        {
            string str = "";
            str += "[ ";
            for (int i = 0; i < M; i++)
            {

                for (int j = 0; j < N; j++)
                {
                    str += a[i * N + j] + ", ";
                }
                str = str + "\n ";
            }

            return str.Remove(str.Length - 4, 4) + " ]";
        }
        public string get2FElems(float[] a, int M, int N)
        {
            string str = "";
            str += "[ ";
            for (int i = 0; i < M; i++)
            {

                for (int j = 0; j < N; j++)
                {
                    str += a[i * N + j] + ", ";
                }
                str = str + "\n ";
            }

            return str.Remove(str.Length - 4, 4) + " ]";
        }

        public double[] getRandom(int N, double seed)
        {
            Random rand = new Random();
            double[] v = new double[N];
            for (int i = 0; i < N; i++)
                v[i] = (i + 2) * rand.NextDouble()*5 / seed;
            return v;
        }
        public float[] getFRandom(int N, float seed)
        {
            Random rand = new Random();
            float[] v = new float[N];
            for (int i = 0; i < N; i++)
                v[i] = (i + 2) * rand.Next() * 5 / seed;
            return v;
        }
        public Form1()
        {
            InitializeComponent();
        }

        

        //CPP
        private void button1_Click(object sender, EventArgs e)
        {
            int m = 2, k = 3, n = 4;             //MxK    KxN

            double[] a = { 1, 2, 3, 4, 5 };
            double[] b = { 1, 2, 3, 4, 5 };
            double[] c = new double[5];


            double[] mat = { 1, 3, 1, 2, 4, 2 };
            double[] mat1 = { 2, 1, 2, 1, 1, 2, 1, 1, 3, 3, 2, 1 };
            double[] mat2 = new double[m * k];

            double[] iRes = new double[3];

            IntPtr pProc;
            IntPtr pDll = LoadLibrary(@"cpp_Dll.dll");
            using (StreamWriter streamWrighter = new StreamWriter("cpp_dll.csv"))
            {

                if (pDll != null)
                {
                     richTextBox1.Clear();
                     richTextBox1.AppendText("DLL [cpp_Dll.dll] Loaded \n");
                    /* richTextBox1.AppendText("\n a[] = [" + a[0] + ", " + a[1] + ", " + a[2] + ", " + a[3] + ", " + a[4] + "]");
                     richTextBox1.AppendText("\n b[] = [" + b[0] + ", " + b[1] + ", " + b[2] + ", " + b[3] + ", " + b[4] + "]");
                     richTextBox1.AppendText("\n mat[]:\n" + get2DElems(mat, m, k) + "\n");
                     richTextBox1.AppendText("\n mat1[]:\n" + get2DElems(mat1, k, n) + "\n");
                     */
                    pProc = GetProcAddress(pDll, "sumVector");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("\nFailed to retrieve function [sumVector]\n");
                    }
                    else
                    {
                        int N = 100000;

                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;
                        FMDtype sumVector = (FMDtype)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype));
                        for (int i = 0; i < 50; i++)
                        {
                            a = getRandom(N, 1);
                            b = getRandom(N, 2);
                            c = getRandom(N, 3);
                            QueryPerformanceFrequency(out Ffrequence);
                            QueryPerformanceCounter(out FbeginCount);

                            sumVector(a, b, c, N);

                            QueryPerformanceCounter(out FendCount);
                            Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                            if (Ftime < minTime) minTime = Ftime;
                            if (Ftime > maxTime) maxTime = Ftime;

                            sumTime += Ftime;
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + "," + sumTime / 50.0 + "\n");

                        //richTextBox1.AppendText("\n Сложение векторов: " + getVectorElems(c));

                    }

                    pProc = GetProcAddress(pDll, "mulVector");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("Failed to retrieve function [mulVector]\n");
                    }
                    else
                    {

                        int N = 100000;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;
                        FMDtype mulVector = (FMDtype)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype));
                        for (int i = 0; i < 50; i++)
                        {
                            a = getRandom(N, 1);
                            b = getRandom(N, 2);
                            c = getRandom(N, 3);
                            QueryPerformanceFrequency(out Ffrequence);
                            QueryPerformanceCounter(out FbeginCount);

                            mulVector(a, b, c, 10);
                            QueryPerformanceCounter(out FendCount);
                            Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                            if (Ftime < minTime) minTime = Ftime;
                            if (Ftime > maxTime) maxTime = Ftime;
                            sumTime += Ftime;
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG="  + sumTime / 50.0 + "\n");
                        //richTextBox1.AppendText("\n Умножение векторов: " + getVectorElems(c));

                    }

                    pProc = GetProcAddress(pDll, "transposeMatrix");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("Failed to retrieve function [transposeMatrix]\n");
                    }
                    else
                    {
                        int M = 500;
                        int K = 800;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;

                        mat2 = new double[M * K];
                        FMDtype2 transposeMatrix = (FMDtype2)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype2));
                        for (int i = 0; i < 50; i++)
                        {
                            mat = getRandom(M * K, 1);
                            QueryPerformanceFrequency(out Ffrequence);
                            QueryPerformanceCounter(out FbeginCount);

                            transposeMatrix(mat, mat2, m, k);

                            QueryPerformanceCounter(out FendCount);
                            Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                            if (Ftime < minTime) minTime = Ftime;
                            if (Ftime > maxTime) maxTime = Ftime;
                            sumTime += Ftime;
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + sumTime / 50.0);

                        //richTextBox1.AppendText("\n Транспонирование: \n" + get2DElems(mat2, k, m) + "\n");
                    }
                    richTextBox1.AppendText("\nВсе\n");
                }
                /*
                pProc = GetProcAddress(pDll, "mulMatrix");
                if ((int)pProc == 0)
                {
                    richTextBox1.AppendText("Failed to retrieve function [mulMatrix]");
                }
                else
                {
                    FMDtype3 mulMatrix = (FMDtype3)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype3));
                    mat2 = new double[m * n];
                    mulMatrix(mat, mat1, mat2, m, n, k);
                    richTextBox1.AppendText("\n Умножение: \n" + get2DElems(mat2, m, n));

                }
                */
                FreeLibrary(pDll);



            }
        }
        //DELPHI
        private void button2_Click(object sender, EventArgs e)
        {
            int m = 2, k = 3, n = 4;             //MxK    KxN

            double[] a = { 1, 2, 3, 4, 5 };
            double[] b = { 1, 2, 3, 4, 5 };
            double[] c = new double[5];


            double[] mat = { 1, 3, 1, 2, 4, 2 };
            double[] mat1 = { 2, 1, 2, 1, 1, 2, 1, 1, 3, 3, 2, 1 };
            double[] mat2 = new double[m * k];

            double[] iRes = new double[3];

            IntPtr pProc;
            IntPtr pDll = LoadLibrary(@"delphi_dll.dll");
            using (StreamWriter streamWrighter = new StreamWriter("delphi_dll.csv"))
            {

                if (pDll != null)
                {
                    richTextBox1.Clear();
                    richTextBox1.AppendText("DLL [delphi_dll.dll] Loaded \n");
                    richTextBox1.AppendText("\n a[] = [" + a[0] + ", " + a[1] + ", " + a[2] + ", " + a[3] + ", " + a[4] + "]");
                    richTextBox1.AppendText("\n b[] = [" + b[0] + ", " + b[1] + ", " + b[2] + ", " + b[3] + ", " + b[4] + "]");
                    richTextBox1.AppendText("\n mat[]:\n" + get2DElems(mat, m, k) + "\n");
                    richTextBox1.AppendText("\n mat1[]:\n" + get2DElems(mat1, k, n) + "\n");

                    pProc = GetProcAddress(pDll, "sumVector");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("\nFailed to retrieve function [sumVector]\n");
                    }
                    else
                    {
                        int N = 100000;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;

                        FMDtype sumVector = (FMDtype)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype));
                        for (int i = 0; i < 50; i++)
                        {
                            a = getRandom(N, 1);
                            b = getRandom(N, 2);
                            c = getRandom(N, 3);
                            QueryPerformanceFrequency(out Ffrequence);
                            QueryPerformanceCounter(out FbeginCount);

                            sumVector(a, b, c, N);

                            QueryPerformanceCounter(out FendCount);
                            Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                            if (Ftime < minTime) minTime = Ftime;
                            if (Ftime > maxTime) maxTime = Ftime;
                            sumTime += Ftime;
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + sumTime / 50.0);

                       // richTextBox1.AppendText("\n Сложение векторов: " + getVectorElems(c));

                    }

                    pProc = GetProcAddress(pDll, "mulVector");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("Failed to retrieve function [mulVector]\n");
                    }
                    else
                    {

                        int N = 100000;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;
                        FMDtype mulVector = (FMDtype)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype));
                        for (int i = 0; i < 50; i++)
                        {
                            a = getRandom(N, 1);
                            b = getRandom(N, 2);
                            c = getRandom(N, 3);
                            QueryPerformanceFrequency(out Ffrequence);
                            QueryPerformanceCounter(out FbeginCount);

                            mulVector(a, b, c, N);
                            QueryPerformanceCounter(out FendCount);
                            Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                            if (Ftime < minTime) minTime = Ftime;
                            if (Ftime > maxTime) maxTime = Ftime;
                            sumTime += Ftime;
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + sumTime / 50.0);
                       // richTextBox1.AppendText("\n Умножение векторов: " + getVectorElems(c));

                    }

                    pProc = GetProcAddress(pDll, "transposeMatrix");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("Failed to retrieve function [transposeMatrix]\n");
                    }
                    else
                    {
                        int M = 500;
                        int K = 800;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;

                        mat2 = new double[M * K];
                        FMDtype2 transposeMatrix = (FMDtype2)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype2));
                        for (int i = 0; i < 50; i++)
                        {
                            mat = getRandom(M * K, 1);
                            QueryPerformanceFrequency(out Ffrequence);
                            QueryPerformanceCounter(out FbeginCount);

                            transposeMatrix(mat, mat2, m, k);

                            QueryPerformanceCounter(out FendCount);
                            Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                            if (Ftime < minTime) minTime = Ftime;
                            if (Ftime > maxTime) maxTime = Ftime;
                            sumTime += Ftime;
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + sumTime / 50.0);

                        // richTextBox1.AppendText("\n Транспонирование: \n" + get2DElems(mat2, k, m) + "\n");
                    }

                    /*pProc = GetProcAddress(pDll, "mulMatrix");
                    if ((int)pProc == 0)
                    {
                        richTextBox1.AppendText("Failed to retrieve function [mulMatrix]");
                    }
                    else
                    {
                        FMDtype3 mulMatrix = (FMDtype3)Marshal.GetDelegateForFunctionPointer(pProc, typeof(FMDtype3));
                        mat2 = new double[m * n];
                        mulMatrix(mat, mat1, mat2, m, n, k);
                        richTextBox1.AppendText("\n Умножение: \n" + get2DElems(mat2, m, n));

                    }*/

                    FreeLibrary(pDll);
                }
            }
        }

        //CUDA
        unsafe private void button3_Click(object sender, EventArgs e)
        {
            int m = 4, k = 4, n = 4;
            float[] a = { 1, 2, 3, 4, 5 };
            float[] b = { 1, 2, 3, 4, 5 };
            float[] c = new float[5];

            float[] mat = { 1, 3, 1, 2, 4, 2, 1, 5, 6, 3, 4, 1, 2, 3, 4, 5 };
            float[] mat1 = { 2, 1, 2, 1, 1, 2, 1, 1, 3, 3, 2, 1, 2, 1, 2, 1 };
            float[] mat2 = new float[m * k];

            double iRes = 0;
            IntPtr pProc;
            IntPtr pDll = LoadLibrary(@"cuda_dll_v2.dll");
            using (StreamWriter streamWrighter = new StreamWriter("cuda_dll.csv"))
            {
                if (pDll != null)
                {
                    richTextBox1.Clear();
                  /*  richTextBox1.AppendText("DLL [cuda_dll_v2.dll] Loaded \n");
                    richTextBox1.AppendText("\n a[] = " + getFVectorElems(a));
                    richTextBox1.AppendText("\n b[] = " + getFVectorElems(b));
                    richTextBox1.AppendText("\n mat[]:\n" + get2FElems(mat, m, k) + "\n");
                    richTextBox1.AppendText("\n mat1[]:\n" + get2FElems(mat1, k, n) + "\n");
                    */
                    pProc = GetProcAddress(pDll, "sumVector");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("Failed to retrieve function [sumVector]");
                    }
                    else
                    {
                        int N = 100000;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;

                        string str1 = "Сумма ";
                        string str2 = "векторов: ";
                        char[] str3 = new char[255];
                        string str4;
                        IntPtr stringPointer1 = (IntPtr)Marshal.StringToHGlobalAnsi(str1);

                        for (int i = 0; i < 50; i++)
                        {
                            a = getFRandom(N, 1);
                            b = getFRandom(N, 2);
                            c = getFRandom(N, 3);
                            fixed (float* pa = a, pb = b, pc = c)
                            fixed (char* s1 = str1, s2 = str2, s3 = str3)
                            {
                                QueryPerformanceFrequency(out Ffrequence);
                                QueryPerformanceCounter(out FbeginCount);
                                
                                char* stringPointer2 = (char*)Marshal.StringToHGlobalAnsi(str2);
                                int j = sumVector((char*)stringPointer1, stringPointer2, s3, pa, pb, pc, 5);

                                QueryPerformanceCounter(out FendCount);
                                Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                                if (Ftime < minTime) minTime = Ftime;
                                if (Ftime > maxTime) maxTime = Ftime;
                                sumTime += Ftime;

                                /* str4 = c[0].ToString() + ", " + c[1].ToString() + ", " + c[2].ToString() + ", " + c[3].ToString() + ", " + c[4].ToString();
                                 String myString = Marshal.PtrToStringAnsi((IntPtr)s3);
                                 richTextBox1.AppendText("\n " + myString + str4);*/
                            }
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + sumTime / 50.0+"\n");

                    }

                    pProc = GetProcAddress(pDll, "mulVector");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("Failed to retrieve function [mulVector]");
                    }
                    else
                    {
                        int N = 100000;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;

                        string str1 = "Умножение ";
                        string str2 = "векторов: ";
                        char[] str3 = new char[255];
                        string str4;
                        IntPtr stringPointer1 = (IntPtr)Marshal.StringToHGlobalAnsi(str1);
                        for (int i = 0; i < 50; i++)
                        {
                            a = getFRandom(N, 1);
                            b = getFRandom(N, 2);
                            c = getFRandom(N, 3);
                            fixed (float* pa = a, pb = b, pc = c)
                            fixed (char* s1 = str1, s2 = str2, s3 = str3)
                            {
                                QueryPerformanceFrequency(out Ffrequence);
                                QueryPerformanceCounter(out FbeginCount);

                                char* stringPointer2 = (char*)Marshal.StringToHGlobalAnsi(str2);
                                int j = mulVector((char*)stringPointer1, stringPointer2, s3, pa, pb, pc, 5);

                                QueryPerformanceCounter(out FendCount);
                                Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                                if (Ftime < minTime) minTime = Ftime;
                                if (Ftime > maxTime) maxTime = Ftime;
                                sumTime += Ftime;
                                /* str4 = getFVectorElems(c);
                                 String myString = Marshal.PtrToStringAnsi((IntPtr)s3);
                                 richTextBox1.AppendText("\n " + myString + str4);*/
                            }
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + sumTime / 50.0+"\n");
                    }


                    pProc = GetProcAddress(pDll, "transposeMatrix");
                    if ((int)pProc == 0)
                    {
                        // Handle the error here.
                        richTextBox1.AppendText("Failed to retrieve function [mulVector]");
                    }
                    else
                    {
                        int M = 500;
                        int K = 800;
                        double minTime = 1000000;
                        double maxTime = 0;
                        double sumTime = 0;


                        string str1 = "Транспонирование";
                        string str2 = ": \n";
                        char[] str3 = new char[255];
                        string str4;
                        IntPtr stringPointer1 = (IntPtr)Marshal.StringToHGlobalAnsi(str1);
                        for (int i = 0; i < 50; i++)
                        {
                            mat = getFRandom(M * K, 1);

                            fixed (float* pa = mat, pb = mat2)
                            fixed (char* s1 = str1, s2 = str2, s3 = str3)
                            {
                                char* stringPointer2 = (char*)Marshal.StringToHGlobalAnsi(str2);
                                QueryPerformanceFrequency(out Ffrequence);
                                QueryPerformanceCounter(out FbeginCount);

                                int j = transposeMatrix((char*)stringPointer1, stringPointer2, s3, pa, pb, k, m);

                                QueryPerformanceCounter(out FendCount);
                                Ftime = ((FendCount - FbeginCount) / (double)Ffrequence) * 1000;
                                if (Ftime < minTime) minTime = Ftime;
                                if (Ftime > maxTime) maxTime = Ftime;
                                sumTime += Ftime;
                                /*
                                str4 = get2FElems(mat2, k, m);
                                String myString = Marshal.PtrToStringAnsi((IntPtr)s3);
                                richTextBox1.AppendText("\n " + myString + str4 + "\n");*/
                            }
                        }
                        streamWrighter.Write("minTime=" + "," + minTime + "\n");
                        streamWrighter.Write("maxTime=" + "," + maxTime + "\n");
                        streamWrighter.Write("AVG=" + sumTime / 50.0);
                    }
                   /* if (pDll != null)
                    {
                        pProc = GetProcAddress(pDll, "mulMatrix");
                        if ((int)pProc == 0)
                        {
                            // Handle the error here.
                            richTextBox1.AppendText("\nFailed to retrieve function [mulMatrix]");
                        }
                        else
                        {
                            mat2 = new float[m * n];
                            string str1 = "Умножение ";
                            string str2 = "матриц: \n";
                            char[] str3 = new char[255];
                            string str4;
                            IntPtr stringPointer1 = (IntPtr)Marshal.StringToHGlobalAnsi(str1);
                            fixed (float* pa = mat, pb = mat1, pc = mat2)
                            fixed (char* s1 = str1, s2 = str2, s3 = str3)
                            {
                                char* stringPointer2 = (char*)Marshal.StringToHGlobalAnsi(str2);
                                int i = mulMatrix((char*)stringPointer1, stringPointer2, s3, pa, pb, pc, m, k, n);
                                str4 = get2FElems(mat2, m, n);
                                String myString = Marshal.PtrToStringAnsi((IntPtr)s3);
                                richTextBox1.AppendText("\n " + myString + str4);
                            }


                        }
                    }*/
                }
            }
        }
    }
}
