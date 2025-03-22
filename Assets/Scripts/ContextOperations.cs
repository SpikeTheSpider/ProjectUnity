using System.Numerics;
using UnityEngine;

public interface IInteractable
{
    void ShowInfo(); // ���������� ���������� �� �������
    void Interact(); // �������������� � ��������
    void Hack(); // ����� ������� (��������� �������� ����)

    string GetDefaultCode(); // ���������� ����������� ��� ��� �������
    string GetHelpInfo(); // ���������� ���������� ���������� ��� �������

    // ����� ������ ��� ���������� � ��������� �������� ����
    string GetCurrentCode(); // ���������� ������� ���, ��������� �������
    void SetCurrentCode(string code); // ��������� ������� ���
}
public enum ButtonAction
{
    Info,
    Interact,
    Hack
}