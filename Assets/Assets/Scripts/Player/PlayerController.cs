using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed;
    public float SpeedRotation;
    public float JumpSpeed;
    public GameObject EggPrefab;
    public Transform EggSpawn;

    private bool _isWithMario = false;
    private int eggsQuantity = 0;

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
            if (Input.GetButton("Jump"))
            {
                y = JumpSpeed;
            }
        }
        


        transform.Rotate(0, x, 0);
        transform.Translate(0, y, z);
        if (Input.GetButtonUp("Fire2") && this.eggsQuantity > 0)
        {
            Debug.Log(Input.mousePosition);
        }
        if (Input.GetButtonUp("Fire2") && this.eggsQuantity > 0)
        {
            this.RemoveEgg();
            Fire();
        }
    }

    private void Fire()
    {
        GameObject egg = (GameObject)Instantiate(
            this.EggPrefab,
            this.EggSpawn.position,
            this. EggSpawn.rotation);

        // Add velocity to the bullet
        egg.GetComponent<Rigidbody>().velocity = egg.transform.forward * 6;

        // Destroy the bullet after 2 seconds
        Destroy(egg, 2.0f);
    }



}
