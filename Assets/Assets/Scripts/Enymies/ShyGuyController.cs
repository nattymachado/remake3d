﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyGuyController : MonoBehaviour {

    public GameObject body;
    public GameObject egg;
    private Vector3 target;
    private bool followPlayer;
    private GameObject yoshi;
    public float timeToChangeDirection = -2;
    public Vector3 positionTarget;

    private Vector3 ChangeDirection()
    {
        Vector3 position = new Vector3(Random.Range(0, 360f), 0, Random.Range(0, 360f));
        return position;
    }

    void FixedUpdate()
    {
        timeToChangeDirection -= Time.deltaTime;
        if (timeToChangeDirection <= 0)
        {
            positionTarget = ChangeDirection();
            Debug.Log(positionTarget);
            timeToChangeDirection = 10;
        }

        if (yoshi != null)
        {
            positionTarget = yoshi.transform.position;
        }

        Vector3 direction = (positionTarget - transform.position).normalized;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.MovePosition(transform.position + direction * 2.0f * Time.fixedDeltaTime);
        Quaternion lookToPlayer = Quaternion.LookRotation(direction);
        GetComponent<Rigidbody>().MoveRotation(lookToPlayer);

    }
    private void OnTriggerEnter(Collider other)
    {

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            yoshi = controller.gameObject;
            
        }

    }

    private void OnTriggerExit(Collider other)
    {

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (controller.gameObject == yoshi)
            {
                yoshi = null;
            }
            

        }

    }

    public void TransformOnEgg()
    {
        Debug.Log("I will be an egg now");
        //egg.SetActive(true);
       // body.SetActive(false);
    }
}