using UnityEngine;
using TMPro; // Для TextMeshPro

[RequireComponent(typeof(Collider2D))] // Нужен для 2D коллайдера
public class Show2DTextOnHover : MonoBehaviour
{
    [Header("Text Settings")]
    public TextMeshPro textComponent; // Компонент TextMeshPro
    public Vector2 textOffset = new Vector2(0, 0.5f); // Смещение спрайта
    public float fadeSpeed = 5f; // Скорость появления

    [Header("Appearance")]
    [TextArea] public string displayText = "Object Info";
    public Color textColor = Color.white;

    private bool _isHovering;
    private float _currentAlpha;

    private void Start()
    {
        // Настройки текста
        if (textComponent != null)
        {
            textComponent.transform.SetParent(transform);
            textComponent.transform.localPosition = textOffset;
            textComponent.text = displayText;
            textComponent.color = new Color(textColor.r, textColor.g, textColor.b, 0);
            textComponent.sortingLayerID = SortingLayer.NameToID("UI"); // Ваш слой
            textComponent.sortingOrder = 10;
        }
        else
        {
            Debug.LogError("TextMeshPro component not assigned!", this);
        }
    }

    private void Update()
    {
        if (textComponent == null) return;

        // Плавное изменение прозрачности
        float targetAlpha = _isHovering ? 1f : 0f;
        _currentAlpha = Mathf.MoveTowards(_currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);

        // Применяем прозрачность
        Color newColor = textComponent.color;
        newColor.a = _currentAlpha;
        textComponent.color = newColor;
    }

    private void OnMouseEnter()
    {
        _isHovering = true;
    }

    private void OnMouseExit()
    {
        _isHovering = false;
    }
}
