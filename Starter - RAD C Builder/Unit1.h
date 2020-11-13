//---------------------------------------------------------------------------

#ifndef Unit1H
#define Unit1H


typedef double(__cdecl  *Found_max_in_mass)(double*, double*, int, int);
typedef double(__cdecl  *Found_Min)(double*, double*, int, int);
typedef double(__cdecl  *Found_Max_in_dual_mass)(double**, int, int);

//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
//#include <System.ShareMem.hpp>
#include <Vcl.Forms.hpp>
//---------------------------------------------------------------------------
class TForm1 : public TForm
{
__published:	// IDE-managed Components
	TMemo *Memo1;
	TButton *Button_VS_C;
	TButton *Button_RAD_C;
	TButton *Button_RAD_Delphi;
	TButton *Button1;
	void __fastcall Button_VS_CClick(TObject *Sender);
	void __fastcall Button1Click(TObject *Sender);
	void __fastcall Button_RAD_CClick(TObject *Sender);
	void __fastcall Button_RAD_DelphiClick(TObject *Sender);
private:	// User declarations
public:		// User declarations
	__fastcall TForm1(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TForm1 *Form1;
//---------------------------------------------------------------------------
#endif
