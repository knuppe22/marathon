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
    string[] ItemNameArray = { "Blue", "Green", "Red", "Purple", "Black", "Stone", "Marshmellow", "Pine", "Maple", "Ginkgo", "Cactus", "Scarecrow", "Asphalt", "Tuxedo" };
    public GameObject[] ShopItemButton = new GameObject[4];
    public GameObject CheckpointPopup;
    public bool CheckPointEvent;
    public Text CheckpointMessage;
    public int CheckpointTime;
    public int CheckpointPeople;
    public int CheckpointGold;
    Dictionary<string, ItemInfo> ItemInfos = new Dictionary<string, ItemInfo>();
    public Image EquippedCloth;
    public Image EquippedRoad;
    public Image[] EquippedBackground = new Image[5];
    public Text CurrentGold;
    public GameObject NameInputPanel;
    public Text PlayerNameInput;
    public GameObject PurchaseButton;
    public GameObject EquipButton;

    //FriendManage에 있던 전역변수들
    public GameObject GrabHandPanel;
    public GameObject[] GrabHandButtonArray = new GameObject[4];
    public GameObject AddFriendSuccess;
    public Text CurrentDistance;
    int GrabHandPage = 0;
    public GameObject[] GrabHandPageControlButton = new GameObject[2];
    int AddFriendPage = 0;
    public GameObject[] AddFriendPageControlButton = new GameObject[2];
    public GameObject[] AddFriendButtons = new GameObject[4];

    //ButtonControl에 있던 전역변수들
    public GameObject GrabHandSuccess;
    public Text GrabHandFriendName;
    public Text GrabHandDistance;

    public List<string> MyFriends = new List<string>();//내 친구 아이디 목록
    public List<string> NearbyUsers = new List<string>();//주변 유저 아이디 목록

    public GameObject FriendRequestPanel;
    public Text FriendRequestMessage;
    string FriendRequestUserId;
    public GameObject ResponseWaitingPanel;

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
        ItemInfos.Add("Marshmellow", new ItemInfo("마시멜로", "Sprites/Thumbnail/tb_silage", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Marshmellow"])));
        ItemInfos.Add("Pine", new ItemInfo("소나무", "Sprites/Thumbnail/tb_tree", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Pine"])));
        ItemInfos.Add("Maple", new ItemInfo("단풍나무", "Sprites/Thumbnail/tb_maple", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Maple"])));
        ItemInfos.Add("Ginkgo", new ItemInfo("은행나무", "Sprites/Thumbnail/tb_gingko", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Ginkgo"])));
        ItemInfos.Add("Cactus", new ItemInfo("선인장", "Sprites/Thumbnail/tb_cactus", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Cactus"])));
        ItemInfos.Add("Scarecrow", new ItemInfo("허수아비", "Sprites/Thumbnail/tb_scareCrow", GenerateItemEffectsDescription(ItemManager.Instance.itemlist["Scarecrow"])));
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
            }
        }
        if (CheckPointEvent)
        {
            CheckpointPopup.gameObject.SetActive(true);
            CheckpointMessage.text = CheckpointTime + ":00에 " + CheckpointPeople + "명이 화면 상에 존재했습니다.\n" + CheckpointGold + "G를 획득하였습니다.";
            CheckPointEvent = false;
        }
        if(AuthManager.Instance.CurrentUserId != null && RunManager.Instance.users.ContainsKey(AuthManager.Instance.CurrentUserId))
            CurrentDistance.text = RunManager.Instance.MeterForm((int)RunManager.Instance.users[AuthManager.Instance.CurrentUserId].score);
        ResponseWaitingPanel.gameObject.SetActive(RunManager.Instance.friendRequesting || RunManager.Instance.handRequesting);

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
        else if ((ShopPage == ItemManager.Instance.itemlist.Count/4-1&&ItemManager.Instance.itemlist.Count%4==0)||(ShopPage==ItemManager.Instance.itemlist.Count/4&&ItemManager.Instance.itemlist.Count%4!=0))
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
            if (ShopPage * 4 + i >= ItemManager.Instance.itemlist.Count)
            {
                ShopItemButton[i].gameObject.SetActive(false);
            }
            else
            {
                ShopItemButton[i].gameObject.SetActive(true);
                Text[] TextArray = ShopItemButton[i].gameObject.GetComponentsInChildren<Text>();
                {
                    TextArray[0].text = ItemInfos[ItemNameArray[4 * ShopPage + i]].ItemName + "("
                    + ItemManager.Instance.itemlist[ItemNameArray[4 * ShopPage + i]].PresPoss + "/"
                    + ItemManager.Instance.itemlist[ItemNameArray[4 * ShopPage + i]].Maximum + ")";
                }
                TextArray[1].text = ItemManager.Instance.itemlist[ItemNameArray[4 * ShopPage + i]].Price + "G";
                ShopItemButton[i].gameObject.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(ItemInfos[ItemNameArray[4 * ShopPage + i]].ItemVisualLocation);
            }
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
        PurchaseUIUpdate();
        ShopUIDisplay(false);
    }
    public void Purchase(int itemindex)
    {
        PurchaseConfirmPanel.gameObject.SetActive(true);
        RequestedItemIndex = 4 * ShopPage + itemindex;
        PurchaseItemImage.sprite = Resources.Load<Sprite>(ItemInfos[ItemNameArray[RequestedItemIndex]].ItemVisualLocation);
        PurchaseItemEffects.text = ItemInfos[ItemNameArray[RequestedItemIndex]].ItemEffectsDescription;
        PurchaseItemPrice.text = ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Price + "G";
        PurchaseUIUpdate();
    }   
    public void CheckpointTest()//테스트용
    {
        CheckPointEvent = true;
    }
    public void EquipItem()
    {
        ItemManager.Instance.EquipmentItem(ItemNameArray[RequestedItemIndex]);
        PurchaseUIUpdate();
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
		}
        else
        {
            EquippedCloth.sprite = Resources.Load<Sprite>("WhiteScreen");
        }
        if (ItemManager.Instance.RoadQ.Count > 0)
            EquippedRoad.sprite = Resources.Load<Sprite>(ItemInfos[ItemManager.Instance.RoadQ[0]].ItemVisualLocation);
        else
            EquippedRoad.sprite = Resources.Load<Sprite>("WhiteScreen");
        for (int i = 0; i < 5; i++)
        {
            if (i < ItemManager.Instance.BackGroundQ.Count)
                EquippedBackground[i].sprite = Resources.Load<Sprite>(ItemInfos[ItemManager.Instance.BackGroundQ[i]].ItemVisualLocation);
            else
                EquippedBackground[i].sprite = Resources.Load<Sprite>("WhiteScreen");
        }
    }
    public void PanelOff()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PanelArray[i].gameObject.activeSelf)
            {
                PanelArray[i].SetActive(false);
                if (i == 2 && PurchaseConfirmPanel.activeSelf) 
                {
                    PurchaseConfirmPanel.gameObject.SetActive(false);
                }
            }
        }
        if (ResponseWaitingPanel.activeSelf)
        {
            RunManager.Instance.RequestCancel();
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

        if(MyName != "")
        {
            RunManager.Instance.ChangeName(MyName);
            NameInputPanel.gameObject.SetActive(false);
        }
    }
    //FriendManage에서 가져온 함수들
    public void AddFriend(int index) //현재 버튼 자체에서 정보 가져옴. 수정 필요
    {
        int num = AddFriendPage * 4 + index;
        RunManager.Instance.StartFriendRequset(NearbyUsers[num]);
    }
    public void GrabHand(int index)
    {
        int num = GrabHandPage * 4 + index;
        RunManager.Instance.StartHandRequest(MyFriends[num]);
    }
    public async void FriendDisplay(bool isInitiate)
    {
        if (isInitiate)
        {
            UIControl.Instance.PanelOnOff(1);
            GrabHandPage = 0;
            if (MyFriends.Count == 0)
            {
                MyFriends = await RunManager.Instance.NearFriends();
            }
        }
        if (GrabHandPage == 0)
        {
            //if (RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends.Count <= 4)
            if (MyFriends.Count <= 4) 
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
        else if ((GrabHandPage == MyFriends.Count / 4 && MyFriends.Count % 4 != 0) || (GrabHandPage == MyFriends.Count / 4 - 1 && MyFriends.Count % 4 == 0))
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
            //if (i >= RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends.Count - 4 * GrabHandPage)
            if (i >= MyFriends.Count - 4 * GrabHandPage)
            {
                GrabHandButtonArray[i].gameObject.SetActive(false);
                Debug.Log("GrabHandButton is disabled");
            }
            else
            {
                GrabHandButtonArray[i].gameObject.SetActive(true);
                Debug.Log("GrabHandButton is enabled");
            }
        }
        for (int i = 0; i < 4; i++)
        {
            //if (i < RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends.Count % 4)
            if(GrabHandButtonArray[i].activeSelf)
            {
                Text[] TextArray = GrabHandButtonArray[i].gameObject.GetComponentsInChildren<Text>();
                /*TextArray[0].text = RunManager.Instance.users[RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends[4 * GrabHandPage + i]].name;
                TextArray[1].text = RunManager.Instance.users[RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends[4 * GrabHandPage + i]].score.ToString();*/
                TextArray[0].text = RunManager.Instance.users[MyFriends[4 * GrabHandPage + i]].name;
                TextArray[1].text = RunManager.Instance.users[MyFriends[4 * GrabHandPage + i]].score.ToString();
            }
        }
        Debug.Log("FriendDisplay is functioning properly");
    }
    public void GrabHandPageControl(bool Isup)
    {
        if (Isup)
            GrabHandPage++;
        else
            GrabHandPage--;
        FriendDisplay(false);
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
    public async void AddFriendDisplay(bool isInitiate)
    {
        if (isInitiate)
        {
            PanelOnOff(0);
            AddFriendPage = 0;
            NearbyUsers = await RunManager.Instance.NearPeople(); //지속적으로 업데이트 되어야 할텐데... 주기?
            Debug.Log(+NearbyUsers.Count);
        }
        if (AddFriendPage == 0)
        {
            if (NearbyUsers.Count <= 4)
            {
                AddFriendPageControlButton[0].gameObject.SetActive(false);
                AddFriendPageControlButton[1].gameObject.SetActive(false);
            }
            else
            {
                AddFriendPageControlButton[0].gameObject.SetActive(false);
                AddFriendPageControlButton[1].gameObject.SetActive(true);
            }
        }
        else if ((AddFriendPage == NearbyUsers.Count / 4 && NearbyUsers.Count % 4 != 0) || (AddFriendPage == NearbyUsers.Count / 4 - 1 && NearbyUsers.Count % 4 == 0)) 
        {
            AddFriendPageControlButton[0].gameObject.SetActive(true);
            AddFriendPageControlButton[1].gameObject.SetActive(false);
        }
        else
        {
            AddFriendPageControlButton[0].gameObject.SetActive(true);
            AddFriendPageControlButton[1].gameObject.SetActive(true);
        }
        for (int i = 0; i < 4; i++)
        {
            if (i >= NearbyUsers.Count - 4 * AddFriendPage)
            {
                AddFriendButtons[i].gameObject.SetActive(false);
                Debug.Log("AddFriendButton is disabled");
            }
            else
            {
                AddFriendButtons[i].gameObject.SetActive(true);
                Debug.Log("AddFriendButton is enabled");
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (AddFriendButtons[i].activeSelf)
            {
                AddFriendButtons[i].gameObject.GetComponentInChildren<Text>().text = RunManager.Instance.users[NearbyUsers[4 * AddFriendPage + i]].name;
            }
        }
        Debug.Log("AddFriendDisplay is functioning properly");
    }
    public void UnequipBackgroundItem(int index)
    {
        string ItemToUnequip;
        if (ItemManager.Instance.BackGroundQ.Count > index)
        {
            ItemToUnequip = ItemManager.Instance.BackGroundQ[index];
            ItemManager.Instance.BackGroundQ.RemoveAt(index);
            ItemManager.Instance.AllQ.Remove(ItemToUnequip);
            ItemManager.Instance.itemlist[ItemToUnequip].Equipment--;
        }
        PurchaseUIUpdate();
        ShowEquippedItems();
        BackgroundManager.Instance.SetBackgroundImage();
    }
    public void UnequipClothItem()
    {
        string ItemToUnequip;
        if (ItemManager.Instance.ClothQ.Count > 0)
        {
            ItemToUnequip = ItemManager.Instance.ClothQ[0];
            ItemManager.Instance.ClothQ.RemoveAt(0);
            ItemManager.Instance.AllQ.Remove(ItemToUnequip);
            ItemManager.Instance.itemlist[ItemToUnequip].Equipment--;
        }
        PurchaseUIUpdate();
        ShowEquippedItems();
        BackgroundManager.Instance.SetRunnerImage();
    }
    public void UnequipRoadItem()
    {
        string ItemToUnequip;
        if (ItemManager.Instance.RoadQ.Count > 0)
        {
            ItemToUnequip = ItemManager.Instance.RoadQ[0];
            ItemManager.Instance.RoadQ.RemoveAt(0);
            ItemManager.Instance.AllQ.Remove(ItemToUnequip);
            ItemManager.Instance.itemlist[ItemToUnequip].Equipment--;
        }
        PurchaseUIUpdate();
        ShowEquippedItems();
        BackgroundManager.Instance.SetRoadImage();
    }
    void PurchaseUIUpdate()
    {
        PurchaseButton.gameObject.SetActive(true);
        EquipButton.gameObject.SetActive(true);
        {
            PurchaseItemName.text = ItemInfos[ItemNameArray[RequestedItemIndex]].ItemName + "("
            + ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].PresPoss + "/"
            + ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Maximum + ")";
        }
        if (ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].PresPoss == ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Maximum)
        {
            PurchaseButton.gameObject.SetActive(false);
        }
        if (ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].Equipment == ItemManager.Instance.itemlist[ItemNameArray[RequestedItemIndex]].PresPoss)
        {
            EquipButton.gameObject.SetActive(false);
        }
    }
    public void FriendRequest(string RequestUserId) //친구추가 요청 UI 띄우는 함수, RequestUserId에 친구추가를 요청한 유저의 ID가 입력되면 됩니다.
    {
        FriendRequestUserId = RequestUserId;
        FriendRequestPanel.gameObject.SetActive(true);
        FriendRequestMessage.text = "'" + RunManager.Instance.users[FriendRequestUserId].name + "' 님이 당신과 함께 뛰고 싶어합니다!\n수락하시겠습니까?";
    }
    public void FriendRequestAcceptCheck(bool Accept)
    {
        if (Accept)
        {
            RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends.Add(FriendRequestUserId);
            DBManager.Instance.SetUserValue("friends", RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends);
            GoldManager.Instance.EarnMoney(300);
        }
        else
        {
            FriendRequestPanel.gameObject.SetActive(false);
        }
    }
    public void AddFriendSuccessful(string FriendName)
    {
        ResponseWaitingPanel.gameObject.SetActive(false);
        AddFriendSuccess.gameObject.SetActive(true);
        AddFriendSuccess.gameObject.GetComponentInChildren<Text>().text = "'" + FriendName + "' 님과 친구가 되었습니다.\n300G를 획득하였습니다.";
        AddFriendDisplay(true);
    }
    public void GrabHandSuccessful(string TargetName, int ArrivedDistance, int EarnedGold)
    {
        ResponseWaitingPanel.gameObject.SetActive(false);
        GrabHandSuccess.gameObject.SetActive(true);
        GrabHandSuccess.gameObject.GetComponentInChildren<Text>().text = "'" + TargetName + "' 님과 손을 잡았습니다.\n현재 당신의 위치는"+ ArrivedDistance+"입니다.\n" + EarnedGold + "G를 획득하였습니다.";
        FriendDisplay(true);
    }
}

