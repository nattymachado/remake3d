using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.AI;

public class PlayerController2 : NetworkBehaviour
{

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public float speed = 6.0f;
    public float speedRotation = 100f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public GameObject cameraPoint;
    public GameObject marioPoint;
    public Text EggText;
    public float EggDistance, EggForce, EggVelocity = 0;
    private bool isWithMagicEgg = false;

    public GameObject EggPrefab;
    public GameObject BigEggPrefab;
    private List<GameObject> _normalEggs;
    private GameObject _specialEgg;
    private int _eggQuantity = 10;
    private int _eggIndex = 0;
    public Transform EggSpawn;
    public Camera PlayerCamera;
    public string playerName=null;
    public GameObject PlayerNameTextMesh;
    public TextMeshPro _textMesh;
    public bool sendPlayerName=false;
    public bool IsWithMario = false;
    private Animator _animator;
    public GameObject TougueRight;
    public GameObject TougueLeft;
    public GameObject TougueCenter;
    public bool IsDizzy = false;
    public GameObject MarioInstance;
    public ToadController ToadScript;
    public Vector3 NestPosition;
    private Image _eggImage;
    public Sprite SmallEggSprite;
    public Sprite BigEggSprite;
    public Sprite MagicEggSprite;
    private int _bigEggPosition = -1;
    private GameObject _sea;
    private LobbyManager _manager;

    public AudioClip JumpClip;
    public AudioClip FireTougueClip;
    public AudioClip FireEggClip;
    private AudioSource _audioSource;
    private Scenario _scenarioScript;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        _manager = LobbyManager.singleton.GetComponent<LobbyManager>();
        _animator = GetComponent<Animator>();
        _scenarioScript = Camera.main.GetComponent<Scenario>();
        ToadScript = _scenarioScript.Toad.GetComponent<ToadController>();
        _eggImage = _scenarioScript.EggImage;
        _sea = _scenarioScript.Sea;
        _audioSource = GetComponent<AudioSource>();
        PlayerCamera = Camera.main;
        _textMesh = PlayerNameTextMesh.GetComponent<TextMeshPro>();

        playerName = _manager.UserName;

        EggText = GameObject.FindGameObjectWithTag("EggQuantityText").GetComponent<Text>();
        UpdateEggText();

        if (isLocalPlayer)
        {
            CameraController2 cameraController = PlayerCamera.GetComponent<CameraController2>();
            cameraController.LookAtTarget(cameraPoint);

            _textMesh.enabled = false;
        }
        


        _normalEggs = new List<GameObject>();
        while (_normalEggs.Count < 10)
        {
            GameObject egg = (GameObject)Instantiate(this.EggPrefab, new Vector3(-100, -100, -1000), Quaternion.identity);
            egg.GetComponent<EggBehaviour>().Owner = transform.gameObject;
            _normalEggs.Add(egg);
        }

