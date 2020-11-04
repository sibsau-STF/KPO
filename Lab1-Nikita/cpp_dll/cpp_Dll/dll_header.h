#include "pch.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>

#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT void sumVector(double* vector1, double* vector2, double* vector, int N);
DLLEXPORT void mulVector(double* vector1, double* vector2, double* vector, int N);
DLLEXPORT void transposeMatrix(double* a, double* b, int n, int m);
DLLEXPORT void mulMatrix(double* a, double* b, double* c, int n, int m, int k);