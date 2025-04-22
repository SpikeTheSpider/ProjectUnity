public enum ButtonAction
{
    Info,
    Interact,
    Hack
}

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