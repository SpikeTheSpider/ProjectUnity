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
    [SerializeField] private TMP_InputField codeInputField; // Поле для ввода кода (TextMeshPro)
    [SerializeField] public TMP_Text errorText; // Текст для ошибок (TextMeshPro)
    [SerializeField] private TMP_Text helpText; // Текст для справки (TextMeshPro)
    [SerializeField] private Button runButton; // Кнопка для выполнения кода
    [SerializeField] private Button closeButton; // Кнопка для закрытия окна

    private IInteractable targetObject; // Объект, с которым взаимодействуем
    private Coroutine validationCoroutine; // Для задержки проверки кода

    // Показываем редактор кода
    public void Show(IInteractable interactable)
    {
        targetObject = interactable;

        // Загружаем сохраненный код, если он есть, иначе стандартный код
        string savedCode = interactable.GetCurrentCode();
        codeInputField.text = !string.IsNullOrEmpty(savedCode) ? savedCode : interactable.GetDefaultCode();

        helpText.text = interactable.GetHelpInfo(); // Загружаем справочную информацию
        errorText.text = "Окно для ошибок";
        gameObject.SetActive(true); // Показываем окно редактора

        Time.timeScale = 0.05f; // Замедляем время
    }

    public void Hide()
    {
        if (validationCoroutine != null)
        {
            StopCoroutine(validationCoroutine);
            validationCoroutine = null;
        }
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Start()
    {
        runButton.onClick.AddListener(OnRunButtonClicked);
        closeButton.onClick.AddListener(Hide);
        codeInputField.onValueChanged.AddListener(OnCodeValueChanged);
    }

    private void OnCodeValueChanged(string text)
    {
        if (!this.isActiveAndEnabled) return;

        if (validationCoroutine != null)
        {
            StopCoroutine(validationCoroutine);
        }
        validationCoroutine = StartCoroutine(ValidateAfterDelay(text));
    }

    // Обработчик нажатия на кнопку выполнения кода
    private async void OnRunButtonClicked()
    {
        string code = codeInputField.text; // Получаем введенный код
        targetObject.SetCurrentCode(code); // Сохраняем код в объекте
        await ExecuteCode(code); // Выполняем код
        // Закрываем окно после выполнения кода
    }

    // Выполнение кода
    private async Task ExecuteCode(string code)
    {
        try
        {
            // Используем сам объект как контекст
            var context = targetObject;

            // Добавляем ссылки на сборки Unity
            var scriptOptions = ScriptOptions.Default
                .WithReferences(
                    typeof(MonoBehaviour).Assembly, // UnityEngine.CoreModule
                    typeof(Debug).Assembly // UnityEngine.CoreModule (для Debug.Log)
                )
                .WithImports("UnityEngine"); // Пространство имен UnityEngine

            // Создаем скрипт, который будет выполняться в контексте объекта
            var script = CSharpScript.Create(code, scriptOptions, context.GetType());

            // Выполняем код, передавая контекст
            await script.RunAsync(context);

            
        }
        catch (CompilationErrorException ex)
        {
            // Обрабатываем ошибки компиляции
            errorText.text = "Compilation error: " + ex.Message;
        }
        catch (System.Exception ex)
        {
            // Обрабатываем другие ошибки
            errorText.text = "Runtime error: " + ex.Message;
        }
    }

    // Проверка кода с задержкой
    private IEnumerator ValidateAfterDelay(string code)
    {
        yield return new WaitForSeconds(0.5f); // Задержка 0.5 секунды
        yield return ValidateAndDisplayErrors(code); // Проверяем код
    }

    // Проверка кода и вывод ошибок
    private async Task ValidateAndDisplayErrors(string code)
    {
        try
        {
            // Используем сам объект как контекст
            var context = targetObject;

            // Добавляем ссылки на сборки Unity
            var scriptOptions = ScriptOptions.Default
                .WithReferences(
                    typeof(MonoBehaviour).Assembly, // UnityEngine.CoreModule
                    typeof(Debug).Assembly // UnityEngine.CoreModule (для Debug.Log)
                )
                .WithImports("UnityEngine"); // Пространство имен UnityEngine

            // Пытаемся скомпилировать код
            var script = CSharpScript.Create(code, scriptOptions, context.GetType());
            var compilation = script.GetCompilation();

            // Получаем диагностику (ошибки и предупреждения)
            var diagnostics = compilation.GetDiagnostics();

            // Если есть ошибки, возвращаем их
            if (diagnostics.Any(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error))
            {
                errorText.text = string.Join("\n", diagnostics
                    .Where(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .Select(d => d.GetMessage()));
            }
            else
            {
                errorText.text = "No errors found."; // Если ошибок нет
            }
        }
        catch (Exception ex)
        {
            errorText.text = $"Validation error: {ex.Message}";
        }
    }
}