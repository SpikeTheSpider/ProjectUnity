using UnityEngine;
using TMPro;

public class InfoWindow : MonoBehaviour
{
    public static InfoWindow Instance { get; private set; } // ��������

    [SerializeField] private TMP_Text infoText; // ����� ��� ����������� ����������
    [SerializeField] private GameObject window; // ������ ����

    private void Awake()
    {
        //// ������������� ���������
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject); // ���������� ���������
        //}
    }

    // ���������� ���� � �����������
    public void Show(string info)
    {
        infoText.text = info; // ������������� �����
        window.SetActive(true); // ���������� ����
        Time.timeScale = 0.05f; // ��������� �����
    }

    // �������� ����
    public void Hide()
    {
        window.SetActive(false); // �������� ����
        Time.timeScale = 1f; // ��������������� �����
    }

    // ����� ��� ������ ��������
    public void OnCloseButtonClicked()
    {
        Hide();
    }
}
