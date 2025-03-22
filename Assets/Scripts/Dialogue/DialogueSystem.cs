using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText; // UI Text ��� ����������� �������
    public Image background; // �������������� ���
    public Font[] fonts; // ������ �������
    public Button nextButton; // ������ ��� �������� � ��������� ������
    public Button prevButton; // ������ ��� �������� � ���������� ������

    private List<string> dialogueLines = new List<string>(); // ������ ����� �������
    private int currentLine = 0; // ������� ������ �������
    private Coroutine displayCoroutine; // ������� �������� ��� ����������� ������

    // �������������
    private void Start()
    {
        nextButton.onClick.AddListener(NextLine);
        prevButton.onClick.AddListener(PreviousLine);
        background.gameObject.SetActive(false); // �������� ��� ��� ������
    }

    // ����� ��� ������� �������
    public void Dialogue(Font[] fonts, string text)
    {
        this.fonts = fonts;
        dialogueLines.Clear();
        currentLine = 0;
        ParseDialogue(text); // ������ �����
        StartDialogue(); // ��������� ������
    }

    // ������� ������ �������
    private void ParseDialogue(string text)
    {
        string[] lines = text.Split(new[] { "<font[n]><name>: @" }, System.StringSplitOptions.None);
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                dialogueLines.Add(line);
            }
        }
    }

    // ������ �������
    private void StartDialogue()
    {
        background.gameObject.SetActive(true);
        currentLine = 0;
        DisplayCurrentLine();
    }

    // ����������� ������� ������
    private void DisplayCurrentLine()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayLine(dialogueLines[currentLine]));
    }

    // �������� ��� ����������� ������
    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        int fontIndex = GetFontIndex(line); // �������� ������ ������
        if (fontIndex != -1)
        {
            dialogueText.font = fonts[fontIndex]; // ������������� �����
        }

        // ���������� ������� �����
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // ��������� ������� ������ �� ������
    private int GetFontIndex(string line)
    {
        if (line.StartsWith("<font[") && line.Contains("]"))
        {
            int startIndex = line.IndexOf('[') + 1;
            int endIndex = line.IndexOf(']');
            string indexStr = line.Substring(startIndex, endIndex - startIndex);
            return int.Parse(indexStr);
        }
        return -1;
    }

    // ������� � ��������� ������
    private void NextLine()
    {
        if (currentLine < dialogueLines.Count - 1)
        {
            currentLine++;
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    // ������� � ���������� ������
    private void PreviousLine()
    {
        if (currentLine > 0)
        {
            currentLine--;
            DisplayCurrentLine();
        }
    }

    // ���������� �������
    private void EndDialogue()
    {
        background.gameObject.SetActive(false);
        dialogueText.text = "";
    }
}