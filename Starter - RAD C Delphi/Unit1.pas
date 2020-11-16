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



procedure TForm1.Button_RAD_CClick(Sender: TObject);
//begin
//  Memo1.Lines.Add(callDll('DLL - RAD C Builder.dll','_getMinRangeOfVector','_getStandardDeviation','_getAvgValue') + '\n');
//end;
var
  arr1, arr2, mas_time: array of double;
  mas_3: T2dArray;
  pmas_1, pmas_2: PDoubleArray;
  found_max: VectorType;
  found_min: VectorType;
  max_dual_mas: Matrixtype;
  size_1, size_2, size_zamer: integer;
  result, count_time, min_time, max_time: double;
  i, j: integer;
  FbeginCount, FendCount, Ffrequence: TlargeInteger;
  Ftime: extended;
  res1, res2, res3: String;

begin
  size_1 := 100000;
  size_2 := 600;
  size_zamer := 50;
  count_time:=0;
  hLib := 0;
  if hLib = 0 then
    hLib := SafeLoadLibrary('DLL - RAD C Builder.dll');

  Memo1.Lines.Clear;
  Memo1.Lines.Add('DLL - RAD C Builder');
  found_max := GetProcAddress(hLib, PChar('_getMinValue'));
  found_min := GetProcAddress(hLib, PChar('_getVolume'));
  max_dual_mas := GetProcAddress(hLib, PChar('_getDispersion'));

  Memo1.Lines.Add('Test Start');
  if (@found_max = nil) OR (@found_min = nil) OR (@max_dual_mas = nil) then
    Memo1.Lines.Add('function load error');

  SetLength(arr1, size_1);
  SetLength(arr2, size_1);
  SetLength(mas_time, size_zamer);

  for i := 0 to size_1 - 1 do
  begin
    arr1[i] := Random(1000);
    arr2[i] := Random(1000);
  end;

  SetLength(mas_3, size_2, size_2);

  for j := 0 to size_2 - 1 do
  begin
  for i := 0 to size_2 - 1 do
  begin
  mas_3[j][i] := Random(1000);
  end;
  end;

  pmas_1 := @arr1[0];
  pmas_2 := @arr2[0];

  for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);
    result := found_max(pmas_1, pmas_2, size_1);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res1:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);

  count_time:=0;

   for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);
    result := found_min(pmas_1, pmas_2, size_1);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res2:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);
  count_time:=0;

   for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);

    result := max_dual_mas(mas_3, size_2, size_2);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res3:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);

  Memo1.Lines.Add(res1 + #$9 + res2 + #$9  + res3);
end;


procedure TForm1.Button_RAD_DelphiClick(Sender: TObject);
//begin
//  Memo1.Lines.Add(callDll('DLL - RAD Delphi.dll','getMinRangeOfVector','getStandardDeviation','getAvgValue') + '\n');
//end;
var
  arr1, arr2, mas_time: array of double;
  mas_3: T2dArray;
  pmas_1, pmas_2: PDoubleArray;
  found_max: VectorType;
  found_min: VectorType;
  max_dual_mas: Matrixtype;
  size_1, size_2, size_zamer: integer;
  result, count_time, min_time, max_time: double;
  i, j: integer;
  FbeginCount, FendCount, Ffrequence: TlargeInteger;
  Ftime: extended;
  res1, res2, res3: String;

begin
  size_1 := 100000;
  size_2 := 600;
  size_zamer := 50;
  count_time:=0;
  hLib := 0;
  if hLib = 0 then
    hLib := SafeLoadLibrary('DLL - RAD Delphi.dll');

  Memo1.Lines.Clear;
  Memo1.Lines.Add('DLL - RAD Delphi.dll');
  found_max := GetProcAddress(hLib, PChar('getMinValue'));
  found_min := GetProcAddress(hLib, PChar('getVolume'));
  max_dual_mas := GetProcAddress(hLib, PChar('getDispersion'));

  Memo1.Lines.Add('Test Start');
  if (@found_max = nil) OR (@found_min = nil) OR (@max_dual_mas = nil) then
    Memo1.Lines.Add('function load error');

  SetLength(arr1, size_1);
  SetLength(arr2, size_1);
  SetLength(mas_time, size_zamer);

  for i := 0 to size_1 - 1 do
  begin
    arr1[i] := Random(1000);
    arr2[i] := Random(1000);
  end;

  SetLength(mas_3, size_2, size_2);

  for j := 0 to size_2 - 1 do
  begin
  for i := 0 to size_2 - 1 do
  begin
  mas_3[j][i] := Random(1000);
  end;
  end;

  pmas_1 := @arr1[0];
  pmas_2 := @arr2[0];

  for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);
    result := found_max(pmas_1, pmas_2, size_1);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res1:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);

  count_time:=0;

   for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);
    result := found_min(pmas_1, pmas_2, size_1);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res2:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);
  count_time:=0;

   for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);

    result := max_dual_mas(mas_3, size_2, size_2);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res3:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);

  Memo1.Lines.Add(res1 + #$9 + res2 + #$9  + res3);
end;



procedure TForm1.Button_VS_CClick(Sender: TObject);
//begin
//  Memo1.Lines.Add(callDll('DLL - C.dll','getMinRangeOfVector','getStandardDeviation','getAvgValue') + '\n');
//end;
var
  arr1, arr2, mas_time: array of double;
  mas_3: T2dArray;
  pmas_1, pmas_2: PDoubleArray;
  found_max: VectorType;
  found_min: VectorType;
  max_dual_mas: Matrixtype;
  size_1, size_2, size_zamer: integer;
  result, count_time, min_time, max_time: double;
  i, j: integer;
  FbeginCount, FendCount, Ffrequence: TlargeInteger;
  Ftime: extended;
  res1, res2, res3: String;

begin
  size_1 := 100000;
  size_2 := 600;
  size_zamer := 50;
  count_time:=0;
  hLib := 0;
  if hLib = 0 then
    hLib := SafeLoadLibrary('DLL - C.dll');

  Memo1.Lines.Clear;
  Memo1.Lines.Add('DLL - C.dll');
  found_max := GetProcAddress(hLib, PChar('getMinValue'));
  found_min := GetProcAddress(hLib, PChar('getVolume'));
  max_dual_mas := GetProcAddress(hLib, PChar('getDispersion'));

  Memo1.Lines.Add('Test Start');
  if (@found_max = nil) OR (@found_min = nil) OR (@max_dual_mas = nil) then
    Memo1.Lines.Add('function load error');

  SetLength(arr1, size_1);
  SetLength(arr2, size_1);
  SetLength(mas_time, size_zamer);

  for i := 0 to size_1 - 1 do
  begin
    arr1[i] := Random(1000);
    arr2[i] := Random(1000);
  end;

  SetLength(mas_3, size_2, size_2);

  for j := 0 to size_2 - 1 do
  begin
  for i := 0 to size_2 - 1 do
  begin
  mas_3[j][i] := Random(1000);
  end;
  end;

  pmas_1 := @arr1[0];
  pmas_2 := @arr2[0];

  for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);
    result := found_max(pmas_1, pmas_2, size_1);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res1:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);

  count_time:=0;

   for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);
    result := found_min(pmas_1, pmas_2, size_1);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res2:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);
  count_time:=0;

   for i := 0 to size_zamer - 1 do
  begin
    QueryPerformanceFrequency(Ffrequence);
    QueryPerformanceCounter(FbeginCount);

    result := max_dual_mas(mas_3, size_2, size_2);
    QueryPerformanceCounter(FendCount);
    Ftime := ((FendCount - FbeginCount) / Ffrequence) * 1000;

    count_time := count_time + Ftime;
    mas_time[i] := Ftime;
  end;

  max_time := mas_time[0];
  min_time := mas_time[0];

  for i := 0 to size_zamer - 1 do
  begin
    if (mas_time[i] > max_time) then
      max_time := mas_time[i];

    if (mas_time[i] < min_time) then
      min_time := mas_time[i];

  end;

  count_time:=count_time/size_zamer;
  res3:=floattostr(min_time) + #$9 + floattostr(max_time) + #$9 + floattostr(count_time);

  Memo1.Lines.Add(res1 + #$9 + res2 + #$9  + res3);
end;
end.
