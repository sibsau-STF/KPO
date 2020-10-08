// dll_sample.cpp: ���������� ���������������� ������� ��� ���������� DLL.
//

#include "stdafx.h"
#include "dll_sample.h"

DLLEXPORT char* PluginFunctions()
{
    return "filterFunct avgTime";
}

DLLEXPORT char* FunctionsDescriptions(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "�������������� �����";
    if (strcmp(str, "message") == 0) return "���������� ������� ����� ���������";
    return "Not found";
}

DLLEXPORT char* FunctionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
    if (strcmp(str, "message") == 0) return "";
    return "Not found";
}

DLLEXPORT char* FunctionsType(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "Main";         //����� ��� ���������
    if (strcmp(str, "message") == 0) return "Description";      //����� � �������� �������
    return "Not found";
}







//DLLEXPORT char* getInfo()
//{
//    return "�������������� �����";
//}

DLLEXPORT BYTE* filterFunct(BYTE* array, int length) {

    for (int i = 1079; i < length; i++)
    {
        array[i] = (BYTE)(255 - array[i]);
    }
    return array;
}