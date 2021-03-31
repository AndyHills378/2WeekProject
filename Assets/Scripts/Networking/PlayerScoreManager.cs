using UnityEngine;
using TMPro;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerScoreManager : NetworkBehaviour
{
    public class AllPlayerScores
    {
        public string PlayerName;
        public int PlayerScore;
        public int PlayerKills;
        public int PlayerDeaths;
        public int PlayerAssists;
        public int playerId;
    }

    [Header("References")]
    [SerializeField] private GameObject[] playerScoreCards;
    [SerializeField] private TMP_Text[] playerNames;
    [SerializeField] private TMP_Text[] playerScores;
    [SerializeField] private TMP_Text[] playerKills;
    [SerializeField] private TMP_Text[] playerDeaths;
    [SerializeField] private TMP_Text[] playerAssists;
    [SerializeField] private GameObject scoreboardCanvas;

    [Header("Personal Scoreboard")]
    [SerializeField] private TMP_Text personalPosition;
    [SerializeField] private TMP_Text personalScore;
    [SerializeField] private TMP_Text personalKills;
    [SerializeField] private TMP_Text personalDeaths;
    [SerializeField] private TMP_Text personalAssists;

    [Header("Team Score")]
    [SerializeField] private Slider blueTeamSlider;
    [SerializeField] private Slider redTeamSlider;
    [SerializeField] private TMP_Text blueTeamSliderText;
    [SerializeField] private TMP_Text redTeamSliderText;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private TMP_Text endGameText;

    [Header("Settings")]
    private int maxScore = 30;

    [HideInInspector] public NetworkManagerLobby networkManager;
    [HideInInspector] public GameObject[] allPlayers;

    [ClientCallback]
    public void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerLobby>();
        allPlayers = GameObject.FindGameObjectsWithTag("GamePlayer");
        blueTeamSlider.value = 0;
        redTeamSlider.value = 0;
    }

    [Client]
    public void UpdateTeamSliders(string team)
    {
        CmdUpdateTeamSliders(team);
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateTeamSliders(string team)
    {
        RpcUpdateTeamSliders(team);
    }
    [ClientRpc]
    public void RpcUpdateTeamSliders(string team)
    {
        if (team == "Blue Team")
        {
            blueTeamSlider.value++;
            blueTeamSliderText.SetText(blueTeamSlider.value.ToString());
        }
        if (team == "Red Team")
        {
            redTeamSlider.value++;
            redTeamSliderText.SetText(redTeamSlider.value.ToString());
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [Command]
    public void CmdUpdateScoreBoard()
    {
        RpcUpdateScoreBoard();
    }

    [ClientRpc]
    public void RpcUpdateScoreBoard()
    {
        for (int i = 0; i < allPlayers.Length; i++) //display scoreboard
        {
            playerNames[i].SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().displayName);
            playerScores[i].SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerScore.ToString());
            playerKills[i].SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerKills.ToString());
            playerDeaths[i].SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerDeaths.ToString());
            playerAssists[i].SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerAssists.ToString());
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

    [Client]
    public void UpdatePersonalScoreboard()
    {
        
        for(int i = 0; i < allPlayers.Length; i++)
        {
            if(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerID == GetComponent<PlayerManager>().networkId)
            {
                personalPosition.SetText(i.ToString());
                personalScore.SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerScore.ToString());
                personalKills.SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerKills.ToString());
                personalDeaths.SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerDeaths.ToString());
                personalAssists.SetText(allPlayers[i].GetComponent<NetworkGamePlayerLobby>().playerAssists.ToString());
            }
        }
    }

    [ClientCallback]
    private void Update()
    {
        for(int i = 0; i < allPlayers.Length; i++)
        {
            playerScoreCards[i].SetActive(true);
        }

        CmdUpdateScoreBoard();
        UpdatePersonalScoreboard();

        if (Input.GetButton("Tab"))
        {
            ShowScoreboard();
        }
        if (Input.GetButtonUp("Tab"))
        {
            HideScoreboard();
        }

        if(blueTeamSlider.value >= maxScore)
        {
            CmdEndgame("Blue");
        }
        if(redTeamSlider.value >= maxScore)
        {
            CmdEndgame("Red");
        }
    }

    [Command]
    public void CmdEndgame(string team)
    {
        RpcEndgame(team);
    }
    [ClientRpc]
    public void RpcEndgame(string team)
    {
        if (team == "Blue")
        {
            endGameCanvas.SetActive(true);
            endGameText.SetText("Blue Team Wins!");

        }
        if (team == "Red")
        {
            endGameCanvas.SetActive(true);
            endGameText.SetText("Red Team Wins!");
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
