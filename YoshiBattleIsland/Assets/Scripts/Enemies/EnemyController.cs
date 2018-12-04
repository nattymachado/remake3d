using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Vector3 target;
    private bool followPlayer;
    private GameObject yoshi;
    public float timeToChangeDirection = -2;
    public Vector3 positionTarget;
    private Animator _animator;
    public bool IsDizzy = false;
    private float collisionTime = 0;
    public bool isBowser = false;
    public Scenario scenarioScript;
    public void Start()
    {
        this._animator = GetComponent<Animator>();
    }

    private Vector3 ChangeDirection()
    {
        Vector3 position = new Vector3(Random.Range(0, 360f), 0, Random.Range(0, 360f));
        return position;
    }

    IEnumerator ControllDizzyState(bool isBig)
    {
        this._animator.SetBool("IsDizzy", true);
        this.IsDizzy = true;
        if (isBig)
        {
            yield return new WaitForSeconds(60);
        } else
        {
            
            yield return new WaitForSeconds(30);
        }
        
        this._animator.SetBool("IsDizzy", false);
        this.IsDizzy = false;

    }

    private void OnCollisionStay(Collision collision)
    {

        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        if (controller != null && !controller.IsDizzy)
        {
            collisionTime += Time.deltaTime;
            if (collisionTime >= 1)
            {
                Debug.Log("Perdeu o Mario!");
                if (controller.MarioInstance!= null)
                {
                    controller.MarioInstance.GetComponent<MarioBehaviour>().ReturnToOrigin();
                }

                //controller.BeDizzy();
                collisionTime = 0;
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Um ovo me atingiu");
        EggBehaviour eggScript = collision.gameObject.GetComponent<EggBehaviour>();
        if (eggScript != null)
        {
            Debug.Log("Um ovo me atingiu");
            
            StartCoroutine(ControllDizzyState(eggScript.IsBig));

        }

    }

    void FixedUpdate()
    {
 
        if (this.IsDizzy)
            return;

        timeToChangeDirection -= Time.deltaTime;
        if (timeToChangeDirection <= 0)
        {
            positionTarget = ChangeDirection();
            timeToChangeDirection = 10;
        }

        if (yoshi != null)
        {
            if (yoshi.GetComponent<PlayerController>().IsDizzy)
            {
                yoshi = null;
                positionTarget = ChangeDirection();
                timeToChangeDirection = 10;
            } else
            {
                positionTarget = yoshi.transform.position;
            }
            
        }

        Vector3 direction = (positionTarget - transform.position).normalized;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.MovePosition(transform.position + direction * 2.0f * Time.fixedDeltaTime);
        Quaternion lookToPlayer = Quaternion.LookRotation(direction);
        GetComponent<Rigidbody>().MoveRotation(lookToPlayer);

    }
    private void OnTriggerEnter(Collider other)
    {

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null && (!controller.IsDizzy))
        {
            yoshi = controller.gameObject;

        }

    }

    private void OnTriggerStay(Collider other)
    {
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
        Debug.Log("I will be an egg now");
    }

    

    
}
