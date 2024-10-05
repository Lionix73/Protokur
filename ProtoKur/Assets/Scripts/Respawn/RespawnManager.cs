using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private GameObject playerGO;
    private GameObject playerCam;

    void Start(){
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void Respawn(){
        playerGO.transform.position = respawnPoint.transform.position;
        playerCam.transform.rotation = respawnPoint.transform.rotation;

    }

    public Transform RespawnPoint{
        set { respawnPoint = value; }
    }
}
