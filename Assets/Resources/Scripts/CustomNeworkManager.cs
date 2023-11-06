using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : NetworkBehaviour
{
    public Vector3 serverSpawnPoint = new Vector3(-100, 0, 0);
    public Vector3 clientSpawnPoint = new Vector3(100, 0, 0);

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += ServerStarted;
        // NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }

    private void ServerStarted()
    {
        if (IsHost)
        {
            // Instantiate a new player prefab at the server spawn point
            GameObject player = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, serverSpawnPoint, Quaternion.identity);
            player.name = "Server Player";
            
            // Call this to make the player object a networked object
            NetworkObject networkObject = player.GetComponent<NetworkObject>();
            if (networkObject)
            {
                networkObject.Spawn();
            }
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allGameObjects)
            {
                // If the GameObject's name is "Character (Clone)", destroy it
                if (obj.name == "Character(Clone)")
                {
                    Destroy(obj);
                }
            }
        }
    }

    // private void ClientConnected(ulong clientId)
    // {
    //     Debug.Log("Client connected--------------------------------------------------");
    //     if (IsClient && !IsHost && NetworkManager.Singleton.LocalClientId == clientId)
    //     {
    //         Debug.Log("Client connected");
    //         // Instantiate a new player prefab at the client spawn point
    //         // GameObject player = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, clientSpawnPoint, Quaternion.identity);
    //         // player.name = "Client Player";
    //         // // Call this to make the player object a networked object
    //         // NetworkObject networkObject = player.GetComponent<NetworkObject>();
    //         // if (networkObject)
    //         // {
    //         //     networkObject.Spawn();
    //         // }
            
    //         GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
    //         foreach (GameObject obj in allGameObjects)
    //         {
    //             // If the GameObject's name is "Character (Clone)", destroy it
    //             if (obj.name == "Character(Clone)")
    //             {
    //                 Destroy(obj);
    //             }
    //         }
    //     }
    // }
}