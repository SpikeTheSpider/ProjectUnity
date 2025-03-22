using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CodeEditorWindow : MonoBehaviour
{
    [SerializeField] private TMP_InputField codeInputField; // ���� ��� ����� ���� (TextMeshPro)
    [SerializeField] private TMP_Text errorText; // ����� ��� ������ (TextMeshPro)
    [SerializeField] private TMP_Text helpText; // ����� ��� ������� (TextMeshPro)
    [SerializeField] private Button runButton; // ������ ��� ���������� ����

    private IInteractable targetObject; // ������, � ������� ���������������
    private Coroutine validationCoroutine; // ��� �������� �������� ����

    // ���������� �������� ����
    public void Show(IInteractable interactable)
    {
        targetObject = interactable;

        // ��������� ����������� ���, ���� �� ����, ����� ����������� ���
        string savedCode = interactable.GetCurrentCode();
        codeInputField.text = !string.IsNullOrEmpty(savedCode) ? savedCode : interactable.GetDefaultCode();

        helpText.text = interactable.GetHelpInfo(); // ��������� ���������� ����������
        gameObject.SetActive(true); // ���������� ���� ���������

        Time.timeScale = 0.05f; // ��������� �����
    }

    public void Hide()
    {
        gameObject.SetActive(false); // �������� ���� ���������

        Time.timeScale = 1f; // ��������������� �����
    }

    private void Start()
    {
        // ����������� ������ ���������� ����
        runButton.onClick.AddListener(OnRunButtonClicked);

        // ��������� ���������� ��������� ������ � ���������
        codeInputField.onValueChanged.AddListener((text) =>
        {
            if (validationCoroutine != null)
            {
                StopCoroutine(validationCoroutine);
            }
            validationCoroutine = StartCoroutine(ValidateAfterDelay(text));
        });
    }

    // ���������� ������� �� ������ ���������� ����
    private async void OnRunButtonClicked()
    {
        string code = codeInputField.text; // �������� ��������� ���
        targetObject.SetCurrentCode(code); // ��������� ��� � �������
        await ExecuteCode(code); // ��������� ���
        Hide(); // ��������� ���� ����� ���������� ����
    }

    // ���������� ����
    private async Task ExecuteCode(string code)
    {
        try
        {
            // ���������� ��� ������ ��� ��������
            var context = targetObject;

            // ��������� ������ �� ������ Unity
            var scriptOptions = ScriptOptions.Default
                .WithReferences(
                    typeof(MonoBehaviour).Assembly, // UnityEngine.CoreModule
                    typeof(Debug).Assembly // UnityEngine.CoreModule (��� Debug.Log)
                )
                .WithImports("UnityEngine"); // ������������ ���� UnityEngine

            // ������� ������, ������� ����� ����������� � ��������� �������
            var script = CSharpScript.Create(code, scriptOptions, context.GetType());

            // ��������� ���, ��������� ��������
            await script.RunAsync(context);

            errorText.text = "Code executed successfully.";
        }
        catch (CompilationErrorException ex)
        {
            // ������������ ������ ����������
            errorText.text = "Compilation error: " + ex.Message;
        }
        catch (System.Exception ex)
        {
            // ������������ ������ ������
            errorText.text = "Runtime error: " + ex.Message;
        }
    }

    // �������� ���� � ���������
    private IEnumerator ValidateAfterDelay(string code)
    {
        yield return new WaitForSeconds(0.5f); // �������� 0.5 �������
        yield return ValidateAndDisplayErrors(code); // ��������� ���
    }

    // �������� ���� � ����� ������
    private async Task ValidateAndDisplayErrors(string code)
    {
        try
        {
            // ���������� ��� ������ ��� ��������
            var context = targetObject;

            // ��������� ������ �� ������ Unity
            var scriptOptions = ScriptOptions.Default
                .WithReferences(
                    typeof(MonoBehaviour).Assembly, // UnityEngine.CoreModule
                    typeof(Debug).Assembly // UnityEngine.CoreModule (��� Debug.Log)
                )
                .WithImports("UnityEngine"); // ������������ ���� UnityEngine

            // �������� �������������� ���
            var script = CSharpScript.Create(code, scriptOptions, context.GetType());
            var compilation = script.GetCompilation();

            // �������� ����������� (������ � ��������������)
            var diagnostics = compilation.GetDiagnostics();

            // ���� ���� ������, ���������� ��
            if (diagnostics.Any(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error))
            {
                errorText.text = string.Join("\n", diagnostics
                    .Where(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .Select(d => d.GetMessage()));
            }
            else
            {
                errorText.text = "No errors found."; // ���� ������ ���
            }
        }
        catch (Exception ex)
        {
            errorText.text = $"Validation error: {ex.Message}";
        }
    }
}