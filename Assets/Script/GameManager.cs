using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    private PlayerHealth player;
    private PrincessHealth princess;

    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        player = FindObjectOfType<PlayerHealth>();
        princess = FindObjectOfType<PrincessHealth>();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (gameWinUI != null)
            gameWinUI.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        if (player != null && player.IsDead)
        {
            GameOver();
        }

        if (princess != null && princess.IsDead)
        {
            GameOver();
        }
    }

    public void GameWin()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (gameWinUI != null)
            gameWinUI.SetActive(true);

        Time.timeScale = 0f;
    }

    void GameOver()
    {
        gameEnded = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    
}