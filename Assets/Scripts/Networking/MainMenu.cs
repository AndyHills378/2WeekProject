using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;
    [SerializeField] private AnimatorFunctions animatorFunctions;
    [SerializeField] private AudioClip buttonClick;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;

    public void HostLobby()
    {
        networkManager.StartHost();
        animatorFunctions.PlaySound(buttonClick);

        landingPagePanel.SetActive(false);
    }
}
