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
    if (strcmp(str, "filterFunct") == 0) return "Функция сдвига каждого цвета";
    if (strcmp(str, "lastTime") == 0) return "Возвращает последнее время обработки";
}

//"PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,особые аргументы;PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,особые аргументы"
//Базовые панели: 
//УНИКАЛЬНЫЙ_ИДЕНТИФИКАТОРMAIN - панель под кнопками
//INFOPANEL - панель в окне информации о плагинах
DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL1,300,20,Величина сдвига красного цвета;SHIFTALLCOLORMAIN,TRACKBAR,SHIFTALLCOLORTRACKBAR1,300,30,Величина сдвига RED,0,255;SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL2,300,20,Величина сдвига зелёного цвета;SHIFTALLCOLORMAIN,TRACKBAR,SHIFTALLCOLORTRACKBAR2,300,30,Величина сдвига GREEN,0,255;SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL3,300,20,Величина сдвига синего цвета;SHIFTALLCOLORMAIN,TRACKBAR,SHIFTALLCOLORTRACKBAR3,300,30,Величина сдвига BLUE,0,255";
    if (strcmp(str, "lastTime") == 0) return "SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL4,300,20,Время обработки кадра:";
}

//Компонент или список компонентов через пробел, из которых берутся аргументы, "" если не требуется
DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTALLCOLORTRACKBAR1 SHIFTALLCOLORTRACKBAR2 SHIFTALLCOLORTRACKBAR3";
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
    if (strcmp(str, "lastTime") == 0) return "SHIFTALLCOLORLABEL4";
}

//Информация о плагине
//Имя
DLLEXPORT char* getName(char* str)
{
    return "Сдвиг каждого цвета";
}
//Уникальный идентефикатор
DLLEXPORT char* getIdName(char* str) 
{
    return "SHIFTALLCOLOR";
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
    int red = argsArray[0];
    int green = argsArray[1];
    int blue = argsArray[2];
    double start = clock();
    for (int i = 1079; i < length; i++)
    {
        if (i % 4 == 0) array[i] = (unsigned char)(array[i] + red);
        if (i % 4 == 3) array[i] = (unsigned char)(array[i] + green);
        if (i % 4 == 2) array[i] = (unsigned char)(array[i] + blue);
    }
    double end = clock();
    lastTimeValue = (double)(end - start) / CLOCKS_PER_SEC;
    return array;
}

DLLEXPORT char* lastTime(char* args) {
    sprintf_s(buffer, 50, "Время обработки кадра: %f секунд", lastTimeValue);
    return buffer;
}