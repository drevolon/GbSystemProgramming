using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[DisallowMultipleComponent]
[AddComponentMenu("Network/Network start positions")]
public class NetworkStartPosition : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.RegisterStartPosition(transform);
    }

    private void OnDestroy()
    {
        NetworkManager.UnRegisterStartPosition(transform);
    }
}
