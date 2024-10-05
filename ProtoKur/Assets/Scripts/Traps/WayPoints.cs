using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform GetWayPoint(int index)
    {
        return transform.GetChild(index);
    }

    public int GetNextWayPointIndex(int currentWayPoint){
        int nextWayPoint = currentWayPoint + 1;

        if (nextWayPoint == transform.childCount)
        {
            nextWayPoint = 0;
        }

        return nextWayPoint;
    }
}
