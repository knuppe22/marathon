using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public string name;
    public float score = 0;
    public int gold = 0;
    public string lastOnline; // "yyyy-MM-ddTHH:mm:ss" 형식으로 사용
    public List<string> friends;
    public List<string> items;
    public List<string> equippedItems;
    public string checkedTime;
    public string friendRequest;
    public string handRequest;

    public User()
    {
        UpdateLastOnline();
        friends = new List<string>();
        items = new List<string>();
    }
    public User(string json) : this()
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }

    public void UpdateLastOnline()
    {
        lastOnline = DateTime.Now.ToString("s");
    }
}
