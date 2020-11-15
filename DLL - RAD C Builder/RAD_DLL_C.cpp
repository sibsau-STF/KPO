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
#include <math.h>

#define DLLEXPORT extern "C" __declspec(dllexport)  __cdecl

#ifdef _MSC_VER
	#define  getMinRangeOfVector _getMinRangeOfVector
	#define  getStandardDeviation _getStandardDeviation
	#define  getAvgValue _getAvgValue
	#define  getMinValue _getMinValue
	#define  getVolume _getVolume
	#define  getDispersion _getDispersion
#endif


DLLEXPORT double getMinRangeOfVector(double* array1, double* array2, int size);
DLLEXPORT double getStandardDeviation(double* array1, double* array2, int size);
DLLEXPORT double getAvgValue(double** array1, int size1, int size2);
DLLEXPORT double getMinValue(double* array1, double* array2, int size);
DLLEXPORT double getVolume(double* array1, double* array2, int size);
DLLEXPORT double getDispersion(double** array1, int size1, int size2);


#pragma hdrstop
#pragma argsused

extern "C" int _libmain(unsigned long reason)
{

	return 1;
}

DLLEXPORT double getMinRangeOfVector(double* array1, double* array2, int size)
{
	double minValue1 = array1[0];
	double maxValue1 = array1[0];
	for (int i = 1; i < size; i++)
	{
		if (minValue1 > array1[i])
			minValue1 = array1[i];
		if (maxValue1 < array1[i])
			maxValue1 = array1[i];
	}

	double minValue2 = array2[0];
	double maxValue2 = array2[0];
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

DLLEXPORT double getStandardDeviation(double* array1, double* array2, int size)
{
	double summ1 = 0;
	for (int i = 0; i < size; i++)
		summ1 += array1[i] * array1[i] - array2[i] * array2[i];

	return sqrt(summ1);
}

DLLEXPORT double getAvgValue(double** array1, int size1, int size2) {
	double summ = 0;
	for (int i = 0; i < size1; i++)
		for (int j = 0; j < size2; j++)
			summ += array1[i][j];
	return summ / (size1 * size2);
}



DLLEXPORT double getMinValue(double* array1, double* array2, int size)
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

DLLEXPORT double getVolume(double* array1, double* array2, int size)
{
	double volume = 1;
	for (int i = 0; i < size; i++)
		volume *= abs(array1[i] - array2[i]);
	return volume;
}

DLLEXPORT double getDispersion(double** array1, int size1, int size2) {
	double summ = 0;
	for (int i = 0; i < size1; i++)
		for (int j = 0; j < size2; j++)
		{
			summ += array1[i][j] * array1[i][j];
		}
	return sqrt(summ / (size1 * size2));
}




