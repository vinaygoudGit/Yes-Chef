using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject newHighScoreLabel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        panelRoot.SetActive(false);
        restartButton.onClick.AddListener(HandleRestartClicked);
        quitButton.onClick.AddListener(HandleQuitClicked);
    }

    private void OnEnable()
    {
        GameManager.OnReady += Subscribe;

        if (GameManager.Instance != null)
            Subscribe();
    }

    private void OnDisable()
    {
        GameManager.OnReady -= Subscribe;

        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleGameStateChanged;

        restartButton.onClick.RemoveListener(HandleRestartClicked);
    }

    private void Subscribe()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
        GameManager.Instance.OnStateChanged += HandleGameStateChanged;
    }

    private void HandleRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    private void HandleQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        panelRoot.SetActive(state == GameManager.GameState.GameOver);

        if (state != GameManager.GameState.GameOver)
            return;

        finalScoreText.text = $"Score: {ScoreManager.Instance.CurrentScore}";
        highScoreText.text = $"High Score: {ScoreManager.Instance.HighScore}";
        newHighScoreLabel.SetActive(ScoreManager.Instance.IsNewHighScoreThisRun);
    }
}