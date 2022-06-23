using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class Fractal2 : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _count = 10;
    [SerializeField] float _speed = 100;
    [SerializeField, Range(1, 8)] private int _depth = 4;
    [SerializeField, Range(1, 360)] private int rotationSpeed;
    private const float _positionOffset = .75f;
    private const float _scaleBias = .5f;


    TransformAccessArray _transformAccess;
    Fractal2[] _objectData;
    MyJobs myJobs;

    float rotation = 0;

    void Start()
    {
        _objectData = SpawnData();

        _transformAccess = new TransformAccessArray(_objectData);

        myJobs = new MyJobs(_speed, rotation, Time.deltaTime);

        JobHandle jobHandle = myJobs.Schedule(_transformAccess);
        jobHandle.Complete();
    }

    private void Update()
    {
        rotation += _speed * Time.deltaTime;
        myJobs.deltaTime = Time.deltaTime;

        myJobs.rotation = rotation;
        var handle = myJobs.Schedule(_transformAccess);
        handle.Complete();
    }

    public struct MyJobs : IJobParallelForTransform
    {
        private float speed;
        public float rotation;
        public float deltaTime;
        public MyJobs(float speed, float rotation, float deltaTime)
        {
            this.speed = speed;
            this.rotation = rotation;
            this.deltaTime = deltaTime;
        }

        public void Execute(int index, TransformAccess transform)
        {

            //transform.rotation = Quaternion.Euler(rotation, rotation, rotation);

            transform.rotation *= Quaternion.Euler(speed * deltaTime, speed * deltaTime, speed * deltaTime);

            Debug.Log($"MyJobs transform: {transform}, speed {speed},  rotation {rotation}");
        }
    }

    private Fractal2[] SpawnData()
    {
        Fractal2[] childs;
        name = "Fractal " + _depth;
        if (_depth <= 1)
        {
            return null;
        }

        for (int i = 0; i < 4; i++)
        {
            childs =  CreateChild(Vector3.up, Quaternion.identity);
            childs.transform.SetParent(transform, false);
        }
        //var childA = CreateChild(Vector3.up, Quaternion.identity);
        //var childB = CreateChild(Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        //var childC = CreateChild(Vector3.left, Quaternion.Euler(0f, 0f, 90f));
        //var childD = CreateChild(Vector3.forward, Quaternion.Euler(90f, 0f, 0f));
        //var childE = CreateChild(Vector3.back, Quaternion.Euler(-90f, 0f, 0f));
        //childA.transform.SetParent(transform, false);
        //childB.transform.SetParent(transform, false);
        //childC.transform.SetParent(transform, false);
        //childD.transform.SetParent(transform, false);
        //childE.transform.SetParent(transform, false);

        return childs;
    }
    private Fractal2 CreateChild(Vector3 direction, Quaternion rotation)
    {
        var child = Instantiate(this);
        child._depth = _depth - 1;
        child.transform.localPosition = _positionOffset * direction;
        child.transform.localRotation = rotation;
        child.transform.localScale = _scaleBias * Vector3.one;
        return child;
    }

    private void OnDestroy()
    {
        if (_transformAccess.isCreated) _transformAccess.Dispose();
    }


}
