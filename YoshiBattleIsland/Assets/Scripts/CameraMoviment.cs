using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoviment : MonoBehaviour {

    public Transform playerTransform;
    public int depth = -8;
    public int high = 8;
    public int direction = -1;
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position + new Vector3(direction, high, depth), speed);
        }
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
