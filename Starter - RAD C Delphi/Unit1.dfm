object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'Form1'
  ClientHeight = 485
  ClientWidth = 716
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
    Top = 88
    Width = 689
    Height = 329
    Lines.Strings = (
      'Memo1')
    TabOrder = 0
  end
  object Button_VS_C: TButton
    Left = 32
    Top = 16
    Width = 115
    Height = 49
    Caption = 'VS_C++_DLL'
    TabOrder = 1
    OnClick = Button_VS_CClick
  end
  object Button_RAD_C: TButton
    Left = 168
    Top = 16
    Width = 107
    Height = 49
    Caption = 'RAD_C++_DLL'
    TabOrder = 2
    OnClick = Button_RAD_CClick
  end
  object Button_RAD_Delphi: TButton
    Left = 296
    Top = 16
    Width = 115
    Height = 49
    Caption = 'RAD_Delphi_DLL'
    TabOrder = 3
    OnClick = Button_RAD_DelphiClick
  end
end
