using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GoldManager : SingletonBehaviour<GoldManager>
{
    [SerializeField]
    public Text Gold;
    private int gold;
    
    public int GetGold()
    {
        return gold;
    }

    public void SetGold(int _gold)
    {
        gold = _gold;
    }

    public bool UseMoney(int price)
    {
        if (gold >= price)
        {
            gold -= price;
            Gold.text = gold.ToString();
            RunManager.Instance.users[AuthManager.Instance.CurrentUserId].gold = gold;
            DBManager.Instance.SetUserValue("gold", gold);
            return true;
        }

        else
            return false;
    }

    public void EarnMoney(int money)
    {
        gold += money;
        Gold.text = gold.ToString();
        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].gold = gold;
        DBManager.Instance.SetUserValue("gold", gold);
    }
}
