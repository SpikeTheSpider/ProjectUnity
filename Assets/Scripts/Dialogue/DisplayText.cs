using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextDisplay : MonoBehaviour
{
    public Text textDisplay; // UI Text для отображения текста
    public float characterDelay = 0.05f; // Задержка между символами

    private Coroutine displayCoroutine; // Текущая корутина для отображения текста

    // Метод для вставки текста
    public void InsertText(string text)
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayText(text));
    }

    // Корутина для отображения текста
    private IEnumerator DisplayText(string text)
    {
        textDisplay.text = ""; // Очищаем текст
        string[] parts = text.Split('@'); // Разделяем текст по символу '@'

        for (int i = 0; i < parts.Length; i++)
        {
            string[] subParts = parts[i].Split(new[] { ' ' }, 2); // Разделяем на текст и время
            string displayText = subParts[0]; // Текст для отображения
            float displayTime = subParts.Length > 1 ? float.Parse(subParts[1]) : 0f; // Время отображения

            // Постепенно выводим текст
            foreach (char c in displayText)
            {
                textDisplay.text += c;
                yield return new WaitForSeconds(characterDelay);
            }

            // Ждем указанное время
            yield return new WaitForSeconds(displayTime);

            // Очищаем текст для следующей части
            textDisplay.text = "";
        }
    }
}