using UnityEngine;
using System;
using Assets.Scripts.Player;
using Assets.Scripts.GravityBox;

public class PasswordTerminal : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private TextAsset infoFile;
    [SerializeField] private TextAsset helpInfoFile;
    [SerializeField] private TextAsset defaultCodeFile;
    [SerializeField] private CodeEditorWindow codeEditorWindow;
    [SerializeField] private GameObject  terminal;
    [SerializeField] private ZoneChecker zoneChecker;
    [SerializeField] private GameObject doorToUnlock;
    [SerializeField] private Sprite unlockedTerminalSprite;
    [SerializeField] private float interactionRadius = 3f;

    private string correctPassword;
    private PlayerPlatformerMovement player;
    private SpriteRenderer spriteRenderer;
    private bool isUnlocked = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerPlatformerMovement>();
        terminal.active = false;
        zoneChecker.CountChanged += OnCountChanged;
        spriteRenderer = GetComponent<SpriteRenderer>();
        GenerateNewPassword();
    }

    private void OnCountChanged()
    {
       if(zoneChecker.Count == 3) 
            terminal.active = true;
    }

    private void GenerateNewPassword()
    {
        System.Random rand = new System.Random();
        correctPassword = rand.Next(0, 1000).ToString("D3");
        Debug.Log($"Новый пароль: {correctPassword} (только для теста)");
    }

    public void Interact()
    {
        if (isUnlocked) return;

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

    public string GetDefaultCode()
    {
        return defaultCodeFile != null ? defaultCodeFile.text :
            "// Введите 3-значный пароль\n" +
            "EnterPassword(\"000\");";
    }

    public string GetHelpInfo()
    {
        return helpInfoFile != null ? helpInfoFile.text :
            "Необходимо подобрать 3-значный пароль.\n" +
            "Используйте функцию EnterPassword(\"123\"),\n" +
            "где \"123\" - ваш вариант пароля.\n\n" +
            "Пароль меняется при каждом запуске программы.\n" +
            "После 3 неудачных попыток пароль сбрасывается.";
    }

    public string GetCurrentCode() => "";

    public void SetCurrentCode(string code)
    {
        ExecuteCodeAndCheckSolution(code);
    }

    private async void ExecuteCodeAndCheckSolution(string code)
    {
        if (isUnlocked) return;

        try
        {
            var scriptOptions = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .WithReferences(typeof(MonoBehaviour).Assembly)
                .WithImports("UnityEngine", "System");

            // Убираем вызов CheckPassword и выполняем код напрямую
            var script = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(
                code: code,
                options: scriptOptions,
                globalsType: this.GetType());

            await script.RunAsync(this);
            codeEditorWindow.errorText.text = "Код скомпелирован успешно.";
        }
        catch (Exception ex)
        {
            codeEditorWindow.errorText.text = $"Ошибка выполнения кода: {ex.Message}";
        }
    }

    // Этот метод будет вызываться из скрипта игрока
    public void EnterPassword(string attempt)
    {
        if (isUnlocked) return;

        if (attempt.Length != 3 || !int.TryParse(attempt, out _))
        {
            Debug.Log("Пароль должен быть 3-значным числом!");
            return;
        }

        if (attempt == correctPassword)
        {
            UnlockTerminal();
        }
        else
        {
            Debug.Log("Неверный пароль! Попробуйте снова.");
        }
    }

    private void UnlockTerminal()
    {
        isUnlocked = true;
        Debug.Log("Доступ разрешен! Пароль верный.");

        if (unlockedTerminalSprite != null)
        {
            spriteRenderer.sprite = unlockedTerminalSprite;
        }

        if (doorToUnlock != null)
        {
            doorToUnlock.SetActive(false);
        }
    }

    public object GetContext() => this;

    public void ShowInfo()
    {
        Debug.Log("Терминал безопасности. Необходимо подобрать 3-значный пароль.");
    }
}