using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {

    LobbyManager lobbyManager;
    public InputField MatchNameInput;
    public Dropdown PlayerNumber;
    public GameObject LobbyRoot;

    // Use this for initialization
    void Start()
    {
        lobbyManager = LobbyManager.singleton.GetComponent<LobbyManager>();
    }

    public void OnClickHostButton()
    {
        Button hostButton = GetComponent<Button>();
        hostButton.interactable = false;
        LobbyRoot.SetActive(true);
        lobbyManager.StartMatchMaker();
        int players = PlayerNumber.value + 2;
        Debug.Log(players);
        Debug.Log(MatchNameInput.text);
        lobbyManager.matchMaker.CreateMatch(MatchNameInput.text, (uint)players, true, "", "", "", 0, 0, lobbyManager.OnMatchCreate);
        lobbyManager.minPlayers = players - 1;


    }
}
