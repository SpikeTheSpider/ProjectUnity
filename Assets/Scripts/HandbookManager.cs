using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HandbookManager : MonoBehaviour
    {
        [System.Serializable]
        public class HandbookEntry
        {
            public string title;
            [TextArea(3, 10)]
            public string description;
        }

        [SerializeField] private GameObject book; // Объект окна
        public HandbookEntry[] entries;
        public GameObject handbookPanel;
        public TMP_Text titleText;
        public TMP_Text descriptionText;
        public GameObject entryButtonPrefab;
        public Transform buttonsParent;

        private void Start()
        {
            GenerateButtons();
            handbookPanel.SetActive(false);
        }

        void GenerateButtons()
        {
            foreach (var entry in entries)
            {
                // Создаем локальную копию entry для каждой итерации
                var currentEntry = entry;

                GameObject buttonObj = Instantiate(entryButtonPrefab, buttonsParent);
                buttonObj.GetComponentInChildren<TMP_Text>().text = currentEntry.title;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => ShowEntry(currentEntry));
            }
        }

        public void ShowEntry(HandbookEntry entry)
        {
            titleText.text = entry.title;
            descriptionText.text = entry.description;
        }

        public void ToggleHandbook()
        {
            handbookPanel.SetActive(!handbookPanel.activeSelf);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                ToggleHandbook();
            }
        }
    }
}
