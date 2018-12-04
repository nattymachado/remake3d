using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class PlayerController2 : NetworkBehaviour
{

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public float speed = 6.0f;
    public float speedRotation = 100f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public GameObject cameraPoint;

    public Text EggText;
    public GameObject EggPrefab;
    public GameObject BigEggPrefab;
    private List<GameObject> _normalEggs;
    private GameObject _specialEgg;
    private int _specialEggPosition = -1;
    private int _eggQuantity = 100;
    private int _eggIndex = 0;
    public Transform EggSpawn;
    public Camera PlayerCamera;
    public string playerName=null;
    public GameObject PlayerNameTextMesh;
    private TextMeshPro _textMesh;
    public bool sendPlayerName=false;
    public bool IsWithMario = false;
    private Animator _animator;
    public Transform TougueSpawnRight;
    public Transform TougueSpawnLeft;
    public Transform TougueSpawnCenter;
    public GameObject TouguePrefab;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        PlayerCamera = Camera.main;
        _textMesh = PlayerNameTextMesh.GetComponent<TextMeshPro>();
        if (isLocalPlayer)
        {
            CameraController2 cameraController = PlayerCamera.GetComponent<CameraController2>();
            cameraController.LookAtTarget(cameraPoint);
            _textMesh.text = "";
        }

        _normalEggs = new List<GameObject>();
        while (_normalEggs.Count < 10)
        {
            GameObject egg = (GameObject)Instantiate(this.EggPrefab, new Vector3(-100, -100, -1000), Quaternion.identity);
            egg.GetComponent<EggBehaviour>().Owner = transform.gameObject;
            _normalEggs.Add(egg);
        }

        _specialEgg = (GameObject)Instantiate(this.EggPrefab, new Vector3(-100, -100, -1000), Quaternion.identity);
        _specialEgg.GetComponent<EggBehaviour>().Owner = transform.gameObject;
    }

    void Update()
    {
         if (isServer && !sendPlayerName)
         {
                RpcSetPlayerName(playerName);
                sendPlayerName = true;
         }
        if (!isLocalPlayer)
        {
            return;
        }

        CheckHeadPosition();

        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            transform.Rotate(0, Input.GetAxis("Horizontal") * speedRotation * Time.deltaTime, 0);


            moveDirection = Vector3.forward * Input.GetAxis("Vertical");
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButtonDown("Jump")) {
                moveDirection.y = jumpSpeed;
            }


        }

        
        this._animator.SetBool("IsStopped", false);

        

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetButtonUp("Fire2"))
        {
           
                //this.RemoveEgg();
            Vector3 direction = this.GetFireDirection();
            Fire(direction);
            CmdEggFire(direction);
            
        }
        if (Input.GetButtonUp("Fire1") && !this.IsWithMario)
        {
            TougueFire();
            CmdTougueFire();
        }
    }

    private void TougueFire()
    {
        _animator.SetBool("OpenMouth", true);
        StartCoroutine(CloseMouth());
        StartCoroutine(CreateTougue());

    }

    void CheckHeadPosition()
    {

        Vector3 objectPos = this.GetMousePosition();
        int headPosition = 1;

        if (objectPos.x < (Screen.width / 2 - 50f))
        {
            headPosition = 1;
            
        }
        else if (objectPos.x > (Screen.width / 2 + 50f))
        {
            headPosition = 2;
        }
        else
        {

            headPosition = 3;
        }
        _animator.SetInteger("HeadPosition", headPosition);
        CmdUpdateHeadPosition(headPosition);
    }

    IEnumerator CreateTougue()
    {
  
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


    private Vector3 GetFireDirection()
    {
        Vector3 mousePos = this.GetMousePosition();

        Vector3 objectPos = PlayerCamera.ScreenToWorldPoint(mousePos);
        return (EggSpawn.transform.position - objectPos).normalized;
    }

    private Vector3 GetMousePosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = -300.0f;
        mousePos.y = 400.0f;
        return mousePos;
    }

    IEnumerator DestroyEggs(GameObject egg)
    {
        yield return new WaitForSeconds(5);
        egg.transform.position = new Vector3(-1000, -1000, -1000);
    }

    private void Fire(Vector3 direction)
    {
        GameObject egg;

        if (this._eggQuantity > 0)
        {

            if (this._specialEggPosition == this._eggQuantity - 1)
            {
                egg = this._specialEgg;
            }
            else
            {
                egg = this._normalEggs[this._eggIndex];
            }

            Debug.Log("EggIndex:" + this._eggIndex);
            this._eggQuantity -= 1;
            this._eggIndex = this._eggIndex < 9 ? this._eggIndex + 1 : 0;     


            egg.transform.position = this.EggSpawn.position;
            egg.transform.LookAt(this.EggSpawn.position + direction * 100);
            egg.GetComponent<Rigidbody>().AddForce(egg.transform.forward * 100);
            egg.GetComponent<Rigidbody>().velocity = egg.transform.forward * 20;
            StartCoroutine(DestroyEggs(egg));
        }

    }

    //Run on Client
    [ClientRpc]
    public void RpcEggFire(Vector3 direction)
    {
        if (!isLocalPlayer)
        {
            Fire(direction);
        }
    }

    //Run on Client
    [ClientRpc]
    public void RpcSetPlayerName(string playerName)
    {
        Debug.Log("PlayerName:" + playerName);
        this.playerName = playerName;
        if (this._textMesh == null)
        {
            this._textMesh = PlayerNameTextMesh.GetComponent<TextMeshPro>();
        }

        this._textMesh.text = playerName;
    }

    //Run on Server
    [Command]
    public void CmdEggFire(Vector3 direction)
    {
        RpcEggFire(direction);
    }

    //Run on Server
    [Command]
    public void CmdTougueFire()
    {
        RpcTougueFire();
    }

    //Run on Server
    [Command]
    public void CmdUpdateHeadPosition(int headPosition)
    {
        RpcUpdateHeadPosition(headPosition);
    }

    //Run on Client
    [ClientRpc]
    public void RpcUpdateHeadPosition(int headPosition)
    {
        if (!isLocalPlayer)
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            _animator.SetInteger("HeadPosition", headPosition);
        }
    }

    //Run on Client
    [ClientRpc]
    public void RpcTougueFire()
    {
        if (!isLocalPlayer)
        {
            TougueFire();
        }
    }

}
