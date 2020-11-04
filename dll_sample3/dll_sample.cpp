#include "stdafx.h"
#include "dll_sample.h"
#include "time.h"
#include <sstream> 
#include <string.h> 
#include <vector>

using namespace std;

//Список функций
DLLEXPORT char* pluginFunctions()
{
    return "filterFunct lastTime";
}

//Описание функции в описании плагина
DLLEXPORT char* functionsDescriptions(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "Функция инверсии цвета";
    if (strcmp(str, "lastTime") == 0) return "Возвращает последнее время обработки";
}

//"PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,особые аргументы;PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,особые аргументы"
//Базовые панели: 
//УНИКАЛЬНЫЙ_ИДЕНТИФИКАТОРMAIN - панель под кнопками
//INFOPANEL - панель в окне информации о плагинах
DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
    if (strcmp(str, "lastTime") == 0) return "INVERSECOLORMAIN,LABEL,INVERSECOLORLABEL1,300,20,Время обработки кадра:";
}

//Компонент или список компонентов через пробел, из которых берутся аргументы, "" если не требуется
DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
    if (strcmp(str, "lastTime") == 0) return "";
}

//Встраивается:
//TRIGGEREND - после обработки кадра
//TRIGGERSTART - до обработки кадра
//MAIN - фильтр-функция
//Название компонента - по клику
//NONE - не встраивается
DLLEXPORT char* functionsType(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "MAIN";
    if (strcmp(str, "lastTime") == 0) return "TRIGGEREND";
}

//Компонент, в который возвращается результат, "" если не требуется
DLLEXPORT char* functionsTarget(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
    if (strcmp(str, "lastTime") == 0) return "INVERSECOLORLABEL1";
}

//Информация о плагине
//Имя
DLLEXPORT char* getName(char* str)
{
    return "Инверсия цвета";
}
//Уникальный идентефикатор
DLLEXPORT char* getIdName(char* str) 
{
    return "INVERSECOLOR";
}
//Автор
DLLEXPORT char* getAuthor(char* str)
{
    return "Статников А.С. БПИ17-01";
}
//Версия
DLLEXPORT char* getVersion(char* str)
{
    return "v1.0";
}

//Непосредственная реализация функций

char* buffer = new char[100];
double lastTimeValue = 0;

DLLEXPORT char* filterFunct(char* array, int length, char* args) {
    double start = clock();
    for (int i = 1079; i < length; i++)
    {
        array[i] = (unsigned char)(255 - array[i]);
    }
    double end = clock();
    lastTimeValue = (double)(end - start) / CLOCKS_PER_SEC;
    return array;
}

DLLEXPORT char* lastTime(char* args) {
    sprintf_s(buffer, 50, "Время обработки кадра: %f секунд", lastTimeValue);
    return buffer;
}