#include "pch.h"
#include "dll_header.h"

using namespace std;

//сумма векторов

DLLEXPORT void sumVector(double* vector1, double* vector2, double* vector, int N) {

	for (int i = 0; i < N; i++) {
		vector[i] = vector1[i] + vector2[i];
	}
}
//умножение векторов
DLLEXPORT void mulVector(double* vector1, double* vector2, double* vector, int N) {

	for (int i = 0; i < N; i++) {
		vector[i] = vector1[i] * vector2[i];
	}
}
//транспонирование
DLLEXPORT void transposeMatrix(double* a, double* b, int n, int m) {
	for (int i = 0; i < m; ++i){
		for (int j = 0; j < n; ++j){
			b[i*n + j] = a[j*m + i];
		}
	}

}
//умножение матриц
DLLEXPORT void mulMatrix(double* A, double* B, double* C, int M, int N, int K) {
	for (int i = 0; i < M; ++i)
	{
		double * c = C + i * N;
		for (int j = 0; j < N; ++j)
			c[j] = 0;
		for (int k = 0; k < K; ++k)
		{
			const double * b = B + k * N;
			float a = A[i*K + k];
			for (int j = 0; j < N; ++j)
				c[j] += a * b[j];
		}
	}
	/*for (int i = 0; i < m; i++) {
		for (int j = 0; j < n; j++) {
			c[i*n+ j] = 0;
			for (int h = 0; h < k; h++) {
				c[i*n+j] += a[i*k + h] * b[h*n + j];
			}
		}
	}*/
}