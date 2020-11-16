#include "stdafx.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>
 
#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT double __cdecl getMinRangeOfVector(far double* array1, double* array2, int size);
DLLEXPORT double __cdecl getStandardDeviation(far double* array1, double* array2, int size);
DLLEXPORT double __cdecl getAvgValue(far double** array1, int size1, int size2);
DLLEXPORT double __cdecl getMinValue(far double* array1, far double* array2, int size);
DLLEXPORT double __cdecl getVolume(far double* array1, far double* array2, int size);
DLLEXPORT double __cdecl getDispersion(far double** array1, int size1, int size2);

DLLEXPORT void __cdecl test(void);