using UnityEngine;

/// <summary>
/// Rotates this object (the player's visual model) to face the current movement direction.
/// Lives entirely separately from PlayerMovement - this only concerns itself with "which way
/// am I facing," never with actual position/velocity.
/// </summary>
public class PlayerFaceRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;

    private Hotkeys hotkeys;

    private void Start()
    {
        hotkeys = Hotkeys.Instance;
    }

    private void Update()
    {
        Vector2 moveInput = hotkeys.moveAction.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude < 0.01f)
            return;

        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
