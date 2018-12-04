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
    private float collisionTime = 0;
    public bool isBowser = false;
    private NavMeshAgent _agent;

    public void Start()
    {
        this._animator = GetComponent<Animator>();
        this._agent = GetComponent<NavMeshAgent>();
    }

    private Vector3 ChangeDirection()
    {
        Debug.Log("Changing direction");
        Vector3 position = new Vector3(Random.Range(0, 100f), 0, Random.Range(0, 100f));
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

    private void OnCollisionStay(Collision collision)
    {
        if (!isServer)
        {
            return;
        }
        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        if (controller != null && !controller.IsDizzy)
        {
            collisionTime += Time.deltaTime;
            this._animator.SetBool("IsHitting", true);
            if (collisionTime >= 0.1f)
            {
                Debug.Log("Perdeu o Mario!");
                if (controller.MarioInstance!= null)
                {
                    controller.MarioInstance.GetComponent<MarioBehaviour>().ReturnToOrigin();
                }

                controller.BeDizzy();
                collisionTime = 0;
            }

        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isServer)
        {
            return;
        }
        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        if (controller != null && this._animator.GetBool("IsHitting"))
        {
            this._animator.SetBool("IsHitting", false);

        }
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

        }

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
            return;
        }
 
        if (this.IsDizzy)
            return;

        timeToChangeDirection -= Time.deltaTime;
        if (timeToChangeDirection <= 0)
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
        if (yoshi.GetComponent<PlayerController>().IsDizzy)
        {
            positionTarget = Vector3.Reflect(yoshi.transform.position, Vector3.forward);
            yoshi = null;
            StartCoroutine(WaitingToBeReadyToGo());
            timeToChangeDirection = 10;
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
        if (!isServer)
        {
            return;
        }
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null && (!controller.IsDizzy))
        {
            Debug.Log("O yoshi entrou");
            yoshi = controller.gameObject;

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (!isServer)
        {
            return;
        }
        if (yoshi == null)
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null && (!controller.IsDizzy))
            {
                yoshi = controller.gameObject;

            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!isServer)
        {
            return;
        }

        PlayerController controller = other.GetComponent<PlayerController>();
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
        if (!isServer)
        {
            return;
        }
        Debug.Log("I will be an egg now");
    }
}
