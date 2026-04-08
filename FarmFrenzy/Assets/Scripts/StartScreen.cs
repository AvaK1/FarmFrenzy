using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : UIManager
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
