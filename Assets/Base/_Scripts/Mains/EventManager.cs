using System;
using System.Collections.Generic;

public enum GameEvent
{
    OnLevelStarted, OnLevelEnded, OnCoinCollected, OnDiamondCollected, NewWeaponInitiated
}

public static class EventManager
{
    private static Dictionary<GameEvent, Action> events = new Dictionary<GameEvent, Action>();

    public static void AddListener(GameEvent gameEvent, Action action)
    {
        if (!events.ContainsKey(gameEvent))
            events[gameEvent] = action;
        else
            events[gameEvent] += action;
    }

    public static void RemoveListener(GameEvent gameEvent, Action action)
    {
        if (events[gameEvent] != null)
            events[gameEvent] -= action;
        if (events[gameEvent] == null)
            events.Remove(gameEvent);
    }

    public static void Brodcast(GameEvent gameEvent)
    {
        if (events[gameEvent] != null)
            events[gameEvent]();
    }
}