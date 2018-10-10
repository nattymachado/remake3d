using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoviment : MonoBehaviour {

    public Transform playerTransform;
    public int depth = -8;
    public int high = 8;
    public int direction = -1;
    public float speed = 0f;

    // Update is called once per frame
    void Update()
    {
        /*if (playerTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position,  new Vector3(playerTransform.position.x + direction, 0, playerTransform.position.z + depth), speed);
            Debug.Log(transform.position);
        }*/
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
