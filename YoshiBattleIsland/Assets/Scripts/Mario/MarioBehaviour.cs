using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;

public class MarioBehaviour : MonoBehaviour {

    public float radiusDetection = 1.0f;
    private bool isWithYoshi = false;
    public GameObject origin = null;
    public GameObject bubble;
    private AudioSource _audio;
    public Transform goal;
    private NavMeshAgent _agent;

    private Vector3 _positionOnYoshi = new Vector3(10f, 10f, 10f);
    private Quaternion _rotationOnYoshi = new Quaternion(-16.313f, -0.087f, 0.608f, 0);

    private void Start()
    {
        bubble.SetActive(true);
        _audio = transform.GetComponent<AudioSource>();
        _audio.enabled = true;
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isWithYoshi)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusDetection);
            int i = 0;
            while (i < hitColliders.Length)
            {
                PlayerController playerController = hitColliders[i].GetComponent<PlayerController>();
                if (playerController != null && !playerController.IsDizzy)
                {
                    transform.GetComponent<Rigidbody>().isKinematic = true;
                    _audio.enabled = false;
                    transform.parent = hitColliders[i].transform;
                    _agent.enabled = false;
                    playerController.IsWithMario = true;
                    playerController.MarioInstance = transform.gameObject;
                    transform.position = new Vector3(hitColliders[i].transform.position.x, hitColliders[i].transform.position.y + 1f, hitColliders[i].transform.position.z - 1f);
                    transform.rotation = Quaternion.identity;
                    transform.GetComponent<BoxCollider>().enabled = false;
                    this.isWithYoshi = true;
                    bubble.SetActive(false);
                }

                i++;
            }

            if (!this.isWithYoshi)
            {
                _audio.enabled = true;
                MoveToOrigin();
            }
        }
        

    }


    void MoveToOrigin()
    {
        bubble.SetActive(true);
        _agent = GetComponent<NavMeshAgent>();
        transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
        _agent.enabled = true;
        if (origin)
        {
            _agent.destination = origin.transform.position;
        }
        
       
       /* Vector3 direction = (origin.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
        Debug.Log("Movendo: " + direction);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.MovePosition(transform.position + direction * 5.0f * Time.deltaTime);
        Quaternion lookToOrigin = Quaternion.LookRotation(direction);
        GetComponent<Rigidbody>().MoveRotation(lookToOrigin);
       */

    }

    public void ReturnToOrigin()
    {
        if (transform.parent)
        {
            PlayerController playerController = transform.parent.GetComponent<PlayerController>();
            if (playerController)
            {
                transform.parent = null;
                transform.GetComponent<Rigidbody>().isKinematic = false;
                playerController.IsWithMario = false;
                playerController.MarioInstance = null;
                transform.GetComponent<BoxCollider>().enabled = true;
                this.isWithYoshi = false;

            }
        }
        
    }
}
