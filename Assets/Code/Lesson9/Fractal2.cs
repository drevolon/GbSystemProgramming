using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class Fractal2 : MonoBehaviour
{
    [SerializeField]
    private int _depth = 6;
    [SerializeField, Range(1, 360)] private int rotationSpeed;
    private const float _positionOffset = .75f;
    private const float _scaleBias = .5f;
    TransformAccessArray _transformAccess;
    [SerializeField] float _speed = 100;


    MyJobs myJobs;
    void Start()
    {
        Spawn();

        myJobs = new MyJobs(_speed, rotationSpeed, Time.deltaTime);
        _transformAccess = new TransformAccessArray();

       
        JobHandle jobHandle = myJobs.Schedule(_transformAccess);
        jobHandle.Complete();
    }

    private Transform[] SpawnData(GameObject prefab, int count)
    {
        Transform[] spawn = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            Transform spawnObject = Instantiate(prefab).transform;
            spawnObject.position = new Vector3(Random.Range(1, 50), Random.Range(1, 50), Random.Range(1, 50));
            spawn[i] = spawnObject;
        }
        return spawn;
    }


    private void Spawn()
    {
        name = "Fractal " + _depth;
        if (_depth <= 1)
        {
            return;
        }
        var childA = CreateChild(Vector3.up, Quaternion.identity);
        var childB = CreateChild(Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        var childC = CreateChild(Vector3.left, Quaternion.Euler(0f, 0f, 90f));
        var childD = CreateChild(Vector3.forward, Quaternion.Euler(90f, 0f, 0f));
        var childE = CreateChild(Vector3.back, Quaternion.Euler(-90f, 0f, 0f));
        childA.transform.SetParent(transform, false);
        childB.transform.SetParent(transform, false);
        childC.transform.SetParent(transform, false);
        childD.transform.SetParent(transform, false);
        childE.transform.SetParent(transform, false);

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

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

    }

    

    private void OnDestroy()
    {
       
    }


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


        transform.rotation *= Quaternion.Euler(speed * deltaTime, speed * deltaTime, speed * deltaTime);

        Debug.Log($"MyJobs transform: {transform}, speed {speed},  rotation {rotation}");
    }
}