using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerStage : MonoBehaviour {
    
    public string[] StagesNames;
   
    int stageInd;
	// Use this for initialization
	void Start () {
        stageInd = Random.Range(0, StagesNames.Length);
        SceneManager.LoadScene(StagesNames[stageInd], LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnStageLoaded;        
	}

    public void OnStageLoaded(Scene scene, LoadSceneMode loadmode) {

        if (scene.name == StagesNames[stageInd] && loadmode == LoadSceneMode.Additive) {
            FindObjectOfType<SinglePlayerManager>().SetPlayerPositions();
            MainNetHandler.instance.ChangeScreen(MainNetHandler.instance.SPInGameUI);
        }

    }

}
