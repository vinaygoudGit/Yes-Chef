using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Intro, Playing, Paused, GameOver }

    public static GameManager Instance { get; private set; }
    public static event Action OnReady;

    [SerializeField] private float gameDuration = 180f; // 3 minutes

    public GameState CurrentState { get; private set; }
    public float TimeRemaining { get; private set; }

    public event Action<GameState> OnStateChanged;
    public event Action<float> OnTimerTick;

    private void Awake()
    {
        Instance = this;
        SetState(GameState.Intro);
        OnReady?.Invoke();
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing)
            return;

        TimeRemaining -= Time.deltaTime;
        OnTimerTick?.Invoke(TimeRemaining);

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            SetState(GameState.GameOver);
        }
    }

    public void StartGame()
    {
        ScoreManager.Instance.ResetForNewGame();
        TimeRemaining = gameDuration;
        SetState(GameState.Playing);
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
            SetState(GameState.Paused);
        else if (CurrentState == GameState.Paused)
            SetState(GameState.Playing);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        Time.timeScale = newState == GameState.Playing ? 1f : 0f;
        Debug.Log(newState);
        OnStateChanged?.Invoke(newState);
    }
}