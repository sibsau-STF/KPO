#include "stdafx.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>
 
#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT double Found_Max(far double* mas_1, far double* mas_2, int N_1, int N_2);
DLLEXPORT double Found_Min(far double* mas_1, far double* mas_2, int N_1, int N_2);
DLLEXPORT double Found_Max_in_dual_mass(far double** mas_1, int N_1, int N_2);