using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class ScriptManager : MonoBehaviour
{
    UIManager uIManager;
    [SerializeField] private GameObject player;

    [SerializeField] private float movementSpeed = 1;
    [SerializeField] UnityEngine.UI.Slider speedSlider;
    [SerializeField] float speedProcent = 1f;//controled by slider

    //start pos/rot
    Vector3 _startPosMovment;
    Vector3 _startRotMovment;

    //end pos/rot
    Vector3 _endPosMovment;
    Vector3 _endRotMovment;



    bool _iEnumeratorTimerOn = false;
    float _iEnumeratorTime = -1f;

    bool _isMoving = false;
    bool _isRotating = false;

    float _movmentCurrentTimer = -1;
    float _setMovmentTimer = -1;

    string code;



    private Vector3 Vector3Lerp(Vector3 a, Vector3 b, float t)
    {
        float x = Mathf.Lerp(a.x, b.x, t);
        float y = Mathf.Lerp(a.y, b.y, t);
        float z = Mathf.Lerp(a.z, b.z, t);

        return new Vector3(x, y, z);
    }
    private void _SetMovmentTimer(int time)
    {
        _movmentCurrentTimer = 0.000001f;
        _setMovmentTimer = (time * speedProcent) / movementSpeed;
    }
    private bool WhatsInFront(string direction, string objectType, float distance)
    {
        uIManager.UserOutTextFunctionDispaly("Checking if " + objectType + " is " + distance + " in front of me");
        RaycastHit hit;
        Vector3 directionVector = player.transform.forward;
        if (direction == "right")
        {
            directionVector = player.transform.right;
        }
        if (direction == "left")
        {
            directionVector = -player.transform.right;
        }
        if (direction == "back")
        {
            directionVector = -player.transform.forward;
        }
        if (Physics.Raycast(player.transform.position, directionVector, out hit, distance))
        {
            if (hit.collider.gameObject.tag == objectType)
            {
                uIManager.UserOutTextFunctionDispaly("There is a " + objectType + " in front of me");
                return true;
            }
            else
            {
                uIManager.UserOutTextFunctionDispaly("There is not a " + objectType + " in front of me");
                return false;
            }
        }
        else
        {
            uIManager.UserOutTextFunctionDispaly("There is not a " + objectType + " in front of me");
            return false;
        }
    }
    private void SetPlayerSpeed(float speed)
    {
        movementSpeed = speed;
    }
    private void Move(Vector3 Direction, int Distance)
    {
        uIManager.UserOutTextFunctionDispaly("Moving " + Distance + " units in direction " + Direction);

        Wait(Distance);
        _SetMovmentTimer(Distance);
        _startPosMovment = player.transform.position;
        _endPosMovment += Direction * Distance;
        _isMoving = true;
    }
    private void Turn(int Angle)
    {
        uIManager.UserOutTextFunctionDispaly("Turning " + Angle + " degrees");

        Wait(1);
        _SetMovmentTimer(1);
        _endRotMovment += new Vector3Int(0, Angle, 0);
        _isRotating = true;
    }

    public void TurnRight() => Turn(90);
    public void TurnLeft() => Turn(-90);
    public void MoveForward(int Distance = 1) => Move(player.transform.forward, Distance);
    public void MoveRight(int Distance = 1) => Move(player.transform.right, Distance);
    public void MoveBack(int Distance = 1) => Move(-player.transform.forward, Distance);
    public void MoveLeft(int Distance = 1) => Move(-player.transform.right, Distance);
    public void Wait(float seconds = 1)
    {
        uIManager.UserOutTextFunctionDispaly("Waiting for  " + seconds + " Seconds");
        _iEnumeratorTimerOn = true;
        _iEnumeratorTime = seconds * speedProcent;
    }
    // This method starts a new game and retrieves the data from DataInputHoldingData
    public void NewGame()
    {
        // Retrieve the data input from the DataInputHoldingData instance
        string data = DataInputHoldingData.instance.dataInput;

        // Check if the data input is not null or an empty string
        if (data != null || data != "")
        {
            // Set the text of the user input field in the UI Manager to the data input
            uIManager.userInputField.text = data;
        }
    }

    public void Stop()
    {
        _isMoving = false;
        _isRotating = false;
        _movmentCurrentTimer = -1;

        _iEnumeratorTimerOn = false;
        _iEnumeratorTime = -1;
        StopAllCoroutines();
    }
    //function that will stop all timers and coroutines, restart player postion and rotatation
    public void StopAll()
    {
        StopAllCoroutines();
        player.transform.position = new Vector3(0, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        _startPosMovment = new Vector3(0, 0, 0);
        _startRotMovment = new Vector3(0, 0, 0);
        _endPosMovment = new Vector3(0, 0, 0);
        _endRotMovment = new Vector3(0, 0, 0);
        Stop();
    }
    public void InputText()
    {
        // Stop any previous execution of the code
        StopAll();

        // Get the text from the input field
        code = uIManager.inputField.GetComponent<TMP_InputField>().text;

        // Start executing the code
        StartCoroutine(_Start());
    }

    //create new function  SetPlayerSpeed
    private IEnumerator _Start()
    {
        // Concatenate the function declaration and the code from the input field
        var _readyCode = "function Start()\n" + code + "\nend";
        // Load the code and get the returned function using the MoonSharp script engine
        Script script = new Script(CoreModules.None);

        // Register global functions and objects that can be used in the code
        script.Globals["MoveForward"] = (Action<int>)MoveForward;
        script.Globals["MoveRight"] = (Action<int>)MoveRight;
        script.Globals["MoveBack"] = (Action<int>)MoveBack;
        script.Globals["MoveLeft"] = (Action<int>)MoveLeft;
        script.Globals["TurnRight"] = (Action)TurnRight;
        script.Globals["TurnLeft"] = (Action)TurnLeft;
        script.Globals["Wait"] = (Action<float>)Wait;
        script.Globals["WhatsInFront"] = (Func<string, string, float, bool>)WhatsInFront;
        script.Globals["print"] = (Action<DynValue>)uIManager.PrintToDebugLogAndTextArea;
        script.Globals["SetPlayerSpeed"] = (Action<float>)SetPlayerSpeed;

        // Initialize variables to detect errors
        bool errorOccurred = false;
        DynValue coroutine = null;
        DynValue result = null;
        try
        {
            // Execute the code and get the coroutine
            script.DoString(_readyCode);
            DynValue function = script.Globals.Get("Start");
            coroutine = script.CreateCoroutine(function);
            coroutine.Coroutine.AutoYieldCounter = 2;
            result = coroutine.Coroutine.Resume();
        }
        catch (SyntaxErrorException ex)
        {
            // If a syntax error was detected, display an error message to the user
            Debug.Log("Syntax error: " + ex.Message);
            uIManager.userOutTextForDebug.text = "Syntax error: " + ex.Message;
        }
        catch (ScriptRuntimeException ex)
        {
            // If a runtime error was detected, display an error message to the user
            Debug.Log("Runtime error: " + ex.DecoratedMessage);
            uIManager.userOutTextForDebug.text = "Runtime error: " + ex.DecoratedMessage;
        }
        catch (Exception ex)
        {
            // If any other exception was thrown, display the error message to the user
            Debug.Log("Error: " + ex.Message);
            uIManager.userOutTextForDebug.text = "Error: " + ex.Message;
        }
        // If no error was detected, execute the coroutine
        if (!errorOccurred)
        {
            // Check if there was no error
            while (result.Type == DataType.YieldRequest)
            {
                // If the result type is a YieldRequest
                if (_iEnumeratorTimerOn)
                {
                    // If the iEnumeratorTimer is on
                    yield return new WaitForSeconds(_iEnumeratorTime);
                    // Wait for the specified amount of time in _iEnumeratorTime
                    _iEnumeratorTimerOn = false;
                    // Turn off the iEnumeratorTimer
                    _iEnumeratorTime = -1;
                    // Reset the value of _iEnumeratorTime
                }
                yield return new WaitForSeconds(.1f * speedProcent);
                // Wait for .1 seconds multiplied by the speedProcent value
                result = coroutine.Coroutine.Resume();
                // Resume the coroutine
            }
        }

    }

    private void Update()
    {
        // Check if the movement is in progress
        if (_isMoving)
        {
            // Increment the movement timer by the delta time
            _movmentCurrentTimer += Time.deltaTime;

            // Calculate the current position of the player based on the start position, end position, and current timer
            player.transform.position = Vector3Lerp(_startPosMovment, _endPosMovment, _movmentCurrentTimer / _setMovmentTimer);

            // Check if the current timer is greater than or equal to the set movement timer
            if (_movmentCurrentTimer >= _setMovmentTimer)
            {
                // Update the start position to be the end position
                _startPosMovment = _endPosMovment;
                // Update the player's position to the end position
                player.transform.position = _endPosMovment;
                // Set the movement flag to false
                _isMoving = false;
            }
        }
        // Check if the rotation is in progress
        if (_isRotating)
        {
            // Increment the movement timer by the delta time
            _movmentCurrentTimer += Time.deltaTime;

            // Calculate the current rotation of the player based on the start rotation, end rotation, and current timer
            player.transform.rotation = Quaternion.Euler(Vector3Lerp(_startRotMovment, _endRotMovment, _movmentCurrentTimer / _setMovmentTimer));

            // Check if the current timer is greater than or equal to the set movement timer
            if (_movmentCurrentTimer >= _setMovmentTimer)
            {
                // Update the start rotation to be the end rotation
                _startRotMovment = _endRotMovment;
                // Update the player's rotation to the end rotation
                player.transform.rotation = Quaternion.Euler(_endRotMovment);
                // Set the rotation flag to false
                _isRotating = false;
            }
        }
        // Check if the Player's y-position is less than -10
        if (player.transform.position.y < -10)
        {
            // Display a message in the debug log
            Debug.Log("You are dead!");
            // Call the RestartScene method on the UI manager
            uIManager.RestartScene();
        }
    }

    private void Start()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        speedSlider.onValueChanged.AddListener(sliderVal => { speedProcent = sliderVal; });
        NewGame();

        //get start position and rotetion of player object
        _startPosMovment = player.transform.position;
        _startRotMovment = player.transform.rotation.eulerAngles;
    }
}
