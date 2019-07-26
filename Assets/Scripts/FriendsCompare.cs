using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsCompare : MonoBehaviour
{
    List<Friends> friends = new List<Friends>();

    float time = 0;

    void MakeFriend()
    {
        Friends A = new Friends(21, 1000);
        friends.Add(A);
    }

    void Checkout()
    {
        Friends F1 = friends[0];
        float F1Meter = F1.Meter;
        float ff = ForText.Instance.Meter - F1Meter;

        if ((int)(Mathf.Abs(ff)) <= ForText.Instance.FriendViewDist)
            Debug.Log("친구 " + F1.ID + "이 일정 거리 안에 있습니다.");
    }

    void Start()
    {
        MakeFriend();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 2)
        {
            Checkout();
            time = 0;
        }
    }
}
