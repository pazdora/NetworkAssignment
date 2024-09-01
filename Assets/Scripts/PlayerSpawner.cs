using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;

    public override void OnNetworkSpawn()
    {
       
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        if (IsOwner)
        {
            Camera.main.transform.SetParent(this.transform);
            Camera.main.transform.localPosition = new Vector3(0, 5, -10);
            Camera.main.transform.localRotation = Quaternion.identity;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

}

