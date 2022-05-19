using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;

public class Lesson2 : MonoBehaviour
{
    private NativeArray<int> _nativeArray;
    void Start()
    {
        _nativeArray = new NativeArray<int>(100, Allocator.Persistent);
        InnizData();
        PrintData();
        MyJobs myJobs = new MyJobs() { nativeArray = _nativeArray };
        myJobs.Schedule();
    }
    /// <summary>
    /// Инициализация случайными значениями
    /// </summary>
    /// <param name="ts"></param>
    void InnizData()
    {
        for (int i = 0; i < _nativeArray.Length; i++)
        {
            _nativeArray[i] = Random.Range(1, 100);
        }
    }
    /// <summary>
    /// Вывод данных
    /// </summary>
    /// <param name="ts"></param>
    void PrintData()
    {
        Debug.Log("Print DATA: ");
        foreach (var item in _nativeArray)
        {
            Debug.Log($"no filtr {item}");
        }
    }

    public struct MyJobs : IJob
    {
        public NativeArray<int> nativeArray;
        public void Execute()
        {
            Debug.Log("Run Execute Job");

            for (int i = 0; i < nativeArray.Length; ++i)
            {
                if (nativeArray[i] > 10) nativeArray[i] = 0;
                Debug.Log($"MyJobs filtr: {nativeArray[i]}");
            }

        }
        
    }

    private void OnDestroy()
    {
        if (_nativeArray.IsCreated)
        _nativeArray.Dispose();
    }
}
