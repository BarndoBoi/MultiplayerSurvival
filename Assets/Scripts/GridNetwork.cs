using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GridNetwork : NetworkBehaviour
{

    [SerializeField]
    Grid Grid;

    // Start is called before the first frame update
    void Start()
    {
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestMapServerRpc()
    { //Runs on server to send map to client
        Debug.Log("Map was requested by client");
        SendMapClientRpc(Grid.height, Grid.width, Grid.seed);
    }

    [ClientRpc]
    public void SendMapClientRpc(int height, int width, int seed)
    {
        Grid.seed = seed;
        Grid.height = height;
        Grid.width = width;
        Grid.CreateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
