#include <windows.h>
#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include "cuda_dll_header.h"


cudaError_t addWithCuda(float * a, float * b, float * c, int N);
cudaError_t mulWithCuda(float * a, float * b, float * c, int N);
cudaError_t transWithCuda(float * a, float * b, int N,int M);
cudaError_t matMulWithCuda(float * a, float * b, float * c, int M, int K, int N);

//сумма векторов
__global__ void addKernel(float *a, float *b, float *c)
{
	int i = threadIdx.x;
	c[i] = a[i] + b[i];
}
int sum(char* s1, char* s2, char* s3, float * a, float * b, float * c, int N)
{
	char* s11; char* s22; char* s33;
	s11 = s1; s22 = s2; s33 = s3;
	while (*s11) { *s33 = *s11; s33++; s11++; }
	while (*s22) { *s33 = *s22; s33++; s22++; }
	*s33 = 0;

	cudaError_t cudaStatus = addWithCuda(a, b, c, N);
	cudaStatus = cudaDeviceReset();
	return 0;
}
cudaError_t addWithCuda(float * a, float * b, float * c, int N)
{
	int size = N * sizeof(float);
	cudaError_t cudaStatus;
	/*Define and initialize arrays in HOST*/
	float* h_A = (float *)malloc(size);
	float* h_B = (float *)malloc(size);
	float* h_C = (float *)malloc(size);

	for (int i = 0; i < N; i++) {
		h_A[i] = a[i];
		h_B[i] = b[i];
		h_C[i] = c[i];
	}

	/*Define and allocate arrays in DEVICE*/
	float* d_A;
	float* d_B;
	float* d_C;
	cudaMalloc((void **)&d_A, size);
	cudaMalloc((void **)&d_B, size);
	cudaMalloc((void **)&d_C, size);

	/*Copy arrays from HOST to DEVICE*/
	cudaStatus = cudaMemcpy(d_A, h_A, size, cudaMemcpyHostToDevice);
	cudaStatus = cudaMemcpy(d_B, h_B, size, cudaMemcpyHostToDevice);
	cudaStatus = cudaMemcpy(d_C, h_C, size, cudaMemcpyHostToDevice);

	/*Define level of parallelism*/
	dim3 threadsPerBlock(16, 16);
	dim3 numBlocks(1, 1, 1);

	/*Launch kernel and synchronize*/
	addKernel << <numBlocks, threadsPerBlock >> > (d_A, d_B, d_C);
	cudaStatus = cudaDeviceSynchronize();

	/*Copy output array from DEVICE to HOST*/
	cudaMemcpy(h_C, d_C, size, cudaMemcpyDeviceToHost);
	for (int i = 0; i < N; i++) {
		c[i] = h_C[i];
	}
	/*Free device memory*/
	cudaFree(d_A);
	cudaFree(d_B);
	cudaFree(d_C);

	return cudaStatus;
}

