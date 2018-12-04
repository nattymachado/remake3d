using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float Speed;
    public float SpeedRotation;
    public float JumpSpeed;
    public int TargetDistance = 100;
    private Animator _animator;
    private Rigidbody _rb;
    private bool _isGrounded=true;
    public ToadController ToadScript;
    private LobbyManager _lobbyManager;
    public Text EggText;
    public GameObject EggPrefab;
    public GameObject BigEggPrefab;
    public GameObject TouguePrefab;
    public GameObject TargetPrefab;
    public Transform EggSpawn;
    public Transform TougueSpawnRight;
    public Transform TougueSpawnLeft;
    public Transform TougueSpawnCenter;
    public Camera playerCamera;
    public GameObject playerCameraObj;
    public float collisionTime = 0;
    public int BigEggPosition = 0;
    private bool _isDizzy = false;
    public GameObject dizzyStars;
    private bool _isWithMario = false;
    private GameObject _marioInstance = null;
    private int _eggsQuantity = 50;
    private bool _isJumping=false;
    private float _distToGround;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;


    public void BeDizzy()
    {
        StartCoroutine(ControllDizzyState());
    }

    private IEnumerator ControllDizzyState()
    {
        this._animator.SetTrigger("Dizzy");
        this._animator.SetBool("IsStopped", true);
        this._isDizzy = true;
        yield return new WaitForSeconds(1);
        this._animator.ResetTrigger("Dizzy");
        
        this._isGrounded = true;
        this._isJumping = false;
        this._isDizzy = false;
        
        

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<EnemyController>() != null)
        {
            this._rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        

        Debug.Log(col.gameObject.name);
        if (this.IsGrounded())
        {
            Debug.Log(col.gameObject.name);
            this._isJumping = false;
            this._rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }

    private bool IsGrounded()
    {

        _distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        return Physics.Raycast(transform.position, - Vector3.up, _distToGround + 0.20f);
    }

    void OnCollisionExit(Collision col)
    {
        
        if (col.gameObject.GetComponent<EnemyController>() != null)
        {
            this._rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
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

    IEnumerator Jump()
    {
        print(Time.time);
        yield return new WaitForSeconds(0.1f);
        this._rb.constraints = RigidbodyConstraints.FreezeRotation;
        this._animator.SetBool("IsJumping", false);
        this._rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }

    public void Start()
    {
        this._animator = GetComponent<Animator>();
        this._rb = GetComponent<Rigidbody>();
        this._lobbyManager = LobbyManager.singleton.GetComponent<LobbyManager>();
        _lobbyManager.PlayersOnArena = _lobbyManager.PlayersOnArena + 1;
        EggText = GameObject.FindGameObjectWithTag("EggQuantityText").GetComponent<Text>();
        EggText.text = "x " + this._eggsQuantity;

        if (isLocalPlayer)
        {
            playerCameraObj.SetActive(true);
        }
    }

    void CheckHeadPosition()
    {

        Vector3 objectPos = this.GetMousePosition();

        if (objectPos.x < (Screen.width / 2 - 50f))
        {
            _animator.SetInteger("HeadPosition", 1);
        }
        else if (objectPos.x > (Screen.width / 2 + 50f))
        {
            _animator.SetInteger("HeadPosition", 2);
        } else
        {

            _animator.SetInteger("HeadPosition", 0);
        }
    }

    void Update()
    {


        /*if ((_lobbyManager.PlayersOnArena + 1) != _lobbyManager.matchSize)
        {
            return;
        }*/

        if (!isLocalPlayer || dizzyStars.activeSelf)
        {
            return;
        }

        Animator anime = this.GetComponent<Animator>();
        if (!dizzyStars.activeSelf)
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedRotation;
            float z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
            float y = 0;

            if (Input.GetButtonDown("Jump") && this.IsGrounded())
            {
                this._isJumping = true;
                this._animator.SetBool("IsJumping", true);
                StartCoroutine(Jump());
                return;
            }

            if ((x != 0 || z != 0))
            {
                
                this._animator.SetBool("IsStopped", false);

            }
            else
            {
                
                this._animator.SetBool("IsStopped", true);
                
            }

            
            transform.Rotate(0, x, 0);
            transform.Translate(0, y, z);
            this.CheckHeadPosition();

            if (Input.GetButtonUp("Fire2") && !this.IsWithMario)
            {
                if (this._eggsQuantity > 0)
                {
                    //this.RemoveEgg();
                    this.Fire();
                }
            }

            if (Input.GetButtonUp("Fire1") && !this.IsWithMario)
            {
                RpcTougueFire();
            }
        }
        
    }

    //Run on Client
    [ClientRpc]
    public void RpcTougueFire()
    {
        TougueFire();

    }

    //Run on Server
    [Command]
    public void CmdTougueFire()
    {
        RpcTougueFire();
    }

    private void Fire()
    {
        GameObject egg;

        if (this.BigEggPosition == this._eggsQuantity + 1)
        {
            egg = (GameObject)Instantiate(
                this.BigEggPrefab,
                this.EggSpawn.position,
                this.EggSpawn.rotation);
                egg.GetComponent<EggBehaviour>().IsBig = true;
            }
            else
            {
                egg = (GameObject)Instantiate(
                    this.EggPrefab,
                    this.EggSpawn.position,
                    this.EggSpawn.rotation);
            }

            Vector3 direction = this.GetFireDirection();

            egg.transform.LookAt(this.EggSpawn.position + direction * TargetDistance);
            egg.GetComponent<Rigidbody>().AddForce(egg.transform.forward * 100);
            egg.GetComponent<Rigidbody>().velocity = egg.transform.forward * 20;

            Destroy(egg, 5.0f);
    }

        private void TougueFire()
    {
        _animator.SetBool("OpenMouth", true);
        StartCoroutine(CloseMouth());
        StartCoroutine(CreateTougue());

    }

    IEnumerator CreateTougue()
    {
        print(Time.time);
        yield return new WaitForSeconds(0.1f);
        int headPosition = _animator.GetInteger("HeadPosition");
        Transform touguePosition = this.TougueSpawnCenter;
        if (headPosition == 1)
        {
            touguePosition = this.TougueSpawnLeft;
        }
        else if (headPosition == 2)
        {
            touguePosition = this.TougueSpawnRight;
        }
        GameObject tougue = (GameObject)Instantiate(
                  this.TouguePrefab,
                  touguePosition.position,
                  touguePosition.rotation);
        tougue.transform.parent = transform;
    }

    IEnumerator CloseMouth()
    {
        print(Time.time);
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("OpenMouth", false);  
    }

    private Vector3 GetMousePosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = -300.0f;
        mousePos.y = 400.0f;
        return mousePos;
    }

    private Vector3 GetFireDirection()
    {


        Vector3 mousePos = this.GetMousePosition();

        Vector3 objectPos = playerCamera.ScreenToWorldPoint(mousePos);
        //Debug.Log(objectPos);
        return  (EggSpawn.transform.position-objectPos).normalized;
        //Debug.Log(direction);
    }

    public void ShowToadMessageWhenFindMario()
    {
        ToadScript.FindMario();
    }



}
