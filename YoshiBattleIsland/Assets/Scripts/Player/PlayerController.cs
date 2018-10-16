using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float Speed;
    public float SpeedRotation;
    public float JumpSpeed;
    public Text EggText;
    public GameObject EggPrefab;
    public GameObject BigEggPrefab;
    public GameObject TargetPrefab;
    public Transform EggSpawn;
    public Camera playerCamera;
    public float collisionTime = 0;
    private int targetDistance = 5;
    private LineRenderer lineRenderer;
    public int BigEggPosition = 0;
    private bool _isDizzy = false;
    public GameObject dizzyBols;
    private Animator _animator;

    private GameObject _activateTarget = null;


    private bool _isWithMario = false;
    private GameObject _marioInstance = null;
    private int eggsQuantity = 0;



    IEnumerator ControllDizzyState()
    {
        this.dizzyBols.SetActive(true);
        this._animator.enabled = true;
        this._animator.SetBool("IsDizzy", true);
        this._isDizzy = true;
        yield return new WaitForSeconds(15);
        this._animator.SetBool("IsDizzy", false);
        this._isDizzy = false;
        this._animator.enabled = false;
        this.dizzyBols.SetActive(false);

    }


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

    public bool IsDizzy
    {
        get
        {
            return this._isDizzy;
        }
    }

    public GameObject MarioInstance
    {
        get
        {
            return this._marioInstance;
        }

        set
        {
            this._marioInstance = value;
        }
    }

    public void AddEgg(bool isBig)
    {
        this.eggsQuantity += 1;
        EggText.text = "x " + this.eggsQuantity;
        
        if (isBig)
        {
            Debug.Log("Is Bowser");
            this.BigEggPosition = this.eggsQuantity;
        }
    }

    public void RemoveEgg()
    {
        this.eggsQuantity -= 1;
        EggText.text = "x " + this.eggsQuantity;
        Debug.Log("Eggs: " + this.eggsQuantity);
    }

    private Quaternion currentRotation;


    void Start()
    {
        EggText = GameObject.FindGameObjectWithTag("EggQuantityText").GetComponent<Text>();
        EggText.text = "x " + this.eggsQuantity;
        lineRenderer = transform.GetComponent<LineRenderer>();
        InvokeRepeating("RefreshTargetDistance", 0f, 2f);
        this._animator = GetComponent<Animator>();
        this.dizzyBols.SetActive(false);
    }

    public void BeDizzy()
    {
        StartCoroutine(ControllDizzyState());
    }

    void DrawTargetLine(Vector3 targetPosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.materials[0].mainTextureScale = new Vector3(2, 1, 1);
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(targetPosition.x, targetPosition.y, targetPosition.z));
    }

    void UpdateTargetPosition(int distance)
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 20.0f;
        if (mousePos.y < 116f)
        {
            mousePos.y = 116f;
        }
        Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);
        DrawTargetLine(objectPos);
        Vector3 direction = (objectPos - transform.position).normalized;
        _activateTarget.transform.position = transform.position + direction * distance;
    }


    void Update()
    {

       
    }

    // Fixed update is used for physics
    void FixedUpdate()
    {
        bool hitSomething = false;




       /* if (lineRenderer)
        {
            RaycastHit hitInfo;

            if (_activateTarget)
            {
                if (Physics.Linecast(transform.position, _activateTarget.transform.position, out hitInfo))
                {

                    if (hitInfo.collider.gameObject.GetComponent<EnemyController>() != null)
                    {
                        this.targetDistance -= 1;
                        Debug.Log("Pagou um shygay");
                    }
                    hitSomething = true;


                }


                if (hitSomething)
                {
                    Debug.Log("Tá pegando");
                }
                
            }

        }*/

        if (!this._isDizzy)
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedRotation;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
            float y = 0;


            if (transform.position.y < JumpSpeed && !IsWithMario)
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
                UpdateTargetPosition(this.targetDistance);
            }

            if (Input.GetButtonDown("Fire2") && !IsWithMario)
            {
                if (_activateTarget == null)
                {
                    this.targetDistance = 5;
                    var mousePos = Input.mousePosition;
                    mousePos.z = 20.0f;
                    Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);
                    Vector3 direction = (objectPos - transform.position).normalized;
                    DrawTargetLine(objectPos);
                    _activateTarget = (GameObject)Instantiate(
                        this.TargetPrefab,
                        transform.position + direction * 5, transform.rotation);
                }

            }
            if (Input.GetButtonUp("Fire2") && !IsWithMario)
            {

                if (eggsQuantity > 0)
                {
                    this.RemoveEgg();
                    Fire();
                }
                else
                {
                    Destroy(_activateTarget);
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    private void RefreshTargetDistance()
    {
        this.targetDistance = 5;
    }

    private void Fire()
    {
        if (_activateTarget)
        {
            GameObject egg ;

            if (this.BigEggPosition == this.eggsQuantity + 1)
            {
                egg = (GameObject)Instantiate(
            this.BigEggPrefab,
            this.EggSpawn.position,
            this.EggSpawn.rotation);
                egg.GetComponent<EggBehaviour>().IsBig = true;
            } else
            {
                egg = (GameObject)Instantiate(
            this.EggPrefab,
            this.EggSpawn.position,
            this.EggSpawn.rotation);
            }
            egg.transform.LookAt(_activateTarget.transform.position);
            egg.GetComponent<Rigidbody>().AddForce(egg.transform.forward * 1000);
            // Add velocity to the bullet
            egg.GetComponent<Rigidbody>().velocity = egg.transform.forward * 6;

            Destroy(_activateTarget);
            //lineRenderer.enabled = false;
            // Destroy the bullet after 2 seconds
            if (this.BigEggPosition != this.eggsQuantity) {
                Destroy(egg, 5.0f);
            }
            
        }
        
    }



}
