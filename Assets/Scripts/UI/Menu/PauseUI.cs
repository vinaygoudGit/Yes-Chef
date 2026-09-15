using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    private Hotkeys hotkeys;
    private bool isSubscribed = false;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(HandleResumeClicked);
        quitButton.onClick.AddListener(HandleQuitClicked);
    }

    private void OnEnable()
    {
        Hotkeys.OnReady += HandleInputReady;

        if (Hotkeys.Instance != null)
            HandleInputReady();
    }

    private void OnDisable()
    {
        Hotkeys.OnReady -= HandleInputReady;
        Unsubscribe();
    }

    private void HandleInputReady()
    {
        if (isSubscribed) return;

        hotkeys = Hotkeys.Instance;
        hotkeys.pauseAction.performed += PauseTheGame;
        isSubscribed = true;
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
        GameManager.Instance.OnStateChanged += HandleGameStateChanged;
    }

    private void Unsubscribe()
    {
        if (hotkeys != null && isSubscribed)
            hotkeys.pauseAction.performed -= PauseTheGame;
        isSubscribed = false;
        GameManager.OnReady -= HandleInputReady;

        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

    private void HandleQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }


    private void PauseTheGame(InputAction.CallbackContext callbackContext)
    {
        GameManager.Instance.TogglePause();
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        pauseUI.SetActive(state == GameManager.GameState.Paused);
    }

}
