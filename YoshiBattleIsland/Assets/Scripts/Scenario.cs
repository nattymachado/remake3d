using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenario : MonoBehaviour  {

    public GameObject rock;
    public GameObject yoshi;
    public GameObject babyMario;
    public GameObject shayGuy;
    public int shayGuyNumber;
    public AudioSource MainAudioSource;
    public AudioClip loseClip;
    public List<GameObject> PlayerSpawnPoints;
    public List<GameObject> MarioSpawnPoints;
    public List<GameObject> ShayGuySpawnPoints;
    public GameObject Toad;
    public int MaxSeconds = 1;
    private float _totalTime;
    private Text _winnerText;
    private Image _winnerImage;
    private GameObject _winnerPanel;
    private bool _isEnded = false;

    // Use this for initialization
    void Awake () {

        SpawnMario();

        /*for (int i =0; i < shayGuyNumber; i++)
        {
            SpawnShayGuy();
        }*/
    }

    public void SpawnShayGuy()
    {
        int spawnPosition = Random.Range(0, this.ShayGuySpawnPoints.Count);
        GameObject shayGuyInstance = Instantiate(shayGuy, this.ShayGuySpawnPoints[spawnPosition].transform.position, Quaternion.identity);
    }

    private void Start()
    {
        _winnerPanel = GameObject.FindGameObjectWithTag("WinnerText");
        _winnerText = _winnerPanel.GetComponentInChildren<Text>();
        _winnerImage = _winnerPanel.GetComponentInChildren<Image>();
        _winnerText.enabled = false;
        _winnerImage.enabled = false;
    }

    void SpawnPlayers()
    {
        GameObject nest = this.PlayerSpawnPoints[Random.Range(0, this.PlayerSpawnPoints.Count)];
        Debug.Log(nest);
        Debug.Log(nest.transform.position);
        GameObject newYoshi = Instantiate(yoshi, nest.transform.position + new Vector3(0,1.2f,0), Quaternion.identity);
        newYoshi.GetComponentInChildren<PlayerController>().ToadScript = Toad.GetComponent<ToadController>();
        nest.GetComponent<NestBehaviour>().Owner = newYoshi.GetInstanceID();
    }

    void SpawnMario()
    {
        int spawnPosition = Random.Range(0, this.MarioSpawnPoints.Count);
        GameObject babyMarioInstance =  Instantiate(babyMario, this.MarioSpawnPoints[spawnPosition].transform.position, Quaternion.identity);
        //MarioBehaviour marioBehaviour = babyMarioInstance.GetComponent<MarioBehaviour>();
       // marioBehaviour.origin = this.MarioSpawnPoints[spawnPosition];
    }

    IEnumerator RestartScene()
    {
        _winnerText.text = "Timeout!!!!";
        _winnerText.enabled = true;
        _winnerImage.enabled = true;
        Debug.Log("Winner 4");
        MainAudioSource.Stop();
        MainAudioSource.clip = loseClip;
        MainAudioSource.Play();
        yield return new WaitForSeconds(50);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        _totalTime += Time.deltaTime;
        int seconds = (int) _totalTime % 60;
        if (seconds  >= MaxSeconds && !_isEnded)
        {
            _isEnded = true;
            StartCoroutine(RestartScene());

        }
        
        
    }
}
