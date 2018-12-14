using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JoinRoom : MonoBehaviour {

    LobbyManager lobbyManager;
    public GameObject PrefabForHost;
    public GameObject ParentForHost;
    public GameObject LobbyRoot;
    private float refreshTime = 0;

    private int joinRoomSize = 4;
    private List<string> matches;

    // Use this for initialization
    void Start () {
        lobbyManager =  LobbyManager.singleton.GetComponent<LobbyManager>();
        matches = new List<string>();
    }

    void Update()
    {
        refreshTime += Time.deltaTime;
        if (refreshTime >= 3)
        {
            Debug.Log("Updating");
            RefreshList();
            refreshTime = 0;
        }
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
                Debug.Log("Current Size:" + match.currentSize);
                Debug.Log("Max size:" + match.maxSize);
                if (!matches.Contains(match.name))
                {
                    GameObject ListGo = Instantiate(PrefabForHost);
                    ListGo.transform.SetParent(ParentForHost.transform);
                    HostSetup hostSetup = ListGo.GetComponent<HostSetup>();
                    hostSetup.Setup(match, LobbyRoot);
                    matches.Add(match.name);
                }
            }
        }
    }

   
}
