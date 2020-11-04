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
    if (strcmp(str, "filterFunct") == 0) return "Функция сдвига цвета";
    if (strcmp(str, "lastTime") == 0) return "Возвращает среднее время обработки";
}

//"PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,особые аргументы;PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,особые аргументы"
//Базовые панели: 
//УНИКАЛЬНЫЙ_ИДЕНТИФИКАТОРMAIN - панель под кнопками
//INFOPANEL - панель в окне информации о плагинах
DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTCOLORMAIN,LABEL,SHIFTCOLORLABEL1,300,20,Величина сдвига;SHIFTCOLORMAIN,TRACKBAR,SHIFTCOLORTRACKBAR1,300,30,Величина сдвига,0,255";
    if (strcmp(str, "lastTime") == 0) return "SHIFTCOLORMAIN,LABEL,SHIFTCOLORLABEL2,300,20,Время обработки кадра:";
}

//Компонент или список компонентов через пробел, из которых берутся аргументы, "" если не требуется
DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTCOLORTRACKBAR1";
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
    if (strcmp(str, "lastTime") == 0) return "SHIFTCOLORLABEL2";
}

//Информация о плагине
//Имя
DLLEXPORT char* getName(char* str)
{
    return "Сдвиг цвета";
}
//Уникальный идентефикатор
DLLEXPORT char* getIdName(char* str) 
{
    return "SHIFTCOLOR";
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

std::vector<std::string> split(const std::string& s, char delimiter) {
    std::vector<std::string> tokens;
    std::string token;
    std::istringstream tokenStream(s);
    while (std::getline(tokenStream, token, delimiter))
    {
        tokens.push_back(token);
    }
    return tokens;
}

char* buffer = new char[100];
double lastTimeValue = 0;

DLLEXPORT char* filterFunct(char* array, int length, char* args) {
    std::vector<std::string> splitedArgs = split(args, ';');
    int* argsArray = new int[splitedArgs.size()];
    for (size_t i = 0; i < splitedArgs.size(); i++)
    {
        argsArray[i] = atoi(splitedArgs[i].c_str());
    }
    int a = argsArray[0];
    double start = clock();
    for (int i = 1079; i < length; i++)
    {
        array[i] = (unsigned char)(array[i] + a);
    }
    double end = clock();
    lastTimeValue = (double)(end - start) / CLOCKS_PER_SEC;
    return array;
}

DLLEXPORT char* lastTime(char* args) {
    sprintf_s(buffer, 50, "Время обработки кадра: %f секунд", lastTimeValue);
    return buffer;
}