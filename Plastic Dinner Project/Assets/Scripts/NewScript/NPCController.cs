using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public ParticleSystem Fireworks;
    public ParticleSystem Nope;
    public Animator animator;
    public Animator NPCanimator;
    public float speed = 5.0f;
    public float tolerance = 4.0f;

    [Header("____________DEBUG__________")]
    public Vector3 destination = Vector3.zero;
    public bool orderTaken = false;
    public Command currentOrder = null;
    public Vector3 exit = Vector3.zero;
    public NPCManager npcManager;
    Color initialcolor;
    private void Awake()
    {
        initialcolor = transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().material.color;
        npcManager = NPCManager.Instance;
    }

    public void OnEnable()
    {
        transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().material.color = initialcolor;

        orderTaken = false;
        destination = npcManager.SetDestination_Impl();
        transform.position = npcManager.GetSpawnPoint();
        exit = npcManager.GetExitPoint();
    }

    public void OnDisable()
    {
        animator.ResetTrigger("TriggerHappy");
        animator.ResetTrigger("TriggerAngry");
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
                transform.LookAt(destination);
                NPCanimator.Play("CustomerWalking");
            }
            else
            {
                orderTaken = true;
                currentOrder = CommandSystem.TakeOrder();
                animator.Play("waiting");
                NPCanimator.Play("CustomerIdle");
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
            transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().material.color = Color.red;
            TriggerAnim("TriggerAngry");
            NopeParticles();
        }

        Vector3 v = exit - transform.position;
        float distance = v.magnitude;

        while (distance > tolerance)
        {
            v = exit - transform.position;
            distance = v.magnitude;
            transform.LookAt(exit);
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
        TriggerAnim(trigger);
        Debug.Log(trigger);
    }
}
