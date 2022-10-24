using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCcontroler : MonoBehaviour
{
    public Vector3 Destination;
    private Rigidbody rb;
    public float speed;
    public float tolerance;
    private bool orderTaken = false;
    public ParticleSystem Fireworks;
    public ParticleSystem Nope;
    public Animator animator;
    public Animator NPCanimator;
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
        if ((Destination - transform.position).magnitude < tolerance)
        {
            NPCanimator.Play("CustomerIdle");
            if (!orderTaken)
            {
                orderTaken = true;
                npcManager.TakeOrder();
                animator.Play("waiting");
                
            }
        }
        else
        {
            var dir = (Destination - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            transform.LookAt(Destination);
            NPCanimator.Play("CustomerWalking");
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

    public void TriggerAnim(string trigger, float delay)
    {
        StartCoroutine(TriggerAnimDelay(trigger,delay));
    }
    public void TriggerAnim(string trigger)
    {
        animator.SetTrigger(trigger);
        if(trigger == "TriggerAngry")
        {
            transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
    IEnumerator TriggerAnimDelay(string trigger, float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerAnim(trigger);
        Debug.Log(trigger);
    }
}