        _specialEgg = (GameObject)Instantiate(this.BigEggPrefab, new Vector3(-100, -100, -1000), Quaternion.identity);
        _specialEgg.GetComponent<EggBehaviour>().Owner = transform.gameObject;
    }

    private void UpdateEggText()
    {
        if (isLocalPlayer)
        {
            if (isWithMagicEgg)
            {
                EggText.text = "x " + this._eggQuantity;
                _eggImage.sprite = MagicEggSprite;
                return;
            }

            EggText.text = "x " + this._eggQuantity;
            if (this._bigEggPosition == this._eggQuantity)
            {
                _eggImage.sprite = BigEggSprite;
                _eggImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            } else
            {
                _eggImage.sprite = SmallEggSprite;
                _eggImage.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    void Update()
    {
        if (_scenarioScript.GameEnded)
        {
            return;
        }

        if (!isLocalPlayer )
        {
            return;
        }
       
        bool isStopped = false;
        CheckHeadPosition();
        _audioSource.clip = null;
        if (controller.isGrounded)
        {

            // We are grounded, so recalculate
            // move direction directly from axes
            if (!IsDizzy)
            {
                transform.Rotate(0, Input.GetAxis("Horizontal") * speedRotation * Time.deltaTime, 0);


                moveDirection = Vector3.forward * Input.GetAxis("Vertical");
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                    _audioSource.clip = JumpClip;

                }
            } else
            {
                moveDirection.x = 0;
                moveDirection.z = 0;
            }
           
            
            

            
            if ((moveDirection.x == 0 && moveDirection.z == 0) || (moveDirection.y != 0))
            {
                isStopped = true;
            } else
            {

                isStopped = false;
                

            }
            this._animator.SetBool("IsStopped", isStopped);
            CmdSetIsStopped(isStopped);

        }

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        Vector3 previousPosition = transform.position;
        _audioSource.Play();
        controller.Move(moveDirection * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.transform.gameObject == _sea)
            {
                transform.position = previousPosition;
                moveDirection.x = 0;
                moveDirection.z = 0;
                controller.Move(moveDirection * Time.deltaTime);
            }
            
        }

        if (Input.GetButtonUp("Fire2") && !IsDizzy && !this.IsWithMario)
        {
           
            RemoveEgg();
            Vector3 direction = this.GetFireDirection();
            Fire(direction);
            CmdEggFire(direction);
            
        }
        if (Input.GetButtonUp("Fire1") && !this.IsWithMario && !IsDizzy)
        {
            TougueFire();
            CmdTougueFire();
        }
    }

    private void RemoveEgg()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (!isLocalPlayer || IsDizzy)
        {
            return;
        }
        ValidateCollider(other);


    }

    private void OnTriggerStay(Collider other)
    {


        if (!isLocalPlayer || IsDizzy)
         {
             return;
         }
        ValidateCollider(other);


    }

    //Run on Client
    [ClientRpc]
    public void RpcDestroyMagicEgg(NetworkInstanceId magicEgg)
    {
        if (!isLocalPlayer)
        {
            DestroyMagicEgg(magicEgg);
        }
    }

    //Run on Server
    [Command]
    public void CmdDestroyMagicEgg(NetworkInstanceId magicEgg)
    {
        RpcDestroyMagicEgg(magicEgg);
    }

    private void DestroyMagicEgg(NetworkInstanceId netId)
    {
        MagicEggBehaviour magicEggInstance = ClientScene.FindLocalObject(netId).GetComponent<MagicEggBehaviour>();
        Destroy(magicEggInstance);
    }

    //Run on Client
    [ClientRpc]
    public void RpcBeDizzy(int timeBeDizzy)
    {
        if (!isLocalPlayer)
        {
            BeDizzy(timeBeDizzy);
        }
    }

    //Run on Server
    [Command]
    public void CmdBeDizzy(int timeBeDizzy)
    {
        RpcBeDizzy(timeBeDizzy);
    }

    private IEnumerator RemoveMagicEgg()
    {
        yield return new WaitForSeconds(40);
        isWithMagicEgg = false;
    }

    private void ValidateCollider(Collider other)
    {
        EggBehaviour eggBehaviour = other.gameObject.GetComponent<EggBehaviour>();
        if (eggBehaviour != null && eggBehaviour.Owner != transform.gameObject)
        {
            int dizzyTime = 3;
            if (eggBehaviour.IsBig)
            {
                dizzyTime = 6;
            }
            BeDizzy(dizzyTime);
            CmdBeDizzy(dizzyTime);
            return;
        }

        MagicEggBehaviour mEggBehaviour = other.gameObject.GetComponent<MagicEggBehaviour>();
        if (mEggBehaviour != null)
        {
            
            isWithMagicEgg  = true;
            UpdateEggText();
            StartCoroutine(RemoveMagicEgg());
            CmdDestroyMagicEgg(mEggBehaviour.netId);
            Destroy(mEggBehaviour.gameObject);
            return;
        }

        if (other.GetType().ToString() == "UnityEngine.CapsuleCollider")
        {
            
            EnemyNavNetworkController enemy = other.gameObject.GetComponent<EnemyNavNetworkController>();
            if (enemy != null && !enemy.IsDizzy)
            {
                if (enemy.isBowser)
                {
                    BeDizzy(5);
                    CmdBeDizzy(5);
                }
                else
                {
                    BeDizzy(3);
                    CmdBeDizzy(3);
                }
            }
        }
    }

    public void ShowToadMessageWhenFindMario()
    {
        ToadScript.FindMario();
    }

    public void ShowToadMessageSomeoneFoundMario()
    {
        ToadScript.SomeoneFoundMario();
    }

    public void BeDizzy(int timeToBeDizzy)
    {
        if (this.MarioInstance)
        {
            this.MarioInstance.GetComponent<MarioBehaviour>().ReturnToOrigin();
        }
        StartCoroutine(ControllDizzyState(timeToBeDizzy));
    }

    private IEnumerator ControllDizzyState(int timeToBeDizzy)
    {
        this.IsDizzy = true;
        this._animator.SetBool("IsDizzy", true);
        this._animator.SetBool("IsStopped", true);
        yield return new WaitForSeconds(timeToBeDizzy);
        this.IsDizzy = false;
        this._animator.SetBool("IsDizzy", false);
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
        GameObject tougue = TougueCenter;
        if (headPosition == 1)
        {
            tougue = TougueLeft;
        }
        else if (headPosition == 2)
        {
            tougue = TougueRight;
        }
        TougueController controller = tougue.GetComponentInChildren<TougueController>();
        controller.PrepareTougue();
        tougue.SetActive(true);

        StartCoroutine(controller.WaitToDestroy());
    }

    IEnumerator CloseMouth()
    {
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

    public void IncreaseEgg(bool isBig)
    {
        this._eggQuantity += 1;
        if (isBig)
        {
            this._bigEggPosition = this._eggQuantity;
        }
        UpdateEggText();
    }

    private void Fire(Vector3 direction)
    {
        GameObject egg;
        _audioSource.clip = FireEggClip;

        if (this._eggQuantity > 0 || this.isWithMagicEgg)
        {

            if (this._bigEggPosition == this._eggQuantity && !this.isWithMagicEgg)
            {
                egg = this._specialEgg;
                this._bigEggPosition = -1;
            }
            else
            {
                egg = this._normalEggs[this._eggIndex];
            }

            if (!isWithMagicEgg)
            {
                this._eggQuantity -= 1;
            }
            
            this._eggIndex = this._eggIndex < 9 ? this._eggIndex + 1 : 0;     


            egg.transform.position = this.EggSpawn.position;
            egg.transform.LookAt(this.EggSpawn.position + direction * EggDistance);
            egg.GetComponent<Rigidbody>().AddForce(egg.transform.forward * EggForce);
            egg.GetComponent<Rigidbody>().velocity = egg.transform.forward * EggVelocity;
            _audioSource.Play();
            UpdateEggText();
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

    //Run on Server
    [Command]
    public void CmdSetPlayerName(string playerName)
    {
        RpcSetPlayerName(playerName);
    }

    //Run on Client
    [ClientRpc]
    public void RpcSetPlayerName(string playerName)
    {
        this.playerName = playerName;
        if (this._textMesh == null)
        {
            this._textMesh = PlayerNameTextMesh.GetComponent<TextMeshPro>();
        }

        //this._textMesh.text = playerName;
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

    //Run on Server
    [Command]
    public void CmdSetIsStopped(bool isStopped)
    {
        RpcSetIsStopped(isStopped);
    }

    //Run on Client
    [ClientRpc]
    public void RpcSetIsStopped(bool isStopped)
    {
        if (!isLocalPlayer)
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            _animator.SetBool("IsStopped", isStopped);
        }
    }

}
