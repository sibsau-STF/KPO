#include "stdafx.h"
#include "dll_sample.h"
#include "time.h"
#include <sstream> 
#include <string.h> 
#include <vector>

using namespace std;

DLLEXPORT double getMinRangeOfVector(far double* array1, far double* array2, int size)
{
	double minValue1 = array1[0], double maxValue1 = array1[0];
	for (int i = 1; i < size; i++)
	{
		if (minValue1 > array1[i])
			minValue1 = array1[i];
		if (maxValue1 < array1[i])
			maxValue1 = array1[i];
	}

	double minValue2 = array1[0], double maxValue2 = array1[0];
	for (int i = 1; i < size; i++)
	{
		if (minValue2 > array2[i])
			minValue2 = array2[i];
		if (maxValue2 < array2[i])
			maxValue2 = array2[i];
	}
	double range1 = maxValue1 - minValue1;
	double range2 = maxValue2 - minValue2;

	return range1 < range2 ? range1 : range2;
}

DLLEXPORT double getStandardDeviation(far double* array1, far double* array2, int size)
{
	double summ1 = 0;
	for (int i = 0; i < size; i++)
		summ1 += array1[i] * array1[i] - array2[i] * array2[i];

	return sqrt(summ1);
}

DLLEXPORT double getAvgValue(far double** array1, int size1, int size2) {
	double summ;
	for (int i = 0; i < size1; i++)
		for (int j = 0; j < size2; j++)
			summ += array1[i][j];
	return summ / (size1 * size2);
}



DLLEXPORT double getMinValue(far double* array1, far double* array2, int size)
{
	double minValue = array1[0];
	for (int i = 1; i < size; i++)
		if (minValue > array1[i])
			minValue = array1[i];

	for (int i = 1; i < size; i++)
		if (minValue > array2[i])
			minValue = array2[i];

	return minValue;
}

DLLEXPORT double getVolume(far double* array1, far double* array2, int size)
{
	double volume = 1;
	for (int i = 0; i < size; i++)
		volume *= abs(array1[i] - array2[i]);
	return volume;
}

DLLEXPORT double getDispersion(far double** array1, int size1, int size2) {
	double summ;
	for (int i = 0; i < size1; i++)
		for (int j = 0; j < size2; j++)
		{
			summ += array1[i][j] * array1[i][j];
		}
	return sqrt(summ / (size1 * size2));
}

