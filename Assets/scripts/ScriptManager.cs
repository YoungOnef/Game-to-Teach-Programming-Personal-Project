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

public class ScriptManager : MonoBehaviour
{
    // UI elements
    public GameObject inputField;
    public TMP_InputField userInputField;
    public TextMeshProUGUI userOutTextForDebug;
    public TextMeshProUGUI userOutText;
    public TextMeshProUGUI userOutTextFunctionDispaly;

    // The cube object
    [SerializeField]
    private GameObject cube;
    private Renderer cubeRenderer;
    public GameObject ScreenButton;

    // Variables for the cube's color and size
    private Color newCubeColor;
    private float randomChannelOne, randomChannelTwo, randomChannelThree;
    private float speed = 2;
    //private float cubeSize = 1.0f;

    // Variables for managing tasks
    private List<UnityAction> listOfTasks = new List<UnityAction>();
    private List<float> listOfTime = new List<float>();
    private IEnumerator currentTask;
    UnityEvent unityEvent = new UnityEvent();
    // Create a UnityAction object that represents a method

    // The name of the current scene
    private string sceneName;

    

    public int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        GameObject newCube = GameObject.Find("cube");
        //renderer = cube:GetComponent("Renderer")
        // Get the renderer component of the cube
        cubeRenderer = cube.GetComponent<Renderer>();

        // Get the name of the current scene
        sceneName = SceneManager.GetActiveScene().name;
        sceneName += ".txt";


