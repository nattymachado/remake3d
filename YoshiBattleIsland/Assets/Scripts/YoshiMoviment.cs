using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoshiMoviment : MonoBehaviour {

    //public float speed = 1;
    public float speedRotation = 200;
    public float jumpForce = 40;
    private int jumpCount = 0;
    public GameObject yoshi;
    public bool isMoving = false;
    public Quaternion currentRotation;
    public Camera yoshiCamera;
    private Rigidbody yoshRigidbody;

    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        /*yoshRigidbody = yoshi.GetComponent<Rigidbody>();
        yoshiCamera.GetComponent<CameraMoviment>().setTarget(yoshi.transform);*/
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    /*public void Update()
    {
        Move();
        Jump();
    }*/

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

    private void Jump()
    {
        if (yoshi.transform.position.y < 0)
        {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            yoshi.transform.Translate(Vector3.up * jumpForce * Time.deltaTime, Space.World);
            //yoshiCamera.transform.Translate(Vector3.up * jumpForce * Time.deltaTime, Space.World);
            jumpCount += 1;
        }
    }

    private void Move()
    {

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && !isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentRotation = Quaternion.Euler(new Vector3(0, yoshi.transform.rotation.eulerAngles.y - 90, 0));
            } else
            {
                currentRotation = Quaternion.Euler(new Vector3(0, yoshi.transform.rotation.eulerAngles.y + 90, 0));
            }
            RotateMoviment(currentRotation);
        } if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) {

            float direction = 1;
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = speed;
            }
            else if (Input.GetKeyDown(KeyCode.S))
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
