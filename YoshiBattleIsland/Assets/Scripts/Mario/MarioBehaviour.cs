using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.Networking;

public class MarioBehaviour :NetworkBehaviour {

    public float radiusDetection = 1.0f;
    private bool isWithYoshi = false;
    public GameObject origin = null;
    public GameObject bubble;
    private AudioSource _audio;
    public Transform goal;
    private NavMeshAgent _agent;

    private void Start()
    {
        bubble.SetActive(true);
        _audio = GetComponent<AudioSource>();
        _audio.enabled = true;
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(CryMario(5));
    }

    private IEnumerator CryMario(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        _audio.Play();
        StartCoroutine(CryMario(5));
    }

    void Update()
    {

        if (!isServer)
        {
            if (transform.parent)
            {
                UpdatePosition();
            }
            
            return;
        }
        if (!isWithYoshi)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusDetection);
            int i = 0;
            while (i < hitColliders.Length)
            {
                
                PlayerController2 playerController = hitColliders[i].GetComponent<PlayerController2>();
                if (playerController != null && !playerController.IsDizzy)
                {
                    Debug.Log("Achei alguma coisa!");
                    CmdSetAttributes(playerController.netId);
                }

                i++;
            }

            if (!this.isWithYoshi)
            {
               
                MoveToOrigin();
            }
        } else
        {
            UpdatePosition();
        }
        

    }

    private void UpdatePosition()
    {
        PlayerController2 playerController = transform.parent.GetComponent<PlayerController2>();
        transform.position = playerController.marioPoint.transform.position;
        transform.rotation = playerController.transform.rotation;
    }
            
    private void SetAttributesWithYoshi(NetworkInstanceId networkId)
    {
        

        PlayerController2 playerController = ClientScene.FindLocalObject(networkId).GetComponent<PlayerController2>();

        if (playerController.isLocalPlayer)
        {
            playerController.ShowToadMessageWhenFindMario();
        }
        else
        {
            playerController.ShowToadMessageSomeoneFoundMario();
        }

        transform.parent = playerController.transform;
        transform.position = playerController.marioPoint.transform.position;
        transform.rotation = playerController.transform.rotation;
        this.isWithYoshi = true;
        bubble.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;
        playerController.IsWithMario = true;
        playerController.MarioInstance = transform.gameObject;
        GetComponent<BoxCollider>().enabled = false;
       

        if (!_audio)
        {
            _audio = GetComponent<AudioSource>();
        }
        _audio.enabled = false; 
        
    }


    void MoveToOrigin()
    {
        bubble.SetActive(true);
        CmdEnableBubble();
        _agent = GetComponent<NavMeshAgent>();
        transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
        _agent.enabled = true;
        if (origin)
        {
            _agent.destination = origin.transform.position;
        }

    }

    //Run on Server
    [Command]
    public void CmdEnableBubble()
    {
        RpcEnableBubble();
    }

    //Run on Client
    [ClientRpc]
    public void RpcEnableBubble()
    {
        bubble.SetActive(true);
    }

    //Run on Server
    [Command]
   public void CmdReturnToOrigin()
   {
      RpcReturnToOrigin();
   }

   //Run on Client
   [ClientRpc]
   public void RpcReturnToOrigin()
   {
     _return();
   }

    //Run on Server
    [Command]
    public void CmdSetAttributes(NetworkInstanceId networkId)
    {
        RpcSetAttributes(networkId);
    }

    //Run on Client
    [ClientRpc]
    public void RpcSetAttributes(NetworkInstanceId networkId)
    {
        SetAttributesWithYoshi(networkId);
    }

    public void ReturnToOrigin()
    {
        if (!isServer)
        {
            return;
        }
        CmdReturnToOrigin();

    }

    private void _return()
    {
        if (transform.parent)
        {
            PlayerController2 playerController = transform.parent.GetComponent<PlayerController2>();
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
