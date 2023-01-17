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
        uIManager.UserOutTextFunctionDispaly("Moving " + Distance + " units in direction " + Direction);
        Wait(Distance);
        MovingTimer = Distance;
        directionMovement = Direction;
        Moving = true;
        
    }
    private void Turn(int Angle)
    {
        uIManager.UserOutTextFunctionDispaly("Turning " + Angle + " degrees");
        Wait(1);
        MovingTimer = 1;
        directionTurn = new Vector3Int(0, Angle, 0);
        currnetTurnAngle += new Vector3Int(0, Angle, 0);
        Moving = true;
        

    }

    public void TurnRight() => Turn(90);
    public void TurnLeft() => Turn(-90);

    public void MoveForward(int Distance = 1) => Move(player.transform.forward, Distance);
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

        NewGame();

    }
    public void NewGame()
    {
        string data = DataInputHoldingData.instance.dataInput;
        if (data != null || data != "")
        {
            uIManager.userInputField.text = data;

        }
    }

    public void Stop()
    {
        Moving = false;
        MovingTimer = -1;
        __TimerON = false;
        __TimerTime = -1;
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


        script.DoString(_readyCode);

        DynValue function = script.Globals.Get("Start");

        // Create the coroutine in C#
        DynValue coroutine = script.CreateCoroutine(function);

        coroutine.Coroutine.AutoYieldCounter = 2;

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

        // Check if the Player's y-position is less than -10
        if (player.transform.position.y < -10)
        {
            // Display a message in the debug log
            Debug.Log("You are dead!");
            uIManager.RestartScene();
        }
    }


    public void InputText()
    {
        code = uIManager.inputField.GetComponent<TMP_InputField>().text;
       
        StartCoroutine(_Start());

    }
    //improve this function
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
}
