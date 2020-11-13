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

function Found_Max(mas_1, mas_2: PDoubleArray; size_1, size_2: integer)
  : Double; cdecl;
var
  i: integer;
  max_value: Double;
begin
  max_value := mas_1[0];
  for i := 0 to size_1 - 1 do
  begin
    if (max_value < mas_1[i]) then
    begin
      max_value := mas_1[i];
    end;
  end;

  for i := 0 to size_2 - 1 do
  begin
    if (max_value < mas_2[i]) then
    begin
      max_value := mas_2[i];
    end;
  end;

  result := max_value;
end;


  function Found_Min(mas_1, mas_2: PDoubleArray; size_1, size_2: integer)
  : Double; cdecl;
var
  i: integer;
  min_value: Double;
begin
  min_value := mas_1[0];
  for i := 0 to size_1 - 1 do
  begin
    if (min_value > mas_1[i]) then
    begin
      min_value := mas_1[i];
    end;
  end;

  for i := 0 to size_2 - 1 do
  begin
    if (min_value > mas_2[i]) then
    begin
      min_value := mas_2[i];
    end;
  end;

  result := min_value;
end;


function Found_Max_in_dual_mass(mas_1: P2dArray; size_1, size_2: integer)
  : Double; cdecl;
var
  i, j: integer;
  max_value: Double;
begin
  max_value := mas_1[0][0];

  for i := 0 to size_1 - 1 do
  begin
    for j := 0 to size_2-1 do
    begin
      if (max_value < mas_1[i][j]) then
      begin
        max_value := mas_1[i][j];
      end;
    end;

  end;

  result := max_value;
end;

{ Эта функция экспортируется }
 exports
   Found_Max,Found_Min,Found_Max_in_dual_mass;

begin

end.
