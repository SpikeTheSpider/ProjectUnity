using Assets.Scripts.Player;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] private float closeDelay = 0.5f; // Задержка перед закрытием меню
    [SerializeField] private float interactionRadius = 3f; // Радиус взаимодействия
    [SerializeField] private float interactionCooldown = 3f; // Кулдаун для вызова меню

    private IInteractable currentInteractable;
    private float lastInteractionTime = -Mathf.Infinity; // Время последнего взаимодействия

    public void ShowMenu(Vector2 position, IInteractable interactable, Vector2 playerPosition, IPlayer player)
    {
        // Проверяем кулдаун
        if (Time.time < lastInteractionTime + interactionCooldown)
        {
            Debug.Log("Interaction is on cooldown!");
            return;
        }

        if (player.IsInteracting)
        {
            Debug.Log("Player is already interacting!");
            return;
        }

        // Проверяем расстояние между игроком и объектом
        if (Vector2.Distance(position, playerPosition) > interactionRadius)
        {
            Debug.Log("Object is too far away!");
            return;
        }

        // Устанавливаем позицию меню с учетом оси Z
        transform.position = new Vector3(position.x, position.y, -2);
        currentInteractable = interactable;
        gameObject.SetActive(true); // Показываем меню

        player.IsInteracting = true; // Блокируем взаимодействие
    }

    public void CloseMenu(IPlayer player)
    {
        gameObject.SetActive(false); // Скрываем меню
        currentInteractable = null;

        player.IsInteracting = false; // Снимаем блокировку
    }

    private void TriggerCooldown()
    {
        lastInteractionTime = Time.time; // Записываем время последнего взаимодействия
    }

    public void OnInfoButtonClicked()
    {
        if (currentInteractable != null)
        {
            currentInteractable.ShowInfo();
            TriggerCooldown(); // Активируем кулдаун
        }
        Invoke(nameof(CloseMenu), closeDelay); // Закрываем меню с задержкой
    }

    public void OnInteractButtonClicked()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
            TriggerCooldown(); // Активируем кулдаун
        }
        Invoke(nameof(CloseMenu), closeDelay); // Закрываем меню с задержкой
    }

    public void OnHackButtonClicked()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Hack();
            TriggerCooldown(); // Активируем кулдаун
        }
        Invoke(nameof(CloseMenu), closeDelay); // Закрываем меню с задержкой
    }
}