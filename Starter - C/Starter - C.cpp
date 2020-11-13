// dll_works_C.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include "stdfax.h"

#include <iostream>
#include <Windows.h>
#include <time.h>


using namespace std;

typedef double(__cdecl  *Found_max_in_mass)(double*, double*, int, int);
typedef double(__cdecl  *Found_Min)(double*, double*, int, int);
typedef double(__cdecl  *Found_Max_in_dual_mass)(double**, int, int);


double My_function_max (double *mas_1, double *mas_2, int N_1, int N_2)
{
	double value_max = mas_1[0];

	for (int i = 0; i < N_1; i++)
	{
		if (value_max < mas_1[i])
		{
			value_max = mas_1[i];
		}
	}

	for (int i = 0; i < N_2; i++)
	{
		if (value_max < mas_2[i])
		{
			value_max = mas_2[i];
		}
	}

	return value_max;

}

//ВЫВОД ВРЕМЕНИ
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

	cout << "time_max="<<time_max<<"\t" << "time_avg=" <<time_avg<< "\t" <<"time_min="<<time_min<<endl;


}


int main()
{
	setlocale(0, "");
	srand(time(0));

	Found_max_in_mass Max_in_mass;
	Found_Min Min_in_mass;
	Found_Max_in_dual_mass Max_in_dual_mass;

	//размерность массива
	int N = 100000;
	//размерность двухмерного массива
	int N_1 = 500;
	//Кол-во замеров
	int time_N = 50;
	//массив со временем
	double *mass_time_max = new double[time_N];


	double *mass_1=new double[N];
	double *mass_2=new double[N];
	for (int i = 0; i < N; i++)
	{
		mass_1[i]= rand() % 1000 + 1;
		mass_2[i] = rand() % 1000 + 1;
	}


	cout << "МАССИВЫ СОЗДАНЫ" << endl;

	//double **mass_3=new double *[N_1];
	//for (int i = 1; i < N_1; i++)
	//{
	//	mass_3[i] = new double [N_1];
	//}

	//for (int i = 0; i < N_1; i++)
	//{
	//	for (int j = 0; j < N_1; j++)
	//	{
	//		mass_3[i][j] = i*j;
	//	}
	//}

	cout << "МАССИВЫ СОЗДАНЫ" << endl;

	//for (int i = 0; i < N; i++)
	//{
	//	cout << "mass_1[" << i << "]= " << mass_1[i] <<"\t"<<"mass_2["<<i<<"]= "<<mass_2[i]<< endl;
	//}

	// Объявление переменных для замера времени
	LARGE_INTEGER FbeginCount, FendCount, Ffrequence;
	double Ftime;
	
	HINSTANCE hinstLib = LoadLibrary(TEXT("dll_sampel_C.dll"));
	double result;

	//ИСПОЛЬЗОВАНИЕ VS DLL
	if (hinstLib == NULL) 
	{
		cout << "ERROR: unable to load DLL: dll_sampel_C.dll\n";

	}
	else
	{
		cout << "library (dll_sampel_C.dll) loaded\n";

		//1-АЯ ФУНКЦИЯ
		Max_in_mass = (Found_max_in_mass)GetProcAddress(hinstLib, "Found_Max");		
		if (Max_in_mass != NULL) 
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
				result = Max_in_mass(mass_1, mass_2, N, N);
				
				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Max из dll_sampel_C.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Max)\n";

		//2-АЯ ФУНКЦИЯ
		Min_in_mass = (Found_Min)GetProcAddress(hinstLib, "Found_Min");
		if (Min_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
				result = Min_in_mass(mass_1, mass_2, N, N);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Min из dll_sampel_C.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Min)\n";

		//delete mass_1;
		//delete mass_2;


		//double **mass_3 = new double*[N_1];
		//for (int i = 0; i < N_1; i++)
		//{
		//	mass_3[i] = new double[N_1];
		//}
		//for (int i = 0; i < N_1; i++)
		//{
		//	for (int j = 0; j < N_1; i++)
		//	{
		//		mass_3[i][j] = rand() % 1000 + 1;
		//	}
		//}



		//3-АЯ ФУНКЦИЯ
		Max_in_dual_mass = (Found_Max_in_dual_mass)GetProcAddress(hinstLib, "Found_Max_in_dual_mass");
		if (Max_in_dual_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
//				result = Max_in_dual_mass(mass_3, N_1, N_1);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Max_in_dual_mass из dll_sampel_C.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Max_in_dual_mass)\n";

		//for (int i = 0; i < N_1; i++)
		//{
		//	delete mass_3[i];
		//}

		//delete mass_3;


	}

	FreeLibrary(hinstLib);


	//ИСПОЛЬЗОВАНИЕ RAD C++ DLL
	HINSTANCE hinstLib_RAD_C = LoadLibrary(TEXT("Project_RAD_DLL.dll"));
	if (hinstLib_RAD_C == NULL)
	{
		cout << "ERROR: unable to load DLL: Project_RAD_DLL.dll\n";

	}
	else
	{
		cout << "library (Project_RAD_DLL.dll) loaded\n";


		//double *mass_1 = new double[N];
		//double *mass_2 = new double[N];
		//for (int i = 0; i < N; i++)
		//{
		//	mass_1[i] = rand() % 1000 + 1;
		//	mass_2[i] = rand() % 1000 + 1;
		//}

		//1-АЯ ФУНКЦИЯ
		Max_in_mass = (Found_max_in_mass)GetProcAddress(hinstLib_RAD_C, "_Found_Max");
		if (Max_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
				result = Max_in_mass(mass_1, mass_2, N, N);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Max из Project_RAD_DLL.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Max)\n";

		//2-АЯ ФУНКЦИЯ
		Min_in_mass = (Found_Min)GetProcAddress(hinstLib_RAD_C, "_Found_Min");
		if (Min_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
				result = Min_in_mass(mass_1, mass_2, N, N);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Min из Project_RAD_DLL.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Min)\n";

		//delete mass_1;
		//delete mass_2;

		//double **mass_3 = new double*[N_1];
		//for (int i = 0; i < N_1; i++)
		//{
		//	mass_3[i] = new double[N_1];
		//}
		//for (int i = 0; i < N_1; i++)
		//{
		//	for (int j = 0; j < N_1; i++)
		//	{
		//		mass_3[i][j] = rand() % 1000 + 1;
		//	}
		//}

		//3-АЯ ФУНКЦИЯ
		Max_in_dual_mass = (Found_Max_in_dual_mass)GetProcAddress(hinstLib_RAD_C, "_Found_Max_in_dual_mass");
		if (Max_in_dual_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
//				result = Max_in_dual_mass(mass_3, N_1, N_1);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Max_in_dual_mass из Project_RAD_DLL.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Max_in_dual_mass)\n";


		//for (int i = 0; i < N_1; i++)
		//{
		//	delete mass_3[i];
		//}

		//delete mass_3;
	}

	FreeLibrary(hinstLib_RAD_C);


	//ИСПОЛЬЗОВАНИЕ RAD Delphi DLL
	HINSTANCE hinstLib_RAD_Delphi = LoadLibrary(TEXT("Project_Delphi_DLL.dll"));
	if (hinstLib_RAD_Delphi == NULL)
	{
		cout << "ERROR: unable to load DLL: Project_Delphi_DLL.dll\n";

	}
	else
	{
		cout << "library (Project_Delphi_DLL.dll) loaded\n";

		//double *mass_1 = new double[N];
		//double *mass_2 = new double[N];
		//for (int i = 0; i < N; i++)
		//{
		//	mass_1[i] = rand() % 1000 + 1;
		//	mass_2[i] = rand() % 1000 + 1;
		//}


		//1-АЯ ФУНКЦИЯ
		Max_in_mass = (Found_max_in_mass)GetProcAddress(hinstLib_RAD_Delphi, "Found_Max");
		if (Max_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
				result = Max_in_mass(mass_1, mass_2, N, N);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Max из Project_Delphi_DLL.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Max)\n";

		//2-АЯ ФУНКЦИЯ
		Min_in_mass = (Found_Min)GetProcAddress(hinstLib_RAD_Delphi, "Found_Min");
		if (Min_in_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
				result = Min_in_mass(mass_1, mass_2, N, N);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Min из Project_Delphi_DLL.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Min)\n";


		//delete mass_1;
		//delete mass_2;


		//double **mass_3 = new double*[N_1];
		//for (int i = 0; i < N_1; i++)
		//{
		//	mass_3[i] = new double[N_1];
		//}
		//for (int i = 0; i < N_1; i++)
		//{
		//	for (int j = 0; j < N_1; i++)
		//	{
		//		mass_3[i][j] = rand() % 1000 + 1;
		//	}
		//}


		//3-АЯ ФУНКЦИЯ
		Max_in_dual_mass = (Found_Max_in_dual_mass)GetProcAddress(hinstLib_RAD_Delphi, "Found_Max_in_dual_mass");
		if (Max_in_dual_mass != NULL)
		{
			for (int count = 0; count < 50; count++)
			{
				// Замер времени начала (до выполнения действий)
				QueryPerformanceFrequency(&Ffrequence);
				QueryPerformanceCounter(&FbeginCount);

				//ВЫЗОВ ФУНКЦИИ
//				result = Max_in_dual_mass(mass_3, N_1, N_1);

				/// Выполнение некоторых вычислений (вызов функций из dll)
	// Замер времени окончания (после выполнения действий)
				QueryPerformanceCounter(&FendCount);
				Ftime = ((FendCount.QuadPart - FbeginCount.QuadPart) / (double)Ffrequence.QuadPart) * 1000;

				mass_time_max[count] = Ftime;
			}
			cout << "Время выполнения функции Found_Max_in_dual_mass из Project_Delphi_DLL.dll :" << endl;
			Time_vyvod(mass_time_max, time_N);
		}
		else
			cout << "ERROR: unable to find DLL function (Found_Max_in_dual_mass)\n";
	

		//for (int i = 0; i < N_1; i++)
		//{
		//	delete mass_3[i];
		//}

		//delete mass_3;


	}

	FreeLibrary(hinstLib_RAD_Delphi);
}

