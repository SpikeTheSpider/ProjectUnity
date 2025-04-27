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

        [SerializeField] private GameObject book;
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
            // Проверяем, есть ли уже открытые окна с тегом "Window"
            GameObject[] windows = GameObject.FindGameObjectsWithTag("Window");
            foreach (GameObject window in windows)
            {
                if (window != handbookPanel && window.activeSelf)
                {
                    // Найдено другое активное окно - не открываем справочник
                    return;
                }
            }

            // Переключаем состояние справочника
            bool activate = !handbookPanel.activeSelf;
            handbookPanel.SetActive(activate);
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