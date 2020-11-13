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

  Min_Max = function(mas_1, mas_2: PDoubleArray; size_1, size_2: integer)
    : double; cdecl;

  Max_in_dual_mass = function(mas: T2dArray; H, W: integer): double; cdecl;

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

var
  mas_1, mas_2, mas_time: array of double;
  mas_3: T2dArray;
  pmas_1, pmas_2: PDoubleArray;
  found_max: Min_Max;
  found_min: Min_Max;
  max_dual_mas: Max_in_dual_mass;

  size_1, size_2, size_zamer: integer;
  result, count_time, min_time, max_time: double;
  i, j: integer;

  FbeginCount, FendCount, Ffrequence: TlargeInteger;
  Ftime: extended;

begin

size_1 := 100000;
  size_2 := 600;
  size_zamer := 50;
     count_time:=0;


  if hLib = 0 then
    hLib := SafeLoadLibrary('Project_RAD_DLL.dll');


  if hLib <> 0 then
  begin
    Memo1.Lines.Clear;
    Memo1.Lines.Add('Project_RAD_DLL.dll Успешно загружена');
    found_max := GetProcAddress(hLib, PChar('_Found_Max'));
    found_min := GetProcAddress(hLib, PChar('_Found_Min'));
    max_dual_mas := GetProcAddress(hLib, PChar('_Found_Max_in_dual_mass'));

    Memo1.Lines.Add('Test Start');
    if (@found_max = nil) OR (@found_min = nil) OR (@max_dual_mas = nil) then
      Memo1.Lines.Add('function load error');

    SetLength(mas_1, size_1);
    SetLength(mas_2, size_1);
    SetLength(mas_time, size_zamer);

    for i := 0 to size_1 - 1 do
    begin
      mas_1[i] := Random(1000);
      mas_2[i] := Random(1000);
    end;

    /// Пример двумерный массив ////
        SetLength(mas_3, size_2, size_2);

        for j := 0 to size_2 - 1 do
        begin
        for i := 0 to size_2 - 1 do
        begin
        mas_3[j][i] := Random(1000);
        end;
        end;

    pmas_1 := @mas_1[0];
    pmas_2 := @mas_2[0];

    for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := found_max(pmas_1, pmas_2, size_1, size_1);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function found_max: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));


    //2-АЯ ФУНКЦИЯ!!!

    count_time:=0;

     for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := found_min(pmas_1, pmas_2, size_1, size_1);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function found_min: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));


          //3-АЯ ФУНКЦИЯ!!!

    count_time:=0;

     for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := max_dual_mas(mas_3, size_2, size_2);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function max_dual_mas: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));



  end
  else
  begin
        Memo1.Lines.Add('Project_RAD_DLL.dll Не найдена!!!');
  end;




end;

procedure TForm1.Button_RAD_DelphiClick(Sender: TObject);

var
  mas_1, mas_2, mas_time: array of double;
  mas_3: T2dArray;
  pmas_1, pmas_2: PDoubleArray;
  found_max: Min_Max;
  found_min: Min_Max;
  max_dual_mas: Max_in_dual_mass;

  size_1, size_2, size_zamer: integer;
  result, count_time, min_time, max_time: double;
  i, j: integer;

  FbeginCount, FendCount, Ffrequence: TlargeInteger;
  Ftime: extended;

