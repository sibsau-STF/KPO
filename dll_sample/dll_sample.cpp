// dll_sample.cpp: ���������� ���������������� ������� ��� ���������� DLL.
//

#include "stdafx.h"
#include "dll_sample.h"


DLLEXPORT char* getInfo()
{
    return "�������������� �����";
}

DLLEXPORT BYTE* filterFunct(BYTE* array, int length) {

    for (int i = 1079; i < length; i++)
    {
        array[i] = (BYTE)(255 - array[i]);
    }
    return array;
}