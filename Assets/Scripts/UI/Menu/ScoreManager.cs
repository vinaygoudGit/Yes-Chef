using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static event Action OnReady;

    private const string HighScoreKey = "YesChef_HighScore";

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    public bool IsNewHighScoreThisRun { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnHighScoreChanged;

    private void Awake()
    {
        Instance = this;
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        OnReady?.Invoke();
    }

    private void OnEnable() => GameEvents.OnOrderCompleted += AddScore;
    private void OnDisable() => GameEvents.OnOrderCompleted -= AddScore;

    public void ResetForNewGame()
    {
        CurrentScore = 0;
        IsNewHighScoreThisRun = false;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    private void AddScore(int amount)
    {
        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void FinalizeScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            IsNewHighScoreThisRun = true;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(HighScore);
        }
    }
}