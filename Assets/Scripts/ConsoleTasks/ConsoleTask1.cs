using UnityEngine;
using System;

public class ConsoleTask1 : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private TextAsset infoFile;
    [SerializeField] private TextAsset helpInfoFile;
    [SerializeField] private TextAsset defaultCodeFile;
    [SerializeField] private CodeEditorWindow codeEditorWindow;
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private float interactionRadius = 3f;

    // Изменено: теперь это свойства, которые можно изменять из скрипта
    public int OxygenLevel { get; set; } = 95;
    public string ModuleName { get; set; } = "Реактор";
    public bool IsOperational { get; set; } = true;
    public float Temperature { get; set; } = -12.5f;

    private string currentCode;
    private scr_Player_Main player;

    private void Start()
    {
        player = FindObjectOfType<scr_Player_Main>();
    }

    public void Interact()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= interactionRadius)
        {
            Hack();
        }
        else
        {
            Debug.Log("Слишком далеко для взаимодействия!");
        }
    }

    public void Hack()
    {
        if (codeEditorWindow != null)
        {
            codeEditorWindow.Show(this);
        }
    }

    public void ShowInfo()
    {
        Debug.Log("Консоль управления станцией. Необходимо исправить типы переменных.");
    }

    public string GetDefaultCode()
    {
        return defaultCodeFile != null ? defaultCodeFile.text :
            "// Исправьте значения переменных:\n" +
            "OxygenLevel = 95;\n" +
            "ModuleName = \"Реактор\";\n" +
            "IsOperational = true;\n" +
            "Temperature = -12.5f;";
    }

    public string GetHelpInfo()
    {
        return helpInfoFile != null ? helpInfoFile.text :
            "Доступные переменные для исправления:\n" +
            "- OxygenLevel (тип: int, должно быть: 95)\n" +
            "- ModuleName (тип: string, должно быть: \"Реактор\")\n" +
            "- IsOperational (тип: bool, должно быть: true)\n" +
            "- Temperature (тип: float, должно быть: -12.5f)\n\n" +
            "Пример правильного решения:\n" +
            "OxygenLevel = 95;\n" +
            "ModuleName = \"Реактор\";\n" +
            "IsOperational = true;\n" +
            "Temperature = -12.5f;";
    }

    public string GetCurrentCode() => currentCode;

    public void SetCurrentCode(string code)
    {
        currentCode = code;
        ExecuteCodeAndCheckSolution();
    }

    private async void ExecuteCodeAndCheckSolution()
    {
        try
        {
            var scriptOptions = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .WithReferences(typeof(MonoBehaviour).Assembly)
                .WithImports("UnityEngine", "System");

            // Выполняем код, передавая этот объект как контекст
            var script = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(
                code: currentCode,
                options: scriptOptions,
                globalsType: this.GetType());

            var result = await script.RunAsync(this);

            CheckSolution();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка выполнения кода: {ex.Message}");
        }
    }

    private void CheckSolution()
    {
        try
        {
            bool oxygenCorrect = OxygenLevel == 95;
            bool moduleCorrect = ModuleName == "Реактор";
            bool operationalCorrect = IsOperational == true;
            bool tempCorrect = Math.Abs(Temperature - (-12.5f)) < 0.01f;

            Debug.Log($"Проверка: Oxygen: {oxygenCorrect}, Module: {moduleCorrect}, Operational: {operationalCorrect}, Temp: {tempCorrect}");

            if (oxygenCorrect && moduleCorrect && operationalCorrect && tempCorrect)
            {
                Debug.Log("Все переменные корректны!");
                if (objectToDisable != null)
                {
                    objectToDisable.SetActive(false);
                    Debug.Log("Задача решена! Система управления восстановлена.");
                }
            }
            else
            {
                Debug.Log("Не все переменные исправлены правильно");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка при проверке решения: {e.Message}");
        }
    }

    public object GetContext() => this;
}