using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SolidOutline : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private Color outlineColor = Color.cyan;
    [SerializeField] private float interactRadius = 3f;
    [SerializeField][Range(1.01f, 1.2f)] private float outlineSize = 1.05f;

    private SpriteRenderer _mainRenderer;
    private SpriteRenderer _outlineRenderer;
    private Transform _playerTransform;
    private bool _isHighlighted;

    private void Awake()
    {
        _mainRenderer = GetComponent<SpriteRenderer>();
        CreateOutline();
    }

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        bool shouldHighlight = distance <= interactRadius;
        bool mouseOver = CheckMouseOver();

        if (shouldHighlight && mouseOver && !_isHighlighted)
        {
            EnableOutline();
        }
        else if ((!shouldHighlight || !mouseOver) && _isHighlighted)
        {
            DisableOutline();
        }
    }

    private bool CheckMouseOver()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        return hit != null && hit.gameObject == gameObject;
    }

    private void CreateOutline()
    {
        // Создаем объект для контура
        GameObject outlineObj = new GameObject("Outline");
        outlineObj.transform.SetParent(transform);
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localRotation = Quaternion.identity;
        outlineObj.transform.localScale = Vector3.one * outlineSize;

        // Настраиваем рендерер контура
        _outlineRenderer = outlineObj.AddComponent<SpriteRenderer>();
        _outlineRenderer.sprite = _mainRenderer.sprite;

        // Важно: используем материал SolidColorMaterial!
        _outlineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        _outlineRenderer.color = outlineColor;

        _outlineRenderer.sortingOrder = _mainRenderer.sortingOrder - 1;
        _outlineRenderer.enabled = false;
    }

    private void EnableOutline()
    {
        _outlineRenderer.enabled = true;
        _isHighlighted = true;
    }

    private void DisableOutline()
    {
        _outlineRenderer.enabled = false;
        _isHighlighted = false;
    }

    private void OnDisable()
    {
        DisableOutline();
    }
}