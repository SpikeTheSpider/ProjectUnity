using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 1.0f;

    [SerializeField] private ContextMenu contextMenu; // Ссылка на контекстное меню
    [SerializeField] private LayerMask interactableLayer; // Слой для взаимодействия
    [SerializeField] private float interactionRadius = 3f; // Радиус взаимодействия

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
        // Вызов контекстного меню при клике
        if (Input.GetMouseButtonDown(0)) // Левый клик
        {
            //HandleClick();
        }

        // Взаимодействие по клавише "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    //private void HandleClick()
    //{
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    // Проверяем, был ли клик на объекте с интерфейсом IInteractable
    //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayer);

    //    if (hit.collider != null)
    //    {
    //        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
    //        if (interactable != null)
    //        {
    //            // Проверяем расстояние между игроком и объектом
    //            if (Vector2.Distance(transform.position, hit.collider.transform.position) <= interactionRadius)
    //            {
    //                contextMenu.ShowMenu(mousePosition, interactable, transform.position, this); // Показываем меню
    //            }
    //            else
    //            {
    //                Debug.Log("Object is too far away!");
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // Если клик был вне объекта, закрываем меню
    //        contextMenu.CloseMenu(this);
    //    }
    //}

    private float interactionCooldown = 3f; // Кулдаун для взаимодействия
    private float lastInteractionTime = -Mathf.Infinity; // Время последнего взаимодействия

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