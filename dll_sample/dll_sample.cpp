#include "stdafx.h"
#include "dll_sample.h"
#include "time.h"
#include <sstream> 
#include <string.h> 
#include <vector>

using namespace std;

//������ �������
DLLEXPORT char* pluginFunctions()
{
    return "filterFunct lastTime";
}

//�������� ������� � �������� �������
DLLEXPORT char* functionsDescriptions(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "������� ������ �����";
    if (strcmp(str, "lastTime") == 0) return "���������� ������� ����� ���������";
}

//"PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,������ ���������;PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,������ ���������"
//������� ������: 
//����������_�������������MAIN - ������ ��� ��������
//INFOPANEL - ������ � ���� ���������� � ��������
DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTCOLORMAIN,LABEL,SHIFTCOLORLABEL1,300,20,�������� ������;SHIFTCOLORMAIN,TRACKBAR,SHIFTCOLORTRACKBAR1,300,30,�������� ������,0,255";
    if (strcmp(str, "lastTime") == 0) return "SHIFTCOLORMAIN,LABEL,SHIFTCOLORLABEL2,300,20,����� ��������� �����:";
}

//��������� ��� ������ ����������� ����� ������, �� ������� ������� ���������, "" ���� �� ���������
DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTCOLORTRACKBAR1";
    if (strcmp(str, "lastTime") == 0) return "";
}

//������������:
//TRIGGEREND - ����� ��������� �����
//TRIGGERSTART - �� ��������� �����
//MAIN - ������-�������
//�������� ���������� - �� �����
//NONE - �� ������������
DLLEXPORT char* functionsType(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "MAIN";
    if (strcmp(str, "lastTime") == 0) return "TRIGGEREND";
}

//���������, � ������� ������������ ���������, "" ���� �� ���������
DLLEXPORT char* functionsTarget(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
    if (strcmp(str, "lastTime") == 0) return "SHIFTCOLORLABEL2";
}

//���������� � �������
//���
DLLEXPORT char* getName(char* str)
{
    return "����� �����";
}
//���������� �������������
DLLEXPORT char* getIdName(char* str) 
{
    return "SHIFTCOLOR";
}
//�����
DLLEXPORT char* getAuthor(char* str)
{
    return "��������� �.�. ���17-01";
}
//������
DLLEXPORT char* getVersion(char* str)
{
    return "v1.0";
}

//���������������� ���������� �������

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
    sprintf_s(buffer, 50, "����� ��������� �����: %f ������", lastTimeValue);
    return buffer;
}