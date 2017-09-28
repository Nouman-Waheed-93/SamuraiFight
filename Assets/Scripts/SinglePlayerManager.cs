using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerManager : MonoBehaviour {

    public SinglePlayerSamurai playerSamurai, NPSamurai;
    public Text CountdownText;

    float GameTime;
    bool MatchStarted;
    bool MatchDecided;
    Animator Fader;

    void Start() {
        Fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Animator>();
        Fader.SetTrigger("FadeOut");
        StartCoroutine("CountDown");
    }

	// Update is called once per frame
	void Update () {
        if (MatchDecided)
            return;
        RecordScreenTouchTime();
	}

    public void SetPlayerPositions() {
        playerSamurai.transform.SetPositionAndRotation(
            GameObject.FindGameObjectWithTag("PlayerPosition1").transform.position, GameObject.FindGameObjectWithTag("PlayerPosition1").transform.rotation);
        NPSamurai.transform.SetPositionAndRotation(GameObject.FindGameObjectWithTag("PlayerPosition2").transform.position
            , GameObject.FindGameObjectWithTag("PlayerPosition2").transform.rotation);
    }

    void RecordScreenTouchTime()
    {

        if (MatchStarted)
        {
            GameTime += Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                float NPAttackTime = PlayerPrefs.GetFloat("Difficulty", 2);
                
                if (NPAttackTime > GameTime)
                {
                    playerSamurai.Attack();
                    NPSamurai.Die();
                    StartCoroutine("PauseGame", true);
                  
                }
                else {
                    playerSamurai.Die();
                    NPSamurai.Attack();
                    StartCoroutine("PauseGame", false);
                }
                MatchDecided = true;        
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                NPSamurai.Attack();
                playerSamurai.Die();
                StartCoroutine("PauseGame", false);
                MatchDecided = true;
            }
        }
    }

    IEnumerator CountDown()
    {

        for (int i = 1; i < 4; i++)
        {

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


    IEnumerator PauseGame(bool winner)
    {
        if (winner)
        {
            MainNetHandler.instance.AddPlayerCoins();
            PlayerPrefs.SetFloat("Difficulty", Mathf.Clamp(PlayerPrefs.GetFloat("Difficulty", 2) - 0.1f, 0.1f, 2));
        }
        if (!MatchStarted)
        {
            StopCoroutine("CountDown");
            CountdownText.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1.5f);
        MainNetHandler.instance.SingleVPauseGame("You " + (winner ? "Win!" : "Lose!"));
    }

}
