using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldButton : MonoBehaviour
{
    [SerializeField]
    private Text Gold;

    public void Click()
    {
        GoldManager.Instance.gold += 100;
        Gold.text = GoldManager.Instance.gold.ToString();
    }

    public void Hundred()
    {
        int P = 100;
        
        if (GoldManager.Instance.UseMoney(P) == true)
        {
            GoldManager.Instance.gold -= P;
            Gold.text = GoldManager.Instance.gold.ToString();
        }

        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Twohundred()
    {
        int P = 200;

        if (GoldManager.Instance.UseMoney(P) == true)
        {
            GoldManager.Instance.gold -= P;
            Gold.text = GoldManager.Instance.gold.ToString();
        }

        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void ButtonA()
    {
        ItemManager.Instance.BuyItem("Stone");
    }

    public void ButtonB()
    {
        ItemManager.Instance.EquipmentItem("Stone");
    }
}
