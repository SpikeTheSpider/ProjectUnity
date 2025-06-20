using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Hub");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
