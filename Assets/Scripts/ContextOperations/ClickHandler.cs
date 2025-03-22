using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private ContextMenu contextMenu;
    [SerializeField] private scr_Player_Main player;
    [SerializeField] private LayerMask interactableLayer; // Слой для взаимодействия

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Левый клик
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Проверяем, был ли клик на объекте с интерфейсом IInteractable
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayer);

            if (hit.collider != null)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    contextMenu.ShowMenu(mousePosition, interactable, player.transform.position, player); // Показываем меню на месте клика
                }
            }
            else
            {
                // Если клик был вне объекта, закрываем меню
                contextMenu.CloseMenu(player);
            }
        }
    }
}