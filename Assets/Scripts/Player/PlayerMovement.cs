using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    Rigidbody rb;
    Hotkeys hotkeys;
    Vector2 moveVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        hotkeys = Hotkeys.Instance;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveVector = hotkeys.moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y) * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }


}