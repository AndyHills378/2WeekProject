using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerRespawn : NetworkBehaviour
{
    private GameObject[] redSpawns;
    private GameObject[] blueSpawns;
    // private float respawnTime = 5f; //TODO: Add this in another time

    private int redNextIndex;
    private int blueNextIndex;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]

    public void Start()
    {
        redSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
        blueSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");
    }

    [Client]
    public void RespawnPlayer(GameObject player, string teamColour)
    {
        RpcRespawnPlayer(player, teamColour);
    }
    [ClientRpc]
    public void RpcRespawnPlayer(GameObject player, string teamColour)
    {
        if (teamColour == "Blue Team")
        {
            player.transform.position = redSpawns[redNextIndex].transform.position;
            redNextIndex += (redNextIndex + 1) % redSpawns.Length;
        }
        if (teamColour == "Red Team")
        {
            player.transform.position = blueSpawns[blueNextIndex].transform.position;
            blueNextIndex += (redNextIndex + 1) % blueSpawns.Length;
        }
    }
}
