using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {

    public GameObject point;
    public Vector3 offset;         //Private variable to store the offset distance between the player and camera
    public float damping = 1;
    public float speed = 1;
    public float speedRotation = 1;

    public void LookAtTarget(GameObject targetPoint)
    {
        point = targetPoint;
       
    }

    void LateUpdate()
    { 
        UpdateRotation();
    }

    void UpdateRotation()
    {
        if (point != null)
        {
            transform.rotation = point.transform.rotation;
            float currentAngle = transform.eulerAngles.y;
            float desiredAngle = point.transform.eulerAngles.y;
            float angle = Mathf.LerpAngle(currentAngle, desiredAngle, damping);

            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 desiredPosition = point.transform.position - (rotation * offset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, speedRotation);
            
        }

    }

}
