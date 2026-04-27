using System.Collections.Generic;
using UnityEngine;


public enum WorldObjectState
{
    Closed,
    Open
}

public class WorldState : MonoBehaviour
{
    public static WorldState Instance;

    private Dictionary<string, WorldObjectState> states = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(string id, WorldObjectState state)
    {
        states[id] = state;
    }

    public WorldObjectState GetState(string id)
    {
        if (states.TryGetValue(id, out var state))
            return state;

        return WorldObjectState.Closed;
    }
}
