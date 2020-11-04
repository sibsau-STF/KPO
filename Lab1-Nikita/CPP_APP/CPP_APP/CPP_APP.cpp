// CPP_APP.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//
#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <string.h>
#include <Windows.h>
#include <fstream>

using namespace std;

typedef double(*DVectorOps)(double* a, double* b, double* c, int N);
typedef double(*FVectorOps)(char* s1, char* s2, char* s3, float *a, float* b, float* c, int N);
typedef double(*DMatrixOps)(double* a, double* b, int N, int M);
typedef double(*FMatrixOps)(char* s1, char* s2, char* s3, float* a, float* b, int N, int M);

// Переменные для замера времени
double 	Ftime;
LARGE_INTEGER FFrequence, FBeginCount, FEndCount;

double minTime = 1000000;
double maxTime = 0;
double sumTime = 0;

template <typename T>
void printVector(T* a, int N) {
	cout << "[" << a[0];
	for (size_t i = 1; i < N; i++) {
		cout << ", " << a[i];
	}
	cout << "]" << endl;
}
template <typename T>
void printMatrix(T* a, int N, int M) {
	cout << "[";
	for (size_t i = 0; i < N; i++) {
		for (size_t j = 0; j < M; j++) {
			cout << a[i*N + j];
			if(j != M-1) cout << ", ";
		}
		if (i != N - 1) cout << endl;
	}
	cout << "]" << endl;
}

