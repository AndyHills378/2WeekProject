using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class NetworkGamePlayerLobby : NetworkBehaviour
{

    [SyncVar]
    public string displayName = "Loading...";
    [SyncVar]
    public int playerScore;
    [SyncVar]
    public int playerKills;
    [SyncVar]
    public int playerDeaths;
    [SyncVar]
    public int playerAssists;
    [SyncVar]
    public int playerHealth;
    [SyncVar]
    public int playerID;

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        this.playerID = connectionToClient.connectionId;
        this.playerScore = 0;
        this.playerKills = 0;
        this.playerDeaths = 0;
        this.playerAssists = 0;
        this.playerHealth = 100;
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
