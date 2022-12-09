using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    public GameObject inputField;
    public TMP_InputField userInputField;
    public TextMeshProUGUI userOutTextForDebug;
    public TextMeshProUGUI userOutText;
    public TextMeshProUGUI userOutTextFunctionDispaly;

    private string sceneName;
    public void inputText()
    {
        //stopping all corotines
        StopAllCoroutines();

        listOfTasks = new List<UnityAction>();
        listOfTime = new List<float>();

        try
        {
            // Get the script text from the input field
            string script = inputField.GetComponent<TMP_InputField>().text;
            Debug.Log("script: " + script);


            StartLua(script);
            userOutTextForDebug.text = "None Error messages from Lua";


            if (listOfTasks.Count == listOfTime.Count)
            {
                currentTask = DoTask();
                StartCoroutine(currentTask);
            }
            else
            {
                Debug.LogError($"ERROR Lists Count NotMaching : listOfTasks.Count({listOfTasks.Count}) listOfTime.Count({listOfTime.Count})");
            }
        }

        catch (SyntaxErrorException ex)
        {
            // if a syntax error was detected, display an error message to the user
            Debug.Log("Syntax error: " + ex.Message);
            userOutTextForDebug.text = "Syntax error: " + ex.Message;
        }
        catch (ScriptRuntimeException ex)
        {
            // if a runtime error was detected, display an error message to the user
            Debug.Log("Runtime error: " + ex.DecoratedMessage);
            userOutTextForDebug.text = "Runtime error: " + ex.DecoratedMessage;
        }
        catch (Exception ex)
        {
            // if any other exception was thrown, display the error message to the user
            Debug.Log("Error: " + ex.Message);
            userOutTextForDebug.text = "Error: " + ex.Message;
        }
        ResetCubePosition();

    }
    private void PrintToDebugLogAndTextArea(DynValue value)
    {
        // Get the type of the value
        string type = value.Type.ToString();

        // Convert the value to a string
        string message = value.ToObject().ToString();

        // Print the type and value to the debug log
        Debug.Log("Printing " + type + " value from Lua script: " + message);

        // Set the text of the Text object to the type and value
        userOutText.SetText(type + ": " + message);
    }
    public void resetText()
    {
        userInputField.text = "";
    }

    public void Stop()
    {
        // Clear the list of tasks and stop the current task, if any
        listOfTasks.Clear();
        listOfTime.Clear();
        StopCoroutine(currentTask);
    }

    public void ResetCubePosition()
    {
        // reset the position of the cube to its original position
        cube.transform.position = Vector3.zero;
    }

    public void SaveInput()
    {

        // get the user input
        string userInput = inputField.GetComponent<TMP_InputField>().text;

        // write the user input to a file
        File.WriteAllText(sceneName, userInput);
    }

    public void LoadSave()
    {
        // read the user input from the file
        string userInput = File.ReadAllText(sceneName);

        // set the input field to the saved user input
        inputField.GetComponent<TMP_InputField>().text = userInput;
    }

    public void HideOrViewConsole()
    {
        if (ScreenButton.activeSelf == true)
        {
            ScreenButton.SetActive(false);

        }
        else
        {
            ScreenButton.SetActive(true);
        }
    }
}
