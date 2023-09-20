using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    public Transform[] wayPoints;
    public Transform targetRot;
    public float turnSpeed;
    Quaternion rotGoal;
    Vector3 direction;
    private int currentPoint;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        currentPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != wayPoints[currentPoint].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPoint].position, speed * Time.deltaTime);
        }
        else
        {
            currentPoint = (currentPoint + 1) % wayPoints.Length;
        }

        direction = (targetRot.position - transform.position).normalized;
        rotGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);
    }
}
