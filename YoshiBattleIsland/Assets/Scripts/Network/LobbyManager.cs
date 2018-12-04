using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkLobbyManager {

    public GameObject Lobby;
    public GameObject LobbyPlayers;
    public GameObject Canvas;
    public GameObject yoshiGreenPrefab;
    public GameObject yoshiBluePrefab;
    public GameObject yoshiRedPrefab;
    public GameObject yoshiPinkPrefab;
    public GameObject yoshiYellowPrefab;
    public GameObject lobbyPlayerPref;
    public int PlayersOnArena = 0;
    public string UserName;
    public string UserColor;


    public List<GameObject> PlayerSpawnPoints;


    public void Start()
    {
        Debug.Log("Setting to false");
        Lobby.SetActive(false);
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("Creating new player");
        Debug.Log(lobbyPlayer.GetComponent<LobbyPlayer>().name);
        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }

   public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Um novo cara conectou");
        GameObject ParentPref = LobbyManager.singleton.GetComponent<LobbyManager>().LobbyPlayers;
        Debug.Log(ParentPref.GetComponentsInChildren<LobbyPlayer>().Length);
        base.OnClientConnect(conn);
    }

    public override void OnStartHost()
    {
        Debug.Log("Starting Host");
        base.OnStartHost();
        Lobby.SetActive(true);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Adding a player");
        base.OnServerAddPlayer(conn, playerControllerId);
    }


    public override void OnLobbyServerPlayersReady()
    {
        base.OnLobbyServerPlayersReady();
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
      
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject ParentPref = LobbyManager.singleton.GetComponent<LobbyManager>().LobbyPlayers;
        Debug.Log(ParentPref.GetComponentsInChildren<LobbyPlayer>().Length);
        LobbyPlayer[] players = ParentPref.GetComponentsInChildren<LobbyPlayer>();
        foreach (LobbyPlayer player in players)
        {
            player.RpcSetPlayerName(player.UserInput.text, player.ColorDropDown.options[player.ColorDropDown.value].text, player.netId);

        }
        GameObject lobbyPlayer = Instantiate(lobbyPlayerPref, new Vector3(0,0,0), Quaternion.identity);
        return lobbyPlayer;
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        int indexNext = Random.Range(0, this.PlayerSpawnPoints.Count);
        GameObject nest = this.PlayerSpawnPoints[indexNext];
        GameObject yoshiPrefab;
        LobbyPlayer lobbyPlayer = conn.playerControllers[0].gameObject.GetComponent<LobbyPlayer>();
        string playerColor = lobbyPlayer.ColorDropDown.options[lobbyPlayer.ColorDropDown.value].text;
        this.PlayerSpawnPoints.RemoveAt(indexNext);
        switch (playerColor)
        {
            case "green":
                yoshiPrefab = yoshiGreenPrefab;
                playerPrefab = yoshiGreenPrefab;
                break;
            case "red":
                yoshiPrefab = yoshiRedPrefab;
                playerPrefab = yoshiRedPrefab;
                break;
            case "blue":
                yoshiPrefab = yoshiBluePrefab;
                playerPrefab = yoshiBluePrefab;
                break;
            case "yellow":
                yoshiPrefab = yoshiYellowPrefab;
                playerPrefab = yoshiYellowPrefab;
                break;
            case "pink":
                yoshiPrefab = yoshiPinkPrefab;
                playerPrefab = yoshiPinkPrefab;
                break;
            default:
                yoshiPrefab = yoshiGreenPrefab;
                break;
        }

        //yoshiPrefab.GetComponent<PlayerController2>().playerName = lobbyPlayer.UserInput.text;
        GameObject player = Instantiate(yoshiPrefab, nest.transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        Debug.Log("player Name:" + lobbyPlayer.UserInput.text);
        player.GetComponent<PlayerController2>().playerName = lobbyPlayer.UserInput.text;
        nest.GetComponent<NestBehaviour>().Owner = player.GetInstanceID();
        return player;
    }


    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        
        base.OnClientSceneChanged(conn);
        Canvas.SetActive(false);
    }




}
