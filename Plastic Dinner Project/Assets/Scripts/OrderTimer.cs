using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTimer : MonoBehaviour
{
    public int[] durations = new int[4];

    public List<Image> foodImages = new List<Image>();
    public Command command = null;
    
    [Header("_________ DEBUG __________")]
    [SerializeField] private Image uiFiller;
    [SerializeField] private GameTimer gameTimer;
    
    [SerializeField] private int orderDuration;
    [SerializeField] private int remaningDuration;
    [SerializeField] private float tColor;

    private void Awake()
    {
        gameTimer = transform.parent.parent.GetComponent<GameTimer>();

        foreach(Image image in foodImages)
        {
            image.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        //ResetOrderDuration();
    }

    public void ResetOrderDuration()
    {
        Debug.Log("Order has arrive !");

        if (gameTimer.timer > gameTimer.timerDuration * (3 / 4))
        {
            orderDuration = durations[0];
        }
        else if (gameTimer.timer > gameTimer.timerDuration * (2 / 4))
        {
            orderDuration = durations[1];
        }
        else if (gameTimer.timer > gameTimer.timerDuration * (1 / 4))
        {
            orderDuration = durations[2];
        }
        else
        {
            orderDuration = durations[3];
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
        NPCManager.Instance.NPC_List[NPCManager.Instance.NPC_List.Count - 1].TriggerAnim("TriggerAngry",orderDuration/1.25f);
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

    public void OnEnd()
    {
        //Call when timer expires or command is success
        foreach (Image image in foodImages)
        {
            image.gameObject.SetActive(false);
        }
        
            CommandSystem.Instance.CustomerLeave();

        command = null;
        gameObject.SetActive(false);
    }
}
