using UnityEngine;


public class AsteroidOrbit : MonoBehaviour
{
    public float angle = 0;

    public float speed = 0.1f;
    public float radius = 15f;
    public bool isCircle = true;

    private Transform centerRotate;
   
    public Vector3 cachedCenter;
    float posX;
    float posY;
    float posZ;

    private void Start()
    {
        centerRotate = FindObjectOfType<CenterSystem>().transform;
        radius = (transform.position - centerRotate.position).magnitude;

        posX=(transform.position.x- centerRotate.position.x);
        posY=transform.position.y;
        posZ=(transform.position.z - centerRotate.position.z); 
    }


    void Update()
    {
        if (isCircle)
        {
            angle += Time.deltaTime;
            var x = Mathf.Cos(angle * speed) * posX;
            var y = posY;
            var z = Mathf.Sin(angle * speed) * posZ;

            transform.position = new Vector3(x, y, z); // + cachedCenter - new Vector3(radius, 0, 0);
        }
        else
        {
            angle = 0;
            cachedCenter = transform.position;
            var x = transform.position.x;
            var y = transform.position.y;
            x += 0.5f * Time.deltaTime;

            transform.position = new Vector2(x, y);
        }
    }




}
