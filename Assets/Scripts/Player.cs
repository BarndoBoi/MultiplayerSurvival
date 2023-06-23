using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{

    NetworkVariable<Vector3> mapPosition = new NetworkVariable<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
