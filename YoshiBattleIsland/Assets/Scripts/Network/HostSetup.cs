using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class HostSetup : MonoBehaviour {

    private MatchInfoSnapshot Match;
    public Text HostName;
    private LobbyManager _lobbyManager;
    private GameObject _lobbyRoot;

	// Use this for initialization
	void Start () {
		_lobbyManager = LobbyManager.singleton.GetComponent<LobbyManager>();
    }
	
	
	public void Setup (MatchInfoSnapshot match, GameObject lobbyRoot) {

        
        Match = match;
        HostName.text = Match.name;
        _lobbyRoot = lobbyRoot;

    }

    public void Join()
    {
        if (_lobbyManager == null)
        {
            _lobbyManager = LobbyManager.singleton.GetComponent<LobbyManager>();
        }

        _lobbyRoot.SetActive(true);
        Transform[] items = _lobbyRoot.GetComponentsInChildren<Transform> (true);

        foreach (Transform item in items) {
            item.gameObject.SetActive(true);
        }
        _lobbyManager.matchMaker.JoinMatch(Match.networkId, "", "", "", 0, 0, _lobbyManager.OnMatchJoined);
    }
}
