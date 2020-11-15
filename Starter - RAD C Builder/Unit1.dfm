object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'Form1'
  ClientHeight = 349
  ClientWidth = 476
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Memo1: TMemo
    Left = 8
    Top = 80
    Width = 449
    Height = 209
    Lines.Strings = (
      'Memo1')
    TabOrder = 0
  end
  object Button_VS_C: TButton
    Left = 8
    Top = 8
    Width = 99
    Height = 50
    Caption = 'DLL C'
    TabOrder = 1
    OnClick = Button_VS_CClick
  end
  object Button_RAD_C: TButton
    Left = 184
    Top = 8
    Width = 91
    Height = 50
    Caption = 'RAD C Builder'
    TabOrder = 2
    OnClick = Button_RAD_CClick
  end
  object Button_RAD_Delphi: TButton
    Left = 344
    Top = 8
    Width = 113
    Height = 50
    Caption = 'RAD Delphi'
    TabOrder = 3
    OnClick = Button_RAD_DelphiClick
  end
  object Button1: TButton
    Left = 104
    Top = 312
    Width = 75
    Height = 25
    Caption = 'Test_Memo'
    TabOrder = 4
    OnClick = Button1Click
  end
end
