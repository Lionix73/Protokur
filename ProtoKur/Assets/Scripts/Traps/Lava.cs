using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    private RespawnManager respawnManager;

    void Start(){
        respawnManager = FindObjectOfType<RespawnManager>();

        if (respawnManager != null){
            Debug.Log("Found respawn mang");
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("PlayerCollider")){
            Debug.Log("Respawn");
            respawnManager.Respawn();
        }
    }
}
