using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemInfo
{
    public string ItemName;
    public Image ItemVisual;
    [TextArea]
    public string ItemEffectsDescription;
    public int ItemPrice;
    public int MaxQuantity;
    public int CurrentQuantity = 0;
}
public class UIControl : SingletonBehaviour<UIControl>
{
    public GameObject[] SuccessPanel = new GameObject[2];
    public GameObject[] PanelArray = new GameObject[3];
    int ShopPage = 0;
    public GameObject ShopUI;
    public GameObject[] ShopPageControlButton = new GameObject[2];
    public GameObject PurchaseConfirmPanel;
    public ItemInfo[] ItemInfos = new ItemInfo[12];
    public int RequestedItemIndex = 0;
    public Text PurchaseItemName;
    public Text PurchaseItemEffects;
    public Image PurchaseItemImage;
    public Text PurchaseItemPrice;
    string[] ItemNameArray = { "Blue", "Green", "Red", "Purple", "Black", "Stone", "Mashmellow", "Pine", "Maple", "Ginkgo", "Asphalt", "Tuxedo" };
    public GameObject[] ShopItemButton = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i=0; i<2; i++)
            {
                if (SuccessPanel[i].gameObject.activeSelf)
                {
                    SuccessPanel[i].SetActive(false);
                }
            }
        }
    }
    public void PanelOnOff(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i != index && PanelArray[i].gameObject.activeSelf)
            {
                PanelArray[i].gameObject.SetActive(false);
            }
            else if (i == index)
                PanelArray[i].gameObject.SetActive(!PanelArray[i].gameObject.activeSelf);
        }
    }
    public void ShopUIDisplay(bool IsInitiate)
    {
        if (IsInitiate)
        {
            PanelOnOff(2);
            ShopPage = 0;
        }
        if (ShopPage == 0)
        {
            ShopPageControlButton[0].gameObject.SetActive(false);
            ShopPageControlButton[1].gameObject.SetActive(true);
        }
        else if (ShopPage == 2)
        {
            ShopPageControlButton[0].gameObject.SetActive(true);
            ShopPageControlButton[1].gameObject.SetActive(false);
        }
        else
        {
            ShopPageControlButton[0].gameObject.SetActive(true);
            ShopPageControlButton[1].gameObject.SetActive(true);
        }
        for(int i=0; i<4; i++)
        {
            Text[] TextArray = ShopItemButton[i].gameObject.GetComponentsInChildren<Text>();
            if (ItemInfos[4 * ShopPage + i].MaxQuantity > 1)
                TextArray[0].text = ItemInfos[4 * ShopPage + i].ItemName + "(" + ItemInfos[4 * ShopPage + i].CurrentQuantity + "/" + ItemInfos[4 * ShopPage + i].MaxQuantity + ")";
            else
                TextArray[0].text = ItemInfos[4 * ShopPage + i].ItemName;
            TextArray[1].text = ItemInfos[4 * ShopPage + i].ItemPrice + "G";
            ShopItemButton[i].gameObject.GetComponentsInChildren<Image>()[1] = ItemInfos[4 * ShopPage + i].ItemVisual;
        }
    }
    public void ShopPageControl(bool Isup)
    {
        if (Isup)
            ShopPage++;
        else
            ShopPage--;
        ShopUIDisplay(false);
    }
    public void PurchaseDenial()
    {
        PurchaseConfirmPanel.gameObject.SetActive(false);
    }
    public void PurchaseConfirm()
    {
        PurchaseConfirmPanel.gameObject.SetActive(false);
        if (ItemInfos[RequestedItemIndex].CurrentQuantity >= ItemInfos[RequestedItemIndex].MaxQuantity)
        {
            Debug.Log("더 이상 구매할 수 없습니다");
        }
        else
        {
            ItemInfos[RequestedItemIndex].CurrentQuantity++;
            ItemManager.Instance.BuyItem(ItemNameArray[RequestedItemIndex]);
        }
        ShopUIDisplay(false);
    }
    public void Purchase(int itemindex)
    {
        PurchaseConfirmPanel.gameObject.SetActive(true);
        RequestedItemIndex = 4*ShopPage+itemindex;
        if (ItemInfos[RequestedItemIndex].MaxQuantity > 1)
            PurchaseItemName.text = ItemInfos[4 * ShopPage + itemindex].ItemName + "(" + ItemInfos[4 * ShopPage + itemindex].CurrentQuantity + "/" + ItemInfos[4 * ShopPage + itemindex].MaxQuantity + ")";
        else
            PurchaseItemName.text = ItemInfos[4 * ShopPage + itemindex].ItemName;
        //PurchaseItemImage = ItemInfos[itemindex].ItemVisual;
        PurchaseItemEffects.text = ItemInfos[4*ShopPage+itemindex].ItemEffectsDescription;
        PurchaseItemPrice.text = ItemInfos[4 * ShopPage + itemindex].ItemPrice + "G";
    }
}
