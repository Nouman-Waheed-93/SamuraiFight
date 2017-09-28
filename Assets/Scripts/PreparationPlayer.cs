using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PreparationPlayer : NetworkLobbyPlayer {

    private Button ReadyBtn;
    
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        MainNetHandler.instance.ChangeNumberOfPlayers(1);
        MainNetHandler.instance.AddPlayerInPreparationScreen(transform);
     
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Button RdyBtn = GetComponentInChildren<Button>();
        RdyBtn.interactable = true;
        RdyBtn.GetComponentInChildren<Text>().text = "Ready";
        GetComponent<Image>().color = Color.blue;
    }

    public void OnClickReadyBtn() {
        SendReadyToBeginMessage();
    }

    public override void OnClientReady(bool readyState)
    {
        base.OnClientReady(readyState);
        if (readyState)
        {
           GetComponentInChildren<Button>().interactable = false;
        }
        else
        {

        }
    }

    void OnDestroy() {
        MainNetHandler.instance.ChangeNumberOfPlayers(-1);
    }

}
