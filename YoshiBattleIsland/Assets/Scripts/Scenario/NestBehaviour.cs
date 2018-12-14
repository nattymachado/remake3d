using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class NestBehaviour : NetworkBehaviour {

    private int ownerInstanceId;
    private Text _winnerText;
    private Image _winnerImage;
    private GameObject _winnerPanel;
    public AudioSource MainAudioSource;
    public AudioClip winClip;
    public AudioClip loseClip;
    private bool _isEnded = false;
    private LobbyManager _manager;
    private ToadController _toad;
    private Scenario _scenario;
    public Camera PlayerCamera;

    public void Start()
    {
        _winnerPanel = GameObject.FindGameObjectWithTag("WinnerText");
        _winnerText = _winnerPanel.GetComponentInChildren<Text>();
        _winnerImage = _winnerPanel.GetComponentInChildren<Image>();
        _winnerText.enabled = false;
        _winnerImage.enabled = false;
        _manager = LobbyManager.singleton.GetComponent<LobbyManager>();
        _scenario = Camera.main.GetComponent<Scenario>();
        _toad = _scenario.Toad.GetComponent<ToadController>();
        PlayerCamera = Camera.main;
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

    IEnumerator RestartScene(string winner, bool isLocal)
    {
        _isEnded = true;
        _scenario.GameEnded = true;
        MainAudioSource = Camera.main.GetComponent<AudioSource>();
        MainAudioSource.clip = winClip;
        _toad.EndGameWithAWinnerButNotYou(winner);
        MainAudioSource.Play();
        yield return new WaitForSeconds(5);
        StartCoroutine(_manager.ShutDownNetwork());

    }

    //Run on Client
    [ClientRpc]
    public void RpcRestart(string winner, bool isLocal)
    {
       StartCoroutine(RestartScene(winner, isLocal));
    }

    //Run on Server
    [Command]
    public void CmdRestart(string winner, bool isLocal)
    {
        RpcRestart(winner, isLocal);
    }


    //Run on Client
    [ClientRpc]
    public void RpcMoveCamera(NetworkInstanceId winner)
    {
        MoveCamera(winner);
    }

    //Run on Server
    [Command]
    public void CmdMoveCamera(NetworkInstanceId winner)
    {
        RpcMoveCamera(winner);
    }

    private void MoveCamera(NetworkInstanceId winner)
    {
        CameraController2 cameraController = PlayerCamera.GetComponent<CameraController2>();
        cameraController.LookAtTarget(ClientScene.FindLocalObject(winner));

    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController2 playerController = other.GetComponent<PlayerController2>();
       

        if (playerController != null && playerController.IsWithMario && !_isEnded && (playerController.NestPosition.x == transform.position.x) && (playerController.NestPosition.z == transform.position.z))
        {
             _isEnded = true;
            CmdMoveCamera(playerController.netId);
             CmdRestart(playerController.playerName, playerController.isLocalPlayer);
            
        }
    }

}
