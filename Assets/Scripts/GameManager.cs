using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverTitleText;
    [SerializeField] private Text gameOverBodyText;
    [SerializeField] private Button restartButton;

    private int score;
    private int totalCollectibles;
    private int collectedCount;
    private bool isGameOver;

    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        UpdateUI();
        HideEndPanel();

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(Restart);
            restartButton.onClick.AddListener(Restart);
        }
    }

    public void Configure(Text score, GameObject endPanel, Text endTitle, Text endBody, Button restart)
    {
        scoreText = score;
        gameOverPanel = endPanel;
        gameOverTitleText = endTitle;
        gameOverBodyText = endBody;
        restartButton = restart;

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(Restart);
            restartButton.onClick.AddListener(Restart);
        }

        UpdateUI();
        HideEndPanel();
    }

    public void RegisterCollectible()
    {
        totalCollectibles++;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver)
        {
            return;
        }

        score += Mathf.Max(0, amount);
        collectedCount++;
        UpdateUI();

        if (totalCollectibles > 0 && collectedCount >= totalCollectibles)
        {
            WinGame();
        }
    }

    public void PlayerDied()
    {
        if (isGameOver)
        {
            return;
        }

        EndGame("Game Over", "You touched an enemy. Press Restart and try again.");
    }

    public void WinGame()
    {
        if (isGameOver)
        {
            return;
        }

        EndGame("You Win!", "All gems collected. Nice run.");
    }

    public int GetScore()
    {
        return score;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score} / {totalCollectibles}";
        }
    }

    private void HideEndPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void EndGame(string title, string body)
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverTitleText != null)
        {
            gameOverTitleText.text = title;
        }

        if (gameOverBodyText != null)
        {
            gameOverBodyText.text = body;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
