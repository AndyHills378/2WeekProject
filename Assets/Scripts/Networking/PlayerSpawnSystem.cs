using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject[] playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;

    public void OnPlayerKilled(GameObject player, string tag, NetworkConnection conn) => RespawnPlayer(player, tag, conn);

    [Server]
    public void RespawnPlayer(GameObject player, string tag, NetworkConnection conn)
    {
        if (spawnPoints[nextIndex].tag == tag)
        {
            player.transform.position = spawnPoints[nextIndex].transform.position;
            player.transform.rotation = spawnPoints[nextIndex].transform.rotation;
            conn.identity.gameObject.GetComponent<NetworkGamePlayerLobby>().playerHealth = 100;
            player.GetComponent<PlayerManager>().playerDead = false;
            RpcRespawnPlayer(player);
        }
        else
        {
            nextIndex = (nextIndex + 1) % spawnPoints.Count;
            RespawnPlayer(player, tag, conn);
        }
    }

    [ClientRpc]
    public void RpcRespawnPlayer(GameObject player)
    {
        player.transform.position = spawnPoints[nextIndex].transform.position;
        player.transform.rotation = spawnPoints[nextIndex].transform.rotation;
        player.GetComponent<PlayerManager>().playerDead = false;
    }

    [ServerCallback]
    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if (spawnPoint = null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab[nextIndex], spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
        NetworkServer.Spawn(playerInstance, conn);
        nextIndex = (nextIndex + 1) % playerPrefab.Length;

    }
}
