using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseTest : MonoBehaviour
{
    bool a = false;
    bool b = false;
    User user;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    async void Update()
    {
        if (DBManager.Instance.RootReference == null)
            return;
        if (!a)
        {
            a = true;
            user = await DBManager.Instance.GetUser();
        }
        else if (user != null && !b)
        {
            b = true;
            Debug.Log(user.name);
            user.UpdateLastOnline();
            DBManager.Instance.SetUser(user);

            user.score = 15000;
            DBManager.Instance.SetUserValue("score", user.score);
        }
    }
}
