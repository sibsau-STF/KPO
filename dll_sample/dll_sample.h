#include "stdafx.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>
 
#define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT char* PluginFunctions();
DLLEXPORT char* FunctionsDescriptions(char* str);
DLLEXPORT char* FunctionsInterfaceCFG(char* str);
DLLEXPORT char* FunctionsType(char* str);

DLLEXPORT char* getName(char* str);
DLLEXPORT char* getIdName(char* str);
DLLEXPORT char* filterFunct(char* array, int length);