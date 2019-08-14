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

        //AuthManager, DBManager만 있는 씬을 따로 만들어서

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

        /*string _Filestr = "Assets/Resources/Data.txt";
        FileInfo fi = new FileInfo(_Filestr);

        if (fi.Exists)
        {
            Parse();
            MeterText.text = IntMeter.ToString();
        }

        else
        {
            MeterText.text = "0";
        }*/
    }


    async void Update()
    {
        time += Time.deltaTime;

        /*if(time > 10)
        {
            User me = await DBManager.Instance.GetUser();
            users[AuthManager.Instance.CurrentUserId] = me;

            time = 0;
        }*/

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
        users[AuthManager.Instance.CurrentUserId].UpdateLastOnline();
        DBManager.Instance.SetUser(users[AuthManager.Instance.CurrentUserId]);

        /*CreateData(Meter.ToString());
        AppendData(RunSpeed.ToString());*/
    }


    /*string m_strPath = "Assets/Resources/";

    public void CreateData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void AppendData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void Parse()
    {
        TextAsset data = Resources.Load("Data", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        Meter = float.Parse(sr.ReadLine());
        IntMeter = (int)Meter;
        RunSpeed = float.Parse(sr.ReadLine());

        sr.Close();
    }*/
}
