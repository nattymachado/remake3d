using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenario : MonoBehaviour {

    public GameObject rock;
    public GameObject yoshi;
    public GameObject babyMario;

    // Use this for initialization
    void Awake () {

        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                Instantiate(rock, new Vector3(x * Random.Range(0f, 500f), 0, Random.Range(0f, 1000f)), Quaternion.identity);
            }
        }

        //Instantiate(yoshi, new Vector3(250f, 0, 250f), Quaternion.identity);

        Instantiate(babyMario, new Vector3(Random.Range(10f, 100f), 0, Random.Range(10f, 100f)), Quaternion.identity);

    }
}
