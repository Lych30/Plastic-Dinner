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
        StartCoroutine(TriggerAnimDelay(trigger, delay));
    }
    public void TriggerAnim(string trigger)
    {
        animator.SetTrigger(trigger);

    }
    IEnumerator TriggerAnimDelay(string trigger, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(trigger);
        Debug.Log(trigger);
    }
}
