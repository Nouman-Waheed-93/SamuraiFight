using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemsParent : MonoBehaviour {

    public static ShopItemsParent instance;

    void Start() {
        if (instance)
        {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
