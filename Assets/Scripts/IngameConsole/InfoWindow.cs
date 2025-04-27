using UnityEngine;
using TMPro;

public class InfoWindow : MonoBehaviour
{
    public static InfoWindow Instance { get; private set; } // Синглтон

    [SerializeField] private TMP_Text infoText; // Текст для отображения информации
    [SerializeField] private GameObject window; // Объект окна

    private void Awake()
    {
        //// Инициализация синглтона
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject); // Уничтожаем дубликаты
        //}
    }

    // Показываем окно с информацией
    public void Show(string info)
    {
        infoText.text = info; // Устанавливаем текст
        window.SetActive(true); // Показываем окно
        Time.timeScale = 0.05f; // Замедляем время
    }

    // Скрываем окно
    public void Hide()
    {
        window.SetActive(false); // Скрываем окно
        Time.timeScale = 1f; // Восстанавливаем время
    }

    // Метод для кнопки закрытия
    public void OnCloseButtonClicked()
    {
        Hide();
    }
}
