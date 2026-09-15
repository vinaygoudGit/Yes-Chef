using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Singleton wrapper around the generated PlayerInputActions asset. Owns the
/// lifetime of the input actions and exposes them to other scripts. Fires
/// OnReady once actions are assigned.
/// </summary>
public class Hotkeys : MonoBehaviour
{
    public static Hotkeys Instance;
    private PlayerInputActions playerInput;
    private InputActionMap playerActionMap;
    public static event Action OnReady;

    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction interactAction;
    [HideInInspector] public InputAction pauseAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        AssignKeys();

        OnReady?.Invoke();
    }

    private void AssignKeys()
    {
        playerInput = new PlayerInputActions();
        playerActionMap = playerInput.asset.FindActionMap("Player");
        if (playerActionMap == null)
        {
            Debug.LogError("Could not find action map 'Player'. Check the map name matches the asset.");
            return;
        }
        moveAction = playerActionMap.FindAction("Movement");
        interactAction = playerActionMap.FindAction("Interact");
        pauseAction = playerActionMap.FindAction("Pause");

        if (interactAction == null)
            Debug.LogError("Could not find action 'Interact' on the Player action map.");
    }

    private void OnEnable()
    {
        if (playerInput == null) return;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        if (playerInput == null) return;
        playerInput.Disable();
    }
}