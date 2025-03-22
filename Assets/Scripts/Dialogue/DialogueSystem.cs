using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText; // UI Text для отображения диалога
    public Image background; // Полупрозрачный фон
    public Font[] fonts; // Массив шрифтов
    public Button nextButton; // Кнопка для перехода к следующей строке
    public Button prevButton; // Кнопка для возврата к предыдущей строке

    private List<string> dialogueLines = new List<string>(); // Список строк диалога
    private int currentLine = 0; // Текущая строка диалога
    private Coroutine displayCoroutine; // Текущая корутина для отображения текста

    // Инициализация
    private void Start()
    {
        nextButton.onClick.AddListener(NextLine);
        prevButton.onClick.AddListener(PreviousLine);
        background.gameObject.SetActive(false); // Скрываем фон при старте
    }

    // Метод для запуска диалога
    public void Dialogue(Font[] fonts, string text)
    {
        this.fonts = fonts;
        dialogueLines.Clear();
        currentLine = 0;
        ParseDialogue(text); // Парсим текст
        StartDialogue(); // Запускаем диалог
    }

    // Парсинг текста диалога
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

    // Запуск диалога
    private void StartDialogue()
    {
        background.gameObject.SetActive(true);
        currentLine = 0;
        DisplayCurrentLine();
    }

    // Отображение текущей строки
    private void DisplayCurrentLine()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayLine(dialogueLines[currentLine]));
    }

    // Корутина для отображения строки
    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        int fontIndex = GetFontIndex(line); // Получаем индекс шрифта
        if (fontIndex != -1)
        {
            dialogueText.font = fonts[fontIndex]; // Устанавливаем шрифт
        }

        // Постепенно выводим текст
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Получение индекса шрифта из строки
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

    // Переход к следующей строке
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

    // Возврат к предыдущей строке
    private void PreviousLine()
    {
        if (currentLine > 0)
        {
            currentLine--;
            DisplayCurrentLine();
        }
    }

    // Завершение диалога
    private void EndDialogue()
    {
        background.gameObject.SetActive(false);
        dialogueText.text = "";
    }
}