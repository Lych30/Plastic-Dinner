using System.Collections.Generic;
using UnityEngine;

public class CommandSystem : MonoBehaviour
{
    public Connection connection;
    public ScoreSystem scoreSystem;
    public MenuManager menu; 

    [Header("______GAME DESIGN________")]
    //public List<Command> commandList = new List<Command>();                  // the list of commands that can appear on screen
    public List<Command> commandsUnlocked = new List<Command>();            // the list of acceptable commmands at the time
    //[HideInInspector]
    public List<Command> CustomerCommands = new List<Command>();            // the list of commmands to complete at the time
    public FoodList foodList;

    [Header("______ DEBUG _______")]
    public bool bConditionsAreMet = false;                          // don't forget to click on game view or input won't be taken
    public List<Food> CameraCommand = new List<Food>();                           // the command the player send

    public static CommandSystem Instance { get; private set; }
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
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.P)/* && menu.isInGame*/)
        {
            //Debug.Log("Ding ! ");
            bConditionsAreMet = false;
            UpdateCurrentCommand();
            CheckConditions();

            if(bConditionsAreMet)
            {
                // AddScore
                string scoreGain = connection.message.Split('|')[1];
                scoreGain = scoreGain.Remove(0,3);
                float score = float.Parse(scoreGain);
                scoreSystem.AddScore(score);
                Debug.Log("Command is Success !");
            }
            else
            {
                // Remove score or do nothing + redo combo
                Debug.Log("Command is Failed !");
            }
        }
    }

    // get all the object on dish with Teachable Machine output
    // translate it to the current command
    // command is a list of food
    public void AddCommandToDo()
    {
        int randomCommand = Random.Range(0, commandsUnlocked.Count);
        CustomerCommands.Add(commandsUnlocked[randomCommand]);
    }
    void UpdateCurrentCommand()
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
    void CheckConditions()
    {
        if(CameraCommand.Count <= 0) 
        { 
            Debug.Log("Command sent is null or not recognized ! ");  
            return; 
        }

        // it might take the object in list in order so don't forget to check individually each food
        if(CheckCommand())
        {
            bConditionsAreMet = true;
        }
        else
        {
            bConditionsAreMet = false;
        }
    }

    // if current command is equal to one of the command To Send -> it's valid
    bool CheckCommand()
    {
        bool hasFound = true;

        List<Food> commandsFood = CustomerCommands[0].foods;
        foreach (Food food in commandsFood)
        {
            Debug.Log(food);
            if(!CameraCommand.Contains(food))
            {
                hasFound = false;
                break;
            }
        }
        if (hasFound)
        {
            CustomerLeave();
            return true;
        }
        CustomerLeave();
        return false;
        //for (int i = 0; i < commandsUnlocked.Count; i++)
        //{
        //    List<Food> commandsFoods = commandsUnlocked[i].foods;
        //    bool hasFound = true;

        //    for(int j = 0; j < currentCommand.Count; j++)
        //    {
        //        if(!commandsFoods.Contains(currentCommand[j]))
        //        {
        //            hasFound = false;
        //            break;
        //        }
        //    }

        //    if(hasFound)
        //    {
        //        return true;
        //    }
        //}
    }

    void CustomerLeave()
    {
        Destroy(NPCManager.Instance.NPC_List[0].gameObject, 5);
        CustomerCommands.RemoveAt(0);
        NPCManager.Instance.NPC_List[0].Destination = NPCManager.Instance.Exit.position;
        NPCManager.Instance.NPC_List.RemoveAt(0);
        NPCManager.Instance.SpawnNPC();
    }
}
