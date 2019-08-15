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
        GoldManager.Instance.EarnMoney(100);
    }

    public void Hundred()
    {
        GoldManager.Instance.UseMoney(100);
    }

    public void Twohundred()
    {
        GoldManager.Instance.UseMoney(200);
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
