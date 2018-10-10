using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed;
    public float SpeedRotation;
    public float JumpSpeed;
    public GameObject EggPrefab;
    public GameObject TargetPrefab;
    public Transform EggSpawn;
    public Camera playerCamera;
    public float collisionTime = 0;

    private GameObject _activateTarget = null;


    private bool _isWithMario = false;
    private int eggsQuantity = 50;


    public bool IsWithMario
    {
        get
        {
            return this._isWithMario;
        }

        set
        {
            this._isWithMario = value;
        }
    }

    public void AddEgg()
    {
        this.eggsQuantity += 1;
        Debug.Log("Eggs: " + this.eggsQuantity);
    }

    public void RemoveEgg()
    {
        this.eggsQuantity -= 1;
        Debug.Log("Eggs: " + this.eggsQuantity);
    }

    private Quaternion currentRotation;


    void Start()
    {
    }

    void Update()
    {

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedRotation;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        float y = 0;


        if (transform.position.y < JumpSpeed)
         {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Jumping");
                y = JumpSpeed;
            }
        }
        
        
        transform.Rotate(0, x, 0);
        transform.Translate(0, y, z);
        if (_activateTarget)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 20.0f;
            if (mousePos.y < 116f)
            {
                mousePos.y = 116f;
            }
            Debug.Log(mousePos);
            Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);
            Vector3 direction = (objectPos - transform.position).normalized;
            _activateTarget.transform.position = transform.position + direction * 5;
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            if (_activateTarget == null)
            {
                var mousePos = Input.mousePosition;
                mousePos.z = 20.0f;
                Debug.Log(mousePos);
                Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);
                Vector3 direction = (objectPos - transform.position).normalized;
                _activateTarget = (GameObject)Instantiate(
                    this.TargetPrefab,
                    transform.position + direction * 5, transform.rotation);
            }
            
        }
        if (Input.GetButtonUp("Fire2"))
        {

            if (eggsQuantity > 0)
            {
                this.RemoveEgg();
                Fire();
            } else
            {
                Destroy(_activateTarget);
            }
        }
    }

    private void Fire()
    {
        if (_activateTarget)
        {
            GameObject egg = (GameObject)Instantiate(
            this.EggPrefab,
            this.EggSpawn.position,
            this.EggSpawn.rotation);
            egg.transform.LookAt(_activateTarget.transform.position);
            egg.GetComponent<Rigidbody>().AddForce(egg.transform.forward * 1000);
            // Add velocity to the bullet
            egg.GetComponent<Rigidbody>().velocity = egg.transform.forward * 6;
            Destroy(_activateTarget);
            // Destroy the bullet after 2 seconds
            Destroy(egg, 5.0f);
        }
        
    }



}
