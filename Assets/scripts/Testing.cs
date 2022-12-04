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


public class testing : MonoBehaviour
{

    // The UI input field where the user will enter their script.
    public GameObject inputField;
    public TMP_InputField userInputField;
    public TextMeshProUGUI userOutText;

    // The cube that will be manipulated by the script.
    [SerializeField]
    private GameObject cube;

    // The renderer for the cube, used to change its color.
    private Renderer CubeRenderer;

    // The new color that will be applied to the cube.
    private Color newCubeColor;

    // Random values used to generate a new color for the cube.
    private float randomChannelOne, randomChannelTwo, randomChannelThree;

    // The name of the current scene.
    string sceneName;

    // The speed at which the cube will move.
    public float speed = 2;

    // The size of the cube.
    public float cubeSize = 1.0f;

    // The object responsible for executing Lua scripts.
    private LuaInterpreter luaInterpreter;

    // The object responsible for running a sequence of tasks defined by the script.
    private TaskRunner taskRunner;

    // The object responsible for handling user input and executing the script.
    private InputHandler inputHandler;

    // Start is called before the first frame update
    // This method is called when the scene is first loaded.
    void Start()
    {
        // Get the renderer for the cube.
        CubeRenderer = cube.GetComponent<Renderer>();

        // Get the name of the current scene.
        sceneName = SceneManager.GetActiveScene().name;
        sceneName += ".txt";

        // Create the objects responsible for executing Lua scripts, running tasks, and handling input.
        luaInterpreter = new LuaInterpreter();
        taskRunner = new TaskRunner();
        inputHandler = new InputHandler(luaInterpreter, taskRunner, userOutText);

        // Set up the input field to call the input handler when the user enters text.
        inputField.GetComponent<TMP_InputField>().onEndEdit.AddListener(inputHandler.HandleInput);
    }

    // This method is called once per frame.
    private void Update()
    {
        // Update the task runner to run any tasks that are currently active.
        taskRunner.RunTasks();
    }
}
// This class is responsible for executing Lua scripts and registering the functions that the script can call.
public class LuaInterpreter
{
    // The list of tasks that will be executed by the script.
    private List<UnityAction> listOfTasks = new List<UnityAction>();

    // The list of times that each task will be executed after.
    private List<float> listOfTime = new List<float>();

    // This method initializes the interpreter and executes the given script.
    public void StartLua(string script)
    {
        try
        {
            // Create a new script environment.
            Script env = new Script();

            // Register the list of tasks and times as global variables in the Lua environment.
            env.Globals["listOfTasks"] = listOfTasks;
            env.Globals["listOfTime"] = listOfTime;

            // Execute the script.
            env.DoString(script);
        }
        catch (Exception ex)
        {
            // If any exceptions are thrown, print the error message to the console.
            Debug.Log("Error: " + ex.Message);
        }
    }

    // This method registers a global function in the Lua environment.
    public void RegisterGlobal(string name, Delegate function)
    {
        // Create a new script environment.
        Script env = new Script();

        // Register the function with the given name in the Lua environment.
        env.Globals[name] = function;
    }
}
public class TaskRunner
    {
        // The list of tasks that will be executed by the script.
        private List<UnityAction> listOfTasks = new List<UnityAction>();

        // The list of times that each task will be executed after.
        private List<float> listOfTime = new List<float>();

        // The current task that is being executed.
        private IEnumerator currentTask;

        // The UnityEvent object used to execute tasks.
        private UnityEvent unityEvent = new UnityEvent();

        // This method adds a new task to the list of tasks that will be executed.
        public void AddTask(UnityAction task, float time)
        {
            listOfTasks.Add(task);
            listOfTime.Add(time);
        }

        // This method runs the tasks in the list of tasks.
        public void RunTasks()
        {
            // If there are no current tasks and there are tasks in the list, start a new task.
            if (currentTask == null && listOfTasks.Count > 0)
            {
                currentTask = DoTask();
                //StartCoroutine(currentTask);
            }
        }

        // This method loops through the list of tasks and waits the specified amount of time before executing each one.
        private IEnumerator DoTask()
        {
            for (int i = 0; i < listOfTasks.Count; i++)
            {
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

        // This method adds a task to the UnityEvent and waits the specified amount of time before removing it.
        private IEnumerator RunCodeUntil(UnityAction callfunction, float waitTime = 0)
        {
            unityEvent.AddListener(callfunction);
            yield return new WaitForSeconds(waitTime);
            unityEvent.RemoveListener(callfunction);
        }
    }

// This class is responsible for handling user input and executing the script.
public class InputHandler
{
    // The object responsible for executing Lua scripts.
    private LuaInterpreter luaInterpreter;

    // The object responsible for running a sequence of tasks defined by the script.
    private TaskRunner taskRunner;

    // The UI element that displays error messages to the user.
    private TextMeshProUGUI userOutText;

    // This constructor takes the objects responsible for executing scripts, running tasks, and displaying error messages.
    public InputHandler(LuaInterpreter luaInterpreter, TaskRunner taskRunner, TextMeshProUGUI userOutText)
    {
        this.luaInterpreter = luaInterpreter;
        this.taskRunner = taskRunner;
        this.userOutText = userOutText;
    }

    // This method handles user input and executes the script.
    public void HandleInput(string input)
    {
        // Stop any currently running tasks.
        //taskRunner.StopAllTasks();

        try
        {
            // Execute the script and clear any error messages.
            luaInterpreter.StartLua(input);
            userOutText.text = "None Error messages from Lua";

            // Run the tasks defined by the script.
            taskRunner.RunTasks();
        }
        catch (SyntaxErrorException ex)
        {
            // If a syntax error was detected, display an error message to the user.
            Debug.Log("Syntax error: " + ex.Message);
            userOutText.text = "Syntax error: " + ex.Message;
        }
        catch (ScriptRuntimeException ex)
        {
            // If a runtime error was detected, display an error message to the user.
            Debug.Log("Runtime error: " + ex.DecoratedMessage);
            userOutText.text = "Runtime error: " + ex.DecoratedMessage;
        }
        catch (Exception ex)
        {
            // If any other exception was thrown, display the error message to the user.
            Debug.Log("Error: " + ex.Message);
            userOutText.text = "Error: " + ex.Message;
        }
    }
}

