using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMove : MonoBehaviour
{
    public Transform[] WayPoints;
    public int NextIndex;
    public float speed;
    public float rotspeed;
    private void Start()
    {
        NextIndex = 0;
    }
    void Update()
    {
        if(Vector3.Distance(transform.position,WayPoints[NextIndex].position) <=10)
        {
            NextIndex++;
        }
    }
    private void FixedUpdate()
    {
        if (NextIndex > WayPoints.Length - 1)
            NextIndex = 0;
        transform.position = Vector3.MoveTowards(transform.position, WayPoints[NextIndex].position, speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation( WayPoints[NextIndex].position - transform.position), rotspeed*Time.fixedDeltaTime);
    }
}
