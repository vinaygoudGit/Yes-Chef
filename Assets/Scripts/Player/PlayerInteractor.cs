using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
    [SerializeField] private PlayerInventory playerInventory;

    private Hotkeys hotkeys;
    private bool isSubscribed = false;

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
        hotkeys.interactAction.performed += Interact;
        isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (hotkeys != null && isSubscribed)
            hotkeys.interactAction.performed -= Interact;
        isSubscribed = false;
    }

    private void Interact(InputAction.CallbackContext callbackContext)
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        // Block interacting with anything else while the ingredient picker is open.
        if (IngredientSelectionUI.Instance != null && IngredientSelectionUI.Instance.IsOpen)
            return;

        detector.CurrentTarget?.Interact(playerInventory);
    }
}