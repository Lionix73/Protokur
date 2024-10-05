using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private WayPoints wayPoints;
    [SerializeField] private float speed;

    private int targetWayPointIndex;

    private Transform previousWayPoint;
    private Transform targetWayPoint;

    private float timeToWaypoint;
    private float elapsedTime;

    private GameObject player;

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        TargetNextWayPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeToWaypoint;
        transform.position = Vector3.Lerp(previousWayPoint.position, targetWayPoint.position, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            TargetNextWayPoint();
        }
    }

    private void TargetNextWayPoint(){
        previousWayPoint = wayPoints.GetWayPoint(targetWayPointIndex);
        targetWayPointIndex = wayPoints.GetNextWayPointIndex(targetWayPointIndex);
        targetWayPoint = wayPoints.GetWayPoint(targetWayPointIndex);

        elapsedTime = 0;

        float distanceToWayPoint = Vector3.Distance(previousWayPoint.position, targetWayPoint.position);
        timeToWaypoint = distanceToWayPoint / speed;
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("PlayerCollider")){
            player.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("PlayerCollider")){
            player.transform.SetParent(null);
        }
    }
}
