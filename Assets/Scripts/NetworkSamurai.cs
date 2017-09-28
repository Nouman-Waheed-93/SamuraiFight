using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class NetworkSamurai : NetworkBehaviour {
    
    // Use this for initialization
    float GameTime;
    bool MatchDecided;
    bool MatchStarted;
    bool InputForwarded;
    Text CountdownText;
    Animator Fader;
    public GameObject[] Armours;
    public GameObject[] Swords;
    public GameObject[] Hats;
    public GameObject[] Characters;
    
    private int currCharInd, currSwordInd, currArmorInd, currHatInd;

    public override void OnStartLocalPlayer ()
    {
        MatchDecided = false;
        CountdownText = GameObject.FindGameObjectWithTag("CountDown").GetComponent<Text>();
        Fader = GameObject.FindGameObjectWithTag("Fader").GetComponentInChildren<Animator>();
        MainNetHandler.instance.localPlayerSamurai = this;
        CmdPlayerReady(PlayerPrefs.GetInt(GlobalVals.characterString, 0),
        PlayerPrefs.GetInt(GlobalVals.armorString, 0),
        PlayerPrefs.GetInt(GlobalVals.swordString, 0),
        PlayerPrefs.GetInt(GlobalVals.hatString, 0));
    }
    
    [Command]
    public void CmdPlayerReady(int charInd, int armourInd, int swordInd, int hatind) {
        print("CmdPlayerReady");
        currCharInd = charInd;
        currArmorInd = armourInd;
        currSwordInd = swordInd;
        currHatInd = hatind;
        StartCoroutine("CheckMainGamesingleton");
           
    }

    IEnumerator CheckMainGamesingleton() {
        while (MainGameHandler.singleton == null) {
            yield return null;
        }
        MainGameHandler.singleton.ReportConnected(this, currCharInd, currArmorInd, currSwordInd, currHatInd);
    }

    [ClientRpc]
    public void RpcStartGame(int charInd, int armourInd, int swordInd, int hatInd) {
        if (isLocalPlayer)
        {
            Fader.SetTrigger("FadeOut");
            StartCoroutine("CountDown");
        }
        Characters[charInd].SetActive(true);

        Armours[armourInd].transform.SetParent(GetComponentInChildren<ArmourParent>().transform);
        Armours[armourInd].transform.localPosition = Vector3.zero;
        Armours[armourInd].transform.localRotation = Quaternion.identity;
        Armours[armourInd].SetActive(true);

        Swords[swordInd].transform.SetParent(GetComponentInChildren<SwordParent>().transform);
        Swords[swordInd].transform.localPosition = Vector3.zero;
        Swords[swordInd].transform.localRotation = Quaternion.identity;
        Swords[swordInd].SetActive(true);

        Hats[hatInd].transform.SetParent(GetComponentInChildren<HatParent>().transform);
        Hats[hatInd].transform.localPosition = Vector3.zero;
        Hats[hatInd].transform.localRotation = Quaternion.identity;
        Hats[hatInd].SetActive(true);
        print(GlobalVals.characterString + " " + charInd + " " + GlobalVals.armorString +
            " "+ armourInd + " " + GlobalVals.swordString + " " + swordInd + " " + GlobalVals.hatString + " " + hatInd);
    }
    
    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer || InputForwarded)
            return;
        
        RecordScreenTouchTime();
        
	}

    IEnumerator CountDown() {
        
        for (int i = 1; i < 4; i++) {

            yield return new WaitForSeconds(0.5f);

            CountdownText.gameObject.SetActive(true);
            CountdownText.text = (i).ToString();

            yield return new WaitForSeconds(0.5f);
            CountdownText.gameObject.SetActive(false);
            
        }

        yield return new WaitForSeconds(0.5f);

        CountdownText.gameObject.SetActive(true);
        CountdownText.text = "Start";
        MatchStarted = true;

        yield return new WaitForSeconds(0.5f);
        CountdownText.gameObject.SetActive(false);
      

    }

    void RecordScreenTouchTime() {

        if (MatchStarted)
        {
            GameTime += Time.deltaTime;
            if (Input.GetMouseButton(0))
            {
                Fader.SetTrigger("FadeIn");
                CmdTakeResult(GameTime);
                InputForwarded = true;
            }
        }
        else {
            if (Input.GetMouseButton(0)) {
                InputForwarded = true;
                CmdDisqualifyMe();
            }
        }
    }

    [Command]
    public void CmdDisqualifyMe() {
        RpcResult(false);
        NetworkSamurai[] allSamurais = FindObjectsOfType<NetworkSamurai>();
        for (int i = 0; i < allSamurais.Length; i++)
        {
            if (allSamurais[i] != this)
                allSamurais[i].RpcResult(true);
        }
    }

    [Command]
    public void CmdTakeResult(float Time) {
        MainGameHandler.singleton.ComputeResults(this, Time);
    }

    [ClientRpc]
    public void RpcResult(bool winner) {

        if (MatchDecided)
            return;
        Animator SamAnim = GetComponentInChildren<Animator>();
        if (winner)
        {
            SamAnim.SetTrigger("Attack");
            if (isLocalPlayer)
                MainNetHandler.instance.AddPlayerCoins();
        }
        else
        {
            SamAnim.SetTrigger("Die");
        }
        MatchDecided = true;
        print("Match Decided RPC " + MatchDecided);
        if (isLocalPlayer)
        {
            if (!MatchStarted)
            {
                StopCoroutine("CountDown");
                CountdownText.gameObject.SetActive(false);
            }
            Fader.SetTrigger("FadeOut");
            StartCoroutine("PauseGame", winner);
        }
    }

    IEnumerator PauseGame(bool winner) {
        yield return new WaitForSeconds(1.5f);
        MainNetHandler.instance.PauseGame("You " + (winner ? "Win!" : "Lose!"));
        MainNetHandler.instance.RematchBtn.SetActive(PlayerPrefs.GetInt(GlobalVals.coinString, 100) >= MainNetHandler.instance.BetCoins);
    }

    public void WannaFightAgain() {
        if (isLocalPlayer)
            CmdRprtPlyrRematch();

    }
    

    [Command]
    void CmdRprtPlyrRematch() {
        MainGameHandler.singleton.AddRematchPlayer();
        print("Cmd");
    }

    [ClientRpc]
    public void RpcRestartBattle() {
        GameTime = 0;
        MatchDecided = false;
        MatchStarted = false;
        InputForwarded = false;
        GetComponentInChildren<Animator>().Rebind();
        PlayerPrefs.SetInt(GlobalVals.coinString, PlayerPrefs.GetInt(GlobalVals.coinString, 100) - MainNetHandler.instance.BetCoins);
        if (!isLocalPlayer)
            return;
        print("Restarting battle");
        MainNetHandler.instance.ChangeScreen(MainNetHandler.instance.InGameScreen);
        StartCoroutine("CountDown");
    }

    [Command]
    public void CmdSetPosition(int positionInd) {

        RpcSetPosition(positionInd);

    }

    [ClientRpc]
    void RpcSetPosition(int positionInd) {

        transform.SetPositionAndRotation(
        GameObject.FindGameObjectWithTag(positionInd == 0 ? "PlayerPosition1" : "PlayerPosition2").transform.position, GameObject.FindGameObjectWithTag(positionInd == 0 ? "PlayerPosition1" : "PlayerPosition2").transform.rotation);


    }

}