begin
    size_1 := 100000;
  size_2 := 600;
  size_zamer := 50;
     count_time:=0;


  if hLib = 0 then
    hLib := SafeLoadLibrary('Project_Delphi_DLL.dll');


  if hLib <> 0 then
  begin
    Memo1.Lines.Clear;
    Memo1.Lines.Add('Project_Delphi_DLL.dll Успешно загружена');
    found_max := GetProcAddress(hLib, PChar('Found_Max'));
    found_min := GetProcAddress(hLib, PChar('Found_Min'));
    max_dual_mas := GetProcAddress(hLib, PChar('Found_Max_in_dual_mass'));

    Memo1.Lines.Add('Test Start');
    if (@found_max = nil) OR (@found_min = nil) OR (@max_dual_mas = nil) then
      Memo1.Lines.Add('function load error');

    SetLength(mas_1, size_1);
    SetLength(mas_2, size_1);
    SetLength(mas_time, size_zamer);

    for i := 0 to size_1 - 1 do
    begin
      mas_1[i] := Random(1000);
      mas_2[i] := Random(1000);
    end;

    /// Пример двумерный массив ////
        SetLength(mas_3, size_2, size_2);

        for j := 0 to size_2 - 1 do
        begin
        for i := 0 to size_2 - 1 do
        begin
        mas_3[j][i] := Random(1000);
        end;
        end;

    pmas_1 := @mas_1[0];
    pmas_2 := @mas_2[0];

    for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := found_max(pmas_1, pmas_2, size_1, size_1);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function found_max: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));


    //2-АЯ ФУНКЦИЯ!!!

    count_time:=0;

     for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := found_min(pmas_1, pmas_2, size_1, size_1);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function found_min: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));


          //3-АЯ ФУНКЦИЯ!!!

    count_time:=0;

     for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := max_dual_mas(mas_3, size_2, size_2);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function max_dual_mas: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));



  end
  else
  begin
        Memo1.Lines.Add('Project_Delphi_DLL.dll Не найдена!!!');
  end;



end;

procedure TForm1.Button_VS_CClick(Sender: TObject);

var
  mas_1, mas_2, mas_time: array of double;
  mas_3: T2dArray;
  pmas_1, pmas_2: PDoubleArray;
  found_max: Min_Max;
  found_min: Min_Max;
  max_dual_mas: Max_in_dual_mass;

  size_1, size_2, size_zamer: integer;
  result, count_time, min_time, max_time: double;
  i, j: integer;

  FbeginCount, FendCount, Ffrequence: TlargeInteger;
  Ftime: extended;

begin
  size_1 := 100000;
  size_2 := 600;
  size_zamer := 50;
     count_time:=0;


  if hLib = 0 then
    hLib := SafeLoadLibrary('dll_sampel_C.dll');


  if hLib <> 0 then
  begin
    Memo1.Lines.Clear;
    Memo1.Lines.Add('dll_sampel_C.dll Успешно загружена');
    found_max := GetProcAddress(hLib, PChar('Found_Max'));
    found_min := GetProcAddress(hLib, PChar('Found_Min'));
    max_dual_mas := GetProcAddress(hLib, PChar('Found_Max_in_dual_mass'));

    Memo1.Lines.Add('Test Start');
    if (@found_max = nil) OR (@found_min = nil) OR (@max_dual_mas = nil) then
      Memo1.Lines.Add('function load error');

    SetLength(mas_1, size_1);
    SetLength(mas_2, size_1);
    SetLength(mas_time, size_zamer);

    for i := 0 to size_1 - 1 do
    begin
      mas_1[i] := Random(1000);
      mas_2[i] := Random(1000);
    end;

    /// Пример двумерный массив ////
        SetLength(mas_3, size_2, size_2);

        for j := 0 to size_2 - 1 do
        begin
        for i := 0 to size_2 - 1 do
        begin
        mas_3[j][i] := Random(1000);
        end;
        end;

    pmas_1 := @mas_1[0];
    pmas_2 := @mas_2[0];

    for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := found_max(pmas_1, pmas_2, size_1, size_1);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function found_max: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));


    //2-АЯ ФУНКЦИЯ!!!

    count_time:=0;

     for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := found_min(pmas_1, pmas_2, size_1, size_1);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function found_min: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));


          //3-АЯ ФУНКЦИЯ!!!

    count_time:=0;

     for i := 0 to size_zamer - 1 do
    begin
      // Замер времени начала (до выполнения действий)
      QueryPerformanceFrequency(Ffrequence);
      QueryPerformanceCounter(FbeginCount);

      result := max_dual_mas(mas_3, size_2, size_2);

      // Замер времени окончания (после выполнения действий)
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

    Memo1.Lines.Add('TIME function max_dual_mas: ');
    Memo1.Lines.Add('Min_time= ' + floattostr(min_time) + '   Max_time= ' +
      floattostr(max_time) + '   Avg_time= ' + floattostr(count_time));



  end
  else
  begin
        Memo1.Lines.Add('dll_sampel_C.dll Не найдена!!!');
  end;




end;

end.
