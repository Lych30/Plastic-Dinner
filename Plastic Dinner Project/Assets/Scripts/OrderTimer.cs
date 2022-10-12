using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{
    [SerializeField] private Image uiFiller;
    private GameTimer gameTimer;

    private int orderDuration;
    private int remaningDuration;
    private float tColor;

    void Start()
    {
        gameTimer = transform.parent.GetComponent<GameTimer>();
        if (gameTimer.timer > gameTimer.timerDuration * (3 / 4))
        {
            orderDuration = 10;
        }
        else if (gameTimer.timer > gameTimer.timerDuration * (2 / 4))
        {
            orderDuration = 8;
        }
        else if (gameTimer.timer > gameTimer.timerDuration * (1 / 4))
        {
            orderDuration = 6;
        }
        else
        {
            orderDuration = 4;
        }
        Begin(orderDuration);
    }

    private void Update()
    {
        tColor += Time.deltaTime / remaningDuration;
        uiFiller.color = Color.LerpUnclamped(Color.green, Color.red, tColor);
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
            uiFiller.fillAmount = Mathf.InverseLerp(0, orderDuration, remaningDuration);
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
