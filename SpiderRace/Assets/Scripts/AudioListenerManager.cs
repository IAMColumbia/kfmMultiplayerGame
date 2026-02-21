using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AudioListenerManager : MonoBehaviour
{
    private readonly List<PlayerInput> players = new();

    public void OnPlayerJoined(PlayerInput player)
    {
        if (!players.Contains(player)) players.Add(player);
        UpdateListeners();
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        players.Remove(player);
        UpdateListeners();
    }

    void UpdateListeners()
    {
        for (int i = 0; i < players.Count; i++)
        {
            var listener = players[i].GetComponentInChildren<AudioListener>(true);
            if (listener != null) listener.enabled = (i == 0);
        }
    }
}
