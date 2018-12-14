using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NetworkSpawner : NetworkBehaviour
{


    public GameObject babyBowser;

    public GameObject babyMario;
    public List<GameObject> MarioSpawnPoints;


    public List<GameObject> ShyGuySpawnPoints;
    public List<GameObject> ShyGuys;
    public List<GameObject> SpawnPlayerPoints;
    public GameObject shyGuy;
    public GameObject Timer;
    public int shyGuyQuantity;
    public AudioClip WhenANewShyGuyBorn;
    private AudioSource _audio;
    public ToadController ToadScript;

    public GameObject MagicEgg;


    void Start()
    {
        _audio = GetComponent<AudioSource>();
        if (!isServer) { return; }

        ToadScript = Camera.main.GetComponent<Scenario>().Toad.GetComponent<ToadController>();

        SpawnPlayerPointsInstantiate();

        GameObject bowser = Instantiate(babyBowser, new Vector3(0, 0, 0), Quaternion.identity);
       
        NetworkServer.Spawn(bowser);

        GameObject timerInstance = Instantiate(Timer, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(timerInstance);

        GameObject magicEggInstance = Instantiate(MagicEgg, new Vector3(-24.45f, 17f, -0.5f), Quaternion.Euler(90, 0, 0));
        NetworkServer.Spawn(magicEggInstance);

        SpawnMario();

        for (int i=0; i < shyGuyQuantity; i++)
        {
            StartCoroutine(WaitToSpawn(i*2));
        }
    }

    private IEnumerator WaitToSpawn(int time)
    {
        yield return new WaitForSeconds(time);
        SpawnShayGuy();
    }

    void SpawnMario()
    {
        int spawnPosition = Random.Range(0, this.MarioSpawnPoints.Count);
        ToadScript.MarioPosition = spawnPosition;
        GameObject babyMarioInstance = Instantiate(babyMario, this.MarioSpawnPoints[spawnPosition].transform.position, Quaternion.identity);
        MarioBehaviour marioBehaviour = babyMarioInstance.GetComponent<MarioBehaviour>();
        marioBehaviour.origin = this.MarioSpawnPoints[spawnPosition];
        NetworkServer.Spawn(babyMarioInstance);
    }

    public void SpawnShayGuy()
    {
        int spawnPosition = Random.Range(0, this.ShyGuySpawnPoints.Count);
        GameObject shyGuyInstance = Instantiate(shyGuy, this.ShyGuySpawnPoints[spawnPosition].transform.position, Quaternion.identity);
        PlayAudioShyGuyBorn();
        NetworkServer.Spawn(shyGuyInstance);
    }

    public void SpawnPlayerPointsInstantiate()
    {
        foreach (GameObject spawnPoint in SpawnPlayerPoints)
        {
            GameObject spawnPointInstance = Instantiate(spawnPoint, spawnPoint.transform.position, Quaternion.identity);
            NetworkServer.Spawn(spawnPointInstance);
        }
       
    }

    public void SendShyGuyToOrigin(GameObject shyGuy)
    {
        StartCoroutine(WaitToInitOtherShyGuy(shyGuy));

    }

    private IEnumerator WaitToInitOtherShyGuy(GameObject shyGuy)
    {
        shyGuy.GetComponent<EnemyNavNetworkController>().IsDizzy = false;
        shyGuy.transform.position = new Vector3(-1000, -1000, -1000);
        yield return new WaitForSeconds(3);
        shyGuy.transform.position = this.ShyGuySpawnPoints[Random.Range(0, this.ShyGuySpawnPoints.Count)].transform.position;
        PlayAudioShyGuyBorn();


    }

    private void PlayAudioShyGuyBorn()
    {
        _audio.clip = WhenANewShyGuyBorn;
        _audio.Play();
    }



}