void cppDllWork(ofstream &fout) {
	//размер вектора, размеры матриц
	int size = 100000, m = 700, k = 3, n = 700;

	//выделение памяти для векторов
	double* a = new double[size];
	double* b = new double[size];
	double* c = new double[size];
	double* mat1 = new double[m*n];
	double* mat2 = new double[n*m];
	//инициализация векторов
	for (int i = 0; i < size; i++) {
		a[i] = rand() % 60;
		b[i] = rand() % 40;
		c[i] = 0;
	}
	//инициализация матрицы
	for (int i = 0; i < m; i++) {
		for (int j = 0; j < n; j++) {
			mat1[i*m + j] = rand() % 6;
		}
	}
	//cout << "Вектор А:\n"; printVector(a, size); cout << endl;
	//cout << "Вектор B:\n"; printVector(b, size); cout << endl;
	//cout << "Матрица Mat1:\n"; printMatrix(mat1, m, n); cout << endl;

	//подключение библиотеки
	HINSTANCE hLib = LoadLibrary(TEXT("cpp_dll.dll"));

	//сумма векторов
	DVectorOps sumVector = (DVectorOps) GetProcAddress(hLib, "sumVector");
	if (sumVector != NULL) {
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;

		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);

			int i = sumVector(a, b, c, size);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;

		}
		//cout << "A+B:\n"; printVector(c, size); cout << endl;
		fout << "minTime =;" << minTime << endl; cout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl; cout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl; cout << "AVG =;" << sumTime / 50.0 << endl;
	}
	//умножение элем. векторов
	DVectorOps mulVector = (DVectorOps)GetProcAddress(hLib, "mulVector");
	if (mulVector != NULL) {
		
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;
		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);
			int i = mulVector(a, b, c, size);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;
		}
		fout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl;
		//cout << "A*B:\n"; printVector(c, size); cout << endl;
	}
	//транспонирование
	DMatrixOps transposeMatrix = (DMatrixOps)GetProcAddress(hLib, "transposeMatrix");
	if (transposeMatrix != NULL) {
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;
		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);
			int i = transposeMatrix(mat1, mat2, m, n);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;
		}
		fout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl;
		//cout << "Транспонирование:\n"; printMatrix(mat2, n,m);
	}
}
void delphiDllWork(ofstream &fout) {
	//размер вектора, размеры матриц
	int size = 100000, m = 70, k = 3, n = 70;

	//выделение памяти для векторов
	double* a = new double[size];
	double* b = new double[size];
	double* c = new double[size];
	double* mat1 = new double[m*n];
	double* mat2 = new double[n*m];
	//инициализация векторов
	for (int i = 0; i < size; i++) {
		a[i] = rand() % 60;
		b[i] = rand() % 40;
		c[i] = 0;
	}
	//инициализация матрицы
	for (int i = 0; i < m; i++) {
		for (int j = 0; j < n; j++) {
			mat1[i*m + j] = rand() % 6;
		}
	}
	//cout << "Вектор А:\n"; printVector(a, size); cout << endl;
	//cout << "Вектор B:\n"; printVector(b, size); cout << endl;
	//cout << "Матрица Mat1:\n"; printMatrix(mat1, m, n); cout << endl;

	//подключение библиотеки
	HINSTANCE hLib = LoadLibrary(TEXT("delphi_dll.dll"));

	//сумма векторов
	DVectorOps sumVector = (DVectorOps)GetProcAddress(hLib, "sumVector");
	if (sumVector != NULL) {
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;

		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);

			int i = sumVector(a, b, c, size);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;

		}
		//cout << "A+B:\n"; printVector(c, size); cout << endl;
		fout << "minTime =;" << minTime << endl; cout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl; cout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl; cout << "AVG =;" << sumTime / 50.0 << endl;
	}
	//умножение элем. векторов
	DVectorOps mulVector = (DVectorOps)GetProcAddress(hLib, "mulVector");
	if (mulVector != NULL) {

		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;
		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);
			int i = mulVector(a, b, c, size);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;
		}
		fout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl;
		//cout << "A*B:\n"; printVector(c, size); cout << endl;
	}
	//транспонирование
	DMatrixOps transposeMatrix = (DMatrixOps)GetProcAddress(hLib, "transposeMatrix");
	if (transposeMatrix != NULL) {
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;
		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);
			int i = transposeMatrix(mat1, mat2, m, n);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;
		}
		fout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl;
		//cout << "Транспонирование:\n"; printMatrix(mat2, n,m);
	}
}
void cudaDllWork(ofstream &fout) {
	//размер вектора, размеры матриц
	int size = 100000, m = 700, k = 3, n = 700;

	//выделение памяти для векторов
	float* a = new float[size];
	float* b = new float[size];
	float* c = new float[size];
	float* mat1 = new float[m*n];
	float* mat2 = new float[n*m];
	//инициализация векторов
	for (int i = 0; i < size; i++) {
		a[i] = rand() % 6;
		b[i] = rand() % 4;
		c[i] = 0;
	}
	//инициализация матрицы
	for (int i = 0; i < m; i++) {
		for (int j = 0; j < n; j++) {
			mat1[i*m + j] = rand() % 6;
		}
	}

	//cout << "Вектор А:\n"; printVector(a, size); cout << endl;
	//cout << "Вектор B:\n"; printVector(b, size); cout << endl;
	//cout << "Матрица Mat1:\n"; printMatrix(mat1, m, n); cout << endl;

	//подключение библиотеки
	HINSTANCE hLib = LoadLibrary(TEXT("cuda_dll_v2.dll"));

	//сумма векторов
	FVectorOps sumVector = (FVectorOps)GetProcAddress(hLib, "sumVector");
	if (sumVector != NULL) {
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;
		char* s1 = new char[3]{ 'A','+','B' };
		char* s2 = new char[2]{ ':', ' ' };
		char* s3 = new char[255];
		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);
		
			int i = sumVector(s1,s2,s3,a, b, c, size);
		
			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;
		}
		fout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl;
		//cout << "A+B:\n"; printVector(c, size); cout << endl;
	}
	//умножение элем. векторов
	FVectorOps mulVector = (FVectorOps)GetProcAddress(hLib, "mulVector");
	if (mulVector != NULL) {
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;
		char* s1 = new char[3] {'A', '+', 'B'};
		char* s2 = new char[2] {':', ' '};
		char* s3 = new char[255];
		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);

			int i = mulVector(s1, s2, s3, a, b, c, size);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;
		}
		fout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl;
		//cout << "A*B:\n"; printVector(c, size); cout << endl;
	}
	//транспонирование
	FMatrixOps transposeMatrix = (FMatrixOps)GetProcAddress(hLib, "transposeMatrix");
	if (transposeMatrix != NULL) {
		minTime = 1000000;
		maxTime = 0;
		sumTime = 0;
		char* s1 = new char[4] {'m', 'a', 't',' '};
		char* s2 = new char[3] {'T', ':',' '};
		char* s3 = new char[255];
		for (int j = 0; j < 50; j++) {
			// замер начала
			QueryPerformanceFrequency(&FFrequence);
			QueryPerformanceCounter(&FBeginCount);

			int i = transposeMatrix(s1, s2, s3, mat1, mat2, m, n);

			QueryPerformanceCounter(&FEndCount);
			Ftime = ((FEndCount.QuadPart - FBeginCount.QuadPart) / (double)FFrequence.QuadPart) * 1000;
			if (Ftime < minTime) minTime = Ftime;
			if (Ftime > maxTime) maxTime = Ftime;
			sumTime += Ftime;
		}
		fout << "minTime =;" << minTime << endl;
		fout << "maxTime =;" << maxTime << endl;
		fout << "AVG =;" << sumTime / 50.0 << endl;
		
		//cout << "Транспонирование:\n"; printMatrix(mat2, n, m);
	}
}
int main()
{
	setlocale(LC_ALL, "rus");
	ofstream fout;
	int command = 0;
	while (true) {
		system("cls");
		cout << "1. c++ dll\n2. delphi dll\n3. cuda dll\n4. Выход\nУкажите номер: "; cin >> command;
		switch (command)
		{
		case 1:
			system("cls");
			fout.open("cpp.csv");
			cppDllWork(fout);
			fout.close();
			system("pause");
			break;
		case 2:
			system("cls");
			fout.open("delphi.csv");
			delphiDllWork(fout);
			fout.close();
			system("pause");
			break;
		case 3:
			system("cls");
			fout.open("cuda.csv");
			cudaDllWork(fout);
			fout.close();
			system("pause");
			break;
		default:
			break;
		}
 		if (command == 4) break;
	}
}