
using Assets.Scripts.GravityBox;
using Microsoft.CodeAnalysis.Scripting;
using System;
using UnityEngine;

namespace Assets.Scripts.ConsoleTasks
{
    public class ConsoleTask2 : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private TextAsset infoFile;
        [SerializeField] private TextAsset helpInfoFile;
        [SerializeField] private TextAsset defaultCodeFile;
        [SerializeField] private CodeEditorWindow codeEditorWindow;
        [SerializeField] private GameObject doorToUnlock;
        [SerializeField] private ZoneChecker zoneChecker;
        [SerializeField] private float interactionRadius = 3f;

        private string currentCode;
        private PlayerPlatformerMovement player;

            private void Start()
    {
        player = FindObjectOfType<PlayerPlatformerMovement>();
    }

        public string GetDefaultCode()
        {
            return defaultCodeFile.text;
        }

        public string GetHelpInfo()
        {
            return helpInfoFile != null ? helpInfoFile.text :
                "Чтобы запустить систему охлаждения, " +
                "необходимо поставить 3 вентилятора на свои места, " +
                "и вывести следующию строку в консоль:\n" +
                "- Активировать охлаждение!\n";
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

                CheckSolution(result);

                codeEditorWindow.errorText.text = "Код скомпелирован успешно.";
            }
            catch (Exception ex)
            {
                codeEditorWindow.errorText.text = $"Ошибка выполнения кода: {ex.Message}";
            }
        }

        private void CheckSolution(ScriptState<object> result)
        {
            bool solution = result.Script.Code.Contains("Активировать охлаждение!");
            bool fans = zoneChecker.Count == 3;

            if (!fans)
            {
                throw new Exception("Количество вентиляторов не соответствует требованиям");
            }

            if (!solution)
            {
                throw new Exception("Код не соответствует требованиям");
            }

            if (doorToUnlock != null)
            {
                doorToUnlock.SetActive(false);
            }
        }

        public void ShowInfo()
        {
            Debug.Log("Консоль управления станцией. Необходимо включить систему охлаждения.");
        }
    }
}
