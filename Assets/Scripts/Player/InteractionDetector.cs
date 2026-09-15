using System;
using UnityEngine;

/// <summary>
/// Sits on a trigger collider near the player. Its only job is to know
/// which IInteractable (if any) is currently in range and notify listeners
/// when that changes. It has no knowledge of input or UI.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;

    public IInteractable CurrentTarget { get; private set; }

    public event Action<IInteractable> OnTargetChanged;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayer(other)) return;

        if (other.TryGetComponent(out IInteractable interactable))
            SetTarget(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayer(other)) return;

        if (other.TryGetComponent(out IInteractable interactable) && ReferenceEquals(interactable, CurrentTarget))
            SetTarget(null);
    }

    private bool IsInLayer(Collider other)
    {
        return (interactableLayer.value & (1 << other.gameObject.layer)) != 0;
    }

    private void SetTarget(IInteractable target)
    {
        if (ReferenceEquals(CurrentTarget, target)) return;

        CurrentTarget = target;
        OnTargetChanged?.Invoke(CurrentTarget);
    }
}
