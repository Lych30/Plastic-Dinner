using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersUI : MonoBehaviour
{
    public List<OrderTimer> orders = new List<OrderTimer>();
    public float timeBeforeNextOrder = 5.0f;
    public bool waitBeforeStartTimer = true;
    public float delay = 1.0f;

    [Header("________________DEBUG___________________")]
    public float timer = 0.0f;

    private void Awake()
    {
        //orders.Clear();
        foreach(OrderTimer order in orders)
        {
            order.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = timeBeforeNextOrder;
        NPCManager.Instance.SpawnNPC();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitBeforeStartTimer)
        {
            delay -= Time.deltaTime;
            if (delay <= 0.0f)
            {
                waitBeforeStartTimer = false;
            }
            return;
        }

        if (timer > timeBeforeNextOrder)
        {
            NPCManager.Instance.SpawnNPC();
        }
        timer += Time.deltaTime;
    }

    void CreateOrder()
    {
        foreach(OrderTimer order in orders)
        {
            if(!order.isActiveAndEnabled)
            {
                Debug.Log("Create Order");
                order.gameObject.SetActive(true);
                order.ResetOrderDuration();

                Command newCommand = CommandSystem.Instance.AddCommandToDo();
                for(int i = 0; i < newCommand.foods.Count; i++)
                {
                    order.foodImages[i].gameObject.SetActive(true);
                    order.foodImages[i].sprite = newCommand.foods[i].sprite; 
                }
                order.command = newCommand;

                break;
            }
        }
        timer = 0.0f;
    }

    //public void Subscribe(OrderTimer order)
    //{
    //    orders.Add(order);
    //    order.gameObject.SetActive(false);
    //}

    public void DestroyOrder(Command command)
    {
        foreach(OrderTimer order in orders)
        {
            if(order.command == command)
            {
                order.OnEnd();
                break;
            }
        }
    }
}
