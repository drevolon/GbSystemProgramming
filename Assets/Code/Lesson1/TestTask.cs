using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestTask : MonoBehaviour
{
    async void Start()
    {
        InizTasks(1, "Task1");
        InizTasks(2, "Task2");
    }

    async void InizTasks(int wait, string currentTask)
    {
        Debug.Log($"Create {currentTask}");
        Task task1 = Task1Async(wait, currentTask);
        await task1;
        Debug.Log($"Return data {currentTask}");
    }

    async Task Task1Async(int wait, string nameTask1)
    {
        Debug.Log($"Begin in Task1Async {nameTask1}");
        await Task.Delay(wait * 1000);
        Debug.Log($"End in Task1Async {nameTask1}");
    }
}
