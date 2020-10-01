#include "stdafx.h"
#include <windows.h>
#include <stdexcept> 
#include <iostream>
 
 #define DLLEXPORT extern "C" __declspec(dllexport)

DLLEXPORT void test();
DLLEXPORT char* getInfo();
DLLEXPORT BYTE* filterFunct(BYTE* array, int length);
DLLEXPORT int calc();