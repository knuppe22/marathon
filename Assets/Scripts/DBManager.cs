using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DBManager : SingletonBehaviour<DBManager>
{
    public DatabaseReference RootReference { get; private set; }
    public DatabaseReference UserReference
    {
        get { return RootReference.Child("users"); }
    }
    public DatabaseReference CurrentUserReference
    {
        get { return UserReference.Child(AuthManager.Instance.CurrentUserId); }
    }

    public void SetDatabase()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://kucatdog-marathon.firebaseio.com/");

        RootReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // TODO: 여러가지 DB 정보 get, set 하는 함수 만들기
    public async Task<User> GetUser()
    {
        return await GetUser(AuthManager.Instance.CurrentUserId);
    }
    public async Task<User> GetUser(string userId)
    {
        Task<DataSnapshot> task = UserReference.Child(userId).GetValueAsync();
        await task;

        if (task.IsFaulted)
        {
            Debug.LogErrorFormat("GetUser({0}) failed", userId);
        }
        else if (task.IsCompleted)
        {
            if (task.Result.Exists)
            {
                Debug.LogFormat("GetUser({0}) succeeded", userId);
                return new User(task.Result.GetRawJsonValue());
            }
            else
            {
                Debug.LogFormat("GetUser({0}) succeeded but no data", userId);
                return new User();
            }
        }

        return null;
    }

    public void SetUser(User userData)
    {
        Debug.Log("GOLD : " + userData.gold);
        CurrentUserReference.SetRawJsonValueAsync(JsonUtility.ToJson(userData));
    }
    
    public void SetUserValue(string key, object value)
    {
        // key에 들어갈 수 있는 것들: "name", "score", "gold", "online", "lastOnline", "friends", "items"
        CurrentUserReference.Child(key).SetValueAsync(value);
    }
}
