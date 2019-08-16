using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class RunManager : SingletonBehaviour<RunManager>
{
    [SerializeField]
    private Text MeterText;

    public Dictionary<string, User> users = new Dictionary<string, User>();

    public float Meter = 0;
    public float RunSpeed = 5;
    public int FriendViewDist = 10000;
    public float HandGoldRate = 1;
    public float CheckGoldRate = 1;
    float time = 0;

    bool dbBoolA, dbBoolB;
    User tmpUser;


    async void Update()
    {
        if (DBManager.Instance.RootReference == null)
            return;
        if (!dbBoolA)
        {
            dbBoolA = true;
            tmpUser = await DBManager.Instance.GetUser();
        }
        else if (tmpUser != null)
        {
            if(!dbBoolB)
            {
                //이전 start
                dbBoolB = true;
                users.Add(AuthManager.Instance.CurrentUserId, tmpUser);

                //AuthManager, DBManager만 있는 씬을 따로 만들어서 씬을 변경?
                //await 두 개가 동시에 되는가.

                Meter = users[AuthManager.Instance.CurrentUserId].score + RunSpeed * CalcOnline();
                MeterText.text = ((int)Meter).ToString();

                GoldManager.Instance.SetGold(users[AuthManager.Instance.CurrentUserId].gold);
                GoldManager.Instance.Gold.text = GoldManager.Instance.GetGold().ToString();

                ItemManager.Instance.PossItem = users[AuthManager.Instance.CurrentUserId].items;

                foreach (string item in ItemManager.Instance.PossItem)
                {

                    ItemManager.Instance.itemlist[item].PresPoss++;
                }

                ItemManager.Instance.ApplyItemEffect();

                ItemManager.Instance.AllQ = users[AuthManager.Instance.CurrentUserId].equippedItems;

                foreach (string item in ItemManager.Instance.AllQ)
                {

                    ItemManager.Instance.itemlist[item].Equipment++;
                }
                BackgroundManager.Instance.SetBackgroundImage();
                BackgroundManager.Instance.SetRoadImage();
                BackgroundManager.Instance.SetRunnerImage();
            }
            else
            {
                //이전 update
                time += Time.deltaTime;

                //CheckPointEvent();

                if (time > 10)
                {
                    UpdateUserData();
                    time = 0;
                }

                Meter += RunSpeed * Time.deltaTime;
                MeterText.text = ((int)Meter).ToString();
            }
        }
    }

    float CalcOnline()
    {
        string lastOnline = users[AuthManager.Instance.CurrentUserId].lastOnline;
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();

        DateTime last = DateTime.Parse(lastOnline);
        DateTime present = DateTime.Parse(users[AuthManager.Instance.CurrentUserId].lastOnline);

        TimeSpan span = present.Subtract(last);
        
        if (span.TotalSeconds >= 300) return 300;

        else return (float)span.TotalSeconds;
    }

    private void OnApplicationQuit()
    {

    }

    void UpdateUserData()
    {
        int gold = GoldManager.Instance.GetGold();
        float score = Meter;
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();

        users[AuthManager.Instance.CurrentUserId].gold = gold;
        users[AuthManager.Instance.CurrentUserId].score = score;

        if (DBManager.Instance.RootReference != null)
            DBManager.Instance.SetUser(users[AuthManager.Instance.CurrentUserId]);
    }

    void CheckPointEvent()
    {
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();
        string onlineTime = users[AuthManager.Instance.CurrentUserId].lastOnline;

        DateTime time = DateTime.Parse(onlineTime);

        if(time.Minute == 0)
        {
            int friendView = 0;
            float gainGold = 100 * friendView * CheckGoldRate;

            GoldManager.Instance.EarnMoney((int)gainGold);


        }

        //checkpointevent는 정각부터 ~ 정각 59초까지.
        //만약 재형선배님이 저장할 string을 만들어주신다면 저장되어있는 시간과 비교하여 실행.
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
}
