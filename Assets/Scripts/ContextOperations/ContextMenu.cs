using Assets.Scripts.Player;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] private float closeDelay = 0.5f; // �������� ����� ��������� ����
    [SerializeField] private float interactionRadius = 3f; // ������ ��������������
    [SerializeField] private float interactionCooldown = 3f; // ������� ��� ������ ����

    private IInteractable currentInteractable;
    private float lastInteractionTime = -Mathf.Infinity; // ����� ���������� ��������������

    public void ShowMenu(Vector2 position, IInteractable interactable, Vector2 playerPosition, IPlayer player)
    {
        // ��������� �������
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

        // ��������� ���������� ����� ������� � ��������
        if (Vector2.Distance(position, playerPosition) > interactionRadius)
        {
            Debug.Log("Object is too far away!");
            return;
        }

        // ������������� ������� ���� � ������ ��� Z
        transform.position = new Vector3(position.x, position.y, -2);
        currentInteractable = interactable;
        gameObject.SetActive(true); // ���������� ����

        player.IsInteracting = true; // ��������� ��������������
    }

    public void CloseMenu(IPlayer player)
    {
        gameObject.SetActive(false); // �������� ����
        currentInteractable = null;

        player.IsInteracting = false; // ������� ����������
    }

    private void TriggerCooldown()
    {
        lastInteractionTime = Time.time; // ���������� ����� ���������� ��������������
    }

    public void OnInfoButtonClicked()
    {
        if (currentInteractable != null)
        {
            currentInteractable.ShowInfo();
            TriggerCooldown(); // ���������� �������
        }
        Invoke(nameof(CloseMenu), closeDelay); // ��������� ���� � ���������
    }

    public void OnInteractButtonClicked()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
            TriggerCooldown(); // ���������� �������
        }
        Invoke(nameof(CloseMenu), closeDelay); // ��������� ���� � ���������
    }

    public void OnHackButtonClicked()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Hack();
            TriggerCooldown(); // ���������� �������
        }
        Invoke(nameof(CloseMenu), closeDelay); // ��������� ���� � ���������
    }
}