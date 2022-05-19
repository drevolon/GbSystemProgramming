using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class Lesson2study3 : MonoBehaviour
{
    private NativeArray<Vector3> _transform;

    void Start()
    {
        _transform = new NativeArray<Vector3>(100, Allocator.Persistent);

        for (int i = 0; i < 100; i++)
        {
            _transform[i] = new Vector3(1, 1, 1);
        }

        MyJobs myJobs = new MyJobs() { transform = _transform };
        JobHandle jobHandle = myJobs.Schedule(100,5, default);
        jobHandle.Complete();
    }

    public struct MyJobs : IJobParallelForTransform
    {
        public NativeArray<Vector3> transform;

        public void Execute(int index, TransformAccess transform)
        {
            Debug.Log($"MyJobs transform: {transform}");
        }
    }

    private void OnDestroy()
    {
        if (_transform.IsCreated) _transform.Dispose();
    }
}
