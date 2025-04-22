using Assets.Scripts.Player;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private ContextMenu contextMenu;
    [SerializeField] private MonoBehaviour player;
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
                    contextMenu.ShowMenu(mousePosition, interactable, player.transform.position, (IPlayer)player); // ���������� ���� �� ����� �����
                }
            }
            else
            {
                // ���� ���� ��� ��� �������, ��������� ����
                contextMenu.CloseMenu((IPlayer)player);
            }
        }
    }
}