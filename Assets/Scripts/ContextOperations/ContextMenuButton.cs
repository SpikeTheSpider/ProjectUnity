using UnityEngine;

public class ContextMenuButton : MonoBehaviour
{
    [SerializeField] private ButtonAction action;
    private ContextMenu contextMenu;

    private void Start()
    {
        contextMenu = GetComponentInParent<ContextMenu>();
    }

    private void OnMouseDown()
    {
        if (contextMenu != null)
        {
            switch (action)
            {
                case ButtonAction.Info:
                    contextMenu.OnInfoButtonClicked();
                    break;
                case ButtonAction.Interact:
                    contextMenu.OnInteractButtonClicked();
                    break;
                case ButtonAction.Hack:
                    contextMenu.OnHackButtonClicked();
                    break;
            }
        }
    }
}