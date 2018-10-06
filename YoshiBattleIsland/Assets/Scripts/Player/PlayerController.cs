using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed;
    public float SpeedRotation;
    public float JumpSpeed;

    private bool _isWithMario = false;

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
    }
    
}
