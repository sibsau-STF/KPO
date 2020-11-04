// dll_sample.cpp: определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include "dll_sample.h"
#include "time.h"
#include <sstream> 
#include <string.h> 
#include <vector>

using namespace std;

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

DLLEXPORT char* pluginFunctions()
{
    return "filterFunct lastTime";
}

DLLEXPORT char* functionsDescriptions(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "Инвертирование цвета";
    if (strcmp(str, "lastTime") == 0) return "Возвращает среднее время обработки";
}

DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "INVERTCOLORMAIN,LABEL,TESTLABEL1,300,20,Тестовый текст;INVERTCOLORMAIN,TEXTBOX,TESTTEXTBOX1,300,30,Тестовый текст textbox;INVERTCOLORMAIN,TRACKBAR,TRACKBAR1,300,30,Тестовый trackbar,0,255";
    if (strcmp(str, "lastTime") == 0) return "INVERTCOLORMAIN,LABEL,LABELlastTime,300,20,Время обработки кадра:";
}

DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "TRACKBAR1";
    if (strcmp(str, "lastTime") == 0) return "";
}

DLLEXPORT char* functionsType(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "MAIN";
    if (strcmp(str, "lastTime") == 0) return "TRIGGEREND";
}

DLLEXPORT char* functionsTarget(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
    if (strcmp(str, "lastTime") == 0) return "LABELlastTime";
}

DLLEXPORT char* getName(char* str)
{
    return "Инвертирование цвета";
}
DLLEXPORT char* getIdName(char* str) 
{
    return "INVERTCOLOR";
}

DLLEXPORT char* getAuthor(char* str)
{
    return "Статников А.С. БПИ17-01";
}

DLLEXPORT char* getVersion(char* str)
{
    return "v1.0";
}

char* buffer = new char[50];
double lastTimeValue = 0;

DLLEXPORT char* filterFunct(char* array, int length, char* args) {
    /*std::vector<std::string> splitedArgs = split(args, ';');
    int* argsArray = new int[splitedArgs.size()];
    for (size_t i = 0; i < splitedArgs.size(); i++)
    {
        args[i] = atoi(splitedArgs[i].c_str());
    }*/

    double start = clock();
    for (int i = 1079; i < length; i++)
    {
        array[i] = (unsigned char)(255 - array[i]);
    }
    double end = clock();
    lastTimeValue = (double)(end - start) / CLOCKS_PER_SEC;
    delete[] args;
    return array;
}

DLLEXPORT char* lastTime(char* args) {
    sprintf_s(buffer, 50, "Время обработки кадра: %f секунд", lastTimeValue);
    return buffer;
}