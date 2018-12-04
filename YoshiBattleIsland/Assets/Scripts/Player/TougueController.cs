using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TougueController : MonoBehaviour {

    private Animator _tougleAnimator;
    public GameObject EggPrefab;
    public GameObject BigEggPrefab;
    private bool _isFiring = false;
    private bool _isDetroyed = false;
    private bool _isWithEgg = false;
    public Transform EggSpawn;
    public Scenario scenarioScript;

    public void Start()
    {
        StartCoroutine(WaitToDestroy());
        scenarioScript = Camera.main.GetComponent<Scenario>();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (!other.isTrigger)
        {
            EnemyNavController controller = other.GetComponent<EnemyNavController>();
            if (controller != null && (!controller.isBowser || controller.IsDizzy))
            {

                controller.TransformOnEgg();
                Destroy(controller.gameObject);
                
                GameObject egg = null;
                if (controller.isBowser)
                {
                    egg = (GameObject)Instantiate(
                         this.BigEggPrefab,
                         this.EggSpawn.position,
                         this.EggSpawn.rotation);
                } else
                {
                    egg = (GameObject)Instantiate(
                         this.EggPrefab,
                         this.EggSpawn.position,
                         this.EggSpawn.rotation);
                    scenarioScript.SpawnShayGuy();
                }
                
                egg.transform.parent = transform.parent;
                this._isDetroyed = true;
                this._isWithEgg = true;
                Destroy(transform.parent.gameObject, 0.2f);

            }
        }
       
        
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        if (!this._isDetroyed && !this._isWithEgg)
        {
            Destroy(transform.parent.gameObject);
        }
        
       
    }

    private void Update()
    {
    }

}
