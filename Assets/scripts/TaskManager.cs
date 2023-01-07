using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TaskManager
{
    // Variables for managing tasks
    private List<UnityAction> listOfTasks = new List<UnityAction>();
    private List<float> listOfTime = new List<float>();
    private IEnumerator currentTask;
    UnityEvent unityEvent = new UnityEvent();

    public void DoTask()
    {
        currentTask = ExecuteTasks();
        StartCoroutine(currentTask);
    }
    void Update()
    {
        // Invoke any tasks that are currently registered with the UnityEvent
        unityEvent.Invoke();
    }

        private IEnumerator ExecuteTasks()
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

    public void AddTaskToList(UnityAction task, float time)
    {
        listOfTasks.Add(task);
        listOfTime.Add(time);
    }

    public void RestartScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
