using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Friend
{
    public string name;
    public Image profileimage;
    public int distance;
}

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
    public Image EquippedCloth;
    public Image EquippedRoad;
    public Image[] EquippedBackground = new Image[5];
    public Text CurrentGold;
    public GameObject NameInputPanel;
    public Text PlayerNameInput;

    //FriendManage에 있던 전역변수들
    public int MyDistance = 500;
    public GameObject GrabHandPanel;
    int InputDistance = 3000;
    public GameObject[] GrabHandButtonArray = new GameObject[4];
    GameObject FriendButton;
    public GameObject AddFriendSuccess;
    public Text CurrentDistance;
    int GrabHandPage = 0;
    public GameObject[] GrabHandPageControlButton = new GameObject[2];
    public List<Friend> FriendList = new List<Friend>();

    //ButtonControl에 있던 전역변수들
    public GameObject GrabHandSuccess;
    public Text GrabHandFriendName;
    public Text GrabHandDistance;

    void Awake() //아이템 등록 완전자동화 가능?
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        ItemInfos.Add("Blue", new ItemInfo("마라톤 복장-파랑", "Sprites/Thumbnail/tb_runnerB", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Blue"])));
        ItemInfos.Add("Green", new ItemInfo("마라톤 복장-초록", "Sprites/Thumbnail/tb_runnerG", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Green"])));
        ItemInfos.Add("Red", new ItemInfo("마라톤 복장-빨강", "Sprites/Thumbnail/tb_runnerR", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Red"])));
        ItemInfos.Add("Purple", new ItemInfo("마라톤 복장-보라", "Sprites/Thumbnail/tb_runnerP", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Purple"])));
        ItemInfos.Add("Black", new ItemInfo("마라톤 복장-검정", "Sprites/Thumbnail/tb_runnerW", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Black"])));
        ItemInfos.Add("Stone", new ItemInfo("돌맹이", "Sprites/Thumbnail/tb_rock", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Stone"])));
        ItemInfos.Add("Mashmellow", new ItemInfo("마시멜로", "Sprites/Thumbnail/tb_silage", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Mashmellow"])));
        ItemInfos.Add("Pine", new ItemInfo("소나무", "Sprites/Thumbnail/tb_tree", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Pine"])));
        ItemInfos.Add("Maple", new ItemInfo("단풍나무", "Sprites/Thumbnail/tb_maple", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Maple"])));
        ItemInfos.Add("Ginkgo", new ItemInfo("은행나무", "Sprites/Thumbnail/tb_gingko", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Ginkgo"])));
        ItemInfos.Add("Asphalt", new ItemInfo("아스팔트", "Sprites/Road/Asphalt", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Asphalt"])));
        ItemInfos.Add("Tuxedo", new ItemInfo("턱시도", "Sprites/Thumbnail/tb_runnerT", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Tuxedo"])));
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
                /*if (PanelArray[i].gameObject.activeSelf && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    if (i == 2)
                    {
                        if (!PurchaseConfirmPanel.activeSelf)
                            PanelArray[i].SetActive(false);
                    }
                    else
                    PanelArray[i].SetActive(false);
                }*/
            }
        }
        if (CheckPointEvent)
        {
            CheckpointPopup.gameObject.SetActive(true);
            CheckpointMessage.text = "19:05에 " + PeoplenuminScreen + "명이 화면 상에 존재했습니다.\n" + CheckpointGoldperPerson * PeoplenuminScreen + "G를 획득하였습니다.";
            CheckPointEvent = false;
        }
        CurrentDistance.text = MyDistance.ToString();
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
            ShopItemButton[i].gameObject.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(ItemInfos[ItemNameArray[4*ShopPage+i]].ItemVisualLocation);
        }
        ShowEquippedItems();
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
        ShowEquippedItems();
    }
    public void Off(GameObject Target)
    {
        Target.SetActive(false);
    }
    void ShowEquippedItems()
    {
        if (ItemManager.Instance.ClothQ.Count > 0)
		{
			EquippedCloth.sprite = Resources.Load<Sprite>(ItemInfos[ItemManager.Instance.ClothQ[0]].ItemVisualLocation);
			Debug.Log("Cloth is equipped");
		}
        if (ItemManager.Instance.RoadQ.Count > 0)
            EquippedRoad.sprite = Resources.Load<Sprite>(ItemInfos[ItemManager.Instance.RoadQ[0]].ItemVisualLocation);
        for (int i = 0; i<ItemManager.Instance.BackGroundQ.Count; i++)
            EquippedBackground[i].sprite = Resources.Load<Sprite>(ItemInfos[ItemManager.Instance.BackGroundQ[i]].ItemVisualLocation);
    }
    public void PanelOff()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PanelArray[i].gameObject.activeSelf)
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
    public void CallNameInputPanel() /*이름 입력 창 활성화 함수*/
    {
        NameInputPanel.gameObject.SetActive(true);
    }
    public void SetPlayerName() /*플레이어 이름 지정 함수, 확인 버튼 누르면 호출*/
    {
        string MyName;
        MyName = PlayerNameInput.text;
        Debug.Log(MyName);
        /*
         







        DB 수정











        */
    }
    //FriendManage에서 가져온 함수들
    public void AddFriend(GameObject AddFriendButton) //현재 버튼 자체에서 정보 가져옴. 수정 필요
    {
        Friend NewFriend = new Friend();
        /*NewFriend.name = InputName.text;
        NewFriend.profileimage = InputProfileImage;
        NewFriend.distance = InputDistance;
        FriendList.Add(NewFriend);*/
        NewFriend.name = AddFriendButton.gameObject.GetComponentInChildren<Text>().text;
        NewFriend.profileimage = AddFriendButton.gameObject.GetComponentsInChildren<Image>()[1];
        NewFriend.distance = InputDistance;
        FriendList.Add(NewFriend);
        AddFriendSuccess.gameObject.SetActive(true);
        AddFriendSuccess.gameObject.GetComponentInChildren<Text>().text = NewFriend.name;
    }
    public void FriendDisplay(bool isInitiate)
    {
        if (isInitiate)
        {
            UIControl.Instance.PanelOnOff(1);
            GrabHandPage = 0;
        }
        if (GrabHandPage == 0)
        {
            if (FriendList.Count <= 4)
            {
                GrabHandPageControlButton[0].gameObject.SetActive(false);
                GrabHandPageControlButton[1].gameObject.SetActive(false);
            }
            else
            {
                GrabHandPageControlButton[0].gameObject.SetActive(false);
                GrabHandPageControlButton[1].gameObject.SetActive(true);
            }
        }
        else if (GrabHandPage == FriendList.Count / 4)
        {
            GrabHandPageControlButton[0].gameObject.SetActive(true);
            GrabHandPageControlButton[1].gameObject.SetActive(false);
        }
        else
        {
            GrabHandPageControlButton[0].gameObject.SetActive(true);
            GrabHandPageControlButton[1].gameObject.SetActive(true);
        }
        for (int i = 0; i < 4; i++)
        {
            GrabHandButtonArray[i].gameObject.SetActive(true);
            if (i >= FriendList.Count - 4 * GrabHandPage)
                GrabHandButtonArray[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject FriendButton = GrabHandButtonArray[i];
            Debug.Log(+i);
            if (i < FriendList.Count % 4)
            {
                Text[] TextArray = FriendButton.gameObject.GetComponentsInChildren<Text>();
                TextArray[0].text = FriendList[4 * GrabHandPage + i].name;
                Debug.Log(+(4 * GrabHandPage + i));
                /*foreach (Image I in FriendButton.GetComponentsInChildren<Image>())
                {
                    if (!I.GetComponent<Button>())
                    {
                        I.sprite = FriendList[4 + GrabHandPage + i].profileimage.sprite;
                        break;
                    }
                }*/
                FriendButton.gameObject.GetComponentsInChildren<Image>()[1].sprite = FriendList[4 * GrabHandPage + i].profileimage.sprite;
                TextArray[1].text = FriendList[4 * GrabHandPage + i].distance.ToString();
            }
        }
    }
    public void GrabHandPageControl(bool Isup)
    {
        if (Isup)
            GrabHandPage++;
        else
            GrabHandPage--;
        FriendDisplay(false);
    }
    //ButtonControl에 있던 함수들
    public void GrabHand(int index) //페이지 기반 수정 필요
    {
        int AverageDistance = (MyDistance + FriendList[index].distance) / 2;
        MyDistance = AverageDistance;
        FriendList[index].distance = AverageDistance;
        FriendDisplay(false);
        GrabHandSuccess.gameObject.SetActive(true);
        GrabHandFriendName.text = FriendList[index].name;
        GrabHandDistance.text = AverageDistance.ToString();
    }
    string GenerateItemEffectsDescription(Item GenerateTarget)
    {
        string Effects = "0";
        switch (GenerateTarget.property)
        {
            case Item.Property.Cloth:
                Effects = "복장 아이템\n";
                break;
            case Item.Property.Road:
                Effects = "도로 아이템\n";
                break;
            case Item.Property.Background:
                Effects = "배경 아이템\n";
                break;
        }
        if (GenerateTarget.PlusRSpeed > 0)
        {
            Effects += "플레이어 속도 +" + GenerateTarget.PlusRSpeed + "m/s\n";
        }
        if (GenerateTarget.PlusFView > 0)
        {
            Effects += "시야 +" + GenerateTarget.PlusFView + "m\n";
        }
        if (GenerateTarget.PlusHGold > 0)
        {
            Effects += "손 잡아주기 골드 +" + GenerateTarget.PlusHGold * 100 + "%\n";
        }
        if (GenerateTarget.PlusCGold > 0)
        {
            Effects += "체크포인트 이벤트 골드 +" + GenerateTarget.PlusCGold * 100 + "%\n";
        }
        Effects = Effects.Substring(0, Effects.Length - 1);
        return Effects;
    }
}

