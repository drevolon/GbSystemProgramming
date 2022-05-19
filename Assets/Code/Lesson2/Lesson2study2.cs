using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class Lesson2study2 : MonoBehaviour
{
    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _velocities;
    private NativeArray<Vector3> _finalPositions;

    void Start()
    {
        _positions = new NativeArray<Vector3>(100, Allocator.Persistent);
        _velocities = new NativeArray<Vector3>(100, Allocator.Persistent);
        _finalPositions = new NativeArray<Vector3>(100, Allocator.Persistent);

        for (int i = 0; i < 100; i++)
        {
            _positions[i] = new Vector3(1,1,1);
            _velocities[i] = new Vector3(1,1,1);
        }

        MyJobs myJobs = new MyJobs() { positions = _positions, velocities= _velocities, finalPositions= _finalPositions };
        JobHandle jobHandle= myJobs.Schedule(100,5);
        jobHandle.Complete();
    }

    public struct MyJobs : IJobParallelFor
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Vector3> velocities;
        public NativeArray<Vector3> finalPositions;

        public void Execute(int index)
        {
            finalPositions[index] = velocities[index]+ positions[index];
            Debug.Log($"MyJobs positions: {positions[index]}, velocities {velocities[index]} , finalPositions {finalPositions[index]}");
        }
    }

    private void OnDestroy()
    {
        if (_positions.IsCreated)  _positions.Dispose();
        if (_velocities.IsCreated)  _velocities.Dispose();
        if (_finalPositions.IsCreated) _finalPositions.Dispose();
    }
}
