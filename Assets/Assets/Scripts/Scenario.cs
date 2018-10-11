using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenario : MonoBehaviour {

    public GameObject rock;
    public GameObject yoshi;
    public GameObject babyMario;
    public List<GameObject> PlayerSpawnPoints;
    public List<GameObject> MarioSpawnPoints;

    // Use this for initialization
    void Awake () {

        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                Instantiate(rock, new Vector3(x * Random.Range(0f, 500f), 0, Random.Range(0f, 1000f)), Quaternion.identity);
            }
        }

        SpawnPlayers();

        SpawnMario();

    }

    void SpawnPlayers()
    {
        GameObject nest = this.PlayerSpawnPoints[Random.Range(0, this.PlayerSpawnPoints.Count)];
        Debug.Log(nest);
        Debug.Log(nest.transform.position);
        GameObject newYoshi = Instantiate(yoshi, nest.transform.position, Quaternion.identity);
        nest.GetComponent<NestBehaviour>().Owner = newYoshi.GetInstanceID();
    }

    void SpawnMario()
    {
        Instantiate(babyMario, this.MarioSpawnPoints[Random.Range(0, this.MarioSpawnPoints.Count)].transform.position, Quaternion.identity);
    }
}
