using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerSamurai : MonoBehaviour {

    public bool IsPlayer;

    public GameObject[] Armours;
    public GameObject[] Swords;
    public GameObject[] Hats;
    public GameObject[] Characters;

    Animator myAnim;

    // Use this for initialization
    void Start () {
        SetAccessories();
        myAnim = GetComponentInChildren<Animator>();
	}
	
    void SetAccessories() {
        
        Characters[IsPlayer? PlayerPrefs.GetInt(GlobalVals.characterString, 0) : 0].SetActive(true);

        int armourInd = (IsPlayer? PlayerPrefs.GetInt(GlobalVals.armorString, 0) : 0);
        Armours[armourInd].transform.SetParent(GetComponentInChildren<ArmourParent>().transform, false);
        Armours[armourInd].transform.localPosition = Vector3.zero;
        Armours[armourInd].transform.localRotation = Quaternion.identity;
        Armours[armourInd].SetActive(true);

        int swordInd = IsPlayer? PlayerPrefs.GetInt(GlobalVals.swordString, 0) : 0;
        Swords[swordInd].transform.SetParent(GetComponentInChildren<SwordParent>().transform,false);
        Swords[swordInd].transform.localPosition = Vector3.zero;
        Swords[swordInd].transform.localRotation = Quaternion.identity;
        Swords[swordInd].SetActive(true);

        int hatInd = IsPlayer ? PlayerPrefs.GetInt(GlobalVals.hatString, 0) : 0;
        Hats[hatInd].transform.SetParent(GetComponentInChildren<HatParent>().transform,false);
        Hats[hatInd].transform.localPosition = Vector3.zero;
        Hats[hatInd].transform.localRotation = Quaternion.identity;
        Hats[hatInd].SetActive(true);
    }

    public void Attack() {
        myAnim.SetTrigger("Attack");
    }

    public void Die() {
        myAnim.SetTrigger("Die");
    }

}
