using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JoinRoom : MonoBehaviour {

    LobbyManager lobbyManager;
    public GameObject PrefabForHost;
    public GameObject ParentForHost;
    public GameObject LobbyRoot;

    private int joinRoomSize = 20;

	// Use this for initialization
	void Start () {
        lobbyManager =  LobbyManager.singleton.GetComponent<LobbyManager>();
    }
	
    public void RefreshList()
    {
        if (lobbyManager == null)
        {
            lobbyManager = LobbyManager.singleton.GetComponent<LobbyManager>();
        }

        if (lobbyManager.matchMaker == null)
        {
            lobbyManager.StartMatchMaker();
        }

        lobbyManager.matchMaker.ListMatches(0, this.joinRoomSize, "", true,  0, 0, OnMatchList);
    }


    private void OnMatchList(bool sucess, string extendedInfo, List<UnityEngine.Networking.Match.MatchInfoSnapshot> matchList)
    {
        if (!sucess)
        {
            Debug.Log("Problems to refresh thr rooms!");
        } else
        {
            foreach (UnityEngine.Networking.Match.MatchInfoSnapshot match in matchList)
            {
                GameObject ListGo = Instantiate(PrefabForHost);
                ListGo.transform.SetParent(ParentForHost.transform);
                HostSetup hostSetup = ListGo.GetComponent<HostSetup>();
                hostSetup.Setup(match, LobbyRoot);
            }
        }
    }
}
