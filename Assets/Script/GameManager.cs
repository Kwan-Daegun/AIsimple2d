using UnityEngine;

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
}