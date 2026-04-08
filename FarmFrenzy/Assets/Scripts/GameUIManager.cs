using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : UIManager
{
    public static GameUIManager Instance;
    private InputSystem_Actions input;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text timeText, healthText, pestsKilledText;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        GameManager.Instance.ResetGame();
        input = new InputSystem_Actions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameOverPanel.SetActive(false);
        GameManager.Instance.startTime = Time.time;
        //startTime = Time.time;
        pestsKilledText.text = "Pests killed: " + GameManager.Instance.pestsKilled.ToString();
        healthText.text = "Health: " + playerController.playerHealth.ToString();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Player.Pause.WasPressedThisFrame())
        {
            OnSettingsButton();
        }

        System.TimeSpan time = System.TimeSpan.FromSeconds(Time.time - GameManager.Instance.startTime);
        timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }

    public void DisplayGameOver()
    {
        //gameOverPanel.SetActive(true);
        //PauseOrResumeGame(gameOverPanel);
        SceneManager.LoadScene("EndScene");
    }

    public void UpdatePestsAndHealth()
    {
        pestsKilledText.text = "Pests killed: " + GameManager.Instance.pestsKilled.ToString();
        healthText.text = "Health: " + playerController.playerHealth.ToString();
    }
}
