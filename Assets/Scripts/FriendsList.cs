using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsList : MonoBehaviour
{
    class Friend
    {
        public int ID;
        public float Meter;

        public Friend(int ID, int Meter)
        {
            this.ID = ID;
            this.Meter = Meter;
        }
    }

    List<Friend> friends = new List<Friend>();

    void MakeFriend()
    {
        Friend A = new Friend(21, 1000);
        friends.Add(A);
    }
    float time = 0;

    void Checkout()
    {
        ForText M = GameObject.Find("Meter").AddComponent<ForText>();

        Friend F1 = friends[0];
        float F1Meter = F1.Meter;
        float ff = ForText.Meter - F1Meter;

        if (Mathf.Abs(ff) <= 10000)
            Debug.Log("친구 " + F1.ID + "이 일정 거리 안에 있습니다.");
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeFriend();
    }

    // Update is called once per frame
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
