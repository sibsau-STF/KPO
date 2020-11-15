library Project_Delphi_DLL;

{ Important note about DLL memory management: ShareMem must be the
  first unit in your library's USES clause AND your project's (select
  Project-View Source) USES clause if your DLL exports any procedures or
  functions that pass strings as parameters or function results. This
  applies to all strings passed to and from your DLL--even those that
  are nested in records and classes. ShareMem is the interface unit to
  the BORLNDMM.DLL shared memory manager, which must be deployed along
  with your DLL. To avoid using BORLNDMM.DLL, pass string information
  using PChar or ShortString parameters. }

uses
  System.SysUtils,
  System.Classes;

{$R *.res}

type
  TDoubleArray = array [Word] of Double;
  PDoubleArray = ^TDoubleArray;

type
  T2dArray = array of array of Double;
  T2dArrayInt = array of array of integer;
  P2dArray = array of PDoubleArray;
  pdouble = ^Double;

function getMinRangeOfVector(array1, array2: PDoubleArray; size: integer)
  : Double; cdecl;
var
  i: integer;
  minValue1, minValue2, maxValue1, maxValue2, range1, range2, range: Double;
begin
  minValue1 := array1[0];
	maxValue1 := array1[0];
  for i := 1 to size - 1 do
  begin
    if (minValue1 > array1[i]) then
    begin
			minValue1 := array1[i];
    end;
		if (maxValue1 < array1[i]) then
    begin
			maxValue1 := array1[i];
    end;
  end;

  minValue2 := array2[0];
	maxValue2 := array2[0];
  for i := 1 to size - 1 do
  begin
    if (minValue2 > array2[i]) then
    begin
			minValue2 := array2[i];
    end;
		if (maxValue2 < array2[i]) then
    begin
			maxValue2 := array2[i];
    end;
  end;

  range1 := maxValue1 - minValue1;
	range2 := maxValue2 - minValue2;
  if (range1 < range2) then
  begin
    range := range1;
  end
  else
  begin
    range := range2;
  end;
  result := range;
end;


function getStandardDeviation(array1, array2: PDoubleArray; size: integer)
  : Double; cdecl;
var
  i: integer;
  summ1: Double;
begin
  summ1 := 0;
  for i := 1 to size - 1 do
  begin
    summ1 := summ1 + array1[i] * array1[i] - array2[i] * array2[i];
  end;
  result := sqrt(summ1);
end;


function getAvgValue(array1: T2dArray; size1, size2: integer)
  : Double; cdecl;
var
  i, j: integer;
  summ: Double;
begin
  summ := 0;
  for i := 1 to size1 - 1 do
  begin
    for j := 1 to size2 - 1 do
    begin
      summ := summ + array1[i][j];
    end;
  end;
  result := summ / (size1 * size2);
end;


 function getMinValue(array1, array2: PDoubleArray; size: integer)
  : Double; cdecl;
var
  i: integer;
  minValue: Double;
begin
  minValue := array1[0];
  for i := 1 to size - 1 do
  begin
    if (minValue > array1[i]) then
    begin
			minValue := array1[i];
    end;
  end;

  for i := 1 to size - 1 do
  begin
    if (minValue > array2[i]) then
    begin
			minValue := array2[i];
    end;
  end;

  result := minValue;
end;


function getVolume(array1, array2: PDoubleArray; size: integer)
  : Double; cdecl;
var
  i: integer;
  volume: Double;
begin
  volume := 1;
  for i := 1 to size - 1 do
  begin
    volume := volume * abs(array1[i] - array2[i]);
  end;
  result := volume;
end;


function getDispersion(array1: T2dArray; size1, size2: integer)
  : Double; cdecl;
var
  i, j: integer;
  summ: Double;
begin
  summ := 0;
  for i := 1 to size1 - 1 do
  begin
    for j := 1 to size2 - 1 do
    begin
      summ := summ + array1[i][j] * array1[i][j];
    end;
  end;
  result := summ / (size1 * size2);
end;


{ Эта функция экспортируется }
 exports
   getMinRangeOfVector,getStandardDeviation,getAvgValue, getMinValue, getVolume, getDispersion;

begin

end.
