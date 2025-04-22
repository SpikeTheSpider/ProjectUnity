using Assets.Scripts.Player;
using System;
using UnityEngine;

[Serializable]
public class PlayerIsometricMovement : MonoBehaviour, IPlayer
{
    #region Serialized
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private ContextMenu contextMenu; // Ссылка на контекстное меню
    [SerializeField] private LayerMask interactableLayer; // Слой для взаимодействия
    [SerializeField] private float interactionRadius = 3f; // Радиус взаимодействия
    #endregion

    private Rigidbody2D rb;
    private float moveH, moveV;

    #region Interaction
    private float interactionCooldown = 3f; // Кулдаун для взаимодействия
    private float lastInteractionTime = -Mathf.Infinity; // Время последнего взаимодействия
    public bool IsInteracting { get; set; }
    #endregion 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveH = Input.GetAxis("Horizontal") * moveSpeed;
        moveV = Input.GetAxis("Vertical") * moveSpeed;
        rb.linearVelocity = new Vector2(moveH, moveV); // OPTIONAL: rb.MovePosition();
    }

    private void Update()
    {
        if (!IsInteracting)
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        // Проверяем нажатие клавиши "E" и кулдаун
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastInteractionTime + interactionCooldown)
        {
            // Проверяем объекты в радиусе взаимодействия
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
            foreach (var hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    lastInteractionTime = Time.time; // Записываем время последнего взаимодействия
                    break; // Взаимодействуем только с первым найденным объектом
                }
            }
        }
    }
}