using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private ContextMenu contextMenu;
    [SerializeField] private scr_Player_Main player;
    [SerializeField] private LayerMask interactableLayer; // ���� ��� ��������������

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ����� ����
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ���������, ��� �� ���� �� ������� � ����������� IInteractable
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayer);

            if (hit.collider != null)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    contextMenu.ShowMenu(mousePosition, interactable, player.transform.position, player); // ���������� ���� �� ����� �����
                }
            }
            else
            {
                // ���� ���� ��� ��� �������, ��������� ����
                contextMenu.CloseMenu(player);
            }
        }
    }
}