        // Add a Rigidbody component to the cube game object and set its useGravity property to true
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Initialize the userOutTextFunctionDispaly variable
        //userOutTextFunctionDispaly = GameObject.Find("FunctionDisplayText").GetComponent<TextMeshProUGUI>();
       

        

    }

    // Update is called once per frame
    void Update()
    {
        // Invoke any tasks that are currently registered with the UnityEvent
        unityEvent.Invoke();

        // Check if the cube's y-position is less than -10
        if (cube.transform.position.y < -10)
        {
            // Display a message in the debug log
            Debug.Log("You are dead!");
        }


    }



    // This method loops through the list of tasks and waits the specified amount of time before executing each one.
    private IEnumerator DoTask()
    {
        for (int i = 0; i < listOfTasks.Count; i++)
        {
            // Update the userOutTextFunctionDispaly variable to show the name of the function being executed
            userOutTextFunctionDispaly.text = listOfTasks[i].Method.Name;

            // Add the task to the UnityEvent and wait the specified amount of time.
            unityEvent.AddListener(listOfTasks[i]);
            yield return new WaitForSeconds(listOfTime[i]);

            // Remove the task from the UnityEvent.
            unityEvent.RemoveListener(listOfTasks[i]);


        }

        // Clear the list of tasks and times.
        listOfTasks.Clear();
        listOfTime.Clear();
    }
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
        //ResetCubePosition();

    }

    public void SetCubeColor(float r, float g, float b)
    {
        // set the cube color using the r, g, and b values provided by the user
        newCubeColor = new Color(r, g, b, 1f);
        cubeRenderer.material.SetColor("_Color", newCubeColor);

        // update the predefined color values so they match the new color of the cube
        randomChannelOne = r;
        randomChannelTwo = g;
        randomChannelThree = b;

        userOutTextFunctionDispaly.text = "SetCubeColor";
    }
    public void SetCubeSize(float size)
    {
        // set the scale of the cube to the specified size
        cube.transform.localScale = new Vector3(size, size, size);
    }
    public void MoveForward(float Time = 1f)
    {
        print($"MoveForward {Time}");
        listOfTasks.Add(MoveF);
        listOfTime.Add(Time);
    }
    private void MoveRight(float Time = 1f)
    {
        print($"MoveRight {Time}");
        listOfTasks.Add(MoveR);
        listOfTime.Add(Time);
    }
    private void MoveLeft(float Time = 1f)
    {
        print($"MoveLeft {Time}");
        listOfTasks.Add(MoveL);
        listOfTime.Add(Time);
    }
    private void MoveBack(float Time = 1f)
    {
        print($"MoveBack {Time}");
        listOfTasks.Add(MoveB);
        listOfTime.Add(Time);
    }
    private void Wait(float Time = 1f)
    {

        print($"Wait {Time}");
        listOfTasks.Add(() => { });
        listOfTime.Add(Time);

    }
    // The "move" function moves the cube by the specified amount in the specified direction.
    private void Move(double distance, string direction, double delay)
    {
        Vector3 displacement = new Vector3(0, 0, 0);

        // Set the displacement based on the specified direction
        if (direction == "up")
        {
            displacement = new Vector3(0, (float)distance * speed * Time.deltaTime, 0);
        }
        else if (direction == "down")
        {
            displacement = new Vector3(0, -(float)distance * speed * Time.deltaTime, 0);
        }
        else if (direction == "left")
        {
            displacement = new Vector3(-(float)distance * speed * Time.deltaTime, 0, 0);
        }
        else if (direction == "right")
        {
            displacement = new Vector3((float)distance * speed * Time.deltaTime, 0, 0);
        }
        else if (direction == "forward")
        {
            displacement = new Vector3(0, 0, (float)distance * speed * Time.deltaTime);
        }
        else if (direction == "backward")
        {
            displacement = new Vector3(0, 0, -(float)distance * speed * Time.deltaTime);
        }

        // Add a task to the list of tasks that moves the cube by the specified amount in the specified direction.
        // The task will wait for the specified delay plus the time delta before executing.
        listOfTasks.Add(() => cube.transform.position += displacement);
        listOfTime.Add((float)delay + Time.deltaTime);
    }



    private void MoveF() => cube.transform.position += Vector3.forward * speed * Time.deltaTime;
    private void MoveR() => cube.transform.position += Vector3.right * speed * Time.deltaTime;
    private void MoveB() => cube.transform.position -= Vector3.forward * speed * Time.deltaTime;
    private void MoveL() => cube.transform.position -= Vector3.right * speed * Time.deltaTime;


    // This method sets the speed at which the cube moves.
    private void SetCubeSpeed(float speed)
    {
        // Set the speed field to the specified value
        userOutTextFunctionDispaly.text = "SetCubeSpeed";
        this.speed = speed;
    }

    private void StartLua(string script)
    {
        // Create a new instance of the Lua interpreter
        Script lua = new Script();


        //lua.Globals["this"] = this;

        //lua.Globals["newCube"] = newCube;

        // Register the "print" function so that the script can print messages to the debug log
        lua.Globals["print"] = (Action<DynValue>)PrintToDebugLogAndTextArea; ;

        // Register the custom functions that the script can call
        RegisterFunctions(lua);

        // Execute the script
        lua.DoString(script);
    }
    // This method prints the type and value of the specified value to the debug log and the Text object
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
    // The "turn" function turns the cube in the specified direction after waiting for the specified amount of time.

    private void Turn(string direction)
    {
        Vector3 rotation = new Vector3(0, 0, 0);

        // Set the rotation based on the specified direction
        if (direction == "left")
        {
            rotation = new Vector3(0, -90, 0);
        }
        else if (direction == "right")
        {
            rotation = new Vector3(0, 90, 0);
        }
        else if (direction == "back")
        {
            rotation = new Vector3(0, 180, 0);
        }
        cube.transform.Rotate(rotation);
    }
    public void Teleport(float x, float y, float z)
    {
        Debug.Log("Teleport");
        Debug.Log(x);
        Debug.Log(y);
        Debug.Log(z);
        // Get the current position of the game object
        Vector3 currentPosition = transform.position;

        // Set the new position of the game object
        currentPosition = new Vector3(x, y, z);

        // Update the transform's position
        cube.transform.position = currentPosition;
    }
    // This method registers the custom functions that the script can call.
    private void RegisterFunctions(Script lua)
    {

        // Register the "SetCubeColor" function
        lua.Globals["SetCubeColor"] = (Action<float, float, float>)SetCubeColor;

        // Register the "SetCubeSize" function
        lua.Globals["SetCubeSize"] = (Action<float>)SetCubeSize;

        // Register the "SetCubeSpeed" function
        lua.Globals["SetCubeSpeed"] = (Action<float>)SetCubeSpeed;

        //functions to move the cube
        lua.Globals["MoveForward"] = (Action<float>)MoveForward;
        lua.Globals["MoveRight"] = (Action<float>)MoveRight;
        lua.Globals["MoveLeft"] = (Action<float>)MoveLeft;
        lua.Globals["MoveBack"] = (Action<float>)MoveBack;
        lua.Globals["Move"] = (Action<double, string, double>)Move;
        lua.Globals["Turn"] = (Action<string>)Turn;
        lua.Globals["Wait"] = (Action<float>)Wait;

        lua.Globals["Teleport"] = (Action<float, float, float>)Teleport;


        /*-- Set the cube's color to blue
SetCubeColor(0, 0, 1)

-- Wait for 2 seconds
Wait(1)

SetCubeSize(5)

SetCubeSpeed(4)
Wait(1)
-- Set the cube's color to red
SetCubeColor(1, 0, 0)

-- Move the cube forward by 1 unit
MoveForward(1)

-- Wait for 1 second
Wait(1)

-- Move the cube right by 1 unit
MoveRight(1)
Wait(1)
MoveBack(1)

        */
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
