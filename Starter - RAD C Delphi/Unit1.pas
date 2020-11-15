unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls;

type
  FdoubleOps = function(a, b: double): double; cdecl;
  TDoubleArray = array [Word] of double;
  PDoubleArray = ^TDoubleArray;
  T2dArray = array of array of double;
  P2dArray = ^T2dArray;

  VectorType = function(array1, array2: PDoubleArray; size: integer)
    : double; cdecl;

  MatrixType = function(array1: T2dArray; size1, size2: integer): double; cdecl;

type
  TForm1 = class(TForm)
    Memo1: TMemo;
    Button_VS_C: TButton;
    Button_RAD_C: TButton;
    Button_RAD_Delphi: TButton;
    procedure Button_VS_CClick(Sender: TObject);
    procedure Button_RAD_CClick(Sender: TObject);
    procedure Button_RAD_DelphiClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  hLib: THandle = 0;

implementation

{$R *.dfm}


function callDll(dllname, functName1, functName2, functName3: String): String;
var
funct1, funct2: VectorType;
funct3: MatrixType;
N, N2, countIterations, i, j, count: integer;
arr1, arr2: array of double;
array1, array2: PDoubleArray;
array3: T2dArray;
minTime, maxTime, summTime, avgTime, time: double;
FbeginCount, FendCount, Ffrequence: TlargeInteger;
res: String;
begin

  //размерность массива
	N := 10000;
	//размерность двухмерного массива
	N2 := 100;
	// ол-во замеров
	countIterations := 10;

  if hLib = 0 then
    hLib := SafeLoadLibrary(dllname);
  SetLength(arr1, N);
  SetLength(arr2, N);
  SetLength(array3, N2, N2);
  funct1 := GetProcAddress(hLib, PChar(functName1));
	funct2 := GetProcAddress(hLib, PChar(functName2));
	funct3 := GetProcAddress(hLib, PChar(functName3));
  res := '';

	for i := 0 to N - 1 do
	begin
		arr1[i] := Random;
		arr2[i] := Random;
	end;

  array1 := @arr1[0];
  array2 := @arr2[0];

  minTime := 100000;
	maxTime := 0;
	summTime := 0;
	for count := 0 to count - 1 do
	begin
		QueryPerformanceFrequency(Ffrequence);
		QueryPerformanceCounter(FbeginCount);
		funct1(array1, array2, N);
		QueryPerformanceCounter(FendCount);
		time := ((FendCount - FbeginCount) / Ffrequence) * 1000;
		summTime :=  summTime + time;
		if minTime > time then minTime := time;
		if maxTime < time then maxTime := time;
	end;
	avgTime := summTime / countIterations;
	res := res + FloatToStr(avgTime) + '\t' + FloatToStr(minTime) + '\t' + FloatToStr(maxTime) + '\t';

  minTime := 100000;
	maxTime := 0;
	summTime := 0;
	for count := 0 to count - 1 do
	begin
		QueryPerformanceFrequency(Ffrequence);
		QueryPerformanceCounter(FbeginCount);
		funct2(array1, array2, N);
		QueryPerformanceCounter(FendCount);
		time := ((FendCount - FbeginCount) / Ffrequence) * 1000;
		summTime :=  summTime + time;
		if minTime > time then minTime := time;
		if maxTime < time then maxTime := time;
	end;
	avgTime := summTime / countIterations;
	res := res + FloatToStr(avgTime) + '\t' + FloatToStr(minTime) + '\t' + FloatToStr(maxTime) + '\t';

  for i := 0 to N2 - 1 do
    for j := 0 to N2 - 1 do
      begin
        array3[i][j] := Random(1000);
      end;

  minTime := 100000;
	maxTime := 0;
	summTime := 0;
	for count := 0 to count - 1 do
	begin
		QueryPerformanceFrequency(Ffrequence);
		QueryPerformanceCounter(FbeginCount);
		funct3(array3, N, N);
		QueryPerformanceCounter(FendCount);
		time := ((FendCount - FbeginCount) / Ffrequence) * 1000;
		summTime :=  summTime + time;
		if minTime > time then minTime := time;
		if maxTime < time then maxTime := time;
	end;
	avgTime := summTime / countIterations;
	res := res + FloatToStr(avgTime) + '\t' + FloatToStr(minTime) + '\t' + FloatToStr(maxTime) + '\t';
  result := res;
end;

procedure TForm1.Button_RAD_CClick(Sender: TObject);
begin
  Memo1.Lines.Add(callDll('DLL - RAD C Builder.dll','_getMinRangeOfVector','_getStandardDeviation','_getAvgValue') + '\n');
end;

procedure TForm1.Button_RAD_DelphiClick(Sender: TObject);
begin
  Memo1.Lines.Add(callDll('DLL - RAD Delphi.dll','getMinRangeOfVector','getStandardDeviation','getAvgValue') + '\n');
end;

procedure TForm1.Button_VS_CClick(Sender: TObject);
begin
  Memo1.Lines.Add(callDll('DLL - C.dll','getMinRangeOfVector','getStandardDeviation','getAvgValue') + '\n');
end;

end.
