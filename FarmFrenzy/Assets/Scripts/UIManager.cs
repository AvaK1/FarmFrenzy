using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    private bool timePaused;

    private void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void OnExitButton()
    {
        GameManager.Instance.ExitGame();
    }

    public void OnSettingsButton()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        PauseOrResumeGame(settingsPanel);
    }

    public void PauseOrResumeGame(GameObject panel)
    {
        if (panel.activeSelf)
        {
            Time.timeScale = 0.0f;
            timePaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            timePaused = false;
        }
    }

    public void GoToTitle()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StartScene");
    }
}
