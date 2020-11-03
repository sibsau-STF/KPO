// dll_sample.cpp: ���������� ���������������� ������� ��� ���������� DLL.
//

#include "stdafx.h"
#include "dll_sample.h"

DLLEXPORT char* pluginFunctions()
{
    return "filterFunct";
}

DLLEXPORT char* functionsDescriptions(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "�������������� �����";
    //if (strcmp(str, "avgTime") == 0) return "���������� ������� ����� ���������";
    return "Not found";
}

DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "INVERTCOLORMAIN,LABEL,TESTLABEL1,300,20,�������� �����;INVERTCOLORMAIN,TEXTBOX,TESTTEXTBOX1,300,30,�������� ����� textbox;INVERTCOLORMAIN,TRACKBAR,TRACKBAR1,300,30,�������� trackbar,0,255";
    //if (strcmp(str, "avgTime") == 0) return "";
    return "Not found";
}

DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "TRACKBAR1";         //����� ��� ���������
    //if (strcmp(str, "avgTime") == 0) return "DESCRIPTION";      //����� � �������� �������
    return "Not found";
}



DLLEXPORT char* getName(char* str)
{
    return "�������������� �����";
}
DLLEXPORT char* getIdName(char* str) 
{
    return "INVERTCOLOR";
}


DLLEXPORT char* filterFunct(char* array, int length) {

    for (int i = 1079; i < length; i++)
    {
        array[i] = (unsigned char)(255 - array[i]);
    }
    return array;
}