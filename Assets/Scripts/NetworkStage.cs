using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkStage : NetworkBehaviour {

    public static NetworkStage instance;
    public string[] Stages;
    public bool StageLoaded;

    [SyncVar]
    int StgInd;
    [SyncVar]
    int CurrPositionInd;

    void Start() {
        instance = this;
    }

    public override void OnStartServer() {
        print("Jo jo phalan wala");
        StgInd = Random.Range(0, Stages.Length);
   }

    public override void OnStartClient() {
        SceneManager.LoadScene(Stages[StgInd], LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnStageLoaded;
    }

    public void OnStageLoaded(Scene scene, LoadSceneMode loadmode)
    {

        if (scene.name == Stages[StgInd] && loadmode == LoadSceneMode.Additive)
        {
            StageLoaded = true;
            //NetworkSamurai[] allSamurais = FindObjectsOfType<NetworkSamurai>();
            //for (int i = 0; i < allSamurais.Length; i++)
            //{
            //    if (allSamurais[i] == MainNetHandler.instance.localPlayerSamurai)
            //    {
            //        allSamurais[i].transform.SetPositionAndRotation(
            //        GameObject.FindGameObjectWithTag(CurrPositionInd == 0 ? "PlayerPosition1" : "PlayerPosition2").transform.position, GameObject.FindGameObjectWithTag(CurrPositionInd == 0 ? "PlayerPosition1" : "PlayerPosition2").transform.rotation);
            //    }
            //    else
            //    {
            //        allSamurais[i].transform.SetPositionAndRotation(
            //        GameObject.FindGameObjectWithTag(CurrPositionInd != 0 ? "PlayerPosition1" : "PlayerPosition2").transform.position, GameObject.FindGameObjectWithTag(CurrPositionInd == 0 ? "PlayerPosition1" : "PlayerPosition2").transform.rotation);

            //    }
            //}
            MainNetHandler.instance.localPlayerSamurai.CmdSetPosition(CurrPositionInd);
            CurrPositionInd++;

            }
        }
    
    }


