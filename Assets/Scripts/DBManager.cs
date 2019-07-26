using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DBManager : SingletonBehaviour<DBManager>
{
    public DatabaseReference RootReference { get; private set; }
    public DatabaseReference UserReference
    {
        get { return RootReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId); }
    }

    public Dictionary<string, User> userInDatabase;

    public void SetDatabase()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://kucatdog-marathon.firebaseio.com/");

        RootReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // DB에 본인이 없을 경우 새로 생성
    public void InitializeUser()
    {
        UserReference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (!task.Result.HasChildren)
                {
                    User.currentUser = new User();
                }
                else
                {
                    GetUser();
                    User.currentUser = userInDatabase[FirebaseAuth.DefaultInstance.CurrentUser.UserId];
                }
            }
        });
    }

    // TODO: 여러가지 DB 정보 get, set 하는 함수 만들기
    public void GetUser()
    {
        GetUser(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
    }
    public void GetUser(string userId)
    {
        if (userInDatabase[userId] != null)
        {
            userInDatabase.Remove(userId);
        }

        RootReference.Child("users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                userInDatabase[userId] = new User(task.Result); 
            }
        });
    }
}
