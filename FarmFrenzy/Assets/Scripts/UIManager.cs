using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

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

    public virtual void PauseOrResumeGame(GameObject panel) //stops/resumes gametime
    {
        if (panel.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void GoToTitle()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StartScene");
    }
}
