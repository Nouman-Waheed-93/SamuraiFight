using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class MainNetHandler : NetworkLobbyManager {

    public static MainNetHandler instance;

    public GameObject LoadingScreen, MainScreen, BattlePreparationScreen,
        InGameScreen, NotEnoughCoinsError, BetScreen, PauseScreen,ShopScreen, RematchBtn, SPPauseScreen, SPInGameUI, SPPreparationScreen;
    public Text StatusText, CoinsTxt, ResultTxt, SPRestultTxt;

    public int SinglePlayerBet;
     
    private GameObject CurrentScreen;
    [HideInInspector]
    public NetworkSamurai localPlayerSamurai;
   

    private ulong currentMatchId;

    private int totalNumberPlayers;
    bool _isMatchmaking, _disconnectServer;

    public delegate void EndGame();
    public EndGame endGameDelegate;

    [HideInInspector]
    public int BetCoins;

    void Start() {
        instance = this;
        endGameDelegate = EmptyCallBack;
        CoinsTxt.text = "Coins : " + PlayerPrefs.GetInt(GlobalVals.coinString, 100);
    }
    

    public void Bet(int coins) {
        BetCoins = coins;
        if (PlayerPrefs.GetInt("Coins", 100) >= coins)
        {
            StartFindingBattle();
        }
        else {
            NotEnoughCoinsError.SetActive(true);
        }
    }

    public void HideCoinsError() {
        NotEnoughCoinsError.SetActive(false);
    }

    public void StartFindingBattle() {
        StartMatchMaker();
        _isMatchmaking = true;
        matchMaker.ListMatches(0, 2, BetCoins.ToString(), true, 0, 0, OnMatchList);
        ChangeScreen(LoadingScreen);
        StatusText.text = "Finding Battle!";
    }

    public void ToBettingScreen() {
        ChangeScreen(BetScreen);
    }

    public void ToMainScreen() {
        ChangeScreen(MainScreen);
    }

    public void ToShopScreen() {
        ChangeScreen(ShopScreen);
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);
        print(matchList.Count + " matches");
        if (success)
        {
            if (matchList.Count > 0) {
                print("More than one match");
                JoinMatch(matchList[0]);
                StatusText.text = "Preparing...";
            }
            else {
                CreateMatch();
                StatusText.text = "Creating Battle...";
            }
        }
        else {
            ChangeScreen(MainScreen);
        }

    }

    public void ChangeScreen(GameObject newScreen) {
        if (!CurrentScreen)
            CurrentScreen = MainScreen;
        CurrentScreen.SetActive(false);
        if(newScreen)
            newScreen.SetActive(true);
        CurrentScreen = newScreen;
    }
    
    public void JoinMatch(MatchInfoSnapshot matchToJoin) {
      
        matchMaker.JoinMatch(matchToJoin.networkId, "", "", "", 0, 0, OnMatchJoined);
        _isMatchmaking = true;
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        if (!success)
            ChangeScreen(MainScreen);

    }

    public void CreateMatch() {

        matchMaker.CreateMatch("Fight" + BetCoins, 2, true, "", "", "", 0, 0, OnMatchCreate);
        _isMatchmaking = true;
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        if (success) {
            print("Created Match");
            StatusText.text = "Waiting for Opponent";
            currentMatchId = (System.UInt64)matchInfo.networkId;
        }
        else {
            ChangeScreen(MainScreen);
        }
    }

    public override void OnDestroyMatch(bool success, string extendedInfo)
    {
        base.OnDestroyMatch(success, extendedInfo);
        if (_disconnectServer)
        {
            print("Stopped");
            StopMatchMaker();
            StopHost();
        }
       
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        endGameDelegate = StopHostClbk;

    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;
        //      ChangeScreen(BattlePreparationScreen);
        
        return obj;
    }

    public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
    {
        base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        base.OnLobbyServerDisconnect(conn);
    }

    public void AddPlayerInPreparationScreen(Transform playerT) {
        playerT.SetParent(BattlePreparationScreen.transform, false);
    }

    public override void OnDropConnection(bool success, string extendedInfo)
    {
        base.OnDropConnection(success, extendedInfo);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    public override void OnClientNotReady(NetworkConnection conn)
    {
        base.OnClientNotReady(conn);
    }

    public override void OnLobbyClientAddPlayerFailed()
    {
        base.OnLobbyClientAddPlayerFailed();
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        base.OnLobbyClientConnect(conn);
    }

    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        base.OnLobbyClientDisconnect(conn);
        print("Lobby client disconect");
        _isMatchmaking = false;
    }

    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();
    }

    public override void OnLobbyClientExit()
    {
        base.OnLobbyClientExit();

    }

    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        base.OnLobbyServerConnect(conn);
    }

    public override void OnLobbyServerSceneChanged(string sceneName)
    {
        base.OnLobbyServerSceneChanged(sceneName);
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }

    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        base.OnLobbyStartClient(lobbyClient);
    }

    public override void OnLobbyStartHost()
    {
        base.OnLobbyStartHost();
    }

    public override void OnLobbyStartServer()
    {
        base.OnLobbyStartServer();
    }

    public override void OnLobbyStopClient()
    {
        base.OnLobbyStopClient();
        print("Stopping lobby client");
        _isMatchmaking = false;
    }

    public override void OnLobbyStopHost()
    {
        base.OnLobbyStopHost();
        print("Stoping lobby host");
        _isMatchmaking = false;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).buildIndex == 1)
        {          
                StopHostClbk();
        }
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnSetMatchAttributes(bool success, string extendedInfo)
    {
        base.OnSetMatchAttributes(success, extendedInfo);
    }

    public override void OnStartClient(NetworkClient lobbyClient)
    {
        base.OnStartClient(lobbyClient);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        print("stop client");
        _isMatchmaking = false;
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public override void ServerChangeScene(string sceneName)
    {
        base.ServerChangeScene(sceneName);
    }

    public override NetworkClient StartHost()
    {
        return base.StartHost();
    }

    public override NetworkClient StartHost(ConnectionConfig config, int maxConnections)
    {
        return base.StartHost(config, maxConnections);
    }

    public override NetworkClient StartHost(MatchInfo info)
    {
        return base.StartHost(info);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if (!NetworkServer.active)
        {//only to do on pure client (not self hosting client)
         //   ChangeScreen(MainScreen);
            endGameDelegate = StopClientClbk;
        }
    }

    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        print("CLient disconnect");
        _isMatchmaking = false;
        ChangeScreen(MainScreen);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        _isMatchmaking = false;
        ChangeScreen(MainScreen);
    }

    public void ChangeNumberOfPlayers(int delta)
    {
        totalNumberPlayers += delta;
        print("We have " + totalNumberPlayers + " players");
        if (totalNumberPlayers == 2)
        {
            RematchBtn.SetActive(true);
            ChangeScreen(BattlePreparationScreen);
        }
        else {
            RematchBtn.SetActive(false);
            if (_isMatchmaking)
                ChangeScreen(LoadingScreen);
            else
                ChangeScreen(MainScreen);
        }
    }

    public override void OnLobbyServerPlayersReady()
    {
        bool allready = true;
        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            if (lobbySlots[i] != null)
                allready &= lobbySlots[i].readyToBegin;
        }

        if (allready)
        {
            //add coins here
            ServerChangeScene(playScene);
        }
    }

    public void AddPlayerCoins() {
        PlayerPrefs.SetInt(GlobalVals.coinString, PlayerPrefs.GetInt(GlobalVals.coinString, 100) + 2 * BetCoins);
        CoinsTxt.text = "Coins : " + PlayerPrefs.GetInt(GlobalVals.coinString, 100);
    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        base.OnLobbyClientSceneChanged(conn);
        if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).buildIndex == 1)
        {
            ChangeScreen(InGameScreen);
            PlayerPrefs.SetInt(GlobalVals.coinString, PlayerPrefs.GetInt(GlobalVals.coinString, 100) - BetCoins);
            CoinsTxt.text = "Coins : " + PlayerPrefs.GetInt(GlobalVals.coinString, 100);
        }
    }

    public void StopHostClbk()
    {
        if (_isMatchmaking)
        {
            print("Destroying Match ");
            matchMaker.DestroyMatch((NetworkID)currentMatchId, 0, OnDestroyMatch);
            _disconnectServer = true;
        }
        else
        {
            print("Stopping Host");
            StopHost();
        }

        _isMatchmaking = false;
       // ChangeScreen(MainScreen);
    }

    public void StopClientClbk()
    {
        StopClient();

        if (_isMatchmaking)
        {
            StopMatchMaker();
        }

        _isMatchmaking = false;
        ChangeScreen(MainScreen);
    }

    public void EmptyCallBack() {
        
    }

    public void EndMatch() {
   
        endGameDelegate();
        
    }

    public void PauseGame(string message) {
        ChangeScreen(PauseScreen);
        if (PlayerPrefs.GetInt(GlobalVals.coinString, 100) > BetCoins)
            RematchBtn.SetActive(false);
        ResultTxt.text = message;
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void StartFightingScene() {
        ServerChangeScene(playScene);
    }

    public void RestartMatch() {
        if (localPlayerSamurai && totalNumberPlayers == 2) {
            print("RestartMatch Clicked");
            if(PlayerPrefs.GetInt(GlobalVals.coinString, 100) >= BetCoins) {
                localPlayerSamurai.WannaFightAgain();
            }
            RematchBtn.SetActive(false);
        }
    }

    public void SingleVPauseGame(string message) {
        ChangeScreen(SPPauseScreen);
        SPRestultTxt.text = message;
    }

    public void StartSingleVersionMode() {
        if (PlayerPrefs.GetInt(GlobalVals.coinString, 100) > SinglePlayerBet)
        {
            BetCoins = SinglePlayerBet;
            ChangeScreen(LoadingScreen);
            PlayerPrefs.SetInt(GlobalVals.coinString, PlayerPrefs.GetInt(GlobalVals.coinString, 100) - BetCoins);
            CoinsTxt.text = "Coins : " + PlayerPrefs.GetInt(GlobalVals.coinString, 100);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
        }
        else {
            NotEnoughCoinsError.SetActive(true);
        }
    }

    public void QuitSingleVersionMode() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        ChangeScreen(MainScreen);
    }

    public void ToSinglePlayerPreparation() {
        ChangeScreen(SPPreparationScreen);
    }
    
}
