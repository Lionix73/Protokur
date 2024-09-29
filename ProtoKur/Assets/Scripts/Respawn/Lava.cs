using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    private RespawnManager respawnManager;

    void Start(){
        respawnManager = FindObjectOfType<RespawnManager>();
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            respawnManager.Respawn();
        }
    }
}
