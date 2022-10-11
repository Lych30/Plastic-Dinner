using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{
    [SerializeField] private Image uiFiller;

    public int duration;

    private int remaningDuration;

    void Start()
    {
        Begin(duration);
    }

    private void Begin (int second)
    {
        remaningDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remaningDuration >= 0)
        {
            uiFiller.fillAmount = Mathf.InverseLerp(0, duration, remaningDuration);
            remaningDuration--;
            yield return new WaitForSeconds(1);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        //Call when timer expires
    }
}
