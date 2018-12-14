using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyNavNetworkController : NetworkBehaviour
{

    private Vector3 target;
    private bool followPlayer;
    private GameObject yoshi;
    public float timeToChangeDirection = -1;
    public Vector3 positionTarget;
    private Animator _animator;
    public bool IsDizzy = false;
    private bool _isWaitingToGo = false;
    public bool isBowser = false;
    private NavMeshAgent _agent;
    public List<GameObject> GoPoints;

    public void Start()
    {
        this._animator = GetComponent<Animator>();
        this._agent = GetComponent<NavMeshAgent>();
    }

    private Vector3 ChangeDirection()
    {
        int positionInt = Random.Range(0, this.GoPoints.Count);
        Vector3 position = this.GoPoints[positionInt].transform.position;
        return position;
    }

    IEnumerator ControllDizzyState(bool isBig)
    {
        this._agent.enabled = false;
        this._animator.SetBool("IsDizzy", true);
        this.IsDizzy = true;
        if (isBig)
        {
            yield return new WaitForSeconds(4);
        } else
        {
            
            yield return new WaitForSeconds(6);
        }
        this._agent.enabled = true;
        this._animator.SetBool("IsDizzy", false);
        this.IsDizzy = false;

    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
        {
            return;
        }

        EggBehaviour eggScript = collision.gameObject.GetComponent<EggBehaviour>();
        if (eggScript != null)
        {
            
            StartCoroutine(ControllDizzyState(eggScript.IsBig));
            CmdDizzy(eggScript.IsBig);

        }

    }

    //Run on Client
    [ClientRpc]
    public void RpcDizzy(bool isBig)
    {
        if (!isServer)
        {
            StartCoroutine(ControllDizzyState(isBig));

        }
    }

    //Run on Server
    [Command]
    public void CmdDizzy(bool isBig)
    {
        RpcDizzy(isBig);
    }

    void MoveToTarget(Vector3 position)
    {
        
        _agent.enabled = true;
        _agent.destination = position;

    }

    void Update()
    {
        if (!isServer)
        {
            _agent.enabled = false;
            return;
        }
 
        if (this.IsDizzy)
            return;
        timeToChangeDirection -= Time.deltaTime;
        if (this._agent.isStopped || (timeToChangeDirection <= 0))
        {
            positionTarget = ChangeDirection();
            timeToChangeDirection = 10;

        }

        if (yoshi != null && !this._isWaitingToGo)
        {
            SetYoshiAsTarget();
            
        }

        Vector3 direction = (positionTarget - transform.position).normalized;
        MoveToTarget(positionTarget);
    }


    private void SetYoshiAsTarget()
    {
        if (yoshi.GetComponent<PlayerController2>().IsDizzy)
        {
            positionTarget = Vector3.Reflect(yoshi.transform.position, Vector3.forward);
            yoshi = null;
            StartCoroutine(WaitingToBeReadyToGo());
        }
        else
        {
            positionTarget = yoshi.transform.position;
        }
    }


    IEnumerator WaitingToBeReadyToGo()
    {
        this._isWaitingToGo = true;
        yield return new WaitForSeconds(4);
        this._isWaitingToGo = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        PlayerController2 controller = other.GetComponent<PlayerController2>();
        if (controller != null && (!controller.IsDizzy))
        {
            yoshi = controller.gameObject;

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (yoshi == null)
        {
            PlayerController2 controller = other.GetComponent<PlayerController2>();
            if (controller != null && (!controller.IsDizzy))
            {
                yoshi = controller.gameObject;

            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        PlayerController2 controller = other.GetComponent<PlayerController2>();
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
       // Debug.Log("I will be an egg now");
    }
}
