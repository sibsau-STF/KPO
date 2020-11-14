#include "stdafx.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>
 
#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT double __cdecl getMinRangeOfVector(double* array1, double* array2, int size);
DLLEXPORT double __cdecl getStandardDeviation(double* array1, double* array2, int size);
DLLEXPORT double __cdecl getAvgValue(double** array1, int size1, int size2);
DLLEXPORT double __cdecl getMinValue(double* array1, double* array2, int size);
DLLEXPORT double __cdecl getVolume(double* array1, double* array2, int size);
DLLEXPORT double __cdecl getDispersion(double** array1, int size1, int size2);

DLLEXPORT void __cdecl test(void);