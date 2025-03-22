using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextDisplay : MonoBehaviour
{
    public Text textDisplay; // UI Text ��� ����������� ������
    public float characterDelay = 0.05f; // �������� ����� ���������

    private Coroutine displayCoroutine; // ������� �������� ��� ����������� ������

    // ����� ��� ������� ������
    public void InsertText(string text)
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayText(text));
    }

    // �������� ��� ����������� ������
    private IEnumerator DisplayText(string text)
    {
        textDisplay.text = ""; // ������� �����
        string[] parts = text.Split('@'); // ��������� ����� �� ������� '@'

        for (int i = 0; i < parts.Length; i++)
        {
            string[] subParts = parts[i].Split(new[] { ' ' }, 2); // ��������� �� ����� � �����
            string displayText = subParts[0]; // ����� ��� �����������
            float displayTime = subParts.Length > 1 ? float.Parse(subParts[1]) : 0f; // ����� �����������

            // ���������� ������� �����
            foreach (char c in displayText)
            {
                textDisplay.text += c;
                yield return new WaitForSeconds(characterDelay);
            }

            // ���� ��������� �����
            yield return new WaitForSeconds(displayTime);

            // ������� ����� ��� ��������� �����
            textDisplay.text = "";
        }
    }
}