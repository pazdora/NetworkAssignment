using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private TMP_InputField chatInputField;
    [SerializeField] private TextMeshProUGUI chatDisplay;


    private void Start()
    {
        chatInputField.onEndEdit.AddListener(OnSendChatMessage);
    }

    private void OnDestroy()
    {
        chatInputField.onEndEdit.RemoveListener(OnSendChatMessage);
    }

    public void OnSendChatMessage(string message)
    {
        if (!IsOwner || string.IsNullOrWhiteSpace(message)) return;

        SendChatMessageServerRpc(message);
        chatInputField.text = "";
    }

    [ServerRpc]
    private void SendChatMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        DisplayChatMessageClientRpc(message);
    }

    [ClientRpc]
    private void DisplayChatMessageClientRpc(string message)
    {
      chatDisplay.text += "\n" + message;
    }
}
