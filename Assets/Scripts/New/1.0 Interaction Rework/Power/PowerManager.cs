using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance { get; private set; }

    [Header("Default Circuits")]
    [SerializeField] private List<PowerCircuit> defaultCircuits = new();

    private readonly Dictionary<string, bool> circuits = new();

    public event Action<string, bool> OnPowerChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (PowerCircuit circuit in defaultCircuits)
        {
            if (string.IsNullOrWhiteSpace(circuit.circuitName))
            {
                Debug.LogWarning("PowerManager contains a circuit with no name.", this);
                continue;
            }

            if (circuits.ContainsKey(circuit.circuitName))
            {
                Debug.LogWarning($"Duplicate power circuit '{circuit.circuitName}' found.", this);
                continue;
            }

            circuits.Add(circuit.circuitName, circuit.powered);
        }
    }

    public bool HasCircuit(string circuitName)
    {
        return circuits.ContainsKey(circuitName);
    }

    public bool IsPowered(string circuitName)
    {
        if (circuits.TryGetValue(circuitName, out bool powered))
        {
            return powered;
        }

        Debug.LogWarning($"Power circuit '{circuitName}' does not exist.");
        return false;
    }

    public void EnablePower(string circuitName)
    {
        SetPower(circuitName, true);
    }

    public void DisablePower(string circuitName)
    {
        SetPower(circuitName, false);
    }

    public void TogglePower(string circuitName)
    {
        if (!circuits.ContainsKey(circuitName))
        {
            Debug.LogWarning($"Power circuit '{circuitName}' does not exist.");
            return;
        }

        SetPower(circuitName, !circuits[circuitName]);
    }

    private void SetPower(string circuitName, bool powered)
    {
        if (!circuits.ContainsKey(circuitName))
        {
            Debug.LogWarning($"Power circuit '{circuitName}' does not exist.");
            return;
        }

        if (circuits[circuitName] == powered)
        {
            return;
        }

        circuits[circuitName] = powered;

        Debug.Log($"Power circuit '{circuitName}' is now {(powered ? "ON" : "OFF")}");

        OnPowerChanged?.Invoke(circuitName, powered);
    }
}
