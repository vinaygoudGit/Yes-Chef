using System;

/// <summary>
/// A shared, global broadcast channel. Scripts that cause something (like CustomerWindow)
/// raise an event here; scripts that care about it (like a future ScoreManager) subscribe
/// to it. Neither side needs a direct reference to the other.
/// </summary>
public static class GameEvents
{
    public static event Action<int> OnOrderCompleted;
    public static event Action<CustomerWindow, Order> OnOrderSpawned;

    public static void RaiseOrderCompleted(int score) => OnOrderCompleted?.Invoke(score);
    public static void RaiseOrderSpawned(CustomerWindow window, Order order) => OnOrderSpawned?.Invoke(window, order);
}
