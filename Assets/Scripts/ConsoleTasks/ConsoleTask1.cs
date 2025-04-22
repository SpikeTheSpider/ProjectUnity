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

    // ��������: ������ ��� ��������, ������� ����� �������� �� �������
    public int OxygenLevel { get; set; } = 95;
    public string ModuleName { get; set; } = "�������";
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

    public void ShowInfo()
    {
        Debug.Log("������� ���������� ��������. ���������� ��������� ���� ����������.");
    }

    public string GetDefaultCode()
    {
        return defaultCodeFile != null ? defaultCodeFile.text :
            "// ��������� �������� ����������:\n" +
            "OxygenLevel = 95;\n" +
            "ModuleName = \"�������\";\n" +
            "IsOperational = true;\n" +
            "Temperature = -12.5f;";
    }

    public string GetHelpInfo()
    {
        return helpInfoFile != null ? helpInfoFile.text :
            "��������� ���������� ��� �����������:\n" +
            "- OxygenLevel (���: int, ������ ����: 95)\n" +
            "- ModuleName (���: string, ������ ����: \"�������\")\n" +
            "- IsOperational (���: bool, ������ ����: true)\n" +
            "- Temperature (���: float, ������ ����: -12.5f)\n\n" +
            "������ ����������� �������:\n" +
            "OxygenLevel = 95;\n" +
            "ModuleName = \"�������\";\n" +
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

            // ��������� ���, ��������� ���� ������ ��� ��������
            var script = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(
                code: currentCode,
                options: scriptOptions,
                globalsType: this.GetType());

            var result = await script.RunAsync(this);

            CheckSolution();
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ���������� ����: {ex.Message}");
        }
    }

    private void CheckSolution()
    {
        try
        {
            bool oxygenCorrect = OxygenLevel == 95;
            bool moduleCorrect = ModuleName == "�������";
            bool operationalCorrect = IsOperational == true;
            bool tempCorrect = Math.Abs(Temperature - (-12.5f)) < 0.01f;

            Debug.Log($"��������: Oxygen: {oxygenCorrect}, Module: {moduleCorrect}, Operational: {operationalCorrect}, Temp: {tempCorrect}");

            if (oxygenCorrect && moduleCorrect && operationalCorrect && tempCorrect)
            {
                Debug.Log("��� ���������� ���������!");
                if (objectToDisable != null)
                {
                    objectToDisable.SetActive(false);
                    Debug.Log("������ ������! ������� ���������� �������������.");
                }
            }
            else
            {
                Debug.Log("�� ��� ���������� ���������� ���������");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"������ ��� �������� �������: {e.Message}");
        }
    }

    public object GetContext() => this;
}