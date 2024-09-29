using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private GameObject playerGO;

    void Start(){
        if (playerGO == null){
            playerGO = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void Respawn(){
        playerGO.transform.position = respawnPoint.transform.position;
    }

    public Transform RespawnPoint{
        set { respawnPoint = value; }
    }
}
