using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class Lesson2study3 : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _count=10;
    [SerializeField] float _speed = 100;

    TransformAccessArray _transformAccess;
    Transform[] _objectData;
    MyJobs myJobs;
    


    void Start()
    {
        _objectData= SpawnData(_prefab, _count);

        _transformAccess = new TransformAccessArray(_objectData);

        myJobs = new MyJobs(_speed,Time.deltaTime);

        JobHandle jobHandle = myJobs.Schedule(_transformAccess);
        jobHandle.Complete();
    }

    private void Update()
    {
        myJobs._deltaTime = Time.deltaTime;
        var handle=myJobs.Schedule(_transformAccess);
        handle.Complete();
    }

    public struct MyJobs : IJobParallelForTransform
    {
        private float _speed;
        public float _deltaTime;
        public MyJobs(float speed, float deltaTime)
        {
            _speed = speed;
            _deltaTime = deltaTime;
        }

        public void Execute(int index, TransformAccess transform)
        {
            Debug.Log($"MyJobs transform: {transform}");
            
            transform.rotation = Quaternion.Euler(_speed * _deltaTime, transform.position.y, transform.position.z);
        }
    }

    private Transform[] SpawnData(GameObject prefab, int count)
    {
        Transform[] spawn=new Transform[count];
        for (int i = 0; i < count; i++)
        {
            Transform spawnObject= Instantiate(prefab).transform;
            spawnObject.position = new Vector3 (Random.Range(1,5), Random.Range(1, 5), Random.Range(1, 5));
            spawn[i] = spawnObject;
        }
        return spawn;
    }

    private void OnDestroy()
    {
        if (_transformAccess.isCreated) _transformAccess.Dispose();
    }


}
