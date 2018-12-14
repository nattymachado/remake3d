using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Timer : NetworkBehaviour  {

   
    public int MaxMinutes = 5;
    private float _totalTime = 0;
    private ToadController _toad;
    private LobbyManager _manager;
    private bool _alreadySentMessage = false;
    private Scenario _scenario;

    public void Start()
    {
        _toad = Camera.main.GetComponent<Scenario>().Toad.GetComponent<ToadController>();
        _manager = LobbyManager.singleton.GetComponent<LobbyManager>();
        _scenario = Camera.main.GetComponent<Scenario>();
    }

    private void Update()
    {
        if (!isServer)
        {
            return;
        }

        _totalTime += Time.deltaTime;
        int _minutes = (int) _totalTime / 60;
        if (_minutes >= MaxMinutes)
        {
            Debug.Log("Acabou o tempo 1");
            RpcRestart();
        } else if (_minutes >= (MaxMinutes/2) && !_alreadySentMessage) {
            RpcSendHalfTimeMessage(_toad.MarioPosition);
        }
    }

    IEnumerator RestartScene()
    {
        Debug.Log("Acabou o tempo 3");
        _scenario.GameEnded = true;
        _toad.EndGameTimeout();
        yield return new WaitForSeconds(3);
        StartCoroutine(_manager.ShutDownNetwork());
    }

    //Run on Client
    [ClientRpc]
    public void RpcRestart()
    {
        Debug.Log("Acabou o tempo 2");
        StartCoroutine(RestartScene());
    }

    private void SendHalfTimeMessage(int mPosition)
    {
        _alreadySentMessage = true;
        _toad.MarioPositionIs(mPosition);
    }

    //Run on Client
    [ClientRpc]
    public void RpcSendHalfTimeMessage(int mPosition)
    {
        SendHalfTimeMessage(mPosition);
    }


}
