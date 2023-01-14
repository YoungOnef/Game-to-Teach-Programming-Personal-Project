using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.Events;


public class ScriptManager : MonoBehaviour
{
    private UIManager uIManager;


    [SerializeField]
    private GameObject player;

    int cycles = 0; // variable to keep track of number of times MoveUp() is called

    // method to move the object forward by a given distance
    public void MoveForward(float distance)
    {
        player.transform.position += player.transform.forward * distance;
        cycles += 1; // increment the cycles
    }

    // method to turn the object in a given direction
    public void Turn(string direction)
    {
        if (direction == "left")
        {
            player.transform.Rotate(Vector3.up, -90f);
        }
        else if (direction == "right")
        {
            player.transform.Rotate(Vector3.up, 90f);
        }
    }

    private IEnumerator _Start()
    {
        string code = @"
    -- Lua code to control the player object
    Turn(""left"")
MoveForward(1)
    ";

        // Load the code and get the returned function
        Script script = new Script(CoreModules.None);
        script.Globals["MoveForward"] = (Action<float>)MoveForward; // register the MoveForward() method to be used in the Lua script
        script.Globals["Turn"] = (Action<string>)Turn; // register the Turn() method to be used in the Lua script
        script.DoString(code); // execute the Lua script

        DynValue result = null;

        for (result = script.DoString(code); // call the script
            result.Type == DataType.YieldRequest; // check if script has ended
            result = script.DoString(code)) // continue the script
        {
            print(cycles); // print the number of cycles
            yield return new WaitForSeconds(0.001f); // pause execution for 0.001 seconds
        }
    }

    private void Start()
    {
        StartCoroutine(_Start()); // start the script
    }

}