using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenu : NetworkBehaviour
{

    public LobbyManager lobbyManager;
    public GameObject JoinRoomObj;
    public GameObject CreateRoomObj;
    private JoinRoom _joinRoom; 

    public void OnClickJoinButton()
    {
        lobbyManager.StartMatchMaker();
        JoinRoomObj.SetActive(true);
        _joinRoom = JoinRoomObj.GetComponent<JoinRoom>();
        _joinRoom.RefreshList();
    }

    public void OnClickCreateButton()
    {
       CreateRoomObj.SetActive(true);
    }

    public void OnClickBackButton()
    {
        if (lobbyManager)
        {
            lobbyManager.StopMatchMaker();
        }
       
        JoinRoomObj.SetActive(false);
        CreateRoomObj.SetActive(false);
            
    }



}
