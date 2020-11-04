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
    if (strcmp(str, "filterFunct") == 0) return "������� �������� �����";
    if (strcmp(str, "lastTime") == 0) return "���������� ��������� ����� ���������";
}

//"PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,������ ���������;PANELNAME,COMPONENTTYPE,COMPONENTNAME,WIDTH,HEIGHT,TEXT,������ ���������"
//������� ������: 
//����������_�������������MAIN - ������ ��� ��������
//INFOPANEL - ������ � ���� ���������� � ��������
DLLEXPORT char* functionsInterfaceCFG(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
    if (strcmp(str, "lastTime") == 0) return "INVERSECOLORMAIN,LABEL,INVERSECOLORLABEL1,300,20,����� ��������� �����:";
}

//��������� ��� ������ ����������� ����� ������, �� ������� ������� ���������, "" ���� �� ���������
DLLEXPORT char* functionsArgs(char* str)
{
    if (strcmp(str, "filterFunct") == 0) return "";
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
    if (strcmp(str, "lastTime") == 0) return "INVERSECOLORLABEL1";
}

//���������� � �������
//���
DLLEXPORT char* getName(char* str)
{
    return "�������� �����";
}
//���������� �������������
DLLEXPORT char* getIdName(char* str) 
{
    return "INVERSECOLOR";
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

char* buffer = new char[100];
double lastTimeValue = 0;

DLLEXPORT char* filterFunct(char* array, int length, char* args) {
    double start = clock();
    for (int i = 1079; i < length; i++)
    {
        array[i] = (unsigned char)(255 - array[i]);
    }
    double end = clock();
    lastTimeValue = (double)(end - start) / CLOCKS_PER_SEC;
    return array;
}

DLLEXPORT char* lastTime(char* args) {
    sprintf_s(buffer, 50, "����� ��������� �����: %f ������", lastTimeValue);
    return buffer;
}