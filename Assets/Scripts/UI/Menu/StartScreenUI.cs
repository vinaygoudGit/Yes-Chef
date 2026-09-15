using UnityEngine;
using UnityEngine.UI;

public class StartScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Button startButton;

    private bool isSubscribed = false;

    private void Awake()
    {
        panelRoot.SetActive(true);
        startButton.onClick.AddListener(HandleStartClicked);
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
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (isSubscribed) return;

        GameManager.Instance.OnStateChanged += HandleGameStateChanged;
        isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (GameManager.Instance != null && isSubscribed)
            GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
        isSubscribed = false;
    }

    private void HandleStartClicked()
    {
        GameManager.Instance.StartGame();
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        panelRoot.SetActive(state == GameManager.GameState.Intro);
    }
}