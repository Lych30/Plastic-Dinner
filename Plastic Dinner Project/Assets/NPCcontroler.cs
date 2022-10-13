using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCcontroler : MonoBehaviour
{
    public Vector3 Destination;
    private Rigidbody rb;
    public float speed;
    private bool orderTaken = false;
    public ParticleSystem Fireworks;
    public ParticleSystem Nope;
    [Header("_______DEBUG________")]
    public NPCManager npcManager;
    void Start()
    {
        npcManager = NPCManager.Instance;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Destination - transform.position).magnitude < 2f)
        {
            if (!orderTaken)
            {
                orderTaken = true;
                npcManager.TakeOrder();
                
            }
        }
        else
        {
            var dir = (Destination - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    public void TriggerFireworks()
    {
        Fireworks.Play();
    }

    public void NopeParticles()
    {
        Nope.Play();
    }
}
