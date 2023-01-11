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

    private Script luaScript;
    void Start()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        GameObject newPlayer = GameObject.Find("Player");

        luaScript = new Script();

        /*
Turn("left")
MoveForward(10)
Turn("left")
MoveForward(10) */
    }

    public void MoveForward(float distance)
    {
        player.transform.position += player.transform.forward * distance;
    }

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
    private void StartLua(string script)
    {
        try
        {
            // Bind the C# functions to the Lua script
            luaScript.Globals["MoveForward"] = (Action<float>)MoveForward;
            luaScript.Globals["Turn"] = (Action<string>)Turn;

            // Execute the Lua script
            luaScript.DoString(script);
            uIManager.userOutTextForDebug.text = "None Error messages from Lua";
        }
        catch (SyntaxErrorException ex)
        {
            // if a syntax error was detected, display an error message to the user
            Debug.Log("Syntax error: " + ex.Message);
            uIManager.userOutTextForDebug.text = "Syntax error: " + ex.Message;
        }
        catch (ScriptRuntimeException ex)
        {
            // if a runtime error was detected, display an error message to the user
            Debug.Log("Runtime error: " + ex.DecoratedMessage);
            uIManager.userOutTextForDebug.text = "Runtime error: " + ex.DecoratedMessage;
        }
        catch (Exception ex)
        {
            // if any other exception was thrown, display the error message to the user
            Debug.Log("Error: " + ex.Message);
            uIManager.userOutTextForDebug.text = "Error: " + ex.Message;
        }
    }
    public void InputText()
    {
        string script = uIManager.inputField.GetComponent<TMP_InputField>().text;
        StartLua(script);
    }
}