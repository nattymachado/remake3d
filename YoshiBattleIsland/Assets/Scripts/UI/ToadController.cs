using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToadController : MonoBehaviour {


    public Text toadTalk;
    public GameObject ToadAndBaloon;
    private LobbyManager _manager;
    private bool _findMario = false;
    public int MarioPosition;
    

    // Use this for initialization
    void Start () {
        _manager = LobbyManager.singleton.GetComponent<LobbyManager>();
        InitGame();
    }

    public void InitGame()
    {
        toadTalk.text = "Hi  "+_manager.UserName+"  ...  Baby  Mario  is  crying, find  him!";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
    }

    public void EndGameWithAWinner(string winner)
    {
        toadTalk.text = "And the winner is:   " + winner + "   ! Mario is saved!!!";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
    }

    public void EndGameWithAWinnerButNotYou(string winner)
    {
        toadTalk.text = "And  the  winner  is:  " + winner + " ! Mario  is  safe !!!";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
    }

    public void EndGameTimeout()
    {
        toadTalk.text = "Ohhh !  The  time  is  out.  Nobody  saved  Baby  Mario !";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
    }

    public void FindMario()
    {
        toadTalk.text = "You  found  Baby  Mario ... Return  to  your  nest  to  win  the  game!  Run,  run ... ";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
        _findMario = true;
    }

    public void SomeoneFoundMario()
    {
        toadTalk.text = "Someone  found  Baby  Mario, try  to  steal  him ...  Run, run ... ";
        ToadAndBaloon.SetActive(true);
        StartCoroutine(HideToad());
        _findMario = true;
    }

    public void MarioPositionIs(int mPosition)
    {
        if (!_findMario)
        {
            string position = "";
            switch (mPosition)
            {
                case 0:
                    position = "bushes.";
                    break;
                case 1:
                    position = "pines.";
                    break;
                case 2:
                    position = "flowers.";
                    break;
                case 3:
                    position = "trees.";
                    break;
            }


            toadTalk.text = "Ohh! Time is passing, run to save Mario. He is in the " + position;
            ToadAndBaloon.SetActive(true);
            StartCoroutine(HideToad());

        }
       
    }

    IEnumerator HideToad()
    {
        yield return new WaitForSeconds(5f);
        ToadAndBaloon.SetActive(false);
    }
}
