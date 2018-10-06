using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestBehaviour : MonoBehaviour {

    private int ownerInstanceId;
    private bool _isOwnerAlreadyExit = false;

    public int Owner
    {
        get
        {
            return this.ownerInstanceId;
        }

        set
        {
            this.ownerInstanceId = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         
        if (other.transform.parent && other.transform.parent.gameObject.GetInstanceID() == this.ownerInstanceId && _isOwnerAlreadyExit)
        {
            if (other.gameObject.GetComponent<PlayerController>() != null && other.gameObject.GetComponent<PlayerController>().IsWithMario)
            {
                Debug.Log("O JOGO ACABOU!!!!");
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.transform.parent.gameObject.GetInstanceID() == this.ownerInstanceId)
        {
            _isOwnerAlreadyExit = true;
        }
    }

}
