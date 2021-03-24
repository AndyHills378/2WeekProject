using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CheckTerrainTexture : NetworkBehaviour
{

    [SerializeField] private Transform playerTransform;
    [SerializeField] Terrain terrain;


    [SerializeField] private AudioClip grassClip;
    [SerializeField] private AudioClip dirtClip;
    [SerializeField] private AudioClip sandClip;

    private PlayerMovement playerMovement;
    private AudioSource source;
    private int posX;
    private int posZ;
    public float[] textureValues;

    AudioClip previousClip;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    void Start()
    {
        terrain = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
        source = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        terrain.terrainData.SyncTexture(TerrainData.AlphamapTextureName);
    }

    [ClientCallback]
    // Update is called once per frame
    public void Update()
    {
        // For better performance, move this out of update 
        // and only call it when you need a footstep.
        if (playerMovement.isWalking)
        {
            PlayFootstep();
        }

    }

    [Client]
    public void GetTerrainTexture()
    {
        ConvertPosition(playerTransform.position);
        CheckTexture();
    }

    [Client]
    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - terrain.transform.position;

        Vector3 mapPosition = new Vector3
        (terrainPosition.x / terrain.terrainData.size.x, 0,
        terrainPosition.z / terrain.terrainData.size.z);

        float xCoord = mapPosition.x * terrain.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * terrain.terrainData.alphamapHeight;

        posX = (int)xCoord;
        posZ = (int)zCoord;
    }

    [Client]
    void CheckTexture()
    {
        float[,,] aMap = terrain.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        //Debug.Log(aMap[0, 0, 0]);
        
        if(aMap != null)
        {
            textureValues[0] = aMap[0, 0, 0];
            textureValues[1] = aMap[0, 0, 1];
            textureValues[2] = aMap[0, 0, 2];
        }

    }

    [Client]
    public void PlayFootstep()
    {
        GetTerrainTexture();
        if (source.isPlaying)
        {
            StartCoroutine(WaitForSoundFinish());
        }
        else
        {
            if (textureValues[0] > 0)
            {
                source.PlayOneShot(grassClip, textureValues[0]);
            }
            if (textureValues[1] > 0)
            {
                source.PlayOneShot(grassClip, textureValues[1]);
            }
            if (textureValues[2] > 0)
            {
                source.PlayOneShot(sandClip, textureValues[2]);
            }
        }
    }

    [Client]
    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip =
        clipArray[Random.Range(0, clipArray.Length - 1)];

        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Length - 1)];

            attempts--;
        }

        previousClip = selectedClip;
        return selectedClip;

    }

    IEnumerator WaitForSoundFinish()
    {
        yield return new WaitForSeconds(2f);
    }
}