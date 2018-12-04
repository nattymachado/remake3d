using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyServer : NetworkLobbyPlayer {

    private LobbyManager _manager;

    public void Start()
    {
        _manager = LobbyManager.singleton.GetComponent<LobbyManager>();
    }

    public void OnServerStartGame()
    {
        Debug.Log(_manager.numPlayers);
        _manager.minPlayers = _manager.numPlayers;
        Debug.Log("Init Game");
    }
}
    