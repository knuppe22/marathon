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

    public void SetDatabase()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://kucatdog-marathon.firebaseio.com/");

        RootReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // TODO: 여러가지 DB 정보 get, set 하는 함수 만들기
}
