using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter.Debugging;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.Shapes;
using Newtonsoft.Json.Linq;
using UnityEngine.SocialPlatforms;

public class ScriptManager : MonoBehaviour
{
    // Declare the uIManager field
    private UIManager uIManager;

    // The cube object
    [SerializeField]
    private GameObject cube;
    private Renderer cubeRenderer;


    // Variables for the cube's color and size
    private Color newCubeColor;
    private float randomChannelOne, randomChannelTwo, randomChannelThree;
    private float speed = 1;
    //private float cubeSize = 1.0f;

    // Variables for managing tasks
    private List<UnityAction> listOfTasks = new List<UnityAction>();
    private List<float> listOfTime = new List<float>();
    private IEnumerator currentTask;
    UnityEvent unityEvent = new UnityEvent();
    // Create a UnityAction object that represents a method


    public int score = 0;
    float time = 0.0001f;
    float defaultTime = 1f;

    // Set up bool variables to indicate whether something is in front/back/left/right of the cube
    public bool somethingInFront = false;
    public bool somethingInBack = false;
    public bool somethingInLeft = false;
    public bool somethingInRight = false;

    // Start is called before the first frame update
    void Start()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        GameObject newCube = GameObject.Find("cube");
        //renderer = cube:GetComponent("Renderer")
        // Get the renderer component of the cube
        cubeRenderer = cube.GetComponent<Renderer>();

        

        // Add a Rigidbody component to the cube game object and set its useGravity property to true
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        rb.useGravity = true;

        string data = DataInputHoldingData.instance.dataInput;

