using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class User
{
    public static User currentUser;

    public string name;
    public float score;
    public int gold;
    public bool online;
    public DateTime lastOnline;
    public string[] friends;
    public string[] items;

    // TODO: User 생성자 만들기
    public User()
    {

    }
    public User(DataSnapshot snapshot)
    {

    }

    // yyyy-MM-ddTHH:mm:ss 형식으로 사용
    public void SetLastOnline(string dateAsString)
    {
        lastOnline = DateTime.Parse(dateAsString);
    }
    public string GetLastOnline()
    {
        return string.Format("{0:s}", lastOnline);
    }
}
