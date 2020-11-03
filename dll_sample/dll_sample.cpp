// dll_sample.cpp: определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include "dll_sample.h"

DLLEXPORT char* pluginFunctions()
{
    return "filterFunct";
}

DLLEXPORT char* functionsDescriptions(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "Инвертирование цвета";
    //if (strcmp(str, "avgTime") == 0) return "Возвращает среднее время обработки";
    return "Not found";
}

DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "INVERTCOLORMAIN,LABEL,TESTLABEL1,300,20,Тестовый текст;INVERTCOLORMAIN,TEXTBOX,TESTTEXTBOX1,300,30,Тестовый текст textbox;INVERTCOLORMAIN,TRACKBAR,TRACKBAR1,300,30,Тестовый trackbar,0,255";
    //if (strcmp(str, "avgTime") == 0) return "";
    return "Not found";
}

DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "TRACKBAR1";         //Место под картинкой
    //if (strcmp(str, "avgTime") == 0) return "DESCRIPTION";      //Место в описании плагина
    return "Not found";
}



DLLEXPORT char* getName(char* str)
{
    return "Инвертирование цвета";
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