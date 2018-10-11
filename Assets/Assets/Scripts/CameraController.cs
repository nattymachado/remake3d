using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float speed = 0.5f;
    public float y = 2;
    public float z = 2;
    public float x = 2;


    void Start()
    {
        transform.LookAt(player.transform.position);
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position - 10 * player.transform.forward + new Vector3(x, y, z), speed);
        transform.LookAt(player.transform.position);
    }

}
