using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend
{
    public string name;
    public Image profileimage;
    public int distance;
}

public class FriendManage : SingletonBehaviour<FriendManage>
{
    public List<Friend> FriendList = new List<Friend>();

    public int MyDistance = 500;
    public GameObject GrabHandButton;
    public GameObject GrabHandPanel;
    int InputDistance = 3000;
    public GameObject[] GrabHandButtonArray = new GameObject[4];
    GameObject FriendButton;
    public GameObject AddFriendSuccess;
    public Text CurrentDistance;
    int GrabHandPage = 0;
    public GameObject[] GrabHandPageControlButton = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentDistance.text = MyDistance.ToString();
        // GameObject.FindWithTag("MyDistance").gameObject.GetComponent<Text>().text = MyDistance.ToString();
    }
    
    public void AddFriend(GameObject AddFriendButton)
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
                Debug.Log(+(4*GrabHandPage+i));
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
}
