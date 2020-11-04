#include "stdafx.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>
 
#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT char* pluginFunctions();
DLLEXPORT char* functionsDescriptions(char* str);
DLLEXPORT char* functionsInterfaceCFG(char* str);
DLLEXPORT char* functionsArgs(char* str);
DLLEXPORT char* functionsType(char* str);
DLLEXPORT char* functionsTarget(char* str);

DLLEXPORT char* getName(char* str);
DLLEXPORT char* getIdName(char* str);
DLLEXPORT char* getAuthor(char* args);
DLLEXPORT char* getVersion(char* str);

DLLEXPORT char* filterFunct(char* array, int length, char* args);
DLLEXPORT char* lastTime(char* args);

