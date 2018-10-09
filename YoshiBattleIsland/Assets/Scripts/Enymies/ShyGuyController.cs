using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyGuyController : MonoBehaviour {

    public GameObject body;
    public GameObject egg;

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
}
