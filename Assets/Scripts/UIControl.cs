using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemInfo
{
    public string ItemName;
    public string ItemVisualLocation;
    [TextArea]
    public string ItemEffectsDescription;
    public ItemInfo(string ItemName, string ItemVisualLocation, string ItemEffectsDescription)
    {
        this.ItemName = ItemName;
        this.ItemVisualLocation = ItemVisualLocation;
        this.ItemEffectsDescription = ItemEffectsDescription;
    }
}
public class UIControl : SingletonBehaviour<UIControl>
{
    public GameObject[] PopupPanel = new GameObject[3];
    public GameObject[] PanelArray = new GameObject[3];
    int ShopPage = 0;
    public GameObject ShopUI;
    public GameObject[] ShopPageControlButton = new GameObject[2];
    public GameObject PurchaseConfirmPanel;
    public int RequestedItemIndex = 0;
    public Text PurchaseItemName;
    public Text PurchaseItemEffects;
    public Image PurchaseItemImage;
    public Text PurchaseItemPrice;
    string[] ItemNameArray = { "Blue", "Green", "Red", "Purple", "Black", "Stone", "Mashmellow", "Pine", "Maple", "Ginkgo", "Asphalt", "Tuxedo" };
    public GameObject[] ShopItemButton = new GameObject[4];
    public GameObject CheckpointPopup;
    public int[] CheckpointTime = new int[2]; //체크포인트 이벤트 발생 시간(우선 프레임단위)
    public int CheckpointGoldperPerson;
    public int PeoplenuminScreen;
    bool CheckPointEvent;
    public Text CheckpointMessage;
    Dictionary<string, ItemInfo> ItemInfos = new Dictionary<string, ItemInfo>();

    void Awake()
    {
        ItemInfos.Add("Blue", new ItemInfo("마라톤 복장-파랑", "", "복장 아이템\n플레이어 속도 +0.1m/s"));
        ItemInfos.Add("Green", new ItemInfo("마라톤 복장-초록", "", "복장 아이템\n플레이어 속도 +0.1m/s"));
        ItemInfos.Add("Red", new ItemInfo("마라톤 복장-빨강", "", "복장 아이템\n플레이어 속도 +0.1m/s"));
        ItemInfos.Add("Purple", new ItemInfo("마라톤 복장-보라", "", "복장 아이템\n플레이어 속도 +0.1m/s\n시야 +50m"));
        ItemInfos.Add("Black", new ItemInfo("마라톤 복장-검정", "", "복장 아이템\n플레이어 속도 +0.1m/s\n손 잡아주기 골드 +20%"));
        ItemInfos.Add("Stone", new ItemInfo("돌맹이", "Sprites/Thumbnail/tb_rock", "배경 아이템\n플레이어 속도 +1m/s"));
        ItemInfos.Add("Mashmellow", new ItemInfo("마시멜로", "Sprites/Thumbnail/tb_silage", "배경 아이템\n체크포인트 이벤트 골드 +30%"));
        ItemInfos.Add("Pine", new ItemInfo("소나무", "Sprites/Thumbnail/tb_tree", "배경 아이템\n플레이어 속도 +1.5m/s"));
        ItemInfos.Add("Maple", new ItemInfo("단풍나무", "Sprites/Thumbnail/tb_maple", "배경 아이템\n플레이어 속도 +1.5m/s"));
        ItemInfos.Add("Ginkgo", new ItemInfo("은행나무", "Sprites/Thumbnail/tb_ginkgo", "배경 아이템\n플레이어 속도 +1.5m/s"));
        ItemInfos.Add("Asphalt", new ItemInfo("아스팔트", "", "도로 아이템\n시야 +500m"));
        ItemInfos.Add("Tuxedo", new ItemInfo("턱시도", "", "복장 아이템\n플레이어 속도 +3m/s\n손 잡아주기 골드 +100%"));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i=0; i<3; i++)
            {
                if (PopupPanel[i].gameObject.activeSelf)
                {
                    PopupPanel[i].SetActive(false);
                }
                if (PanelArray[i].gameObject.activeSelf && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    if (i == 2)
                    {
                        if (!PurchaseConfirmPanel.activeSelf)
                            PanelArray[i].SetActive(false);
                    }
                    else
                    PanelArray[i].SetActive(false);
                }
            }
        }
        if (CheckPointEvent)
        {
            CheckpointPopup.gameObject.SetActive(true);
            CheckpointMessage.text = "19:05에 " + PeoplenuminScreen + "명이 화면 상에 존재했습니다.\n" + CheckpointGoldperPerson * PeoplenuminScreen + "G를 획득하였습니다.";
            CheckPointEvent = false;
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
                PanelArray[i].gameObject.SetActive(true);
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
            if (ItemManager.Instance.itemlist[ItemNameArray[4 * ShopPage + i]].Maximum > 1)
                TextArray[0].text = ItemInfos[ItemNameArray[4 * ShopPage + i]].ItemName + "(" 
                +ItemManager.Instance.itemlist[ItemNameArray[4 * ShopPage + i]].PresPoss + "/" 
                +ItemManager.Instance.itemlist[ItemNameArray[4 * ShopPage + i]].Maximum + ")";
            else
                TextArray[0].text = ItemInfos[ItemNameArray[4 * ShopPage + i]].ItemName;
            TextArray[1].text = ItemManager.Instance.itemlist[ItemNameArray[4*ShopPage+i]].Price + "G";
            Debug.Log(ItemInfos[ItemNameArray[4 * ShopPage + i]].ItemVisualLocation);
            ShopItemButton[i].gameObject.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(ItemInfos[ItemNameArray[4*ShopPage+i]].ItemVisualLocation);
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
        //PurchaseConfirmPanel.gameObject.SetActive(false);
    }
    public void PurchaseConfirm()
    {
        //PurchaseConfirmPanel.gameObject.SetActive(false);
        if (ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].PresPoss >= ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Maximum)
        {
            Debug.Log("더 이상 구매할 수 없습니다");
        }
        else
        { 
            ItemManager.Instance.BuyItem(ItemNameArray[RequestedItemIndex]);
        }
        ShopUIDisplay(false);
    }
    public void Purchase(int itemindex)
    {
        PurchaseConfirmPanel.gameObject.SetActive(true);
        RequestedItemIndex = 4*ShopPage+itemindex;
        if (ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Maximum > 1)
            PurchaseItemName.text = ItemInfos[ItemNameArray[RequestedItemIndex]].ItemName + "(" 
          + ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].PresPoss + "/" 
          + ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Maximum + ")";
        else
            PurchaseItemName.text = ItemInfos[ItemNameArray[RequestedItemIndex]].ItemName;
        PurchaseItemImage.sprite = Resources.Load<Sprite>(ItemInfos[ItemNameArray[RequestedItemIndex]].ItemVisualLocation);
        PurchaseItemEffects.text = ItemInfos[ItemNameArray[RequestedItemIndex]].ItemEffectsDescription;
        PurchaseItemPrice.text = ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Price + "G";
    }
    public void CheckpointTest()//테스트용
    {
        CheckPointEvent = true;
    }
    public void EquipItem()
    {
        ItemManager.Instance.EquipmentItem(ItemNameArray[RequestedItemIndex]);
    }
    public void Off(GameObject Target)
    {
        Target.SetActive(false);
    }
}
