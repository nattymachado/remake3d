using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioBehaviour : MonoBehaviour {

    public float radiusDetection = 1.0f;
    private bool isWithYoshi = false;

    void Update()
    {
        if (!isWithYoshi)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusDetection);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].GetComponent<PlayerController>() != null)
                {
                    transform.GetComponent<Rigidbody>().isKinematic = true;
                    transform.parent = hitColliders[i].transform;
                    hitColliders[i].GetComponent<PlayerController>().IsWithMario = true;
                    transform.position = new Vector3(hitColliders[i].transform.position.x + 1, hitColliders[i].transform.position.y + 1, hitColliders[i].transform.position.z - 1);
                    transform.GetComponent<BoxCollider>().enabled = false;
                    isWithYoshi = true;
                }

                i++;
            }
        }
        

    }
}