//поэлем.умножение векторов
__global__ void mulKernel(float *a, float *b, float *c)
{
	int i = threadIdx.x;
	c[i] = a[i] * b[i];
}
int mul(char* s1, char* s2, char* s3, float * a, float * b, float * c, int N)
{
	char* s11; char* s22; char* s33;
	s11 = s1; s22 = s2; s33 = s3;
	while (*s11) { *s33 = *s11; s33++; s11++; }
	while (*s22) { *s33 = *s22; s33++; s22++; }
	*s33 = 0;

	cudaError_t cudaStatus = mulWithCuda(a, b, c, N);
	cudaStatus = cudaDeviceReset();
	return 0;
}
cudaError_t mulWithCuda(float * a, float * b, float * c, int N)
{
	int size = N * sizeof(float);
	cudaError_t cudaStatus;
	/*Define and initialize arrays in HOST*/
	float* h_A = (float *)malloc(size);
	float* h_B = (float *)malloc(size);
	float* h_C = (float *)malloc(size);

	for (int i = 0; i < N; i++) {
		h_A[i] = a[i];
		h_B[i] = b[i];
		h_C[i] = c[i];
	}

	/*Define and allocate arrays in DEVICE*/
	float* d_A;
	float* d_B;
	float* d_C;
	cudaMalloc((void **)&d_A, size);
	cudaMalloc((void **)&d_B, size);
	cudaMalloc((void **)&d_C, size);

	/*Copy arrays from HOST to DEVICE*/
	cudaStatus = cudaMemcpy(d_A, h_A, size, cudaMemcpyHostToDevice);
	cudaStatus = cudaMemcpy(d_B, h_B, size, cudaMemcpyHostToDevice);
	cudaStatus = cudaMemcpy(d_C, h_C, size, cudaMemcpyHostToDevice);

	/*Define level of parallelism*/
	dim3 threadsPerBlock(16, 16);
	dim3 numBlocks(1, 1, 1);

	/*Launch kernel and synchronize*/
	mulKernel << <numBlocks, threadsPerBlock >> > (d_A, d_B, d_C);
	cudaStatus = cudaDeviceSynchronize();

	/*Copy output array from DEVICE to HOST*/
	cudaMemcpy(h_C, d_C, size, cudaMemcpyDeviceToHost);
	for (int i = 0; i < N; i++) {
		c[i] = h_C[i];
	}
	/*Free device memory*/
	cudaFree(d_A);
	cudaFree(d_B);
	cudaFree(d_C);

	return cudaStatus;
}

//транспонирование
__global__ void transKernel(float* inputMatrix, float* outputMatrix, int width, int height)
{
	int x = blockDim.x * blockIdx.x + threadIdx.x;
	int y = blockDim.y * blockIdx.y + threadIdx.y;

	for (int x = 0; x < width; x++)
		for (int y = 0; y < height; y++)
			outputMatrix[x * height + y] = inputMatrix[y * width + x];
}
int transpose(char* s1, char* s2, char* s3, float * a, float * b,  int N, int M)
{
	char* s11; char* s22; char* s33;
	s11 = s1; s22 = s2; s33 = s3;
	while (*s11) { *s33 = *s11; s33++; s11++; }
	while (*s22) { *s33 = *s22; s33++; s22++; }
	*s33 = 0;

	cudaError_t cudaStatus = transWithCuda(a, b, N, M);
	cudaStatus = cudaDeviceReset();
	return 0;
}
cudaError_t transWithCuda(float * a, float * b, int N, int M)
{
	int width = N;     //Ширина матрицы
	int height = M;    //Высота матрицы
	int size = width * height;

	cudaError_t stat;

	//Выделение памяти под матрицы на хосте
	float* a_h = (float *)malloc(sizeof(float) * size);
	float* b_h = (float *)malloc(sizeof(float) * size);

	// define an mxk matrix a column by column
	for (int i = 0; i < N; i++) {
		for (int j = 0; j < M; j++) {
			a_h[IDX2C(i, j, N)] = a[IDX2C(i, j, N)];
		} 
	} 

	//Выделение памяти под матрицы на девайсе
	float* d_a; //Исходная матрица 
	float* d_b; //Транспонированная матрица 

	//Выделяем глобальную память для храния данных на девайсе
	cudaMalloc((void**)&d_a, size * sizeof(float));
	cudaMalloc((void**)&d_b, size * sizeof(float));

	//Копируем исходную матрицу с хоста на девайс
	cudaMemcpy(d_a, a_h, size * sizeof(float), cudaMemcpyHostToDevice);


	dim3 gridSize = dim3(width / 2, height / 2, 1);
	dim3 blockSize = dim3(2, 2, 1);
	if (size > 256) {
		blockSize = dim3(256, 256, 1);
		dim3 gridSize = dim3(width / 256, height / 256, 1);
	}

	//Запуск ядра 
	transKernel << <gridSize, blockSize >> > (d_a, d_b, width, height);


	//Копируем результат с девайса на хост
	cudaMemcpy(b_h, d_b, size * sizeof(float), cudaMemcpyDeviceToHost);


	float* C = (float *)malloc(sizeof(float) * size);

	for (int i = 0; i < M; i++) {
		for (int j = 0; j < N; j++) {
			C[IDX2C(i, j, M)] = b_h[IDX2C(i, j, M)];
			b[IDX2C(i, j, M)] = b_h[IDX2C(i, j, M)];
		}
	}

	//Удаление ресурсов с видеокарты
	cudaFree(d_a);
	cudaFree(d_b);

	free(a_h);
	free(b_h);
	free(C);
	return stat;
}

