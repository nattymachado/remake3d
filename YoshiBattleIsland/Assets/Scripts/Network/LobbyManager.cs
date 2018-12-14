using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : NetworkLobbyManager {

    public GameObject Lobby;
    public GameObject LobbyPlayers;
    public GameObject Canvas;
    public GameObject yoshiGreenPrefab;
    public GameObject yoshiBluePrefab;
    public GameObject yoshiRedPrefab;
    public GameObject yoshiPinkPrefab;
    public GameObject yoshiYellowPrefab;
    public GameObject yoshiPurplePrefab;
    public GameObject yoshiGrayPrefab;
    public GameObject yoshiBabyBluePrefab;
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
        gamePlayer.GetComponent<PlayerController2>().PlayerNameTextMesh.GetComponent<TextMeshPro>().text = lobbyPlayer.GetComponent<LobbyPlayer>().UserInput.text;
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

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Stopping server");
        Destroy(LobbyManager.singleton.gameObject);
        SceneManager.LoadScene("LobbyScene");

        
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        Debug.Log("Stopping host");
        Destroy(LobbyManager.singleton.gameObject);
        SceneManager.LoadScene("LobbyScene");
        
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        Debug.Log("Stopping client");
        Destroy(LobbyManager.singleton.gameObject);
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Client disconnect");
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Server disconnect");
        StartCoroutine(ShutDownNetwork());
    }

    public IEnumerator ShutDownNetwork()
    {

        if (!NetworkServer.active)
        {
            Debug.Log("StopClient");
            LobbyManager.singleton.StopClient();
        }
        else
        {
            Debug.Log("Waiting connections");
            while (CheckConnections() > 1)
                yield return null;

            Debug.Log("StopHost");
            LobbyManager.singleton.StopHost();
            LobbyManager.singleton.StopServer();
            NetworkServer.DisconnectAll();
        }
        
        
       /// StartCoroutine(ExitDelay());
    }

    /*IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(0.1f);//attends un peu
        Destroy(LobbyManager.singleton.gameObject);

        //yield return new WaitForSeconds(0.1f);//attends un peu

        //SceneManager.LoadScene("LobbyScene"); // SCENE SUIVANTE

    }*/

    private int CheckConnections()
    {
        int connectionsCount = 0;
        foreach (NetworkConnection conn in NetworkServer.connections)
        {
            Debug.Log("Testing connections:" + conn);
            if (conn != null)
            {
                connectionsCount += 1;
            }
        }
        Debug.Log("Active Connections:" + connectionsCount);
        return connectionsCount;
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
            case "dark blue":
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
            case "grey":
                yoshiPrefab = yoshiGrayPrefab;
                playerPrefab = yoshiGrayPrefab;
                break;
            case "purple":
                yoshiPrefab = yoshiPurplePrefab;
                playerPrefab = yoshiPurplePrefab;
                break;
            case "baby blue":
                yoshiPrefab = yoshiBabyBluePrefab;
                playerPrefab = yoshiBabyBluePrefab;
                break;
            default:
                yoshiPrefab = yoshiGreenPrefab;
                break;
        }
        GameObject player = Instantiate(yoshiPrefab, nest.transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
       
        player.GetComponent<PlayerController2>().NestPosition = nest.transform.position;
        return player;
    }


    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        
        base.OnClientSceneChanged(conn);
        Canvas.SetActive(false);
    }




}
