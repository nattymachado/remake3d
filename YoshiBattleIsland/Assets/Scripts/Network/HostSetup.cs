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

        _lobbyManager.matchMaker.ListMatches(0, 1, Match.name, true, 0, 0, OnMatchList);

        
    }

    private void OnMatchList(bool sucess, string extendedInfo, List<UnityEngine.Networking.Match.MatchInfoSnapshot> matchList)
    {
        if (!sucess)
        {
            Debug.Log("Problems to check the information of the match");
        }
        else
        {
            if (matchList.Count != 1)
            {
                Debug.Log("Problems to check the information of the match");
            } else
            {
                Debug.Log("Current Size:" + matchList[0].currentSize);
                Debug.Log("Max size:" + matchList[0].maxSize);
                if (matchList[0].currentSize < (matchList[0].maxSize))
                {
                    Debug.Log("Join ...");
                    _lobbyRoot.SetActive(true);
                    Transform[] items = _lobbyRoot.GetComponentsInChildren<Transform>(true);

                    foreach (Transform item in items)
                    {
                        item.gameObject.SetActive(true);
                    }
                    _lobbyManager.matchMaker.JoinMatch(Match.networkId, "", "", "", 0, 0, _lobbyManager.OnMatchJoined);
                } else
                {
                    Debug.Log("A sala já esta cheia");
                }
            }
        }

    }

    private void GoToLobby()
    {

    }
}
