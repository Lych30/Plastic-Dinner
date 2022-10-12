using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSystem : MonoBehaviour
{
    [Header("______GAME DESIGN________")]
    public List<Command> commandList = new List<Command>();                  // the list of commands that can appear on screen
    public List<Command> commandsToSend = new List<Command>();            // the list of acceptable commmands at the time

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
            Debug.Log("Ding ! ");

            UpdateCurrentCommand();
            CheckConditions();

            if(bConditionsAreMet)
            {
                // AddScore
            }
            else
            {
                // Remove score or do nothing + redo combo
            }
        }
    }

    // get all the object on dish with Teachable Machine output
    // translate it to the current command
    // command is a list of food
    void UpdateCurrentCommand()
    {

    }

    // if current command is equal one of the command in list -> conditions are met -> Score ++ * combo multiplier
    void CheckConditions()
    {
        if(currentCommand.Count <= 0) 
        { 
            Debug.LogError("Error Command sent is null or not recognized ! ");  
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
        // if current command foods
        for (int j = 0; j < currentCommand.Count; j++)
        {
            for (int i = 0; i < commandsToSend.Count; i++)
            {
                List<Food> commandParsing = commandsToSend[i].foods;
                bool hasFound = true;
                for(int k =  0; k < commandParsing.Count; k++)
                {
                    // is not equal to one of the command to send food
                    if(!commandParsing.Contains(currentCommand[j]))
                    {
                        hasFound = false;
                        break;
                    }
                }

                if(hasFound)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
