using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyGuyController2 : MonoBehaviour {

    public GameObject body;
    public GameObject egg;
    public float collisionTime = 0;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void TransformOnEgg()
    {
        Debug.Log("I will be an egg now");
        //egg.SetActive(true);
        //body.SetActive(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collided");

        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            collisionTime += Time.deltaTime;
            if (collisionTime >= 20)
            {
                Debug.Log("Perdeu o Mario!");
                collisionTime = 0;
            }

        }

    }

}
