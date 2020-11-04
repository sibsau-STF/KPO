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
    if (strcmp(str, "filterFunct") == 0) return "������� ������ ������� �����";
    if (strcmp(str, "lastTime") == 0) return "���������� ��������� ����� ���������";
}

//"PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,������ ���������;PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,������ ���������"
//������� ������: 
//����������_�������������MAIN - ������ ��� ��������
//INFOPANEL - ������ � ���� ���������� � ��������
DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL1,300,20,�������� ������ �������� �����;SHIFTALLCOLORMAIN,TRACKBAR,SHIFTALLCOLORTRACKBAR1,300,30,�������� ������ RED,0,255;SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL2,300,20,�������� ������ ������� �����;SHIFTALLCOLORMAIN,TRACKBAR,SHIFTALLCOLORTRACKBAR2,300,30,�������� ������ GREEN,0,255;SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL3,300,20,�������� ������ ������ �����;SHIFTALLCOLORMAIN,TRACKBAR,SHIFTALLCOLORTRACKBAR3,300,30,�������� ������ BLUE,0,255";
    if (strcmp(str, "lastTime") == 0) return "SHIFTALLCOLORMAIN,LABEL,SHIFTALLCOLORLABEL4,300,20,����� ��������� �����:";
}

//��������� ��� ������ ����������� ����� ������, �� ������� ������� ���������, "" ���� �� ���������
DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "SHIFTALLCOLORTRACKBAR1 SHIFTALLCOLORTRACKBAR2 SHIFTALLCOLORTRACKBAR3";
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
    if (strcmp(str, "lastTime") == 0) return "SHIFTALLCOLORLABEL4";
}

//���������� � �������
//���
DLLEXPORT char* getName(char* str)
{
    return "����� ������� �����";
}
//���������� �������������
DLLEXPORT char* getIdName(char* str) 
{
    return "SHIFTALLCOLOR";
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
    sprintf_s(buffer, 50, "����� ��������� �����: %f ������", lastTimeValue);
    return buffer;
}