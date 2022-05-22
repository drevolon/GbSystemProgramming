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

    float rotation = 0;

    void Start()
    {
        _objectData= SpawnData(_prefab, _count);

        _transformAccess = new TransformAccessArray(_objectData);

        myJobs = new MyJobs(_speed,  rotation);

        JobHandle jobHandle = myJobs.Schedule(_transformAccess);
        jobHandle.Complete();
    }

    private void Update()
    {
        rotation += _speed * Time.deltaTime;
        myJobs.rotation = rotation;
        var handle=myJobs.Schedule(_transformAccess);
        handle.Complete();
    }

    public struct MyJobs : IJobParallelForTransform
    {
        private float speed;
        public float rotation;
        public MyJobs(float speed,  float rotation)
        {
            this.speed = speed;
            this.rotation = rotation;
        }

        public void Execute(int index, TransformAccess transform)
        {

            transform.rotation = Quaternion.Euler(rotation, rotation, rotation);

            Debug.Log($"MyJobs transform: {transform}, speed {speed},  rotation {rotation}");
        }
    }

    private Transform[] SpawnData(GameObject prefab, int count)
    {
        Transform[] spawn=new Transform[count];
        for (int i = 0; i < count; i++)
        {
            Transform spawnObject= Instantiate(prefab).transform;
            spawnObject.position = new Vector3 (Random.Range(1,50), Random.Range(1, 50), Random.Range(1, 50));
            spawn[i] = spawnObject;
        }
        return spawn;
    }

    private void OnDestroy()
    {
        if (_transformAccess.isCreated) _transformAccess.Dispose();
    }


}
