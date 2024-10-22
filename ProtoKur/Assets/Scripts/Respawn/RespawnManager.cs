using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;

    private GameObject playerGO;
    private GameObject playerCam;

    void Start(){

        //playerGO = Instantiate(playerPrefab, respawnPoint.position, respawnPoint.rotation);
        //playerCam = Instantiate(cameraPrefab, respawnPoint.position, respawnPoint.rotation);
    }

    public void Respawn(){
        playerGO.transform.position = respawnPoint.transform.position;
        
        playerCam.GetComponent<PlayerCam>().enabled = false;

        Vector3 respawnEulerAngles = respawnPoint.transform.rotation.eulerAngles;
        playerCam.GetComponent<PlayerCam>().SetRotation(respawnEulerAngles.x, respawnEulerAngles.y);
        
        playerCam.GetComponent<PlayerCam>().enabled = true;
    }

    public Transform RespawnPoint{
        set { respawnPoint = value; }
    }
}
