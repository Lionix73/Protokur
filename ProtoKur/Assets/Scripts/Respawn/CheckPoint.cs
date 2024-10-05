using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private RespawnManager respawnManager;
    void Awake(){
        if(respawnManager == null){
            respawnManager = FindObjectOfType<RespawnManager>();
            Debug.Log("Found Respawn Manag");
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("PlayerCollider")){
            Debug.Log("Player Entered Checkpoint");
            respawnManager.RespawnPoint = transform;
        }
    }
}
