using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatePlanet : MonoBehaviour
{
    [SerializeField]
    private float _Speed=10f;
    private float _Rotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _Rotation += _Speed * Time.deltaTime;
        transform.rotation= Quaternion.Euler(0,_Rotation, 0);
    }
}
