using System.Collections;
using UnityEngine;
using MoonSharp.Interpreter;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ScriptManager : MonoBehaviour
{
    UIManager uIManager;
    [SerializeField] private GameObject player;

    [SerializeField] private float movementSpeed = 1;
    [SerializeField] Slider speedSlider;
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
        _iEnumeratorTimerOn = true;
        _iEnumeratorTime = seconds * speedProcent;
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
        StopAll();
        code = uIManager.inputField.GetComponent<TMP_InputField>().text;

        StartCoroutine(_Start());

    }
    //create new function  SetPlayerSpeed
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
        script.Globals["SetPlayerSpeed"] = (Action<float>)SetPlayerSpeed;


        script.DoString(_readyCode);

        DynValue function = script.Globals.Get("Start");

        // Create the coroutine in C#
        DynValue coroutine = script.CreateCoroutine(function);

        coroutine.Coroutine.AutoYieldCounter = 2;

        DynValue result = null;

        result = coroutine.Coroutine.Resume();
        while (result.Type == DataType.YieldRequest)
        {
            if (_iEnumeratorTimerOn)
            {
                yield return new WaitForSeconds(_iEnumeratorTime);
                _iEnumeratorTimerOn = false;
                _iEnumeratorTime = -1;
            }
            yield return new WaitForSeconds(.1f * speedProcent);
            result = coroutine.Coroutine.Resume();
        }
    }



    private void Update()
    {
        if (_isMoving)
        {
            _movmentCurrentTimer += Time.deltaTime;

            player.transform.position = Vector3Lerp(_startPosMovment, _endPosMovment, _movmentCurrentTimer / _setMovmentTimer);
            if (_movmentCurrentTimer >= _setMovmentTimer)
            {
                _startPosMovment = _endPosMovment;
                player.transform.position = _endPosMovment;
                _isMoving = false;
            }
        }
        if (_isRotating)
        {
            _movmentCurrentTimer += Time.deltaTime;
            player.transform.rotation = Quaternion.Euler(Vector3Lerp(_startRotMovment, _endRotMovment, _movmentCurrentTimer / _setMovmentTimer));
            if (_movmentCurrentTimer >= _setMovmentTimer)
            {
                _startRotMovment = _endRotMovment;
                player.transform.rotation = Quaternion.Euler(_endRotMovment);
                _isRotating = false;
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
