using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public int networkId;

    [SerializeField] private Image damageImage;

    public bool playerDead;
    private int scoreBonus = 130;

    public GameObject[] playerInstances;
    [HideInInspector] public PlayerSpawnSystem spawnSystem;

    [ClientCallback]
    public void Start()
    {
        playerInstances = GameObject.FindGameObjectsWithTag("GamePlayer");
        spawnSystem = GameObject.FindGameObjectWithTag("SpawnSystem").GetComponent<PlayerSpawnSystem>();
        networkId = connectionToClient.connectionId;
        playerDead = false;
    }

    [Client]
    public void AdjustKills()
    {
        CmdAdjustKills();
    }
    [Command]
    public void CmdAdjustKills()
    {
        RpcAdjustKills();
    }

    [ClientRpc]
    public void RpcAdjustKills()
    {
        for (int i = 0; i < playerInstances.Length; i++)
        {
            if (playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerID == networkId)
            {
                playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerKills++;
                playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerScore += scoreBonus;
                GetComponent<PlayerScoreManager>().UpdateTeamSliders(this.gameObject.tag);
            }
        }
    }

    [Client]
    public void AdjustDeaths()
    {
        CmdAdjustDeaths();
    }

    [Command(requiresAuthority =false)]
    public void CmdAdjustDeaths()
    {
        RpcAdjustDeaths();
    }

    [ClientRpc]
    public void RpcAdjustDeaths()
    {
        for (int i = 0; i < playerInstances.Length; i++)
        {
            if (playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerID == networkId)
            {
                playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerDeaths++;
                string tag = this.gameObject.tag;
                var conn = this.connectionToClient;
                spawnSystem.OnPlayerKilled(this.gameObject, tag, conn);
            }
        }
    }

    [Client]
    public bool AdjustHealth(int damage)
    {
        damageImage.gameObject.SetActive(true);
        CmdAdjustHealth(damage);
        damageImage.gameObject.SetActive(false);
        return playerDead;
    }

    [Command(requiresAuthority =false)]
    public void CmdAdjustHealth(int damage)
    {
        RpcAdjustHealth(damage);
    }

    [ClientRpc]
    public void RpcAdjustHealth(int damage)
    {
        for (int i = 0; i < playerInstances.Length; i++)
        {
            if (playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerID == networkId)
            {
                playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerHealth -= damage;
                if (playerInstances[i].GetComponent<NetworkGamePlayerLobby>().playerHealth <= 0)
                {
                    playerDead = true;
                }
            }
        }
    }
}
    /*[Client]
    public void AdjustAssists(int assists)
    {
        CmdAdjustAssists();
    }
    [Command]
    public void CmdAdjustAssists()
    {
        RpcAdjustAssists();
    }
    [ClientRpc]
    public void RpcAdjustAssists()
    {

    }*/