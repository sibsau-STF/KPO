//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "Unit1.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1 *Form1;
//---------------------------------------------------------------------------

//����� �������
void Time_vyvod(double *mas_1, int N_1)
{
	double time_max = mas_1[0];
	double time_min = mas_1[0];
	double time_avg = 0;

	for (int i = 0; i < N_1; i++)
	{
		if (time_max < mas_1[i])
		{
			time_max = mas_1[i];
		}

		if (time_min > mas_1[i])
		{
			time_min = mas_1[i];
		}

		time_avg = time_avg + mas_1[i];

	}

	time_avg = time_avg / 50;

   //	cout << "time_max="<<time_max<<"\t" << "time_avg=" <<time_avg<< "\t" <<"time_min="<<time_min<<endl;

   Form1->Memo1->Lines->Add("time_min="+FloatToStr(time_min)+"\t time_max="+FloatToStr(time_max)+"\t time_avg="+FloatToStr(time_avg));


}



__fastcall TForm1::TForm1(TComponent* Owner)
	: TForm(Owner)
{

}
//---------------------------------------------------------------------------
void __fastcall TForm1::Button_VS_CClick(TObject *Sender)
{
	Found_max_in_mass Max_in_mass;
	Found_Min Min_in_mass;
	Found_Max_in_dual_mass Max_in_dual_mass;

	//����������� �������
	int N = 100000;
	//����������� ����������� �������
	int N_1 = 600;
	//���-�� �������
	int time_N = 50;
	//������ �� ��������
	double *mass_time_max = new double[time_N];


     	double *mass_1=new double[N];
	double *mass_2=new double[N];
	for (int i = 0; i < N; i++)
	{
		mass_1[i]= rand() % 1000 + 1;
		mass_2[i] = rand() % 1000 + 1;
	}

		Memo1->Lines->Clear();

        // ���������� ���������� ��� ������ �������
	LARGE_INTEGER FbeginCount, FendCount, Ffrequence;
	double Ftime;

	HINSTANCE hinstLib = LoadLibrary(TEXT("dll_sampel_C.dll"));
	double result;


//������������� VS DLL
	if (hinstLib == NULL)
	{
	   //	cout << "ERROR: unable to load DLL: dll_sampel_C.dll\n";
		   Memo1->Lines->Add("ERROR: unable to load DLL: dll_sampel_C.dll");
	}
	else
	{
	   //	cout << "library (dll_sampel_C.dll) loaded\n";

	   Memo1->Lines->Add("library (dll_sampel_C.dll) loaded");

		//1-�� �������
		Max_in_mass = (Found_max_in_mass)GetProcAddress(hinstLib, "Found_Max");
		if (Max_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Max_in_mass(mass_1, mass_2, N, N);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		  //	cout << "����� ���������� ������� Found_Max �� dll_sampel_C.dll :" << endl;
		   Memo1->Lines->Add("����� ���������� ������� Found_Max �� dll_sampel_C.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Max)\n";
			Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Max)\n");

		//2-�� �������
		Min_in_mass = (Found_Min)GetProcAddress(hinstLib, "Found_Min");
		if (Min_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Min_in_mass(mass_1, mass_2, N, N);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		   //	cout << "����� ���������� ������� Found_Min �� dll_sampel_C.dll :" << endl;
			Memo1->Lines->Add("����� ���������� ������� Found_Min �� dll_sampel_C.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Min)\n";
			  Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Min)\n");
		//delete mass_1;
		//delete mass_2;

		double mass_4[20][20];

		double **mass_3 = new double*[N_1];
		for (int i = 0; i < N_1; i++)
		{
			mass_3[i] = new double[N_1];
		}

			for (int y = 0; y < N_1; y++)
		{
			for (int x = 0; x < N_1; x++)
			{
				mass_3[y][x] =  x*y;

			   //	 Form1->Memo1->Lines->Add("mass_3["+FloatToStr(y)+"]["+FloatToStr(x)+"]="+FloatToStr(mass_3[y][x]));
			}
		}







	 int N_2=20;

		for (int y = 0; y < N_2; y++)
		{
			for (int x = 0; x < N_2; x++)
			{
				mass_4[y][x] =  1;

			   //	 Form1->Memo1->Lines->Add("mass_3["+FloatToStr(y)+"]["+FloatToStr(x)+"]="+FloatToStr(mass_3[y][x]));
			}
		}


		double **flip_mass=new double*[N_1];
		for(int i = 0; i <N_2; i++)
		{
				flip_mass[i] = &mass_4[i][0];

		}

		 //	double **flip2_mass=&mass_3[0][0];

		HINSTANCE  hinstLib_1 = LoadLibrary(TEXT("dll_sampel_C.dll"));
		//3-�� �������
		Max_in_dual_mass = (Found_Max_in_dual_mass)GetProcAddress(hinstLib_1, "Found_Max_in_dual_mass");
		if (Max_in_dual_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Max_in_dual_mass(mass_3, N_1, N_1);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		  //	cout << "����� ���������� ������� Found_Max_in_dual_mass �� dll_sampel_C.dll :" << endl;
			 Memo1->Lines->Add("����� ���������� ������� Found_Max_in_dual_mass �� dll_sampel_C.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Max_in_dual_mass)\n";
			   Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Max_in_dual_mass");
		//for (int i = 0; i < N_1; i++)
		//{
		//	delete mass_3[i];
		//}

		//delete mass_3;


	}

	FreeLibrary(hinstLib);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::Button1Click(TObject *Sender)
{
Memo1->Lines->Text='fghfghfhg';
}
//---------------------------------------------------------------------------

void __fastcall TForm1::Button_RAD_CClick(TObject *Sender)
{
	Found_max_in_mass Max_in_mass;
	Found_Min Min_in_mass;
	Found_Max_in_dual_mass Max_in_dual_mass;

	//����������� �������
	int N = 100000;
	//����������� ����������� �������
	int N_1 = 600;
	//���-�� �������
	int time_N = 50;
	//������ �� ��������
	double *mass_time_max = new double[time_N];


     	double *mass_1=new double[N];
	double *mass_2=new double[N];
	for (int i = 0; i < N; i++)
	{
		mass_1[i]= rand() % 1000 + 1;
		mass_2[i] = rand() % 1000 + 1;
	}

		Memo1->Lines->Clear();

        // ���������� ���������� ��� ������ �������
	LARGE_INTEGER FbeginCount, FendCount, Ffrequence;
	double Ftime;

	HINSTANCE hinstLib = LoadLibrary(TEXT("Project_RAD_DLL.dll"));
	double result;


//������������� VS DLL
	if (hinstLib == NULL)
	{
	   //	cout << "ERROR: unable to load DLL: dll_sampel_C.dll\n";
		   Memo1->Lines->Add("ERROR: unable to load DLL: Project_RAD_DLL.dll");
	}
	else
	{
	   //	cout << "library (dll_sampel_C.dll) loaded\n";

	   Memo1->Lines->Add("library (Project_RAD_DLL.dll) loaded");

		//1-�� �������
		Max_in_mass = (Found_max_in_mass)GetProcAddress(hinstLib, "_Found_Max");
		if (Max_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Max_in_mass(mass_1, mass_2, N, N);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		  //	cout << "����� ���������� ������� Found_Max �� dll_sampel_C.dll :" << endl;
		   Memo1->Lines->Add("����� ���������� ������� Found_Max �� Project_RAD_DLL.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Max)\n";
			Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Max)\n");

		//2-�� �������
		Min_in_mass = (Found_Min)GetProcAddress(hinstLib, "_Found_Min");
		if (Min_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Min_in_mass(mass_1, mass_2, N, N);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		   //	cout << "����� ���������� ������� Found_Min �� dll_sampel_C.dll :" << endl;
			Memo1->Lines->Add("����� ���������� ������� Found_Min �� Project_RAD_DLL.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Min)\n";
			  Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Min)\n");
		//delete mass_1;
		//delete mass_2;

		double mass_4[20][20];

		double **mass_3 = new double*[N_1];
		for (int i = 0; i < N_1; i++)
		{
			mass_3[i] = new double[N_1];
		}

			for (int y = 0; y < N_1; y++)
		{
			for (int x = 0; x < N_1; x++)
			{
				mass_3[y][x] =  x*y;

			   //	 Form1->Memo1->Lines->Add("mass_3["+FloatToStr(y)+"]["+FloatToStr(x)+"]="+FloatToStr(mass_3[y][x]));
			}
		}







	 int N_2=20;

		for (int y = 0; y < N_2; y++)
		{
			for (int x = 0; x < N_2; x++)
			{
				mass_4[y][x] =  1;

			   //	 Form1->Memo1->Lines->Add("mass_3["+FloatToStr(y)+"]["+FloatToStr(x)+"]="+FloatToStr(mass_3[y][x]));
			}
		}


		double **flip_mass=new double*[N_1];
		for(int i = 0; i <N_2; i++)
		{
				flip_mass[i] = &mass_4[i][0];

		}

		 //	double **flip2_mass=&mass_3[0][0];

		HINSTANCE  hinstLib_1 = LoadLibrary(TEXT("Project_RAD_DLL.dll"));
		//3-�� �������
		Max_in_dual_mass = (Found_Max_in_dual_mass)GetProcAddress(hinstLib_1, "_Found_Max_in_dual_mass");
		if (Max_in_dual_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Max_in_dual_mass(mass_3, N_1, N_1);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		  //	cout << "����� ���������� ������� Found_Max_in_dual_mass �� dll_sampel_C.dll :" << endl;
			 Memo1->Lines->Add("����� ���������� ������� Found_Max_in_dual_mass �� dll_sampel_C.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Max_in_dual_mass)\n";
			   Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Max_in_dual_mass");
		//for (int i = 0; i < N_1; i++)
		//{
		//	delete mass_3[i];
		//}

		//delete mass_3;


	}

	FreeLibrary(hinstLib);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::Button_RAD_DelphiClick(TObject *Sender)
{
	Found_max_in_mass Max_in_mass;
	Found_Min Min_in_mass;
	Found_Max_in_dual_mass Max_in_dual_mass;

	//����������� �������
	int N = 100000;
	//����������� ����������� �������
	int N_1 = 600;
	//���-�� �������
	int time_N = 50;
	//������ �� ��������
	double *mass_time_max = new double[time_N];


     	double *mass_1=new double[N];
	double *mass_2=new double[N];
	for (int i = 0; i < N; i++)
	{
		mass_1[i]= rand() % 1000 + 1;
		mass_2[i] = rand() % 1000 + 1;
	}

		Memo1->Lines->Clear();

        // ���������� ���������� ��� ������ �������
	LARGE_INTEGER FbeginCount, FendCount, Ffrequence;
	double Ftime;

	HINSTANCE hinstLib = LoadLibrary(TEXT("Project_Delphi_DLL.dll"));
	double result;


//������������� VS DLL
	if (hinstLib == NULL)
	{
	   //	cout << "ERROR: unable to load DLL: dll_sampel_C.dll\n";
		   Memo1->Lines->Add("ERROR: unable to load DLL: Project_Delphi_DLL.dll");
	}
	else
	{
	   //	cout << "library (dll_sampel_C.dll) loaded\n";

	   Memo1->Lines->Add("library (Project_Delphi_DLL.dll) loaded");

		//1-�� �������
		Max_in_mass = (Found_max_in_mass)GetProcAddress(hinstLib, "Found_Max");
		if (Max_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Max_in_mass(mass_1, mass_2, N, N);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		  //	cout << "����� ���������� ������� Found_Max �� dll_sampel_C.dll :" << endl;
		   Memo1->Lines->Add("����� ���������� ������� Found_Max �� Project_Delphi_DLL.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Max)\n";
			Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Max)\n");

		//2-�� �������
		Min_in_mass = (Found_Min)GetProcAddress(hinstLib, "Found_Min");
		if (Min_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Min_in_mass(mass_1, mass_2, N, N);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		   //	cout << "����� ���������� ������� Found_Min �� dll_sampel_C.dll :" << endl;
			Memo1->Lines->Add("����� ���������� ������� Found_Min �� Project_Delphi_DLL.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Min)\n";
			  Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Min)\n");
		//delete mass_1;
		//delete mass_2;

		double mass_4[20][20];

		double **mass_3 = new double*[N_1];
		for (int i = 0; i < N_1; i++)
		{
			mass_3[i] = new double[N_1];
		}

			for (int y = 0; y < N_1; y++)
		{
			for (int x = 0; x < N_1; x++)
			{
				mass_3[y][x] =  x*y;

			   //	 Form1->Memo1->Lines->Add("mass_3["+FloatToStr(y)+"]["+FloatToStr(x)+"]="+FloatToStr(mass_3[y][x]));
			}
		}







	 int N_2=20;

		for (int y = 0; y < N_2; y++)
		{
			for (int x = 0; x < N_2; x++)
			{
				mass_4[y][x] =  1;

			   //	 Form1->Memo1->Lines->Add("mass_3["+FloatToStr(y)+"]["+FloatToStr(x)+"]="+FloatToStr(mass_3[y][x]));
			}
		}


		double **flip_mass=new double*[N_1];
		for(int i = 0; i <N_2; i++)
		{
				flip_mass[i] = &mass_4[i][0];

		}

		 //	double **flip2_mass=&mass_3[0][0];

		HINSTANCE  hinstLib_1 = LoadLibrary(TEXT("Project_Delphi_DLL.dll"));
		//3-�� �������
		Max_in_dual_mass = (Found_Max_in_dual_mass)GetProcAddress(hinstLib_1, "Found_Max_in_dual_mass");
		if (Max_in_dual_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// ����� ������� ������ (�� ���������� ��������)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//����� �������
				result = Max_in_dual_mass(mass_3, N_1, N_1);

				/// ���������� ��������� ���������� (����� ������� �� dll)
	// ����� ������� ��������� (����� ���������� ��������)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
		  //	cout << "����� ���������� ������� Found_Max_in_dual_mass �� dll_sampel_C.dll :" << endl;
			 Memo1->Lines->Add("����� ���������� ������� Found_Max_in_dual_mass �� Project_Delphi_DLL.dll :");
			Time_vyvod(mass_time_max, time_N);
		}
		else
		   //	cout << "ERROR: unable to find DLL function (Found_Max_in_dual_mass)\n";
			   Memo1->Lines->Add("ERROR: unable to find DLL function (Found_Max_in_dual_mass");
		//for (int i = 0; i < N_1; i++)
		//{
		//	delete mass_3[i];
		//}

		//delete mass_3;


	}

	FreeLibrary(hinstLib);
}
//---------------------------------------------------------------------------

