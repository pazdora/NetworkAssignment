using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public NetworkVariable<int> playerScore = new NetworkVariable<int>();
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore.Value.ToString();
        }
    }

    public void AddScore(int amount)
    {
        if (IsServer)
        {
            playerScore.Value += amount;
            UpdateScoreTextClientRpc(playerScore.Value);
        }
    }

    [ClientRpc]
    private void UpdateScoreTextClientRpc(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore.Value.ToString();
        }
    }
}
