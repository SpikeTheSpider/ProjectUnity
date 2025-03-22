using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneToLoad; // ��� ����� ��� ��������

    public void Interact()
    {
        Debug.Log("������� �� �������: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad); // ��������� ����� �����
    }

    // ��������� ������ ���������� IInteractable
    public string GetCurrentCode() => throw new System.NotImplementedException();
    public string GetDefaultCode() => throw new System.NotImplementedException();
    public string GetHelpInfo() => throw new System.NotImplementedException();
    public void Hack() => throw new System.NotImplementedException();
    public void SetCurrentCode(string code) => throw new System.NotImplementedException();
    public void ShowInfo() => throw new System.NotImplementedException();
}
