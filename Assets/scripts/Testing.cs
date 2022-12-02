using MoonSharp.Interpreter;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LuaCode : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] float speed = 1f;
    private Renderer CubeRenderer;

    Script myLuaScript;
    private Color newCubeColor;
    private Vector3 _colorVector3 = new Vector3(0, 0, 0);
    private string Var1 = "Red";
    private string Var2 = "Green";
    private string Var3 = "Blue";
    private string VarStings = "Variables:\n";

    private Button run;
    private Button restart;
    private Label OutPutCode;
    private TextField CodeField;
    private Label Variables;
    private List<UnityAction> listOfTasks = new List<UnityAction>();
    private List<float> listOfTime = new List<float>();
    private IEnumerator CurrnetTask;//CreateCorutine List
    UnityEvent unityEvent = new UnityEvent();// UnityEvents

    void Update()
    {
        //Run All Added Events PerUpdate
        unityEvent.Invoke();
    }
    private IEnumerator RunCodeUntil(UnityAction coll, float waitTime = 0)
    {
        unityEvent.AddListener(coll);
        yield return new WaitForSeconds(waitTime);
        unityEvent.RemoveListener(coll);
    }
    private IEnumerator DoTask()
    {
        for (int i = 0; i < listOfTasks.Count; i++)
        {
            StartCoroutine(RunCodeUntil(listOfTasks[i], listOfTime[i]));
            yield return new WaitForSeconds(listOfTime[i]);
        }
        StopAllCoroutines();
        listOfTasks = new List<UnityAction>();
        listOfTime = new List<float>();
        yield return true;
    }
    void GetUiElements()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        run = root.Q<Button>("RunBt");
        restart = root.Q<Button>("ResetBt");

        CodeField = root.Q<TextField>("CodeField");

        OutPutCode = root.Q<Label>("OutPutCode");
        Variables = root.Q<Label>("Variables");

        Variables.text = string.Join("\n• ", new string[] {
            VarStings,
            Var1,
            Var2,
            Var3
        });

        run.clicked += inputText;
        restart.clicked += _ResetFunc;
    }
    void _ResetFunc()
    {
        StopAllCoroutines();
        CodeField.value = "";
        myLuaScript = new Script();
        _colorVector3 = new Vector3(0, 0, 0);
        resetText();
        inputText();
        print("RESET");
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
    private void MoveF() => cube.transform.position += Vector3.forward * speed * Time.deltaTime;
    private void MoveR() => cube.transform.position += Vector3.right * speed * Time.deltaTime;

    private static int Mul(int a, int b)
    {
        int num = a + b;
        print($"Sum: {num}");
        return num;
    }
    public void inputText()
    {
        StopAllCoroutines();
        listOfTasks = new List<UnityAction>();
        listOfTime = new List<float>();
        try
        {
            StartLua(CodeField.text);
            OutPutCode.text = "None Error";
            if (listOfTasks.Count == listOfTime.Count)
            {
                CurrnetTask = DoTask();
                StartCoroutine(CurrnetTask);
            }
            else
            {
                Debug.LogError($"ERROR Lists Count NotMaching : listOfTasks.Count({listOfTasks.Count}) listOfTime.Count({listOfTime.Count})");
            }
        }
        catch (ScriptRuntimeException ex)
        {
            OutPutCode.text = "An error occured!: \n" + ex.ToString();
        }

    }

    public void StartLua(string rawLuaCode)
    {

        //creating a new script Object
        myLuaScript = new Script();

        //defining global veriable and sending the veriable
        myLuaScript.Globals[Var1] = _colorVector3.x;
        myLuaScript.Globals[Var2] = _colorVector3.y;
        myLuaScript.Globals[Var3] = _colorVector3.z;

        myLuaScript.Globals["MoveForward"] = (Action<float>)MoveForward;
        myLuaScript.Globals["MoveRight"] = (Action<float>)MoveRight;
        myLuaScript.Globals["Mul"] = (Func<int, int, int>)Mul;
        //myLuaScript.Globals["Mul"] = (Func<int, int, int>)Mul;


        //running the script via lua
        DynValue result = myLuaScript.DoString(rawLuaCode);

        //getting veriable back
        _colorVector3.x = (float)myLuaScript.Globals.Get(Var1).CastToNumber();
        _colorVector3.y = (float)myLuaScript.Globals.Get(Var2).CastToNumber();
        _colorVector3.z = (float)myLuaScript.Globals.Get(Var3).CastToNumber();

        newCubeColor = Vec3ToColor(_colorVector3);

        CubeRenderer.material.SetColor("_Color", newCubeColor);
    }
    Color Vec3ToColor(Vector3 vec3) => new Color(vec3.x, vec3.y, vec3.z);
    public void resetText() => OutPutCode.text = "None Error";
    public void SetText(string text) => OutPutCode.text = text;

    private void Awake()
    {
        CubeRenderer = cube.GetComponent<Renderer>();
        GetUiElements();

    }
    void Start()
    {
        resetText();
        inputText();
    }
}