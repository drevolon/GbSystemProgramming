using UnityEngine;
public class Fractal : MonoBehaviour
{
    [SerializeField, Range(1, 8)] private int _depth = 4;

    [SerializeField, Range(1, 360)] private int rotationSpeed;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;



    private const float _positionOffset = .75f;
    private const float _scaleBias = .5f;
    private void Start()
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
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
    private Fractal CreateChild(Vector3 direction, Quaternion rotation)
    {
        var child = Instantiate(this);
        child._depth = _depth - 1;
        child.transform.localPosition = _positionOffset * direction;
        child.transform.localRotation = rotation;
        child.transform.localScale = _scaleBias * Vector3.one;
        return child;
    }

    private static readonly Vector3[] _directions = new Vector3[]
    {
        Vector3.up,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back,
    };
    private static readonly Quaternion[] _rotations = new Quaternion[]
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f),
    };
    //private FractalPart CreatePart(int levelIndex, int childIndex, float scale)
    //{
    //    var go = new GameObject($"Fractal Path L{levelIndex} C{childIndex}");
    //    go.transform.SetParent(transform, false);
    //    go.AddComponent<MeshFilter>().mesh = mesh;
    //    go.AddComponent<MeshRenderer>().material = material;
    //    return new FractalPart()
    //    {
    //        Direction = _directions[childIndex],
    //        Rotation = _rotations[childIndex],
    //        Transform = go.transform
    //    };
    //}

}

