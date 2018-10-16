using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float speed = 0.5f;
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
        float h = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, player.transform.position - distance * player.transform.forward + new Vector3(x, y, z), speed);
        transform.position = new Vector3(transform.position.x, h, transform.position.z);
        transform.LookAt(new Vector3(player.transform.position.x, h, player.transform.position.z));
    }

}
