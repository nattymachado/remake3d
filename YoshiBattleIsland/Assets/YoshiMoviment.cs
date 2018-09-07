using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoshiMoviment : MonoBehaviour {

    public float speed = 1;
    public float speedRotation = 200;
    public GameObject yoshi;
    public bool isMoving = false;
    public Quaternion currentRotation;
    private Rigidbody yoshRigidbody;

   
    void Start()
    {
        yoshRigidbody = yoshi.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;
        



        Move();
     }

    private void RotateMoviment(Quaternion target)
    {

        yoshi.transform.rotation = Quaternion.RotateTowards(yoshi.transform.rotation, target, speedRotation * Time.deltaTime);
        if (yoshi.transform.rotation.eulerAngles.y == target.eulerAngles.y)
        {
            isMoving = false;
        } else
        {
            isMoving = true;
        }
    }

    private void Move()
    {

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !isMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentRotation = Quaternion.Euler(new Vector3(0, yoshi.transform.rotation.eulerAngles.y - 90, 0));
            } else
            {
                currentRotation = Quaternion.Euler(new Vector3(0, yoshi.transform.rotation.eulerAngles.y + 90, 0));
            }

            RotateMoviment(currentRotation);
        } if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) {

            float direction = 1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = speed;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = speed * -1;
            }
            yoshRigidbody.MovePosition(yoshi.transform.position + (yoshRigidbody.transform.forward * direction));
        } else if (isMoving)
        {
            RotateMoviment(currentRotation);
        }

    }

}
