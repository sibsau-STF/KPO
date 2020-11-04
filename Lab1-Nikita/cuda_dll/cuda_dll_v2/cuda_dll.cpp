#include "cuda_dll_header.h"

DLLEXPORT int sumVector(char* s1, char* s2, char* s3, float * a, float * b, float * c, int N)
{
	return sum(s1, s2, s3, a, b, c, N);
}

DLLEXPORT int mulVector(char* s1, char* s2, char* s3, float * a, float * b, float * c, int N)
{
	return mul(s1, s2, s3, a, b, c, N);
}

DLLEXPORT int transposeMatrix(char* s1, char* s2, char* s3, float * a, float * b, int N, int M)
{
	return transpose(s1, s2, s3, a, b, N,M);
}

DLLEXPORT int mulMatrix(char* s1, char* s2, char* s3, float * a, float * b, float * c, int M, int K, int N)
{
	return matMul(s1, s2, s3, a, b, c, M, K, N);
}