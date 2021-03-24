using UnityEngine;
using TMPro;
using Mirror;
using System.Collections.Generic;

public class PlayerScoreManager : NetworkBehaviour
{
    public class AllPlayerScores
    {
        public string PlayerName;
        public int PlayerScore;
        public int PlayerKills;
        public int PlayerDeaths;
        public int PlayerAssists;
    }

    [Header("References")]
    [SerializeField] private GameObject[] playerScoreCards;
    [SerializeField] private TMP_Text[] playerNames;
    [SerializeField] private TMP_Text[] playerScores;
    [SerializeField] private TMP_Text[] playerKills;
    [SerializeField] private TMP_Text[] playerDeaths;
    [SerializeField] private TMP_Text[] playerAssists;
    [SerializeField] private GameObject scoreboardCanvas;

    [HideInInspector] public NetworkManagerLobby networkManager;
    [HideInInspector] public GameObject[] allPlayers;

    public SyncList<AllPlayerScores> allPlayerScoreCards = new SyncList<AllPlayerScores>();

    [ClientCallback]
    public void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerLobby>();
        allPlayers = GameObject.FindGameObjectsWithTag("GamePlayer");
        for(int i = 0; i < allPlayers.Length; i++)
        {
            AllPlayerScores playerScore = new AllPlayerScores();
            playerScore.PlayerName = allPlayers[i].GetComponent<NetworkGamePlayerLobby>().displayName;
            playerScore.PlayerScore = allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerScore;
            playerScore.PlayerKills = allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerKills;
            playerScore.PlayerDeaths = allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerDeaths;
            playerScore.PlayerAssists = allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerAssists;
            CmdAddToScoreboard(playerScore);
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [Command]
    public void CmdAddToScoreboard(AllPlayerScores playerScore)
    {
        allPlayerScoreCards.Add(playerScore);
    }

    [Command]
    public void CmdUpdateScoreBoard()
    {
        RpcUpdateScoreBoard();
    }

    [ClientRpc]
    public void RpcUpdateScoreBoard()
    {
        for (int i = 0; i < allPlayerScoreCards.Count; i++)
        {
            playerNames[i].SetText(allPlayerScoreCards[i].PlayerName);
            playerScores[i].SetText(allPlayerScoreCards[i].PlayerScore.ToString());
            playerKills[i].SetText(allPlayerScoreCards[i].PlayerKills.ToString());
            playerDeaths[i].SetText(allPlayerScoreCards[i].PlayerDeaths.ToString());
            playerAssists[i].SetText(allPlayerScoreCards[i].PlayerAssists.ToString());
        }
    }

    [Client]
    public void ShowScoreboard()
    {
        scoreboardCanvas.SetActive(true);
    }

    [Client]
    public void HideScoreboard()
    {
        scoreboardCanvas.SetActive(false);
    }

    [ClientCallback]
    private void Update()
    {
        for(int i = 0; i < allPlayerScoreCards.Count; i++)
        {
            playerScoreCards[i].SetActive(true);
        }

        CmdUpdateScoreBoard();
        if (Input.GetButton("Tab"))
        {
            ShowScoreboard();
        }
        if (Input.GetButtonUp("Tab"))
        {
            HideScoreboard();
        }
    }
}
