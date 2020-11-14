#include "stdafx.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>
 
#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT double getMinRangeOfVector(far double* array1, far double* array2, int size);
DLLEXPORT double getStandardDeviation(far double* array1, far double* array2, int size);
DLLEXPORT double getAvgValue(far double** array1, int size1, int size2);
DLLEXPORT double getMinValue(far double* array1, far double* array2, int size);
DLLEXPORT double getVolume(far double* array1, far double* array2, int size);
DLLEXPORT double getDispersion(far double** array1, int size1, int size2);