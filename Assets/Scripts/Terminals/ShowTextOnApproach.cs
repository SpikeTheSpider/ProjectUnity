using UnityEngine;

public class ShowPanelOnApproach : MonoBehaviour
{
    [SerializeField] private TextAsset infoFile;

    public Transform player;
    public float activationDistance = 3f;
    public InfoWindow infoPanel;
    public KeyCode interactKey = KeyCode.E;

    private bool wasShownBefore = false;
    private bool isPlayerInRange = false;
    private static InfoWindow currentActivePanel;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
            else Debug.LogError("Player not found!");
        }

        if (infoPanel != null) infoPanel.Hide();
    }

    void Update()
    {
        if (player == null || infoPanel == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        isPlayerInRange = distance <= activationDistance;

        // (1) Автоматический показ при первом приближении
        if (isPlayerInRange && !wasShownBefore)
        {
            ShowPanel();
            wasShownBefore = true;
        }

        // (2) Открытие/закрытие по нажатию E
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            if (currentActivePanel == infoPanel && infoPanel.isActiveAndEnabled)
            {
                infoPanel.Hide();
                currentActivePanel = null;
            }
            else
            {
                ShowPanel();
            }
        }

        // (3) Автоматическое закрытие при отдалении
        if (!isPlayerInRange && currentActivePanel == infoPanel)
        {
            infoPanel.Hide();
            currentActivePanel = null;
        }
    }

    private void ShowPanel()
    {
        if (currentActivePanel != null && currentActivePanel != infoPanel)
        {
            currentActivePanel.Hide(); // Закрываем другую открытую панель
        }

        infoPanel.Show(infoFile.text);
        currentActivePanel = infoPanel;
    }
}