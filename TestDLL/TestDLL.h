#include <string>
#define TESTDLL_API extern "C" __declspec(dllexport)

TESTDLL_API std::string getInfo();
TESTDLL_API BYTE* filterFunct(BYTE* array);
