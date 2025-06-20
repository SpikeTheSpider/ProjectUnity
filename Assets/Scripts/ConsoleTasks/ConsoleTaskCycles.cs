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
        Debug.Log($"����� ������: {correctPassword} (������ ��� �����)");
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
            Debug.Log("������� ������ ��� ��������������!");
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
            "// ������� 3-������� ������\n" +
            "EnterPassword(\"000\");";
    }

    public string GetHelpInfo()
    {
        return helpInfoFile != null ? helpInfoFile.text :
            "���������� ��������� 3-������� ������.\n" +
            "����������� ������� EnterPassword(\"123\"),\n" +
            "��� \"123\" - ��� ������� ������.\n\n" +
            "������ �������� ��� ������ ������� ���������.\n" +
            "����� 3 ��������� ������� ������ ������������.";
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

            // ������� ����� CheckPassword � ��������� ��� ��������
            var script = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(
                code: code,
                options: scriptOptions,
                globalsType: this.GetType());

            await script.RunAsync(this);
            codeEditorWindow.errorText.text = "��� ������������� �������.";
        }
        catch (Exception ex)
        {
            codeEditorWindow.errorText.text = $"������ ���������� ����: {ex.Message}";
        }
    }

    // ���� ����� ����� ���������� �� ������� ������
    public void EnterPassword(string attempt)
    {
        if (isUnlocked) return;

        if (attempt.Length != 3 || !int.TryParse(attempt, out _))
        {
            Debug.Log("������ ������ ���� 3-������� ������!");
            return;
        }

        if (attempt == correctPassword)
        {
            UnlockTerminal();
        }
        else
        {
            Debug.Log("�������� ������! ���������� �����.");
        }
    }

    private void UnlockTerminal()
    {
        isUnlocked = true;
        Debug.Log("������ ��������! ������ ������.");

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
        Debug.Log("�������� ������������. ���������� ��������� 3-������� ������.");
    }
}