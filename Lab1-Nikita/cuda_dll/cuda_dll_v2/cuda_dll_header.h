#pragma once
#include <windows.h>;
#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <stdio.h>
#include <stdlib.h>

//����������
#define DLLEXPORT extern "C" __declspec(dllexport)

//��������� ������������� �������
#define IDX2C(i ,j , ld ) ((( j )*( ld ))+( i ))

//����� ��������
int sum(char* s1, char* s2, char* s3, float *a, float *b, float *c, int N);
//��������� ����-�� ��������
int mul(char* s1, char* s2, char* s3, float *a, float *b, float *c, int N);
//�����-���
int transpose(char* s1, char* s2, char* s3, float *a, float *b, int N, int M);
//��������� ������
int matMul(char* s1, char* s2, char* s3, float *a, float *b, float *c, int M, int K, int N);