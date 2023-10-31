using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Tooltip("Points for the agent to take as reference to patrol.")] 
    [SerializeField] private Transform[] wayPoints;
    [Tooltip("Object at which the agent will be looking at all the time.")]
    [SerializeField] private Transform targetRot;
    [Tooltip("Speed at which the agent will turn.")]
    [SerializeField] private float turnSpeed;
    [Tooltip("Speed at which the agent will move.")]
    [SerializeField] private float speed;

    private Quaternion rotGoal;
    private Vector3 direction;
    private int currentPoint = 0;
    
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
