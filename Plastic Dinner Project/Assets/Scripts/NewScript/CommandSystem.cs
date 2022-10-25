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
                    textCut.text = cutsCount.ToString();
                }

                // SEND COMMAND
                /// Send Command

                //                    //Debug.Log("Ding ! ");
                //                    isCameraCommandCorrect = false;
                //                    UpdateCameraCommand();
                //                    CheckCameraCommand();
                //                    SoundManager.Instance.PlaySound("Ding");

                //                    if (isCameraCommandCorrect)
                //                    {
                //                        // AddScore
                //                        NPCManager.Instance.NPC_List[0].TriggerFireworks();
                //                        NPCManager.Instance.NPC_List[0].StopAllCoroutines();
                //                        NPCManager.Instance.NPC_List[0].TriggerAnim("TriggerHappy");
                //                        string scoreGain = connection.message.Split('|')[1];
                //                        scoreGain = scoreGain.Remove(0, 3);
                //                        float score = float.Parse(scoreGain);
                //                        scoreSystem.AddScore(score);
                //                        scoreSystem.AddMultplier(0.5f);
                //                        ordersUI.DestroyOrder(commandFound);
                //                        Debug.Log("Command is Success !");
                //                        SoundManager.Instance.PlaySound("OrderValidated");
                //                    }
                //                    else
                //                    {
                //                        NPCManager.Instance.NPC_List[0].NopeParticles();
                //                        scoreSystem.AddMultplier(-0.2f);
                //                        // Reset combo
                //                        Debug.Log("Command is Failed !");
                //                        SoundManager.Instance.PlaySound("OrderMissed");
                //                    }
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

//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class CommandSystem : MonoBehaviour
//{
//    public Connection connection;
//    public ScoreSystem scoreSystem;
//    public MenuManager menu; 
//    public OrdersUI ordersUI;

//    public GameObject cutsUI;
//    public TMP_Text textCut;
//    public float timerCutDelay = 20.0f;
//    public int cutsCount = 0;

//    [Header("____ CUT DEBUG ______")]
//    public float cutDelay = 0.0f;
//    public bool needsCut = false;

//    [Header("______GAME DESIGN________")]
//    //public List<Command> commandList = new List<Command>();               // the list of commands that can appear on screen
//    public List<Command> commandsUnlocked = new List<Command>();            // the list of acceptable commmands at the time
//    //[HideInInspector]
//    public List<Command> CustomerCommands = new List<Command>();            // the list of commmands to complete at the time
//    public FoodList foodList;

//    [Header("______ DEBUG _______")]
//    public bool isCameraCommandCorrect = false;                          // don't forget to click on game view or input won't be taken
//    public List<Food> CameraCommand = new List<Food>();                           // the command the player send
//    public Command commandFound = null;

//    public static CommandSystem Instance;
//    private void Awake()
//    {
//        // If there is an instance, and it's not me, delete myself.

//        if (Instance != null && Instance != this)
//        {
//            Destroy(this);
//        }
//        else
//        {
//            Instance = this;
//        }
//    }

//    private void Start()
//    {
//        cutsUI.SetActive(false);
//        cutDelay = timerCutDelay;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(menu.isInGame)
//        {
//            if (!needsCut)
//            {
//                cutDelay -= Time.deltaTime;
//                if (cutDelay <= 0)
//                {
//                    needsCut = true;

//                    cutsCount = Random.Range(10, 30);
//                    cutsUI.SetActive(true);
//                    textCut.text = cutsCount.ToString();

//                }

//                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
//                {
//                /// Send Command

//                    //Debug.Log("Ding ! ");
//                    isCameraCommandCorrect = false;
//                    UpdateCameraCommand();
//                    CheckCameraCommand();
//                    SoundManager.Instance.PlaySound("Ding");

