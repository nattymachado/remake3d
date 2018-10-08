using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TougueController : MonoBehaviour {

    private Animator _tougleAnimator;

    public void Start()
    {
        _tougleAnimator = transform.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {

        
       Debug.Log("Peguei algo");
       ShyGuyController controller = other.GetComponent<ShyGuyController>();
       if (controller != null && controller.transform.parent != transform)
        {
            controller.TransformOnEgg();
            //controller.transform.parent = transform;
            _tougleAnimator.SetBool("IsWithEgg", true);
            Destroy(controller.gameObject, 1.0f);
        }
        
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            _tougleAnimator.SetTrigger("MovimentTougle");
        }
    }

}
