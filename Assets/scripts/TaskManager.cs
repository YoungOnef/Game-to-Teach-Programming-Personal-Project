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

public class TaskManager : MonoBehaviour
{
    private List<UnityAction> listOfTasks = new List<UnityAction>();
    private List<float> listOfTime = new List<float>();
    private IEnumerator currentTask;
    UnityEvent unityEvent = new UnityEvent();

    void Update()
    {
        // Invoke any tasks that are currently registered with the UnityEvent
        unityEvent.Invoke();

    }
    // This method adds a task to the UnityEvent and waits the specified amount of time before removing it.
    private IEnumerator RunCodeUntil(UnityAction callfunction, float waitTime = 0)
    {

        unityEvent.AddListener(callfunction);
        yield return new WaitForSeconds(waitTime);

        // Remove the function from the UnityEvent
        unityEvent.RemoveListener(callfunction);
    }
    private IEnumerator DoTask()
    {
        for (int i = 0; i < listOfTasks.Count; i++)
        {
            string methodName = listOfTasks[i].Method.Name;
            userOutTextFunctionDispaly.text = methodName;

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
}
