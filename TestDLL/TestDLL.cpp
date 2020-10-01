// TestDLL.cpp : Определяет экспортируемые функции для DLL.
//

#include "pch.h"
#include "framework.h"
#include "TestDLL.h"
#include <string>



// Пример экспортированной функции.
TESTDLL_API std::string getInfo()
{
    return "TestDll";
}

TESTDLL_API BYTE* filterFunct(BYTE* array) {
    return array;
}

