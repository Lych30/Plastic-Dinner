using System.Collections;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public ParticleSystem Fireworks;
    public ParticleSystem Nope;
    public Animator animator;
    public float speed = 5.0f;
    public float tolerance = 2.0f;

    [Header("____________DEBUG__________")]
    public Vector3 destination = Vector3.zero;
    public bool orderTaken = false;
    public Command currentOrder = null;
    public Vector3 exit = Vector3.zero;
    public NPCManager npcManager;

    private void Awake()
    {
        npcManager = NPCManager.Instance;
    }

    public void OnEnable()
    {
        orderTaken = false;
        destination = npcManager.SetDestination_Impl();
        transform.position = npcManager.GetSpawnPoint();
        exit = npcManager.GetExitPoint();
    }

    public void OnDisable()
    {
        orderTaken = false;
        currentOrder = null;
    }

    private void Update()
    {
        Vector3 v = destination - transform.position;
        float distance = v.magnitude;

        // Move to a table
        if (!orderTaken)
        {
            if (distance > tolerance)
            {
                Vector3 dir = v.normalized;
                transform.position += dir * speed * Time.deltaTime;
            }
            else
            {
                orderTaken = true;
                currentOrder = CommandSystem.TakeOrder();
                animator.Play("waiting");
            }
        }
    }

    public void Destroy(bool isSuccess = true)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToExit(isSuccess));
    }

    IEnumerator MoveToExit(bool isSuccess)
    {
        if(isSuccess)
        {
            TriggerFireworks();
            TriggerAnim("TriggerHappy");
        }
        else
        {
            TriggerAnim("TriggerAngry");
            NopeParticles();
        }

        Vector3 v = exit - transform.position;
        float distance = v.magnitude;

        while (distance > tolerance)
        {
            v = exit - transform.position;
            distance = v.magnitude;

            Vector3 dir = v.normalized;
            transform.position += dir * speed * Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
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
