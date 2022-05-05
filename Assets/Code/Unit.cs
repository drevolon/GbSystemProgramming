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
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await TaskAsync1(cancellationToken);

        await TaskAsync2(cancellationToken, frames);

        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    void Update()
    {
        textHealth.text = health.ToString();
        btn.enabled = isHeal ? false : true;
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
        if (!cancellationToken.IsCancellationRequested)
        {
            Debug.Log($"Task 1 Begin {Time.time}");
            await Task.Delay(1000);
            Debug.Log($"Task 1 End {Time.time}");
        }

        return true;
    }

    async Task<bool> TaskAsync2(CancellationToken cancellationToken, int frames)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            Debug.Log($"Task 2 Begin {Time.time}");
            for (int i = 0; i < frames; i++)
            {
                await Task.Yield();
            }

            Debug.Log($"Task 2 End {Time.time}");
        }

        return true;
    }

    async void OnClickBtnStart()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        Task<bool> task1 =  TaskAsync1(cancellationToken);
        Task<bool> task2 =  TaskAsync2(cancellationToken, frames);

        Task<bool> result = await Task.WhenAny(task1, task2);


        WhatTaskFasterAsync(cancellationToken, task1, task2);

        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }
    void OnClickBtnStop()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    async void WhatTaskFasterAsync(CancellationToken cancellation, Task<bool> task1, Task<bool> task2)
    {
        Task<bool> result = await Task.WhenAny(task1, task2);
        Debug.Log(result);
    }
}
