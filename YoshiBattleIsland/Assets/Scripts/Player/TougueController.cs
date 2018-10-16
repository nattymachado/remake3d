using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TougueController : MonoBehaviour {

    private Animator _tougleAnimator;
    private bool _isFiring = false;

    public void Start()
    {
        _tougleAnimator = transform.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {

       EnemyController controller = other.GetComponent<EnemyController>();
       if (controller != null && (!controller.isBowser || controller.IsDizzy ))
        {
            Debug.Log(transform.localScale.z);
            if (transform.localScale.z > 0.1)
            {
                _tougleAnimator.SetBool("IsWithEgg", true);
                _tougleAnimator.SetBool("IsBig", controller.isBowser);
                controller.TransformOnEgg();
                transform.parent.parent.GetComponent<PlayerController>().AddEgg(controller.isBowser);
                controller.gameObject.SetActive(false);
                Destroy(controller.gameObject, 0.2f);
                
            }
        }
        
    }

    IEnumerator WaitForAnimation(float time)
    {
        //print(Time.time);
        yield return new WaitForSeconds(time);
        _tougleAnimator.SetBool("IsWithEgg", false);
        _isFiring = false;
 
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && !_isFiring  && !transform.parent.parent.GetComponent<PlayerController>().IsWithMario && !transform.parent.parent.GetComponent<PlayerController>().IsDizzy)
        {
            _isFiring = true;
            _tougleAnimator.SetTrigger("MovimentTougle");
            StartCoroutine(WaitForAnimation(1f));
        }
    }

}
