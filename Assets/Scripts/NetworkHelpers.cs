using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkHelpers : MonoBehaviour
{
    public void ConnectToServer()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void HostServer()
    {
        NetworkManager.Singleton.StartHost();
    }
}
