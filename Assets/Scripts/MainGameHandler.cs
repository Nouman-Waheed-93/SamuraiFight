using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainGameHandler : NetworkBehaviour {

    struct result {
        public NetworkSamurai plyr;
        public float TimeTouch;
        public int charInd, armourInd, swordInd, hatInd;
    }

    result[] playerResult = new result[2];

    int playersInterestedInRematch;
    private int ResultsGotten;

    public static MainGameHandler singleton;

    private int PlayersConnected;
	// Use this for initialization
	void Awake () {
        singleton = this;
	}
	
    public void ClearResults() {
        ResultsGotten = 0;
    }

    public void ReportConnected(NetworkSamurai plyr, int charInd, int armourInd, int swordInd, int hatInd) {
        playerResult[PlayersConnected].charInd = charInd;
        playerResult[PlayersConnected].armourInd = armourInd;
        playerResult[PlayersConnected].swordInd = swordInd;
        playerResult[PlayersConnected].hatInd = hatInd;
        playerResult[PlayersConnected].plyr = plyr;
        PlayersConnected++;
        if (PlayersConnected > 1) {
           for (int i = 0; i < 2; i++) {
                playerResult[i].plyr.RpcStartGame(playerResult[i].charInd, playerResult[i].armourInd, playerResult[i].swordInd, playerResult[i].hatInd);
            }
        }
    }

    public void ComputeResults(NetworkSamurai plyr, float TimeTouch) {
        if (!isServer)
            return;

        if (ResultsGotten > 1)
            return;

        print("Recieved Result " + ResultsGotten + " time is " + TimeTouch);

        if (!playerResult[0].plyr)
            playerResult[0].plyr = plyr;
        else if (!playerResult[1].plyr)
            playerResult[1].plyr = plyr;

        if (playerResult[0].plyr == plyr)
            playerResult[0].TimeTouch = TimeTouch;
        else
            playerResult[1].TimeTouch = TimeTouch;

        ResultsGotten++;
        if (ResultsGotten > 1)
        {
            if (playerResult[0].TimeTouch < playerResult[1].TimeTouch)
            {
                playerResult[0].plyr.RpcResult(true);
                playerResult[1].plyr.RpcResult(false);
                print("player 1 is the winner");
            }
            else
            {
                playerResult[1].plyr.RpcResult(true);
                playerResult[0].plyr.RpcResult(false);
                print("Player 2 is the winner");
            }
        }
        else
        {
            //Check if server got both results after 5 seconds
            StartCoroutine("CheckResultsGotten");
        }

    }

    IEnumerator CheckResultsGotten() {
        yield return new WaitForSeconds(5);
        if (ResultsGotten < 2)
        {
            playerResult[0].plyr.RpcResult(true);
            NetworkSamurai[] allSamurais = FindObjectsOfType<NetworkSamurai>();
            for (int i = 0; i < allSamurais.Length; i++)
            {
                if (allSamurais[i] != playerResult[0].plyr)
                    allSamurais[i].RpcResult(false);
          
            }
        }        
    }

    public void AddRematchPlayer() {
        playersInterestedInRematch++;
        print("ADdd Rematch Player" + playersInterestedInRematch);
        if (playersInterestedInRematch == 2) {
            ResultsGotten = 0;
            playersInterestedInRematch = 0;
            NetworkSamurai[] allSamurais = FindObjectsOfType<NetworkSamurai>();
            for (int i = 0; i < 2; i++) {
                playerResult = new result[2];
                allSamurais[i].RpcRestartBattle();
            }
        }
    }

}
