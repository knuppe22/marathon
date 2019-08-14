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

    async void Start()
    {
        User me = await DBManager.Instance.GetUser();
        users.Add(AuthManager.Instance.CurrentUserId, me);

        //AuthManager, DBManager만 있는 씬을 따로 만들어서 씬을 변경?
        //await 두 개가 동시에 되는가.

        Meter = users[AuthManager.Instance.CurrentUserId].score + RunSpeed * CalcOnline();
        MeterText.text = ((int)Meter).ToString();

        GoldManager.Instance.gold = users[AuthManager.Instance.CurrentUserId].gold;
        GoldManager.Instance.Gold.text = GoldManager.Instance.gold.ToString();

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
        
        //DBManager.Instance.SetUser(users[AuthManager.Instance.CurrentUserId]); 자기꺼 저장.
    }


    void Update()
    {
        time += Time.deltaTime;

        CheckPointEvent();

        if(time > 10)
        {
            //DBManager.Instance.SetUser(users[AuthManager.Instance.CurrentUserId]);
            time = 0;
        }

        Meter += RunSpeed * Time.deltaTime;
        MeterText.text = ((int)Meter).ToString();
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
        UpdateUserData();
    }

    void UpdateUserData()
    {
        int gold = GoldManager.Instance.gold;
        float score = Meter;
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();

        users[AuthManager.Instance.CurrentUserId].gold = gold;
        users[AuthManager.Instance.CurrentUserId].score = score;

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

            GoldManager.Instance.gold += (int)gainGold;


        }

        //checkpointevent는 정각부터 ~ 정각 59초까지.
        //만약 재형선배님이 저장할 string을 만들어주신다면 저장되어있는 시간과 비교하여 실행.
    }
}
