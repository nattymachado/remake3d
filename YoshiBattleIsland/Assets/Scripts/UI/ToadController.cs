using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToadController : MonoBehaviour {


    public Text toadTalk;
    public GameObject ToadAndBaloon;
    private LobbyManager _manager;

    // Use this for initialization
    void Start () {
        _manager = LobbyManager.singleton.GetComponent<LobbyManager>();
        InitGame();
        

    }


    public void InitGame()
    {
        toadTalk.text = "Olá "+_manager.UserName+" .. corra e encontre o Mario. Preste atenção ele esta chorando ...";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
    }

    public void FindMario()
    {
        toadTalk.text = "Você achou o Mario .. Volte para o seu ninho para ganhar o jogo! Corra ... ";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
    }

    IEnumerator HideToad()
    {
        yield return new WaitForSeconds(5f);
        ToadAndBaloon.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