        if (data != null || data != "")
        {
            uIManager.userInputField.text = data;
            
        }

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
            uIManager.RestartScene();
        }
    }
    // Set up a function that takes a string as an argument
    // The string represents the direction that we want to check
    bool CheckForObjectWithTagInDirection(string direction, string tag, float maxDistance = 1f)
    {
        // Set the starting position of the ray
        Vector3 startingPosition = cube.transform.position;


        // Set up a variable to store the result of the raycast
        RaycastHit hit;

        // Check for objects in the specified direction
        if (direction == "front")
        {
            // Check for objects in front of the cube
            Vector3 forwardDirection = cube.transform.forward;
            if (Physics.Raycast(startingPosition, forwardDirection, out hit, maxDistance))
            {
                if (hit.transform.tag == tag)
                {
                    // An object with the specified tag was found in the specified direction
                    return true;
                }
                else
                {
                    // An object was found in the specified direction, but it doesn't have the specified tag
                    return false;
                }
            }
            else
            {
                // No object was found in the specified direction
                return false;
            }
        }
        else if (direction == "back")
        {
            // Check for objects behind the cube
            Vector3 backDirection = -cube.transform.forward;
            if (Physics.Raycast(startingPosition, backDirection, out hit, maxDistance))
            {
                if (hit.transform.tag == tag)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else if (direction == "left")
        {
            // Check for objects to the left of the cube
            Vector3 leftDirection = -cube.transform.right;
            if (Physics.Raycast(startingPosition, leftDirection, out hit, maxDistance))
            {
                if (hit.transform.tag == tag)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else if (direction == "right")
        {
            // Check for objects to the right of the cube
            Vector3 rightDirection = cube.transform.right;
            if (Physics.Raycast(startingPosition, rightDirection, out hit, maxDistance))
            {
                if (hit.transform.tag == tag)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            // The specified direction is not valid
            Debug.LogError("Invalid direction: " + direction);
            return false;
        }
    }



    public Vector3 GetCubePosition()
    {
        // Get the position of the cube
        Vector3 position = cube.transform.position;

        // Return the position of the cube
        return position;
    }
    

    // This method loops through the list of tasks and waits the specified amount of time before executing each one.
    private IEnumerator DoTask()
    {
        for (int i = 0; i < listOfTasks.Count; i++)
        {
            // Update the userOutTextFunctionDispaly variable to show the name of the function being executed
            uIManager.userOutTextFunctionDispaly.text = listOfTasks[i].Method.Name;

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
    public void InputText()
    {
        //stopping all corotines
        StopAllCoroutines();

        listOfTasks = new List<UnityAction>();
        listOfTime = new List<float>();

        try
        {
            // Get the script text from the input field
            string script = uIManager.inputField.GetComponent<TMP_InputField>().text;
            Debug.Log("script: " + script);


            StartLua(script);
            uIManager.userOutTextForDebug.text = "None Error messages from Lua";

            
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

        ResetCubeData();


    }

    public void SetCubeColor(float r, float g, float b)
    {
        // set the cube color using the r, g, and b values provided by the user
        newCubeColor = new Color(r, g, b, 1f);
        cubeRenderer.material.SetColor("_Color", newCubeColor);

        // update the predefined color values so they match the new color of the cube
        //randomChannelOne = r;
        //randomChannelTwo = g;
        //randomChannelThree = b;

        uIManager.UserOutTextFunctionDispaly("SetCubeColor");

        listOfTasks.Add(() => { });
        listOfTime.Add(time);
    }
    public void SetCubeSize(float size)
    {

        uIManager.UserOutTextFunctionDispaly("SetCubeSize");
        // set the scale of the cube to the specified size
        //cube.transform.localScale = new Vector3(size, size, size);

        listOfTasks.Add(() => cube.transform.localScale = new Vector3(size, size, size));
        listOfTime.Add(time);
    }
    public void MoveForward(float Time = 1f)
    {
        uIManager.UserOutTextFunctionDispaly("MoveForward");
        print($"MoveForward {Time}");
        listOfTasks.Add(MoveF);
        listOfTime.Add(Time);
    }
    private void MoveRight(float Time = 1f)
    {
        uIManager.UserOutTextFunctionDispaly("MoveRight");
        print($"MoveRight {Time}");
        listOfTasks.Add(MoveR);
        listOfTime.Add(Time);
    }
    private void MoveLeft(float Time = 1f)
    {
        uIManager.UserOutTextFunctionDispaly("MoveLeft");
        print($"MoveLeft {Time}");
        listOfTasks.Add(MoveL);
        listOfTime.Add(Time);
    }
    private void MoveBack(float Time = 1f)
    {
        uIManager.UserOutTextFunctionDispaly("MoveBack");
        print($"MoveBack {Time}");
        listOfTasks.Add(MoveB);
        listOfTime.Add(Time);
    }
    private void Wait(float Time = 1f)
    {
        uIManager.UserOutTextFunctionDispaly("Wait");
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
        uIManager.UserOutTextFunctionDispaly("Move");
        // Add a task to the list of tasks that moves the cube by the specified amount in the specified direction.
        // The task will wait for the specified delay plus the time delta before executing.
        listOfTasks.Add(() => cube.transform.position += displacement);
        listOfTime.Add((float)delay + Time.deltaTime);
    }

    private void MoveF() => cube.transform.position += cube.transform.forward * speed * Time.deltaTime;
    private void MoveR() => cube.transform.position += cube.transform.right * speed * Time.deltaTime;
    private void MoveB() => cube.transform.position -= cube.transform.forward * speed * Time.deltaTime;
    private void MoveL() => cube.transform.position -= cube.transform.right * speed * Time.deltaTime;

    // The "turn" function turns the cube in the specified direction after waiting for the specified amount of time.

    private void Turn(string direction)
    {
        //UserOutTextFunctionDispaly("Turn");
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
        //listOfTasks.Add(() => cube.transform.position += displacement);
        //cube.transform.Rotate(rotation);
        listOfTasks.Add(() => cube.transform.Rotate(rotation * Time.deltaTime));
        listOfTime.Add(1);
    }
    public void Teleport(float x, float y, float z)
    {

        uIManager.UserOutTextFunctionDispaly("Teleport");
        // Get the current position of the game object
        Vector3 currentPosition = transform.position;

        // Set the new position of the game object
        currentPosition = new Vector3(x, y, z);

        // Update the transform's position
        //cube.transform.position = currentPosition;

        listOfTasks.Add(() => cube.transform.position = currentPosition);
        listOfTime.Add(time);

    }
    // This method sets the speed at which the cube moves.
    private void SetCubeSpeed(float speed)
    {
        uIManager.UserOutTextFunctionDispaly("SetCubeSpeed");
        // Set the speed field to the specified value
        //this.speed = speed;
        listOfTasks.Add(() => this.speed = speed);
        listOfTime.Add(time);
    }

    private void StartLua(string script)
    {
        // Create a new instance of the Lua interpreter
        Script lua = new Script();

        // Register the "print" function so that the script can print messages to the debug log

        lua.Globals["print"] = (Action<DynValue>)uIManager.PrintToDebugLogAndTextArea;

        // Register the custom functions that the script can call
        RegisterFunctions(lua);

        RegisterVeriables(lua);
        // Execute the script
        lua.DoString(script);
    }
    private void RegisterVeriables(Script lua)
    {
        //// Set the value of the "somethingInFront" global variable in the Lua script
        //DynValue dynValue = DynValue.NewBoolean(somethingInFront);
        //lua.Globals.Set("somethingInFront", dynValue);

        //// Set the value of the "somethingInBack" global variable in the Lua script
        //dynValue = DynValue.NewBoolean(somethingInBack);
        //lua.Globals.Set("somethingInBack", dynValue);

        //// Set the value of the "somethingInLeft" global variable in the Lua script
        //dynValue = DynValue.NewBoolean(somethingInLeft);
        //lua.Globals.Set("somethingInLeft", dynValue);

        //// Set the value of the "somethingInRight" global variable in the Lua script
        //dynValue = DynValue.NewBoolean(somethingInRight);
        //lua.Globals.Set("somethingInRight", dynValue);
    }


//print("Something is in front of the cube!")
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
        lua .Globals["MoveRight"] = (Action<float>)MoveRight;
        lua.Globals["MoveLeft"] = (Action<float>)MoveLeft;
        lua.Globals["MoveBack"] = (Action<float>)MoveBack;
        lua.Globals["Move"] = (Action<double, string, double>)Move;
        lua.Globals["Turn"] = (Action<string>)Turn;
        lua.Globals["Wait"] = (Action<float>)Wait;

        lua.Globals["Teleport"] = (Action<float, float, float>)Teleport;

        // Register the "CheckForObjectWithTagInDirection" function
        lua.Globals["CheckForObjectWithTagInDirection"] = (Func<string, string,float, bool>)CheckForObjectWithTagInDirection;

        // Register the "GetCubePosition" function
        lua.Globals["GetCubePosition"] = (Func<Vector3>)GetCubePosition;


        /*-- Examples
MoveForward()
MoveRight()
MoveBack()
MoveLeft()
Turn("left")
MoveForward()
Turn("right")
Teleport(0,0,0)
SetCubeSize(5)
Wait()
SetCubeSize(1)
SetCubeSpeed(5)
SetCubeColor(5,5,5)
        */
    }
    // This method prints the type and value of the specified value to the debug log and the Text object
    

    public void Stop()
    {
        // Clear the list of tasks and stop the current task, if any
        listOfTasks.Clear();
        listOfTime.Clear();
        StopCoroutine(currentTask);
        unityEvent.RemoveAllListeners();
    }
    public void ResetCubeData()
    {
        // reset the position of the cube to its original position
        // Reset the position of the cube
        cube.transform.position = Vector3.zero;

        // Reset the color of the cube
        cubeRenderer.material.color = Color.black;

        // Reset the size of the cube
        cube.transform.localScale = Vector3.one;

        
    }
}
