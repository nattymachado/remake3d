using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TougueController : MonoBehaviour {

    private bool _isFiring = false;
    public Scenario scenarioScript;
    public GameObject BigEgg;
    public GameObject Egg;
    private PlayerController2 owner;

    public void Start()
    {
        scenarioScript = Camera.main.GetComponent<Scenario>();
        owner = GetComponentInParent<PlayerController2>();

    }

    private void OnTriggerStay(Collider other)
    {
        CheckCollider(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
        
    }

    public void PrepareTougue()
    {
        BigEgg.SetActive(false);
        Egg.SetActive(false);
    }

    private void CheckCollider(Collider other)
    {
        if (other.isTrigger && other.GetType().ToString() == "UnityEngine.CapsuleCollider" && !_isFiring)
        {
           
            EnemyNavNetworkController controller = other.GetComponent<EnemyNavNetworkController>();
            if (controller != null && (!controller.isBowser || controller.IsDizzy))
            {

                _isFiring = true;
                bool isBig = false;
                if (!controller.isBowser)
                {
                    controller.gameObject.SetActive(false);
                    Egg.SetActive(true);
                    scenarioScript.NetworkSpawner.SendShyGuyToOrigin(controller.gameObject);
                } else
                {
                    isBig = true;
                    Destroy(controller.gameObject);
                    BigEgg.SetActive(true);
                }

                owner.IncreaseEgg(isBig);

            }
        }

    }

    public IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        transform.parent.gameObject.SetActive(false);
        _isFiring = false;
        
    }

    private void Update()
    {
    }

}
