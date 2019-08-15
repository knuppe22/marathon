using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GoldManager : SingletonBehaviour<GoldManager>
{
    [SerializeField]
    public Text Gold;
    public int gold;
    

    public bool UseMoney(int price)
    {
        if (gold >= price)
        {
            gold -= price;
            Gold.text = gold.ToString();
            return true;
        }

        else
            return false;
    }
}
