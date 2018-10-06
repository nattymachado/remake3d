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
        
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            _tougleAnimator.SetTrigger("MovimentTougle");
        }
    }

}
