//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "Unit1.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1 *Form1;
//---------------------------------------------------------------------------


void callDll(CONST WCHAR* dllname, const char* functname1, const char* functname2, const char* functname3) {
	HINSTANCE hinstLib = LoadLibrary(dllname);
	if (hinstLib == NULL)
	{
		return;
	}
	VectorType funct1 = (VectorType)GetProcAddress(hinstLib, functname1);
	VectorType funct2 = (VectorType)GetProcAddress(hinstLib, functname2);
	MatrixType funct3 = (MatrixType)GetProcAddress(hinstLib, functname3);

	LARGE_INTEGER FbeginCount, FendCount, Ffrequence;

	//����������� �������
	int N = 10000;
	//����������� ����������� �������
	int N2 = 100;
	//���-�� �������
	int countIterations = 10;

	double minTime, maxTime, summTime, avgTime;
	double* array1 = new double[N];
	double* array2 = new double[N];
	for (int i = 0; i < N; i++)
	{
		array1[i] = rand() % 1000 + 1;
		array2[i] = rand() % 1000 + 1;
	}

	minTime = DBL_MAX;
	maxTime = -DBL_MAX;
	summTime = 0;
	for (int count = 0; count < N; count++)
	{
		QueryPerformanceFrequency(&Ffrequence);
		QueryPerformanceCounter(&FbeginCount);
		funct1(array1, array2, N);
		QueryPerformanceCounter(&FendCount);
		double time = (double)((FendCount.QuadPart - FbeginCount.QuadPart)) * 1000 / Ffrequence.QuadPart;
		summTime += time;
		if (minTime > time) minTime = time;
		if (maxTime < time) maxTime = time;
	}
	avgTime = summTime / countIterations;
	Form1->Memo1->Lines->Add(FloatToStr(avgTime) + "\t" + FloatToStr(minTime) + "\t" + FloatToStr(maxTime) + "\t");

	minTime = DBL_MAX;
	maxTime = -DBL_MAX;
	summTime = 0;
	for (int count = 0; count < N; count++)
	{
		QueryPerformanceFrequency(&Ffrequence);
		QueryPerformanceCounter(&FbeginCount);
		funct2(array1, array2, N);
		QueryPerformanceCounter(&FendCount);
		double time = (double)((FendCount.QuadPart - FbeginCount.QuadPart)) * 1000 / Ffrequence.QuadPart;
		summTime += time;
		if (minTime > time) minTime = time;
		if (maxTime < time) maxTime = time;
	}
	avgTime = summTime / countIterations;
	Form1->Memo1->Lines->Add(FloatToStr(avgTime) + "\t" + FloatToStr(minTime) + "\t" + FloatToStr(maxTime) + "\t");

	double** array3 = new double*[N];
	for (int i = 0; i < N2; i++)
	{
		array3[i] = new double[N2];
		for (int j = 0; j < N2; j++)
		{
			array3[i][j] = rand() % 1000 + 1;
		}
	}
	minTime = DBL_MAX;
	maxTime = -DBL_MAX;
	summTime = 0;
	for (int count = 0; count < N; count++)
	{
		QueryPerformanceFrequency(&Ffrequence);
		QueryPerformanceCounter(&FbeginCount);
		funct3(array3, N2, N2);
		QueryPerformanceCounter(&FendCount);
		double time = (double)((FendCount.QuadPart - FbeginCount.QuadPart)) * 1000 / Ffrequence.QuadPart;
		summTime += time;
		if (minTime > time) minTime = time;
		if (maxTime < time) maxTime = time;
	}
	avgTime = summTime / countIterations;
	Form1->Memo1->Lines->Add(FloatToStr(avgTime) + "\t" + FloatToStr(minTime) + "\t" + FloatToStr(maxTime) + "\t");
}

__fastcall TForm1::TForm1(TComponent* Owner)
	: TForm(Owner)
{

}
//---------------------------------------------------------------------------

void __fastcall TForm1::Button1Click(TObject *Sender)
{
Memo1->Lines->Text='fghfghfhg';
}
//---------------------------------------------------------------------------



void __fastcall TForm1::Button_VS_CClick(TObject *Sender)
{
	callDll(TEXT("DLL - C.dll"), "getMinRangeOfVector", "getStandardDeviation", "getAvgValue");
}
//---------------------------------------------------------------------------

void __fastcall TForm1::Button_RAD_CClick(TObject *Sender)
{
	callDll(TEXT("DLL - RAD C Builder.dll"), "_getMinRangeOfVector", "_getStandardDeviation", "_getAvgValue");
}
//---------------------------------------------------------------------------

void __fastcall TForm1::Button_RAD_DelphiClick(TObject *Sender)
{
	callDll(TEXT("DLL - RAD Delphi.dll"), "getMinRangeOfVector", "getStandardDeviation", "getAvgValue");
}
//---------------------------------------------------------------------------

