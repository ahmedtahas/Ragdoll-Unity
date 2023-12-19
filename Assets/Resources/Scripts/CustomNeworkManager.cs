using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : NetworkManager
{
    public Vector3 serverSpawnPoint = new Vector3(-100, 0, 0);
    public Vector3 clientSpawnPoint = new Vector3(100, 0, 0);



    private void OnEnabled()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisabled()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("Client connected: " + clientId);
        CharacterManager characterManager = GameManager.Instance.playerTransform.GetComponent<CharacterManager>();
        characterManager.selection.Value = PlayerPrefs.GetString(Constants.SELECTED_CHARACTER) switch
        {
            Constants.CHRONOPEN => 1,
            Constants.HOLSTAR => 2,
            Constants.STELE => 3,
            Constants.PUGILSE => 4,
            Constants.ROOT => 5,
            Constants.TIN => 6,
            _ => 0
        };
    }
}