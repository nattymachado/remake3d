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
    public GameObject Toad;
    public int MaxMinutes = 5;
    public int minutes = -1;
    private float _totalTime;
    private Text _winnerText;
    private Image _winnerImage;
    private GameObject _winnerPanel;
    public bool GameEnded = false;
    public NetworkSpawner NetworkSpawner;
    public Image EggImage;
    public GameObject Sea;

    // Use this for initialization
    void Awake () {
    }

   
    private void Start()
    {
        _winnerPanel = GameObject.FindGameObjectWithTag("WinnerText");
        _winnerText = _winnerPanel.GetComponentInChildren<Text>();
        _winnerImage = _winnerPanel.GetComponentInChildren<Image>();
        _winnerText.enabled = false;
        _winnerImage.enabled = false;
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
        
        
        
    }
}
