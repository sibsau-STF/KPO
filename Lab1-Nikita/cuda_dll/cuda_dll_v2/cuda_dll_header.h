#pragma once
#include <windows.h>;
#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <stdio.h>
#include <stdlib.h>

//соглашение
#define DLLEXPORT extern "C" __declspec(dllexport)

//векторное представление матрицы
#define IDX2C(i ,j , ld ) ((( j )*( ld ))+( i ))

//сумма векторов
int sum(char* s1, char* s2, char* s3, float *a, float *b, float *c, int N);
//умножение элем-ов векторов
int mul(char* s1, char* s2, char* s3, float *a, float *b, float *c, int N);
//транс-ние
int transpose(char* s1, char* s2, char* s3, float *a, float *b, int N, int M);
//умножение матриц
int matMul(char* s1, char* s2, char* s3, float *a, float *b, float *c, int M, int K, int N);