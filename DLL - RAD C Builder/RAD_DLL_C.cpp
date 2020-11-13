//   Important note about DLL memory management when your DLL uses the
//   static version of the RunTime Library:
//
//   If your DLL exports any functions that pass String objects (or structs/
//   classes containing nested Strings) as parameter or function results,
//   you will need to add the library MEMMGR.LIB to both the DLL project and
//   any other projects that use the DLL.  You will also need to use MEMMGR.LIB
//   if any other projects which use the DLL will be performing new or delete
//   operations on any non-TObject-derived classes which are exported from the
//   DLL. Adding MEMMGR.LIB to your project will change the DLL and its calling
//   EXE's to use the BORLNDMM.DLL as their memory manager.  In these cases,
//   the file BORLNDMM.DLL should be deployed along with your DLL.
//
//   To avoid using BORLNDMM.DLL, pass string information using "char *" or
//   ShortString parameters.
//
//   If your DLL uses the dynamic version of the RTL, you do not need to
//   explicitly add MEMMGR.LIB as this will be done implicitly for you

#include <windows.h>

#define DLLEXPORT extern "C" __declspec(dllexport)  __cdecl

#ifdef _MSC_VER
	#define  Found_Max _Found_Max
	#define  Found_Min _Found_Min
	#define  Found_Max_in_dual_mass _Found_Max_in_dual_mass
#endif


DLLEXPORT double Found_Max(double *mas_1,double *mas_2,int N_1,int N_2);

DLLEXPORT double Found_Min(double *mas_1, double *mas_2, int N_1, int N_2);

DLLEXPORT double Found_Max_in_dual_mass(double **mas_1, int N_1, int N_2);


#pragma hdrstop
#pragma argsused

extern "C" int _libmain(unsigned long reason)
{

	return 1;
}

	//Находим максимальный элемент из двух массивов
DLLEXPORT double Found_Max(double *mas_1, double *mas_2, int N_1, int N_2)
{
	double value_max = mas_1[0];

	for (int i = 0; i < N_1; i++)
	{
		if (value_max<mas_1[i])
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


//Находим максимальный элемент из двух массивов
DLLEXPORT double Found_Min(double *mas_1, double *mas_2, int N_1, int N_2)
{
	double value_min = mas_1[0];

	for (int i = 0; i < N_1; i++)
	{
		if (value_min > mas_1[i])
		{
			value_min = mas_1[i];
		}
	}

	for (int i = 0; i < N_2; i++)
	{
		if (value_min > mas_2[i])
		{
			value_min = mas_2[i];
		}
	}

	return value_min;

}


//Находим максимальный элемент из двух массивов
DLLEXPORT double Found_Max_in_dual_mass(double **mas_1, int N_1, int N_2)
{
	double Max_value=mas_1[0][0];

	for (int i = 0; i < N_1; i++)
	{
		for (int j = 0; j < N_2; j++)
		{
			if (Max_value < mas_1[i][j])
			{
				Max_value = mas_1[i][j];
			}
		}
	}

	return Max_value;

}




