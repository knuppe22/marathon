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
    public string ItemPrice;
    public bool isPurchased;
}
public class UIControl : MonoBehaviour
{
    public GameObject[] SuccessPanel = new GameObject[2];
    public GameObject[] PanelArray = new GameObject[3];
    int ShopPage = 0;
    public GameObject[] ShopItemArray = new GameObject[12];
    public GameObject ShopUI;
    public GameObject[] PageControlButton = new GameObject[2];
    public GameObject PurchaseConfirmPanel;
    public ItemInfo[] ItemInfos = new ItemInfo[12];
    public int RequestedItemIndex = 0;
    public GameObject[] SoldOutImages = new GameObject[4];
    public Text PurchaseItemName;
    public Text PurchaseItemEffects;
    public Image PurchaseItemImage;
    public Text PurchaseItemPrice;
    public int[] PurchasedQuantity = new int[5];//3개까지 구매 가능 아이템 구매 수량, 가격기준 오름차순 정렬
    public Text[] PurchasedQuantityInButton = new Text[5];
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
            PageControlButton[0].gameObject.SetActive(false);
            PageControlButton[1].gameObject.SetActive(true);
        }
        else if (ShopPage == 2)
        {
            PageControlButton[0].gameObject.SetActive(true);
            PageControlButton[1].gameObject.SetActive(false);
        }
        else
        {
            PageControlButton[0].gameObject.SetActive(true);
            PageControlButton[1].gameObject.SetActive(true);
        }
        Button[] DisplayedItems = ShopUI.gameObject.GetComponentsInChildren<Button>();
        for(int i=2; i<DisplayedItems.Length; i++)
        {
            DisplayedItems[i].gameObject.SetActive(false);
        }
        for(int i=0; i<4; i++)
        {
            SoldOutImages[i].gameObject.SetActive(false);
        }
        for(int j=0+4*ShopPage; j<4+4*ShopPage; j++)
        {
            if (ItemInfos[j].isPurchased)
                SoldOutImages[j - 4 * ShopPage].gameObject.SetActive(true);
            else
                ShopItemArray[j].gameObject.SetActive(true);
        }
    }
    public void PageControl(bool Isup)
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
        if (RequestedItemIndex < 10 && RequestedItemIndex > 4 && PurchasedQuantity[RequestedItemIndex - 5] < 2)
        {
            PurchasedQuantity[RequestedItemIndex - 5]++;
            PurchasedQuantityInButton[RequestedItemIndex - 5].text = PurchasedQuantity[RequestedItemIndex - 5].ToString();
        }
        else
            ItemInfos[RequestedItemIndex].isPurchased = true;
        ShopUIDisplay(false);
    }
    public void Purchase(int itemindex)
    {
        PurchaseConfirmPanel.gameObject.SetActive(true);
        RequestedItemIndex = itemindex;
        PurchaseItemName.text = ItemInfos[itemindex].ItemName;
        //PurchaseItemImage = ItemInfos[itemindex].ItemVisual;
        PurchaseItemEffects.text = ItemInfos[itemindex].ItemEffectsDescription;
        PurchaseItemPrice.text = ItemInfos[itemindex].ItemPrice;
    }
}
