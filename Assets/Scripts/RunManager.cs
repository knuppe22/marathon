using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class RunManager : SingletonBehaviour<RunManager>
{
    [SerializeField]
    public Text MeterText;
    
    public Dictionary<string, User> users = new Dictionary<string, User>();
    public float RunSpeed = 3;
    public float FriendViewDist = 5000;
    public float HandGoldRate = 0.4f;
    public float CheckGoldRate = 1;
    float time = 0;
    float requestTime = 0;

    bool startFinished = false;
    bool gpsBool = false;
    public bool friendRequesting = false;
    public bool handRequesting = false;
    bool requestIO = false;
    string requestVS = "";
    float handPreSave = 0;

    async void Start()
    {
#if !UNITY_EDITOR
        Input.location.Start();
#endif

        User tmpUser = await DBManager.Instance.GetUser();

        users.Add(AuthManager.Instance.CurrentUserId, tmpUser);

        //AuthManager, DBManager만 있는 씬을 따로 만들어서 씬을 변경?
        //await 두 개가 동시에 되는가.
        
        GoldManager.Instance.SetGold(users[AuthManager.Instance.CurrentUserId].gold);

        ItemManager.Instance.PossItem = users[AuthManager.Instance.CurrentUserId].items;
        if (ItemManager.Instance.PossItem != null)
        {
            foreach (string item in ItemManager.Instance.PossItem)
            {
                Debug.Log("MyItems: " + item);
                ItemManager.Instance.itemlist[item].PresPoss++;
            }
        }

        ItemManager.Instance.ApplyItemEffect();


        float AddMeter = RunSpeed * CalcOnline();
        users[AuthManager.Instance.CurrentUserId].score += AddMeter;

        ItemManager.Instance.AllQ = users[AuthManager.Instance.CurrentUserId].equippedItems;
        if (ItemManager.Instance.AllQ != null)
        {
            foreach (string item in ItemManager.Instance.AllQ)
            {
                switch (ItemManager.Instance.itemlist[item].property)
                {
                    case Item.Property.Cloth:
                        ItemManager.Instance.ClothQ.Add(item);
                        break;
                    case Item.Property.Road:
                        ItemManager.Instance.RoadQ.Add(item);
                        break;
                    case Item.Property.Background:
                        ItemManager.Instance.BackGroundQ.Add(item);
                        break;

                }
                ItemManager.Instance.itemlist[item].Equipment++;
            }
        }

        if (users[AuthManager.Instance.CurrentUserId].friends != null)
        {
            foreach (string friend in users[AuthManager.Instance.CurrentUserId].friends)
            {
                User tmpFriend = await DBManager.Instance.GetUser(friend);
                if (users.ContainsKey(friend)) users[friend] = tmpFriend;
                else users.Add(friend, tmpFriend);
            }
        }

        BackgroundManager.Instance.SetBackgroundImage();
        BackgroundManager.Instance.SetRoadImage();
        BackgroundManager.Instance.SetRunnerImage();

        UpdateUserData();
        DBManager.Instance.SetUserValue("handRequest", "");
        DBManager.Instance.SetUserValue("friendRequest", "");

        startFinished = true;
    }

    async void Update()
    {
#if !UNITY_EDITOR
        gpsBool = (Input.location.status == LocationServiceStatus.Running);
#else
        gpsBool = true;
#endif

        if (!startFinished) return;
        
        time += Time.deltaTime;
        requestTime += Time.deltaTime;

        CheckPointEvent();
        
        if (time > 5)
        {
            time = 0;
            UpdateUserData();

            foreach (string friend in users[AuthManager.Instance.CurrentUserId].friends)
            {
                User tmpFriend = await DBManager.Instance.GetUser(friend);
                if (users.ContainsKey(friend)) users[friend] = tmpFriend;
                else users.Add(friend, tmpFriend);
                BackgroundManager.Instance.SetRunnerImage(friend);
            }
        }

        if (requestTime > 1)
        {
            requestTime = 0;
            
            if(friendRequesting)
            {
                users[AuthManager.Instance.CurrentUserId].friendRequest = (string)await DBManager.Instance.GetUserValue("friendRequest");
                if (requestIO)
                {
                    if (users[AuthManager.Instance.CurrentUserId].friendRequest == "")
                    {
                        DBManager.Instance.SetOtherUserValue(requestVS, "friendRequest", AuthManager.Instance.CurrentUserId);
                        AddFriend(requestVS);
                        friendRequesting = false;
                    }
                }
                else
                {
                    if (users[AuthManager.Instance.CurrentUserId].friendRequest == requestVS)
                    {
                        DBManager.Instance.SetUserValue("friendRequest", "");
                        AddFriend(requestVS);
                        friendRequesting = false;
                    }
                }
                handRequesting = false;
            }
            else if(handRequesting)
            {
                users[AuthManager.Instance.CurrentUserId].handRequest = (string)await DBManager.Instance.GetUserValue("handRequest");
                if (requestIO)
                {
                    if (users[AuthManager.Instance.CurrentUserId].handRequest == "")
                    {
                        handPreSave = users[requestVS].score;
                        DBManager.Instance.SetOtherUserValue(requestVS, "handRequest", AuthManager.Instance.CurrentUserId);
                        GrabHand(requestVS);
                        handRequesting = false;
                    }
                }
                else
                {
                    if (users[AuthManager.Instance.CurrentUserId].handRequest == requestVS)
                    {
                        DBManager.Instance.SetUserValue("handRequest", "");
                        GrabHand(requestVS);
                        handRequesting = false;
                    }
                }
            }
            else
            {
            }
        }


        users[AuthManager.Instance.CurrentUserId].score += RunSpeed * Time.deltaTime;
        MeterText.text = ((int)users[AuthManager.Instance.CurrentUserId].score).ToString();

    }

    float CalcOnline()
    {
        string lastOnline = users[AuthManager.Instance.CurrentUserId].lastOnline;
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();

        DateTime last = DateTime.Parse(lastOnline);
        DateTime present = DateTime.Parse(users[AuthManager.Instance.CurrentUserId].lastOnline);

        TimeSpan span = present.Subtract(last);

        Debug.Log(span.TotalSeconds);
        
        if (span.TotalSeconds >= 300) return 300;

        else return (float)span.TotalSeconds;
    }

    public void ChangeName(string name)
    {
        users[AuthManager.Instance.CurrentUserId].name = name;
        DBManager.Instance.SetUserValue("name", name);
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            SceneManager.LoadScene("bgTest");
    }

    void UpdateUserData()
    {
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();

        if (DBManager.Instance.RootReference != null)
        {
            DBManager.Instance.SetUserValue("lastOnline", users[AuthManager.Instance.CurrentUserId].lastOnline);
            DBManager.Instance.SetUserValue("score", users[AuthManager.Instance.CurrentUserId].score);

#if !UNITY_EDITOR
            if (gpsBool)
                DBManager.Instance.SetLocation(new Location(Input.location.lastData, users[AuthManager.Instance.CurrentUserId].lastOnline));
#else
            if (gpsBool)
                DBManager.Instance.SetLocation(new Location());
#endif
        }

    }

    void CheckPointEvent()
    {
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();
        string onlineTime = users[AuthManager.Instance.CurrentUserId].lastOnline;

        DateTime time = DateTime.Parse(onlineTime);

        if(time.Minute == 0)
        {
            int friendView = 1;
            float gainGold;

            foreach(string friend in users[AuthManager.Instance.CurrentUserId].friends)
            {
                float realScore = users[friend].score;
                float realSpeed = 3f;
                float runTime = 0f;
                foreach (string item in users[friend].items)
                    if (ItemManager.Instance.itemlist.ContainsKey(item)) realSpeed += ItemManager.Instance.itemlist[item].PlusRSpeed;

                string lastOnline = users[friend].lastOnline;


                DateTime last = DateTime.Parse(lastOnline);
                DateTime present = DateTime.Parse(DateTime.Now.ToString("s"));

                TimeSpan span = present.Subtract(last);

                if (span.TotalSeconds >= 300) runTime = 300;

                else runTime = (float)span.TotalSeconds;

                realScore += realSpeed * runTime;
                float per = (realScore - users[AuthManager.Instance.CurrentUserId].score) / FriendViewDist / 2 + 0.5f;

                if (0 <= per && per <= 1) friendView++;
            }
            
            gainGold = 100 * friendView * CheckGoldRate;

            string checkedTime = users[AuthManager.Instance.CurrentUserId].checkedTime;

            if (checkedTime == "")
            {
                GoldManager.Instance.EarnMoney((int)gainGold);
                DBManager.Instance.SetUserValue("checkedTime", onlineTime);
                users[AuthManager.Instance.CurrentUserId].checkedTime = onlineTime;
                UIControl.Instance.CheckPointEvent = true;
                UIControl.Instance.CheckpointGold = (int)gainGold;
                UIControl.Instance.CheckpointPeople = friendView;
                UIControl.Instance.CheckpointTime = time.Hour;
            }
            else
            {
                DateTime check = DateTime.Parse(checkedTime);

                if (time.Date != check.Date)
                {
                    GoldManager.Instance.EarnMoney((int)gainGold);
                    DBManager.Instance.SetUserValue("checkedTime", onlineTime);
                    users[AuthManager.Instance.CurrentUserId].checkedTime = onlineTime;
                    UIControl.Instance.CheckPointEvent = true;
                    UIControl.Instance.CheckpointGold = (int)gainGold;
                    UIControl.Instance.CheckpointPeople = friendView;
                    UIControl.Instance.CheckpointTime = time.Hour;
                }

                else
                {
                    if (time.Hour != check.Hour)
                    {
                        GoldManager.Instance.EarnMoney((int)gainGold);
                        DBManager.Instance.SetUserValue("checkedTime", onlineTime);
                        users[AuthManager.Instance.CurrentUserId].checkedTime = onlineTime;
                        UIControl.Instance.CheckPointEvent = true;
                        UIControl.Instance.CheckpointGold = (int)gainGold;
                        UIControl.Instance.CheckpointPeople = friendView;
                        UIControl.Instance.CheckpointTime = time.Hour;
                    }
                }
            }
        }
    }

    public string MeterForm(int score)
    {
        int[] cut = new int[] { 10000000, 10000 , -1 };
        int[] div = new int[] { 1000000, 1000, 1 };
        string[] suffix = new string[] { "Mm", "km", "m" };

        for(int i=0; i<cut.Length; i++)
        {
            if (score >= cut[i])
                return (score / div[i]) + suffix[i];
        }

        return "";
    }

    public async Task<List<string>> NearPeople() //친구추가용 친구가 아닌 주변 사람 리스트 만들기.
    {
        List<string> nearUsers = new List<string>(); // 주변 유저들 전체 - 친구인 유저들 = 친구가 아닌 주변 사람 리스트

        if (gpsBool)
        {
            nearUsers = await DBManager.Instance.GetNearUsers(new Location(Input.location.lastData, users[AuthManager.Instance.CurrentUserId].lastOnline));

            foreach (string friend in users[AuthManager.Instance.CurrentUserId].friends)
                if (nearUsers.Contains(friend))
                    nearUsers.Remove(friend);
        }

        return nearUsers;
    }

    public async Task<List<string>> NearFriends() //손잡기용 친구 리스트 만들기.
    {
        List<string> realnearUsers = new List<string>(); // 주변 유저들 전체
        List<string> nearFriends = new List<string>(); // 친구 유저들 전체 - 친구가 아닌 주변 유저들 = 친구 리스트

        if (gpsBool)
        {
            realnearUsers = await DBManager.Instance.GetNearUsers(new Location(Input.location.lastData, users[AuthManager.Instance.CurrentUserId].lastOnline));

            foreach (string user in realnearUsers)
                if (users[AuthManager.Instance.CurrentUserId].friends.Contains(user))
                    nearFriends.Add(user);
        }

        return nearFriends;
    }
    public async void StartFriendRequset(string id)
    {
        string str = (string)(await DBManager.Instance.GetUserValue(id, "friendRequest"));
        if (str != AuthManager.Instance.CurrentUserId)
        {
            DBManager.Instance.SetUserValue("friendRequest", id);
            requestIO = true;
        }
        else
        {
            DBManager.Instance.SetOtherUserValue(id,"friendRequest", "");
            requestIO = false;
        }
        friendRequesting = true;
        handRequesting = false;
        requestVS = id;
    }
    public async void StartHandRequest(string id)
    {
        string str = (string)(await DBManager.Instance.GetUserValue(id, "handRequest"));
        
        Debug.Log("손잡기로그 : " + str + " $$ " + id);
        if (str != AuthManager.Instance.CurrentUserId)
        {
            DBManager.Instance.SetUserValue("handRequest", id);
            requestIO = true;
        }
        else
        {
            handPreSave = users[id].score;
            DBManager.Instance.SetOtherUserValue(id, "handRequest", "");
            requestIO = false;
        }
        friendRequesting = false;
        handRequesting = true;
        requestVS = id;
    }
    public void RequestCancel()
    {
        friendRequesting = false;
        handRequesting = false;
        DBManager.Instance.SetUserValue("handRequest", "");
        DBManager.Instance.SetUserValue("friendRequest", "");
    }
    public void AddFriend(string id)
    {
        users[AuthManager.Instance.CurrentUserId].friends.Add(id);
        DBManager.Instance.SetUserValue("friends", users[AuthManager.Instance.CurrentUserId].friends);
        GoldManager.Instance.EarnMoney(300);
        UIControl.Instance.AddFriendSuccessful(users[id].name);
    }
    public void GrabHand(string id)
    {
        float AverageDistance = (users[AuthManager.Instance.CurrentUserId].score + handPreSave) / 2;
        int golde = 0;

        if (AverageDistance > users[AuthManager.Instance.CurrentUserId].score)
        {
            golde = (int)((AverageDistance - users[AuthManager.Instance.CurrentUserId].score) * HandGoldRate * 0.1f);
            GoldManager.Instance.EarnMoney(golde);
        }
        else
        {
            golde = (int)((users[AuthManager.Instance.CurrentUserId].score - AverageDistance) * HandGoldRate);
            GoldManager.Instance.EarnMoney(golde);
        }

        users[AuthManager.Instance.CurrentUserId].score = AverageDistance;
        DBManager.Instance.SetUserValue("score", users[AuthManager.Instance.CurrentUserId].score);
        UIControl.Instance.GrabHandSuccessful(users[id].name, (int)AverageDistance, golde);
    }
}
