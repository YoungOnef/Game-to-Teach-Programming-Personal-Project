using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;
using TMPro;
using System;


public class ScriptManager : MonoBehaviour
{
    UIManager uIManager;
    [SerializeField]
    private GameObject player;


    [SerializeField] bool __TimerON = false;
    [SerializeField] float __TimerTime = -1f;
    [SerializeField]
    bool Moving = false;
    float MovingTimer = -1;
    string code;
    Vector3 directionMovement = new Vector3(0, 0, 0);
    Vector3 directionTurn = new Vector3Int(0, 0, 0);
    Vector3 currnetTurnAngle = new Vector3Int(0, 0, 0);


    private void Move(Vector3 Direction, int Distance)
    {
        Wait(Distance);
        MovingTimer = Distance;
        directionMovement = Direction;
        Moving = true;
    }
    private void Turn(int Angle)
    {
        Wait(1);
        MovingTimer = 1;
        directionTurn = new Vector3Int(0, Angle, 0);
        currnetTurnAngle += new Vector3Int(0, Angle, 0);
        Moving = true;
    }

    public void TurnRight() => Turn(90);
    public void TurnLeft() => Turn(-90);


    public void MoveForward(int Distance = 1) => Move(player.transform.forward,Distance);
    public void MoveRight(int Distance = 1) => Move(player.transform.right, Distance);
    public void MoveBack(int Distance = 1) => Move(-player.transform.forward, Distance);
    public void MoveLeft(int Distance = 1) => Move(-player.transform.right, Distance);
    public void Wait(float seconds = 1)
    {
        __TimerON = true;
        __TimerTime = seconds;
    }
    private void Start()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    private IEnumerator _Start()
    {
        var _readyCode = "function Start()\n" + code + "\nend";
        // Load the code and get the returned function
        Script script = new Script(CoreModules.None);

        
        script.Globals["MoveForward"] = (Action<int>)MoveForward;
        script.Globals["MoveRight"] = (Action<int>)MoveRight;
        script.Globals["MoveBack"] = (Action<int>)MoveBack;
        script.Globals["MoveLeft"] = (Action<int>)MoveLeft;

        script.Globals["TurnRight"] = (Action)TurnRight;
        script.Globals["TurnLeft"] = (Action)TurnLeft;

        script.Globals["Wait"] = (Action<float>)Wait;

        script.Globals["WhatsInFront"] = (Func<string, string, float, bool>)WhatsInFront;

        script.Globals["print"] = (Action<DynValue>)uIManager.PrintToDebugLogAndTextArea;

        //script.LoadFunction("function Wait (n) return ('Wait' ,n) end \n");
        script.DoString(_readyCode);

        // get the function
        DynValue function = script.Globals.Get("Start");//start funcion

        // Create the coroutine in C#
        DynValue coroutine = script.CreateCoroutine(function);

        coroutine.Coroutine.AutoYieldCounter = 2;//line by line yeld return ????

        DynValue result = null;

        result = coroutine.Coroutine.Resume();
        while (true)
        {

            if (result.Type != DataType.YieldRequest)
                break;

            if (__TimerON)
            {
                yield return new WaitForSeconds(__TimerTime);
                __TimerON = false;
                __TimerTime = -1;
            }
            yield return new WaitForSeconds(.1f);
            result = coroutine.Coroutine.Resume();
        }
    }
    private void Update()
    {
        if (Moving)
        {
            MovingTimer -= Time.deltaTime;
            player.transform.position += directionMovement * Time.deltaTime;
            player.transform.Rotate(directionTurn * Time.deltaTime);
            if (MovingTimer <= 0)
            {
                player.transform.rotation = Quaternion.Euler(currnetTurnAngle);
                player.transform.position = new Vector3(Mathf.Round(player.transform.position.x), player.transform.position.y, Mathf.Round(player.transform.position.z));
                directionMovement = Vector3.zero;
                directionTurn = Vector3.zero;
                Moving = false;
                MovingTimer = -1;
            }
        }
    }

    public void InputText()
    {
        code = uIManager.inputField.GetComponent<TMP_InputField>().text;
       
        StartCoroutine(_Start());


    }


    // Set up a function that takes a string as an argument
    // The string represents the direction that we want to check
    bool WhatsInFront(string direction, string tag, float maxDistance = 1f)
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