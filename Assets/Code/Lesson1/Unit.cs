using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] Text textHealth;
    [SerializeField] Text textSeconds;

    [SerializeField] Button btn;
    [SerializeField] Button btnAsync;
    [SerializeField] Button btnStart;
    [SerializeField] Button btnStop;

    int healSmall = 5; //Уровень Heal
    float timeHeal = 0.5f; //Тик heal
    int periodHeal = 6; //Период heal 
    int maxHeal = 100;
    bool isHeal = false;
    int frames = 60;
    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        btn.onClick.AddListener(OnClickViewDataCoroutine);
        btnAsync.onClick.AddListener(OnClickViewDataAsync);
        btnStart.onClick.AddListener(OnClickBtnStart);
        btnStop.onClick.AddListener(OnClickBtnStop);
        
    }

    private void OnClickViewDataCoroutine()
    {
        StartCoroutine(ReceiveHealing());
    }

    async void OnClickViewDataAsync()
    {
        cancellationTokenSource = new CancellationTokenSource();

        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await TaskAsync1(cancellationToken);

        await TaskAsync2(cancellationToken, frames);

        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    void Update()
    {
        textHealth.text = health.ToString();
        btn.interactable = isHeal ? false : true;

        textSeconds.text = DateTime.Now.Second.ToString();
    }

    IEnumerator ReceiveHealing()
    {
        isHeal = true;
        for (int i = 0; i < periodHeal; i++)
        {
            Debug.Log($"ReceiveHealing BEGIN {Time.time}");
            if (health < maxHeal)
            {
                yield return new WaitForSeconds(timeHeal);
                health += healSmall;
            }
            else
            {
                Debug.Log($"Max Heal");
                isHeal = false;
                yield break;

            }
            Debug.Log($"ReceiveHealing END {Time.time}");
        }

    }

    async Task<bool> TaskAsync1(CancellationToken cancellationToken)
    {
        
            Debug.Log($"Task 1 Begin {Time.time}");
            await Task.Delay(5000, cancellationToken);
            ResetTime();
            Debug.Log($"Task 1 End {Time.time}");
        

        return true;
    }

    void ResetTime()
    {
        textSeconds.text = "0";
    }

    async Task<bool> TaskAsync2(CancellationToken cancellationToken, int frames)
    {
        Debug.Log($"Task 2 Begin {Time.time}");
        for (int i = 0; i < frames; i++)
        {
            await Task.Yield();
            if (cancellationToken.IsCancellationRequested)
            {
                ResetTime();
                return false;
            }
        }

        Debug.Log($"Task 2 End {Time.time}");

        return true;
    }

    async void OnClickBtnStart()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        //Task<bool> task1 =  TaskAsync1(cancellationToken);
        //Task<bool> task2 =  TaskAsync2(cancellationToken, frames);

        Task<bool> task1 = Task.Run(() => TaskAsync1(cancellationToken));
        Task<bool> task2 = Task.Run(() => TaskAsync2(cancellationToken, frames));

        Task<bool> resultTask=WhatTaskFasterAsync(cancellationToken, task1, task2);
        Debug.Log(resultTask);

        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }
    void OnClickBtnStop()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    async Task<bool> WhatTaskFasterAsync(CancellationToken cancellation, Task<bool> task1, Task<bool> task2)
    {
        Debug.Log($"Begin WhatTaskFasterAsync");

        if (!cancellation.IsCancellationRequested)
        {
            Task<bool> result = await Task.WhenAny(task1, task2);




            Debug.Log($"task1 {task1.Result} task2 {task2.Result}");
            return result.Result == true ? true : false;
        }

        Debug.Log($"End WhatTaskFasterAsync");

        return false;
    }
}
