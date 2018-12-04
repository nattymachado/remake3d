using UnityEngine;
using UnityEngine.Networking;

public class NetworkSpawner : NetworkBehaviour
{


    public GameObject babyBowser;

    void Start()
    {
        if (!isServer) { return; }

        GameObject bowser = Instantiate(babyBowser, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(bowser);
    }

}
