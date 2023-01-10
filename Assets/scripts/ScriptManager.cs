using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.Events;


public class ScriptManager : MonoBehaviour
{
    // Declare the uIManager field
    private UIManager uIManager;

    // The Player object
    [SerializeField]
    private GameObject player;
    private Renderer playerRenderer;

    // Variables for the Player's color and size
    private Color newPlayerColor;
    private float speed = 1;
    //private float PlayerSize = 1.0f;

    // Variables for managing tasks
    private List<UnityAction> listOfTasks = new List<UnityAction>();
    private List<float> listOfTime = new List<float>();
    private IEnumerator currentTask;
    UnityEvent unityEvent = new UnityEvent();
    // Create a UnityAction object that represents a method

    public int score = 0;
    float time = 0.0001f;
    //float defaultTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        GameObject newPlayer = GameObject.Find("Player");
        //renderer = Player:GetComponent("Renderer")
        // Get the renderer component of the Player
        playerRenderer = player.GetComponent<Renderer>();

        // Add a Rigidbody component to the Player game object and set its useGravity property to true
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.useGravity = true;
        string data = DataInputHoldingData.instance.dataInput;
        if (data != null || data != "")
        {
            uIManager.userInputField.text = data;
            
        }
    }
    public void Jump(float jumpForce =10)
    {
        // Get the Rigidbody component attached to the player object
        Rigidbody rb = player.GetComponent<Rigidbody>();

        // Apply a force in the upward direction
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        // Check if the Player's y-position is less than -10
        if (player.transform.position.y < -10)
        {
            // Display a message in the debug log
            Debug.Log("You are dead!");
            uIManager.RestartScene();
        }
    }
    private void FixedUpdate()
    {
                
        // Invoke any tasks that are currently registered with the UnityEvent
        unityEvent.Invoke();

    }
    public Vector3 GetPlayerPosition()
    {
        // Get the position of the Player
        Vector3 position = player.transform.position;

        // Return the position of the Player
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
        ResetPlayerData();
    }

    public void SetPlayerColor(float r, float g, float b)
    {
        newPlayerColor = new Color(r, g, b, 1f);
        playerRenderer.material.SetColor("_Color", newPlayerColor);
        uIManager.UserOutTextFunctionDispaly("SetPlayerColor");
        listOfTasks.Add(() => { });
        listOfTime.Add(time);
    }
    public void SetPlayerSize(float size)
    {
        uIManager.UserOutTextFunctionDispaly("SetPlayerSize");
        listOfTasks.Add(() => player.transform.localScale = new Vector3(size, size, size));
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
    private void Wait(float time = 1f)
    {
        uIManager.UserOutTextFunctionDispaly("Wait");
        print($"Wait {time}");
        listOfTasks.Add(() => { });
        listOfTime.Add(time);
        // Start the WaitCoroutine coroutine with the specified time
        StartCoroutine(WaitCoroutine(time));
    }
    private IEnumerator WaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

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
        listOfTasks.Add(() => player.transform.position += displacement);
        listOfTime.Add((float)delay + Time.deltaTime);
    }
    private void MoveF() => player.transform.position += player.transform.forward * speed * Time.deltaTime;
    private void MoveR() => player.transform.position += player.transform.right * speed * Time.deltaTime;
    private void MoveB() => player.transform.position -= player.transform.forward * speed * Time.deltaTime;
    private void MoveL() => player.transform.position -= player.transform.right * speed * Time.deltaTime;

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
        //listOfTasks.Add(() => Player.transform.position += displacement);
        //Player.transform.Rotate(rotation);
        listOfTasks.Add(() => player.transform.Rotate(rotation * Time.deltaTime));
        listOfTime.Add(1);
    }
    public void Teleport(float x, float y, float z)
    {
        uIManager.UserOutTextFunctionDispaly("Teleport");
        // Get the current position of the game object
        Vector3 currentPosition = transform.position;

        // Set the new position of the game object
        currentPosition = new Vector3(x, y, z);

        listOfTasks.Add(() => player.transform.position = currentPosition);
        listOfTime.Add(time);

    }
    // This method sets the speed at which the Player moves.
    private void SetPlayerSpeed(float speed)
    {
        uIManager.UserOutTextFunctionDispaly("SetPlayerSpeed");
        listOfTasks.Add(() => this.speed = speed);
        listOfTime.Add(time);
    }


    private void StartLua(string script)
    {
        Script lua = new Script();

        // Register the "print" function so that the script can print messages to the debug log

        lua.Globals["print"] = (Action<DynValue>)uIManager.PrintToDebugLogAndTextArea;

        // Register the custom functions that the script can call
        RegisterFunctions(lua);

        // Execute the script
        lua.DoString(script);
    }

    private void RegisterFunctions(Script lua)
    {
        lua.Globals["SetPlayerColor"] = (Action<float, float, float>)SetPlayerColor;
        lua.Globals["SetPlayerSize"] = (Action<float>)SetPlayerSize;
        lua.Globals["SetPlayerSpeed"] = (Action<float>)SetPlayerSpeed;
        lua.Globals["MoveForward"] = (Action<float>)MoveForward;
        lua .Globals["MoveRight"] = (Action<float>)MoveRight;
        lua.Globals["MoveLeft"] = (Action<float>)MoveLeft;
        lua.Globals["MoveBack"] = (Action<float>)MoveBack;
        lua.Globals["Move"] = (Action<double, string, double>)Move;
        lua.Globals["Turn"] = (Action<string>)Turn;
        lua.Globals["Wait"] = (Action<float>)Wait;
        lua.Globals["Teleport"] = (Action<float, float, float>)Teleport;
        lua.Globals["WhatsInFront"] = (Func<string, string,float, bool>)CheckForObjectWithTagInDirection;
        lua.Globals["GetPlayerPosition"] = (Func<Vector3>)GetPlayerPosition;
    }


    public void Stop()
    {
        // Clear the list of tasks and stop the current task, if any
        listOfTasks.Clear();
        listOfTime.Clear();
        StopCoroutine(currentTask);
        unityEvent.RemoveAllListeners();
    }
    public void ResetPlayerData()
    {
        player.transform.position = Vector3.zero;
        playerRenderer.material.color = Color.black;
        player.transform.localScale = Vector3.one;
        player.transform.rotation = Quaternion.identity;
    }

    // Set up a function that takes a string as an argument
    // The string represents the direction that we want to check
    bool CheckForObjectWithTagInDirection(string direction, string tag, float maxDistance = 1f)
    {
        // Set the starting position of the ray
        Vector3 startingPosition = player.transform.position;


        // Set up a variable to store the result of the raycast
        RaycastHit hit;

        // Check for objects in the specified direction
        if (direction == "front")
        {
            // Check for objects in front of the Player
            Vector3 forwardDirection = player.transform.forward;
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
            // Check for objects behind the Player
            Vector3 backDirection = -player.transform.forward;
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
            // Check for objects to the left of the Player
            Vector3 leftDirection = -player.transform.right;
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
            // Check for objects to the right of the Player
            Vector3 rightDirection = player.transform.right;
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


}
