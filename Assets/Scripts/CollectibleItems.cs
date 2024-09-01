using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollectibleItems : NetworkBehaviour
{
    private bool isCollected;
   private void OnTriggerEnter(Collider collision)
    {
        if(!IsServer) return;

        if (collision.CompareTag("Player"))
        {
            ulong clientId = gameObject.GetComponent<NetworkObject>().OwnerClientId;
            CollectServerRpc(clientId);
        }
    }

    [ServerRpc]
    private void CollectServerRpc(ulong clientId)
    {
        if (isCollected) return;
        isCollected = true;
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.AddScore(5);
        }

        Destroy(gameObject);
    }
}
