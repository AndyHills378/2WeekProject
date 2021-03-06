using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class CharacterSelect : NetworkBehaviour
{
    [SerializeField] private GameObject characterSelectDisplay = default;
    [SerializeField] private Transform characterPreviewParent = default;
    [SerializeField] private TMP_Text characterNameText = default;
    [SerializeField] private float turnSpeed = 90f;
    [SerializeField] private Character[] characters = default;

    private int currentCharacterIndex = 0;
    private List<GameObject> characterInstances = new List<GameObject>();
    [HideInInspector] public bool characterSelected = false;

    public override void OnStartClient()
    {
        foreach(var character in characters)
        {
            GameObject characterInstance = Instantiate(character.CharacterPreviewPrefab, characterPreviewParent);
            characterInstance.SetActive(false);

            characterInstances.Add(characterInstance);
        }

        characterInstances[currentCharacterIndex].SetActive(true);
        characterNameText.text = characters[currentCharacterIndex].CharacterName;

        characterSelectDisplay.SetActive(true);
    }

    public void Right()
    {
        characterInstances[currentCharacterIndex].SetActive(false);

        currentCharacterIndex = (currentCharacterIndex + 1) % characterInstances.Count;

        characterInstances[currentCharacterIndex].SetActive(true);
        characterNameText.text = characters[currentCharacterIndex].CharacterName;
    }

    public void Select()
    {
        CmdSelect(currentCharacterIndex);
        characterSelectDisplay.SetActive(false);
    }

    [Command(requiresAuthority = false)]
    public void CmdSelect(int characterIndex, NetworkConnectionToClient sender = null)
    {
        GameObject characterInstance = Instantiate(characters[characterIndex].GameplayCharacterPrefab);
        NetworkServer.Spawn(characterInstance, sender);
    }
}
