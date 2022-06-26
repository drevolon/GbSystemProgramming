using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using System;

public class Fractal2 : MonoBehaviour
{
    private struct FractalPart
    {
        public Vector3 Direction;
        public Quaternion Rotation;
        public Transform Transform;
    }

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private FractalPart[][] _parts;

    [SerializeField, Range(1,8)] private int _depth = 4;
    [SerializeField, Range(1, 360)] private int _rotationSpeed;

    private const float _positionOffset = 1.5f;
    private const float _scaleBias =0.5f;
    private const int _childCount = 5;

    private static readonly Vector3[] _directions =  {
        Vector3.up,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };
    private static readonly Quaternion[] _rotations =  {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(90f, 0f, 90f),
        Quaternion.Euler(-90f, 0f, 90f),
    };

    private void OnEnable()
    {
        _parts = new FractalPart[_depth][];

        for (int i = 0, length = 1; i < _parts.Length; i++, length*=_childCount)
        {
            _parts[i]=new FractalPart[length];
        }

        var scale = 1f;
        _parts[0][0] = CreatePart(0, 0, scale);

        for (var levelIndex = 1; levelIndex < _parts.Length; levelIndex++)
        {
            scale *= _scaleBias;
            var levelParts = _parts[levelIndex];
            for (var fractalPartIndex = 0; fractalPartIndex < levelParts.Length; fractalPartIndex*=_childCount)
            {
                for (var childIndex = 0; childIndex < _childCount; childIndex++)
                {
                    levelParts[fractalPartIndex + childIndex] = CreatePart(levelIndex, childIndex, scale);
                }
            }
        }
    }

    private FractalPart CreatePart(int v1, int v2, float scale)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        var deltaRotation = Quaternion.Euler(0f, _rotationSpeed * Time.deltaTime, 0f);
        var rootPart = _parts[0][0];
        rootPart.Rotation *= deltaRotation;
        rootPart.Transform.localRotation=rootPart.Rotation;
        _parts[0][0] = rootPart;

        for (int li = 1; li < _parts.Length; li++)
        {
            var parentParts = _parts[li - 1];
            var levelParts=_parts[li];

            for (var fpi = 0; fpi < levelParts.Length; fpi++)
            {
                var parentTransform = parentParts[fpi / _childCount].Transform;
                var part = levelParts[fpi];
                part.Rotation *= deltaRotation;
                part.Transform.localRotation = parentTransform.localRotation * part.Rotation;
                part.Transform.localPosition = parentTransform.localPosition + parentTransform.localRotation * (_positionOffset * part.Transform.localScale.x * part.Direction);
                levelParts[fpi] = part;
            }
        }
    }

}

