using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject hudRoot;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text timerText;

    private bool isSubscribed = false;

    private void OnEnable()
    {
        GameManager.OnReady += TrySubscribe;
        ScoreManager.OnReady += TrySubscribe;
        TrySubscribe();
    }

    private void OnDisable()
    {
        GameManager.OnReady -= TrySubscribe;
        ScoreManager.OnReady -= TrySubscribe;
        Unsubscribe();
    }

    private void TrySubscribe()
    {
        if (isSubscribed) return;
        if (GameManager.Instance == null || ScoreManager.Instance == null) return;

        GameManager.Instance.OnStateChanged += HandleGameStateChanged;
        GameManager.Instance.OnTimerTick += HandleTimerTick;
        ScoreManager.Instance.OnScoreChanged += HandleScoreChanged;
        ScoreManager.Instance.OnHighScoreChanged += HandleHighScoreChanged;
        isSubscribed = true;

        highScoreText.text = $"High Score: {ScoreManager.Instance.HighScore}";
        scoreText.text = "Score: 0";
    }

    private void Unsubscribe()
    {
        if (!isSubscribed) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
            GameManager.Instance.OnTimerTick -= HandleTimerTick;
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= HandleScoreChanged;
            ScoreManager.Instance.OnHighScoreChanged -= HandleHighScoreChanged;
        }

        isSubscribed = false;
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        bool visible = state == GameManager.GameState.Playing || state == GameManager.GameState.Paused;
        hudRoot.SetActive(visible);
    }

    private void HandleTimerTick(float timeRemaining)
    {
        int totalSeconds = Mathf.CeilToInt(timeRemaining);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerText.text = $"{minutes}:{seconds:00}";
    }

    private void HandleScoreChanged(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }

    private void HandleHighScoreChanged(int newHighScore)
    {
        highScoreText.text = $"High Score: {newHighScore}";
    }
}