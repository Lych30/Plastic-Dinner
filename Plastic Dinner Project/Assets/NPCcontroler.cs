using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCcontroler : MonoBehaviour
{
    public Vector3 Destination;
    private Rigidbody rb;
    public float speed;
    private bool orderTaken = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if ((Destination - transform.position).magnitude < 2f)
        {
            if (!orderTaken)
            {
                
                CommandSystem.Instance.AddCommandToDo();
                orderTaken = true;
            }

        }
        else
        {
            var dir = (Destination - transform.position).normalized;
            transform.position += dir*speed*Time.deltaTime;
        }

    }

    
}
