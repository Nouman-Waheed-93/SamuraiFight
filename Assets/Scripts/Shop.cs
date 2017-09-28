using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public Button HatTab, SwordTab, ArmourTab, CharacterTab;
    public GameObject HatPanel, SwordPanel, ArmourPanel, CharacterPanel;
    public ShopItem[] Hats, Swords, Armours, Characters;
    public GameObject SelectHatBtn, BuyHatBtn, SelectSwordBtn, BuySwordBtn,
        SelectArmourBtn, BuyArmourBtn, SelectCharacterBtn, BuyCharacterBtn;
    public Text HatPriceTxt, SwordPriceTxt, ArmorPriceTxt, CharPriceTxt;

    [System.Serializable]
    public class ShopItem {
        public GameObject Item;
        public int price;
    }

    Button currTab;
    GameObject currPanel, currItem;

	// Use this for initialization
	void Start () {
        SelectHatPanel();
        PlayerPrefs.SetInt(GlobalVals.hatString + 0, 1);
        PlayerPrefs.SetInt(GlobalVals.swordString + 0, 1);
        PlayerPrefs.SetInt(GlobalVals.armorString + 0, 1);
        PlayerPrefs.SetInt(GlobalVals.characterString + 0, 1);
    }

    public void SelectHatPanel() {

        if (currTab)
            currTab.interactable = true;
        if (currPanel)
            currPanel.SetActive(false);
        if (currItem)
            currItem.SetActive(false);
        HatTab.interactable = false;
        HatPanel.SetActive(true);
        currTab = HatTab;
        currPanel = HatPanel;
        currHatInd = PlayerPrefs.GetInt(GlobalVals.hatString, 0);
        Hats[currHatInd].Item.SetActive(true);
        currItem = Hats[currHatInd].Item;
    }

    public void SelectHat() {
        PlayerPrefs.SetInt(GlobalVals.hatString, currHatInd);
        SelectHatBtn.SetActive(false);
    }

    public void BuyHat() {
        if (PlayerPrefs.GetInt(GlobalVals.coinString, 100) > Hats[currHatInd].price)
        {
            PlayerPrefs.SetInt(GlobalVals.hatString + currHatInd, 1);
            bool locked = PlayerPrefs.GetInt(GlobalVals.hatString + currHatInd, 0) == 0;
            BuyHatBtn.SetActive(locked);
            SelectHatBtn.SetActive(!locked);
            PayPrice(Hats[currHatInd].price);
        }
        else {
            MainNetHandler.instance.NotEnoughCoinsError.SetActive(true);
        }
    }

    public void SelectSwordPanel() {
        if (currTab)
            currTab.interactable = true;
        if (currPanel)
            currPanel.SetActive(false);
        if (currItem)
            currItem.SetActive(false);
        SwordTab.interactable = false;
        SwordPanel.SetActive(true);
        currTab = SwordTab;
        currPanel = SwordPanel;
        currSwordInd = PlayerPrefs.GetInt(GlobalVals.swordString, 0);
        Swords[currSwordInd].Item.SetActive(true);
        currItem = Swords[currSwordInd].Item;
    }


    public void SelectSword()
    {
        PlayerPrefs.SetInt(GlobalVals.swordString, currSwordInd);
        SelectSwordBtn.SetActive(false);
    }

    public void BuySword()
    {
        if (PlayerPrefs.GetInt(GlobalVals.coinString, 100) > Swords[currSwordInd].price)
        {
            PlayerPrefs.SetInt(GlobalVals.swordString + currSwordInd, 1);
            bool locked = PlayerPrefs.GetInt(GlobalVals.swordString + currSwordInd, 0) == 0;
            BuySwordBtn.SetActive(locked);
            SelectSwordBtn.SetActive(!locked);
            PayPrice(Swords[currSwordInd].price);
        }
        else
        {
            MainNetHandler.instance.NotEnoughCoinsError.SetActive(true);
        }
    }

    public void SelectArmourPanel() {
        if (currTab)
            currTab.interactable = true;
        if (currPanel)
            currPanel.SetActive(false);
        if (currItem)
            currItem.SetActive(false);
        ArmourTab.interactable = false;
        ArmourPanel.SetActive(true);
        currTab = ArmourTab;
        currPanel = ArmourPanel;
        currArmourInd = PlayerPrefs.GetInt(GlobalVals.armorString, 0);
        Armours[currArmourInd].Item.SetActive(true);
        currItem = Armours[currArmourInd].Item;
    }


    public void SelectArmour()
    {
        PlayerPrefs.SetInt(GlobalVals.armorString, currArmourInd);
        SelectArmourBtn.SetActive(false);
    }

    public void BuyArmor()
    {
        if (PlayerPrefs.GetInt(GlobalVals.coinString, 100) > Armours[currArmourInd].price)
        {
            PlayerPrefs.SetInt(GlobalVals.armorString + currArmourInd, 1);
            bool locked = PlayerPrefs.GetInt(GlobalVals.armorString + currArmourInd, 0) == 0;
            BuyArmourBtn.SetActive(locked);
            SelectArmourBtn.SetActive(!locked);
            PayPrice(Armours[currArmourInd].price);
        }
        else
        {
            MainNetHandler.instance.NotEnoughCoinsError.SetActive(true);
        }
    }

    void PayPrice(int price) {
        PlayerPrefs.SetInt(GlobalVals.coinString, PlayerPrefs.GetInt(GlobalVals.coinString, 100) - price);
        MainNetHandler.instance.CoinsTxt.text = "Coins : " + PlayerPrefs.GetInt(GlobalVals.coinString).ToString();
    }

    public void SelectCharacterPanel() {
        if (currTab)
            currTab.interactable = true;
        if (currPanel)
            currPanel.SetActive(false);
        if (currItem)
            currItem.SetActive(false);
        CharacterTab.interactable = false;
        CharacterPanel.SetActive(true);
        currTab = CharacterTab;
        currPanel = CharacterPanel;
        currCharacterInd = PlayerPrefs.GetInt(GlobalVals.characterString, 0);
        Characters[currCharacterInd].Item.SetActive(true);
        currItem = Characters[currCharacterInd].Item;
    }
    
    public void SelectCharacter()
    {
        PlayerPrefs.SetInt(GlobalVals.characterString, currCharacterInd);
        SelectCharacterBtn.SetActive(false);
    }

    public void BuyCharacter()
    {
        if (PlayerPrefs.GetInt(GlobalVals.coinString, 100) > Characters[currCharacterInd].price)
        {
            PlayerPrefs.SetInt(GlobalVals.characterString + currCharacterInd, 1);
            bool locked = PlayerPrefs.GetInt(GlobalVals.characterString + currCharacterInd, 0) == 0;
            BuyCharacterBtn.SetActive(locked);
            SelectCharacterBtn.SetActive(!locked);
            PayPrice(Characters[currCharacterInd].price);
        }
        else
        {
            MainNetHandler.instance.NotEnoughCoinsError.SetActive(true);
        }
    }


    int currHatInd;
    public void CycleHat(int i) {
        Hats[currHatInd].Item.SetActive(false);
        currHatInd = Mathf.Clamp(currHatInd + i, 0, Hats.Length - 1);
        Hats[currHatInd].Item.SetActive(true);
        currItem = Hats[currHatInd].Item;
        bool locked = PlayerPrefs.GetInt(GlobalVals.hatString + currHatInd, 0) == 0;
        BuyHatBtn.SetActive(locked);
        HatPriceTxt.gameObject.SetActive(locked);
        HatPriceTxt.text = "Price : " + Hats[currHatInd].price;
        SelectHatBtn.SetActive(PlayerPrefs.GetInt(GlobalVals.hatString, 0) != currHatInd && !locked);
    }

    int currSwordInd;
    public void CycleSword(int i)
    {
        Swords[currSwordInd].Item.SetActive(false);
        currSwordInd = Mathf.Clamp(currSwordInd + i, 0, Swords.Length - 1);
        Swords[currSwordInd].Item.SetActive(true);
        currItem = Swords[currSwordInd].Item;
        bool locked = PlayerPrefs.GetInt(GlobalVals.swordString + currSwordInd, 0) == 0;
        BuySwordBtn.SetActive(locked);
        SwordPriceTxt.gameObject.SetActive(locked);
        SwordPriceTxt.text = "Price : " + Swords[currSwordInd].price;
        SelectSwordBtn.SetActive(PlayerPrefs.GetInt(GlobalVals.swordString, 0) != currSwordInd && !locked);

    }

    int currArmourInd;
    public void CycleArmour(int i)
    {
        Armours[currArmourInd].Item.SetActive(false);
        currArmourInd = Mathf.Clamp(currArmourInd + i, 0, Armours.Length - 1);
        Armours[currArmourInd].Item.SetActive(true);
        currItem = Armours[currArmourInd].Item;
        bool locked = PlayerPrefs.GetInt(GlobalVals.armorString + currArmourInd, 0) == 0;
        BuyArmourBtn.SetActive(locked);
        ArmorPriceTxt.gameObject.SetActive(locked);
        ArmorPriceTxt.text = "Price : " + Armours[currArmourInd].price;
        SelectArmourBtn.SetActive(PlayerPrefs.GetInt(GlobalVals.armorString, 0) != currArmourInd && !locked);

    }

    int currCharacterInd;
    public void CycleCharacter(int i)
    {
        Characters[currCharacterInd].Item.SetActive(false);
        currCharacterInd = Mathf.Clamp(currCharacterInd + i, 0, Characters.Length - 1);
        Characters[currCharacterInd].Item.SetActive(true);
        currItem = Characters[currCharacterInd].Item;
        bool locked = PlayerPrefs.GetInt(GlobalVals.characterString + currCharacterInd, 0) == 0;
        BuyCharacterBtn.SetActive(locked);
        CharPriceTxt.gameObject.SetActive(locked);
        CharPriceTxt.text = "Price : " + Characters[currCharacterInd].price;
        SelectCharacterBtn.SetActive(PlayerPrefs.GetInt(GlobalVals.characterString, 0) != currCharacterInd && !locked);

    }

    void OnDisable() {
        currItem.SetActive(false);
    }

    void OnEnable() {
        if(currItem)
            currItem.SetActive(true);
    }

}
