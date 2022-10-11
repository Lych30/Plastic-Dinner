using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSystem : MonoBehaviour
{
    [Header("______GAME DESIGN________")]
    public List<Command> commands = new List<Command>();

    [Header("______ DEBUG _______")]
    public bool bConditionsAreMet = false;                          // don't forget to click on game view or input won't be taken
    public Command currentCommand = null;                           // the command the player send

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton0) && bConditionsAreMet)
        {
            Debug.Log("Ding ! ");

            UpdateCurrentCommand();
            CheckConditions();
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
        
        //bConditionsAreMet = true;
    }
}
