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
    public Text InputName;
    public Image InputProfileImage;
    public GameObject GrabHandButton;
    public GameObject GrabHandPanel;
    int InputDistance = 3000;
    public GameObject[] GrabHandButtonArray = new GameObject[4];
    GameObject FriendButton;
    public GameObject AddFriendSuccess;
    public Text AddedFriendName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindWithTag("MyDistance").gameObject.GetComponent<Text>().text = MyDistance.ToString();
    }
    
    public void AddFriend()
    {
        Friend NewFriend = new Friend();
        NewFriend.name = InputName.text;
        NewFriend.profileimage = InputProfileImage;
        NewFriend.distance = InputDistance;
        FriendList.Add(NewFriend);
        /*AddFriendSuccess.gameObject.SetActive(true);
        AddFriendSuccess.gameObject.GetComponentInChildren<Text>().text = AddedFriendName.text;*/
    }

    public void FriendDisplay()
    {
        //int YPosition = 200;
        /*for(int i=0; i<ExistingButtonArray.Length; i++)
        {
            Destroy(ExistingButtonArray[i]);
        }*/
        for (int i = 0; i < 4; i++)
        {
            GrabHandButtonArray[i].gameObject.SetActive(true);
            if (i >= FriendList.Count)
                GrabHandButtonArray[i].gameObject.SetActive(false);
            Debug.Log(+FriendList.Count);
        }
        int ButtonCount = 0;
        foreach(Friend F in FriendList)
        {
            /*FriendButton = Instantiate(GrabHandButton, GrabHandPanel.transform.position, Quaternion.identity, GrabHandPanel.transform);
            FriendButton.transform.localPosition = new Vector3(0, YPosition, 0);*/
            //GrabHandButtonArray[ButtonCount].gameObject.SetActive(true);
            GameObject FriendButton = GrabHandButtonArray[ButtonCount];
            Text[] TextArray = FriendButton.gameObject.GetComponentsInChildren<Text>();
            TextArray[0].text = F.name;
            foreach(Image I in FriendButton.GetComponentsInChildren<Image>())
            {
                if(!I.GetComponent<Button>()) I.sprite = InputProfileImage.sprite;
            }
            TextArray[1].text = F.distance.ToString();
            ButtonCount++;
            //YPosition -= 170;
        }
    }
    /*public void GrabHand(int index)
    {
        Debug.Log(+index);
        Debug.Log(+FriendList.Count);
        int AverageDistance = (MyDistance + FriendList[index].distance) / 2;
        MyDistance = AverageDistance;
        FriendList[index].distance = AverageDistance;
        FriendDisplay();
    }*/
}
