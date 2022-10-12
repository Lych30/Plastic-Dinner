using System.Collections.Generic;
using UnityEngine;

public class CommandSystem : MonoBehaviour
{
    public Connection connection;
        
    [Header("______GAME DESIGN________")]
    public List<Command> commandList = new List<Command>();                  // the list of commands that can appear on screen
    public List<Command> commandsUnlocked = new List<Command>();            // the list of acceptable commmands at the time
    public FoodList foodList;

    [Header("______ DEBUG _______")]
    public bool bConditionsAreMet = false;                          // don't forget to click on game view or input won't be taken
    public List<Food> currentCommand = new List<Food>();                           // the command the player send

    public bool isInGame = true;
    private void Awake()
    {
        isInGame = true;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton0) && isInGame)
        {
            //Debug.Log("Ding ! ");
            bConditionsAreMet = false;
            UpdateCurrentCommand();
            CheckConditions();

            if(bConditionsAreMet)
            {
                // AddScore
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
    void UpdateCurrentCommand()
    {
        currentCommand.Clear();
        for(int i = 0; i < foodList.foods.Count; i++)
        {
            Food food = foodList.foods[i];
            //Debug.Log(food.name);
            // 
            if (connection.message.Contains(food.name, System.StringComparison.CurrentCultureIgnoreCase))
            {
                currentCommand.Add(food);
            }
        }
    }

    // if current command is equal one of the command in list -> conditions are met -> Score ++ * combo multiplier
    void CheckConditions()
    {
        if(currentCommand.Count <= 0) 
        { 
            Debug.Log("Error : Command sent is null or not recognized ! ");  
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
        for(int i = 0; i < currentCommand.Count; i++)
        {
            bool hasFound = true;
            for(int j = 0; j < commandsUnlocked.Count; j++)
            {
                List<Food> commandsFood = commandsUnlocked[i].foods;
                if (!commandsFood.Contains(currentCommand[i]))
                {
                    hasFound = false;
                    break;
                }
            }

            if (hasFound)
            {
                return true;
            }
        }
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

        return false;
    }
}
