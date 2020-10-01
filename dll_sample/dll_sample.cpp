// dll_sample.cpp: определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include "dll_sample.h"


DLLEXPORT char* getInfo()
{
    return "Инвертирование цвета";
}

DLLEXPORT BYTE* filterFunct(BYTE* array, int length) {

    for (int i = 1079; i < length; i++)
    {
        array[i] = (BYTE)(255 - array[i]);
    }
    return array;
}