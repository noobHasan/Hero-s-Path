using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    public bool isGameOver;

    private void Awake()
    {
        instance = this;
    }

    public void WinGame()
    {
        if(isGameOver) return;

        isGameOver = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;

    }

    public void GameOver()
    {
        if(isGameOver) return;

        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
