using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class SpawnAsteroid2 : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _count = 10;
    [SerializeField] float _speed = 1;
    [SerializeField] float _posXbegin = -41f;
    [SerializeField] float _posXend = 41f;
    [SerializeField] float _posYbegin = -5f;
    [SerializeField] float _posYend = 5f;
    [SerializeField] float _posZbegin = 1f;
    [SerializeField] float _posZend = 50f;
    [SerializeField] float _scaleMin = 0.001f;
    [SerializeField] float _scaleMax = 0.003f;
    [SerializeField] float _radius = 10f;
    [SerializeField] float _speedMoveAsteroid = 1;

    TransformAccessArray _transformAccess;
    Transform[] _objectData;
    MyJobs myJobs;

    float _rotation = 0;
    float _angle = 1f;

    void Start()
    {
        _objectData = SpawnData(_prefab, _count);

        _transformAccess = new TransformAccessArray(_objectData);

        myJobs = new MyJobs(_speed, _rotation, Time.deltaTime, _radius, _speedMoveAsteroid, _angle);

        JobHandle jobHandle = myJobs.Schedule(_transformAccess);
        jobHandle.Complete();
    }

    private void Update()
    {
        _rotation += _speed * Time.deltaTime;
        myJobs.deltaTime = Time.deltaTime;
        _angle += _speedMoveAsteroid * Time.deltaTime;

        myJobs.rotation = _rotation;
        var handle = myJobs.Schedule(_transformAccess);
        handle.Complete();
    }

    public struct MyJobs : IJobParallelForTransform
    {

        private float speed;
        public float rotation;
        public float deltaTime;
        private readonly float radius;
        private float speedMoveAsteroid;
        private float angle;

        public MyJobs(float speed, float rotation, float deltaTime, float radius, float speedMoveAsteroid, float angle)
        {
            this.speed = speed;
            this.rotation = rotation;
            this.deltaTime = deltaTime;
            this.radius = radius;
            this.speedMoveAsteroid = speedMoveAsteroid;
            this.angle = angle;

        }

        public void Execute(int index, TransformAccess transform)
        {
            //F = m * v2 / r => v2=Fr/m

            float objScale = transform.localScale.magnitude;
            float speedCalc = (0.001f / objScale) * speed;

            transform.rotation *= Quaternion.Euler(speedCalc * deltaTime, speedCalc * deltaTime, speedCalc * deltaTime);

            //transform.position= ?????

        }
    }

    private Transform[] SpawnData(GameObject prefab, int count)
    {
        Transform[] spawn = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            Transform spawnObject = Instantiate(prefab).transform;
            spawnObject.position = new Vector3(Random.Range(_posXbegin, _posXend), Random.Range(_posYbegin, _posYend), Random.Range(_posZbegin, _posZend));
            spawnObject.localScale = new Vector3(Random.Range(_scaleMin, _scaleMax), Random.Range(_scaleMin, _scaleMax), Random.Range(_scaleMin, 0.003f));
            spawn[i] = spawnObject;
        }
        return spawn;
    }

    private void OnDestroy()
    {
        if (_transformAccess.isCreated) _transformAccess.Dispose();
    }


}
