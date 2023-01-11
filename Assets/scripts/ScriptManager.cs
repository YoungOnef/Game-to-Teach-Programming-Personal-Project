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


    }

    public IEnumerator MoveForward(float distance)
    {
        player.transform.position += player.transform.forward * distance;
        yield return new WaitForSeconds(distance);
    }

    public IEnumerator Turn(string direction)
    {
        if (direction == "left")
        {
            player.transform.Rotate(Vector3.up, -90f);
        }
        else if (direction == "right")
        {
            player.transform.Rotate(Vector3.up, 90f);
        }
        yield return null;
    }

    private IEnumerator StartLua(string script)
    {

        // Bind the C# functions to the Lua script
        luaScript.Globals["MoveForward"] = (Func<float, IEnumerator>)MoveForward;
        luaScript.Globals["Turn"] = (Func<string, IEnumerator>)Turn;


        // Execute the Lua script
        DynValue result = luaScript.DoString(script);

        // Check if the result is a coroutine
        if (result.Type == DataType.Function)
        {
            DynValue coroutine = luaScript.CreateCoroutine(result);

            bool isRunning = true;
            while (isRunning)
            {
                coroutine = coroutine.Coroutine.Resume();
                if (coroutine.Coroutine.State != CoroutineState.Running)
                {
                    isRunning = false;
                }
                else
                {
                    yield return null;
                }
            }
        }

        HandleLuaErrors();
    }

    private void HandleLuaErrors()
    {
        try
        {
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
        StartCoroutine(StartLua(script));
    }
}
