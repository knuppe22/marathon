﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsCompare : MonoBehaviour
{
    List<string> strfriends = new List<string>();

    float time = 0;

    void MakeFriend(string id)
    {
        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends.Add(id);
        strfriends = RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends;

        DBManager.Instance.SetUserValue("friends", strfriends);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 10)
        {
            foreach(string friend in RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends)
            {
                BackgroundManager.Instance.SetRunner(friend);
                BackgroundManager.Instance.SetRunnerImage(friend);
            }

            time = 0;
        }
    }
}
