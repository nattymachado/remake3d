using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkLobbyPlayer {

    public GameObject ParentPref;
    public Button JoinButton;
    public Text JoinButtonText;
    public Dropdown ColorDropDown;
    public GameObject ColorDropDownArrow;
    public InputField UserInput;

    public void Start()
    {
       
    }


    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        ParentPref = LobbyManager.singleton.GetComponent<LobbyManager>().LobbyPlayers;
        gameObject.transform.SetParent(ParentPref.transform);
    }

    public void OnClickJoinButton()
    {
        LobbyManager manager = LobbyManager.singleton.GetComponent<LobbyManager>();
        manager.UserName = UserInput.text;
        manager.UserColor = ColorDropDown.options[ColorDropDown.value].text;
        CmdUserPropertiesToServer(netId, manager.UserName, manager.UserColor);
        SendReadyToBeginMessage();
        print("I am ready");
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SetupLocalPlayer();
    }

    private void SetupLocalPlayer()
    {
        JoinButton.interactable = true;
        JoinButtonText.text = "JOIN";
        UserInput.interactable = true;
        ColorDropDown.interactable = true;
    }
    private int GetDropDownValueForColor(string color)
    {
        switch (color)
        {
            case "green":
                return 0;
            case "yellow":
                return 1;
            case "pink":
                return 2;
            case "red":
                return 3;
            case "dark blue":
                return 4;
            case "baby blue":
                return 5;
            case "grey":
                return 6;
            case "purple":
                return 7;
            default:
                return 0;
        }
    }

   //Run on Client
   [ClientRpc]
   public void RpcSetPlayerName(string name, string color, NetworkInstanceId playerId)
    {
        LobbyPlayer[] players = ParentPref.GetComponentsInChildren<LobbyPlayer>();
        foreach (LobbyPlayer player in players)
        {
            if (player.netId == playerId)
            {
                player.UserInput.text = name;
                player.ColorDropDown.value = GetDropDownValueForColor(color);
            }
            
        }

    }

    //Run on Server
    [Command]
    public void CmdUserPropertiesToServer(NetworkInstanceId playerId, string playerName, string playerColor)
    {
        RpcSetPlayerName(playerName, playerColor, playerId);
    }
}
    