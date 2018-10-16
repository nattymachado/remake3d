using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NestBehaviour : MonoBehaviour {

    private int ownerInstanceId;
    private bool _isOwnerAlreadyExit = false;
    private Text _winnerText;
    private Image _winnerImage;
    private GameObject _winnerPanel;
    public AudioSource MainAudioSource;
    public AudioClip winClip;
    private bool _isEnded = false;

    public void Start()
    {
        _winnerPanel = GameObject.FindGameObjectWithTag("WinnerText");
        _winnerText = _winnerPanel.GetComponentInChildren<Text>();
        _winnerImage = _winnerPanel.GetComponentInChildren<Image>();
        _winnerText.enabled = false;
        _winnerImage.enabled = false;
    }

    public int Owner
    {
        get
        {
            return this.ownerInstanceId;
        }

        set
        {
            this.ownerInstanceId = value;
        }
    }

    IEnumerator RestartScene()
    {
        _winnerText.text = "You are the winner!!!!";
        _winnerText.enabled = true;
        _winnerImage.enabled = true;
        MainAudioSource.clip = winClip;
        MainAudioSource.Play();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Winner 1");
        if (other.transform.parent && other.transform.parent.gameObject.GetInstanceID() == this.ownerInstanceId && _isOwnerAlreadyExit)
        {
            Debug.Log("Winner 2");
            if (other.gameObject.GetComponent<PlayerController>() != null && other.gameObject.GetComponent<PlayerController>().IsWithMario && !_isEnded)
            {
                Debug.Log("Winner 3");
                _winnerText.enabled = true;
                _winnerImage.enabled = true;
                _isEnded = true;
                StartCoroutine(RestartScene());
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.transform && other.transform.parent && other.transform.parent.gameObject.GetInstanceID() == this.ownerInstanceId)
        {
            _isOwnerAlreadyExit = true;
        }
    }

}
