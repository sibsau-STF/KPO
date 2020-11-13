#include "stdafx.h"
#include "dll_sample.h"
#include "time.h"
#include <sstream> 
#include <string.h> 
#include <vector>

using namespace std;

DLLEXPORT double Found_Max(far double* mas_1, far double* mas_2, int N_1, int N_2)
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


//Находим максимальный элемент из двух массивов
DLLEXPORT double Found_Min(far double* mas_1, far double* mas_2, int N_1, int N_2)
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
DLLEXPORT double Found_Max_in_dual_mass(far double** mas_1, int N_1, int N_2)
{
	double Max_value = mas_1[0][0];

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
