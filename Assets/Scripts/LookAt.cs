using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform targetRot;
    public float turnSpeed;
    Quaternion rotGoal;
    Vector3 direction;

    void Start()
    {
        
    }

    void Update()
    {
        direction = (targetRot.position - transform.position).normalized;
        rotGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);
    }
}
