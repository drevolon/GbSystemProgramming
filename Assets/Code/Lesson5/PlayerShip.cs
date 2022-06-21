using UnityEngine;
using UnityEngine.Networking;

public class PlayerShip : NetworkBehaviour
{
    [SyncVar]
    public Color Color;
    public override void OnStartClient()
    {
        GetComponent<Renderer>().material.color = Color;
    }

}
