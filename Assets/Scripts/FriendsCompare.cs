using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsCompare : MonoBehaviour
{
    List<Friends> friends = new List<Friends>();
    List<string> strfriends = new List<string>();

    float time = 0;

    /*
    async void Start()
    {
        strfriends = RunManager.Instance.users[AuthManager.Instance.CurrentUserId].friends;
        
    }

    private void Start()
    {
        MakeFriend();
    }

    void MakeFriend()
    {
        Friends A = new Friends("21", 1000);

        strfriends.Add("21");
        friends.Add(A);
    }

    void Checkout()
    {
        Friends F1 = friends[0];
        float F1Meter = F1.Meter;
        float ff = RunManager.Instance.Meter - F1Meter;

        if ((int)(Mathf.Abs(ff)) <= RunManager.Instance.FriendViewDist)
            Debug.Log("친구 " + F1.ID + "이 일정 거리 안에 있습니다.");
    }
    */

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