//умножение матриц
__global__ void matMulKernel(float *a, float *b, float *c, int M, int K, int N) {
	//Каждый поток вычисляет 1 элемент матрицы С
	//складывая произведения из строки А на столбец Б
	float Cvalue = 0.0;
	int row = blockIdx.y * blockDim.y + threadIdx.y;
	int col = blockIdx.x * blockDim.x + threadIdx.x;
	if (row > K || col > K) return;
	for (int e = 0; e < M; ++e)
		Cvalue += (a[IDX2C(row, e, M)]) * (b[IDX2C(e, col, K)]);
	c[IDX2C(row, col, M)] = Cvalue;
}
int matMul(char* s1, char* s2, char* s3, float * a, float * b, float * c, int M, int K, int N)
{
	char* s11; char* s22; char* s33;
	s11 = s1; s22 = s2; s33 = s3;
	while (*s11) { *s33 = *s11; s33++; s11++; }
	while (*s22) { *s33 = *s22; s33++; s22++; }
	*s33 = 0;

	cudaError_t cudaStatus = matMulWithCuda(a, b, c, M,K,N);
	cudaStatus = cudaDeviceReset();
	return 0;
}
cudaError_t matMulWithCuda(float * a, float * b, float * c, int M,int K, int N)
{

	cudaError_t cudaStatus;
	//буфер под матрицы А, В, С в ОЗУ
	float *h_a = (float*)malloc(M*K * sizeof(float));
	float *h_b = (float*)malloc(K*N * sizeof(float));
	float *h_c = (float*)malloc(M*N * sizeof(float));
	//буфер под матрицы А, В, С в памяти ГПУ
	float *d_a;
	float *d_b;
	float *d_c;

	for (int i = 0; i < M; i++) 
		for (int j = 0; j < K; j++) 
			h_a[IDX2C(i, j, M)] = a[IDX2C(i, j, M)];

	for (int i = 0; i < K; i++)
		for (int j = 0; j < N; j++)
			h_b[IDX2C(i, j, K)] = b[IDX2C(i, j, K)];

	for (int i = 0; i < M; i++)
		for (int j = 0; j < N; j++)
			h_c[IDX2C(i, j, M)] = c[IDX2C(i, j, M)];


	//выделение памяти в ГПУ
	cudaMalloc((void**)&d_a, M*K * sizeof(float));
	cudaMalloc((void**)&d_b, K*N * sizeof(float));
	cudaMalloc((void**)&d_c, M*N * sizeof(float));
	//копирование значений
	cudaMemcpy(d_a, h_a, M*K * sizeof(float), cudaMemcpyHostToDevice);
	cudaMemcpy(d_b, h_b, K*N * sizeof(float), cudaMemcpyHostToDevice);
	cudaMemcpy(d_c, h_c, M*N * sizeof(float), cudaMemcpyHostToDevice);

	//определение размера блока и матрицы (block dimension, grid dimension)
	dim3 dimBlock(4, 4);
	dim3 dimGrid((K + dimBlock.x - 1) / dimBlock.x, (K + dimBlock.y - 1) / dimBlock.y);

	//вызов кернел-функции (параллельное вычисление элементов матрицы С)
	matMulKernel << <dimGrid, dimBlock >> > (d_a, d_b, d_c, M, K, N);
	cudaThreadSynchronize();

	//копирование значений из ГПУ в буфер в ОЗУ
	cudaMemcpy(h_c, d_c, M*N * sizeof(float), cudaMemcpyDeviceToHost);
	//копирование из буфера в исходную матрицу
	for (int i = 0; i < M; i++)
		for (int j = 0; j < N; j++)
			c[IDX2C(i, j, M)] = h_c[IDX2C(i, j, M)];
	//Free device memory
	cudaFree(d_a);
	cudaFree(d_b);

	return cudaStatus;
}