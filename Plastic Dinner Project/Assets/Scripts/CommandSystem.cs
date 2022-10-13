using System.Collections.Generic;
using UnityEngine;

public class CommandSystem : MonoBehaviour
{
    public Connection connection;
    public ScoreSystem scoreSystem;
    public MenuManager menu; 
    public OrdersUI ordersUI;

    [Header("______GAME DESIGN________")]
    //public List<Command> commandList = new List<Command>();               // the list of commands that can appear on screen
    public List<Command> commandsUnlocked = new List<Command>();            // the list of acceptable commmands at the time
    //[HideInInspector]
    public List<Command> CustomerCommands = new List<Command>();            // the list of commmands to complete at the time
    public FoodList foodList;
    
    [Header("______ DEBUG _______")]
    public bool isCameraCommandCorrect = false;                          // don't forget to click on game view or input won't be taken
    public List<Food> CameraCommand = new List<Food>();                           // the command the player send
    public Command commandFound = null;

    public static CommandSystem Instance;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space)) && menu.isInGame)
        {
            //Debug.Log("Ding ! ");
            isCameraCommandCorrect = false;
            UpdateCameraCommand();
            CheckCameraCommand();
            //SoundManager.Instance.PlaySound("Ding");

            if(isCameraCommandCorrect)
            {
                // AddScore
                string scoreGain = connection.message.Split('|')[1];
                scoreGain = scoreGain.Remove(0,3);
                float score = float.Parse(scoreGain);
                scoreSystem.AddScore(score);
                ordersUI.DestroyOrder(commandFound);

                Debug.Log("Command is Success !");
                //SoundManager.Instance.PlaySound("OrderValidated");
            }
            else
            {
                // Reset combo
                Debug.Log("Command is Failed !");
                //SoundManager.Instance.PlaySound("OrderMissed");
            }
        }
    }

    public Command AddCommandToDo()
    {
        int randomCommand = Random.Range(0, commandsUnlocked.Count);
        Command newCommand = commandsUnlocked[randomCommand];
        CustomerCommands.Add(newCommand);
        return newCommand;
    }

    // get all the object on dish with Teachable Machine output
    // translate it to the current command
    // command is a list of food
    void UpdateCameraCommand()
    {
        CameraCommand.Clear();
        for(int i = 0; i < foodList.foods.Count; i++)
        {
            Food food = foodList.foods[i];
            //Debug.Log(food.name);
            // 
            if (connection.message.Contains(food.name, System.StringComparison.CurrentCultureIgnoreCase))
            {
                CameraCommand.Add(food);
            }
        }
    }

    // if current command is equal one of the command in list -> conditions are met -> Score ++ * combo multiplier
    void CheckCameraCommand()
    {
        if(CameraCommand.Count <= 0) 
        { 
            Debug.Log("Command sent is null or not recognized ! ");  
            return; 
        }

        // it might take the object in list in order so don't forget to check individually each food
        if(CheckCommand())
        {
            isCameraCommandCorrect = true;
        }
        else
        {
            isCameraCommandCorrect = false;
        }
    }

    // if current command is equal to one of the command To Send -> it's valid
    bool CheckCommand()
    {
        // you have to check each customer commands and not just the first one
        foreach(Command customerCommand in CustomerCommands)
        {
            bool hasFound = true;
            List<Food> customerOrder = customerCommand.foods;
            foreach (Food food in customerOrder)
            {
                Debug.Log(food);
                if(CameraCommand.Count != customerOrder.Count)
                {
                    hasFound = false;
                    break;
                }

                if (!CameraCommand.Contains(food))
                {
                    hasFound = false;
                    break;
                }
            }

            if (hasFound)
            {
                commandFound = customerCommand;
                CustomerLeave();
                return true;
            }
        }

        // if order has failed it's fine give them another chance
        //CustomerLeave();
        return false;
    }

    public void CustomerLeave()
    {
        CustomerCommands.Remove(commandFound);
        Destroy(NPCManager.Instance.NPC_List[0].gameObject, 5);
        NPCManager.Instance.NPC_List[0].Destination = NPCManager.Instance.Exit.position;
        NPCManager.Instance.NPC_List.RemoveAt(0);
        
    }
}
