using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float speed = 0.1f;
    public float y = 2;
    public float z = 2;
    public float x = 2;
    public float distance = 15;

    void Start()
    {
        transform.LookAt(player.transform.position);
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y , player.transform.position.z) - distance * player.transform.forward + new Vector3(x, y, z), speed);
        
        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y * 0.9f, player.transform.position.z));
    }

}
