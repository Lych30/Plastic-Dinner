using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandSystem : MonoBehaviour
{
    [Header("_____DEPENDENCE_____")]
    public Connection connection;
    public ScoreSystem scoreSystem;
    public MenuManager menuManager;
    public UIOrderManager uIOrderManager;

    [Header("_____GAMEPLAY_____")]
    public FoodList foodList;
    public List<Command> restaurantMenu;

    [Header("_____CUTS______")]
    public GameObject cutsUI;
    public CutUI cutUIScript;
    public TMP_Text textCut;
    public float timerCutDelay = 20.0f;
    public int minCut = 10;
    public int maxCut = 30;

    [Header("_____CUTS DEBUG______")]
    public float cutDelay = 0.0f;
    public bool needCuts = false;
    public int cutsCount = 0;

    [Header("______________DEBUG_____________")]
    public List<Command> customerOrders = new List<Command>();                              
    public List<Food> CameraFoods = new List<Food>();                             // the command the player send = the list of food the player send
    public Command cameraCommand = null;
    public bool isCameraCommandCorrect = false;

    public static CommandSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        ResetCuts();
    }

    private void Update()
    {
        if(menuManager.isInGame)
        {
            // CUTS
            if(needCuts)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cutsCount--;
                    cutUIScript.CutSwitch();
                    textCut.text = cutsCount.ToString();
                    if (cutsCount <= 0)
                    {
                        ResetCuts();
                    }
                }
            }
            else
            {
                // CUTS
                cutDelay -= Time.deltaTime;
                if (cutDelay <= 0)
                {
                    needCuts = true;
                    cutsCount = Random.Range(minCut, maxCut);
                    cutsUI.SetActive(true);
                    cutUIScript.PlayIntro();
                    textCut.text = cutsCount.ToString();
                }

                // SEND COMMAND
                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
                {

                    // Polish
                    SoundManager.Instance.PlaySound("Ding");

                    // Check camera command
                    GetCameraFoods();
                    CheckCommand();


                    if (isCameraCommandCorrect)
                    {
                        Debug.Log("Command is Success !");

                        // SFX
                        SoundManager.Instance.PlaySound("OrderValidated");

                        // Destroy Order Completely
                        DestroyOrder(cameraCommand);

                        // Add Score
                        string scoreGain = connection.message.Split('|')[1];
                        scoreGain = scoreGain.Remove(0, 3);
                        float score = float.Parse(scoreGain);
                        scoreSystem.AddScore(score);
                        scoreSystem.AddMultplier(0.5f);
                    }
                    else
                    {
                        Debug.Log("Command is Failed !");

                        // SFX
                        SoundManager.Instance.PlaySound("OrderMissed");
                        //NPCManager.Instance.NPC_List[0].NopeParticles();
                        
                        scoreSystem.AddMultplier(-0.2f);
                    }
                }
            }
        }
    }

    public static void DestroyOrder(Command order, bool isSucess = true)
    {
        Instance.DestroyOrder_Impl(order, isSucess);
    }

    private void DestroyOrder_Impl(Command order, bool isSuccess = true)
    {
        NPCManager.DestroyNPC(order, isSuccess);
        uIOrderManager.DestroyOrder(order);
        customerOrders.Remove(order);
    }

    public static Command TakeOrder()
    {
        return Instance.TakeOrder_Impl();
    }

    private Command TakeOrder_Impl()
    {
        return CreateOrder();
    }

    private Command CreateOrder()
    {
        int rng = Random.Range(0, restaurantMenu.Count);
        Command newOrder = restaurantMenu[rng];
        customerOrders.Add(newOrder);
        uIOrderManager.DisplayOrder(newOrder);
        return newOrder;
    }

    private void ResetCuts()
    {
        needCuts = false;
        cutsCount = 0;
        cutsUI.SetActive(false);
        cutDelay = timerCutDelay;
        textCut.text = cutsCount.ToString();
    }

    private void GetCameraFoods()
    {
        CameraFoods.Clear();
        for (int i = 0; i < foodList.foods.Count; i++)
        {
            Food food = foodList.foods[i];
            if (connection.message.Contains(food.name, System.StringComparison.CurrentCultureIgnoreCase))
            {
                CameraFoods.Add(food);
            }
        }
    }

    private void CheckCommand()
    {
        isCameraCommandCorrect = false;
        // Compare custumer's commands to camera's command
        foreach (Command customerOrder in customerOrders)
        {
            // if one of the food is not the good one -> you didn't find the good command
            bool hasFound = true;
            List<Food> customerFoods = customerOrder.foods;
            foreach (Food food in customerFoods)
            {
                if (CameraFoods.Count != customerFoods.Count)
                {
                    hasFound = false;
                    break; // and continue to parse next customer order
                }

                if (!CameraFoods.Contains(food))
                {
                    hasFound = false;
                    break; // and continue to parse next customer order
                }
            }

            if(hasFound)
            {
                isCameraCommandCorrect = true;
                cameraCommand = customerOrder;
                return;
            }
        }
    }
}