//                    if (isCameraCommandCorrect)
//                    {
//                        // AddScore
//                        NPCManager.Instance.NPC_List[0].TriggerFireworks();
//                        NPCManager.Instance.NPC_List[0].StopAllCoroutines();
//                        NPCManager.Instance.NPC_List[0].TriggerAnim("TriggerHappy");
//                        string scoreGain = connection.message.Split('|')[1];
//                        scoreGain = scoreGain.Remove(0, 3);
//                        float score = float.Parse(scoreGain);
//                        scoreSystem.AddScore(score);
//                        scoreSystem.AddMultplier(0.5f);
//                        ordersUI.DestroyOrder(commandFound);
//                        Debug.Log("Command is Success !");
//                        SoundManager.Instance.PlaySound("OrderValidated");
//                    }
//                    else
//                    {
//                        NPCManager.Instance.NPC_List[0].NopeParticles();
//                        scoreSystem.AddMultplier(-0.2f);
//                        // Reset combo
//                        Debug.Log("Command is Failed !");
//                        SoundManager.Instance.PlaySound("OrderMissed");
//                    }
//                }
//            }
//            else 
//            {
//                if (cutsCount > 0)
//                {
//                    if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
//                    {
//                        cutsCount--;
//                        textCut.text = cutsCount.ToString();
//                        if (cutsCount <= 0)
//                        {
//                            cutsUI.SetActive(false);
//                            cutDelay = timerCutDelay;
//                            textCut.text = "0";
//                            needsCut = false;
//                        }
//                    }
//                }
//            }
//        }
//    }

//    public Command AddCommandToDo()
//    {
//        int randomCommand = Random.Range(0, commandsUnlocked.Count);
//        Command newCommand = commandsUnlocked[randomCommand];
//        CustomerCommands.Add(newCommand);
//        return newCommand;
//    }

//    // get all the object on dish with Teachable Machine output
//    // translate it to the current command
//    // command is a list of food
//    void UpdateCameraCommand()
//    {
//        CameraCommand.Clear();
//        for(int i = 0; i < foodList.foods.Count; i++)
//        {
//            Food food = foodList.foods[i];
//            //Debug.Log(food.name);
//            // 
//            if (connection.message.Contains(food.name, System.StringComparison.CurrentCultureIgnoreCase))
//            {
//                CameraCommand.Add(food);
//            }
//        }
//    }

//    // if current command is equal one of the command in list -> conditions are met -> Score ++ * combo multiplier
//    void CheckCameraCommand()
//    {
//        if(CameraCommand.Count <= 0) 
//        { 
//            Debug.Log("Command sent is null or not recognized ! ");  
//            return; 
//        }

//        // it might take the object in list in order so don't forget to check individually each food
//        if(CheckCommand())
//        {
//            isCameraCommandCorrect = true;
//        }
//        else
//        {
//            isCameraCommandCorrect = false;
//        }
//    }

//    // if current command is equal to one of the command To Send -> it's valid
//    bool CheckCommand()
//    {
//        // you have to check each customer commands and not just the first one
//        foreach(Command customerCommand in CustomerCommands)
//        {
//            bool hasFound = true;
//            List<Food> customerOrder = customerCommand.foods;
//            foreach (Food food in customerOrder)
//            {
//                Debug.Log(food);
//                if(CameraCommand.Count != customerOrder.Count)
//                {
//                    hasFound = false;
//                    break;
//                }

//                if (!CameraCommand.Contains(food))
//                {
//                    hasFound = false;
//                    break;
//                }
//            }

//            if (hasFound)
//            {
//                commandFound = customerCommand;

//                return true;
//            }
//        }

//        // if order has failed it's fine give them another chance
//        //CustomerLeave();
//        return false;
//    }

//    public void CustomerLeave()
//    {
//        CustomerCommands.Remove(commandFound);
//        Destroy(NPCManager.Instance.NPC_List[0].gameObject, 5);
//        NPCManager.Instance.NPC_List[0].Destination = NPCManager.Instance.Exit.position;
//        NPCManager.Instance.NPC_List.RemoveAt(0);

//    }
//}
