using Assets.Scripts.Player;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GravityBox : MonoBehaviour, IInteractable
{

    [SerializeField] private TextAsset infoFile; // Текстовый файл с информацией
    [SerializeField] private InfoWindow infoWindow; // Ссылка на информационное окно
    [SerializeField] private IPlayer player;
    [SerializeField] private float interactionRadius = 3f;
    public double gravityX { get; set; } = 0; // Гравитация по оси X (публичное свойство)
    public double gravityY { get; set; } = -5; // Гравитация по оси Y (публичное свойство)

    // Ссылки на текстовые файлы
    [SerializeField] private TextAsset defaultCodeFile; // Стандартный код
    [SerializeField] private TextAsset helpInfoFile; // Справочная информация

    // Ссылка на редактор кода
    [SerializeField] private CodeEditorWindow codeEditorWindow;

    private Rigidbody2D rb; // Компонент Rigidbody2D
    private string currentCode; // Текущий код, введенный игроком

    private void Start()
    {
        // Получаем компонент Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on GravityBox!");
        }

        // Применяем начальную гравитацию
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        // Применяем гравитацию каждый кадр
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (rb != null)
        {
            // Применяем гравитацию через Rigidbody2D
            Vector2 gravity = new Vector2((float)gravityX, (float)gravityY) * Time.fixedDeltaTime;
            rb.linearVelocity += gravity;
        }
    }

    public void SetGravity(double x, double y)
    {
        // Ограничиваем значения гравитации
        gravityX = Mathf.Clamp((float)x, -5f, 5f);
        gravityY = Mathf.Clamp((float)y, -5f, 5f);
    }

    // Реализация методов интерфейса IInteractable
    public void ShowInfo()
    {
        if (infoWindow != null && infoFile != null)
        {
            infoWindow.Show(infoFile.text); // Показываем информацию
        }
        else
        {
            Debug.LogError("InfoWindow or infoFile is not assigned!");
        }
    }

    public void Interact()
    {
        // Проверяем расстояние между игроком и коробкой
        float distance = Vector2.Distance(transform.position,((PlayerPlatformerMovement)player).transform.position);
        if (distance <= interactionRadius)
        {
            // Толкаем коробку от игрока
            Vector2 direction = (transform.position - ((PlayerPlatformerMovement)player).transform.position).normalized;
            rb.AddForce(direction * 10f, ForceMode2D.Impulse); // Толчок
        }
        else
        {
            Debug.Log("Player is too far away to interact with the box!");
        }
    }

    public void Hack()
    {
        // Открываем редактор кода для взлома
        if (codeEditorWindow != null)
        {
            codeEditorWindow.Show(this);
        }
        else
        {
            Debug.LogError("CodeEditorWindow is not assigned!");
        }
    }

    public string GetDefaultCode()
    {
        return defaultCodeFile != null ? defaultCodeFile.text : "gravityX = 1;\ngravityY = 0;";
    }

    public string GetHelpInfo()
    {
        return helpInfoFile != null ? helpInfoFile.text : "Available variables:\n- gravityX: double\n- gravityY: double";
    }

    public string GetCurrentCode()
    {
        return currentCode; // Возвращаем сохраненный код
    }

    public void SetCurrentCode(string code)
    {
        currentCode = code; // Сохраняем введенный код
        Debug.Log("Code saved: " + code); // Отладочный вывод
    }

    // Реализация метода GetContext (если он есть в интерфейсе)
    public object GetContext()
    {
        return this; // Возвращаем сам объект как контекст
    }
}