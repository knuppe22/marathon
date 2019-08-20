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
    public DatabaseReference LocationReference
    {
        get { return RootReference.Child("locations"); }
    }
    public DatabaseReference CurrentLocationReference
    {
        get { return LocationReference.Child(AuthManager.Instance.CurrentUserId); }
    }

    public void SetDatabase()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://kucatdog-marathon.firebaseio.com/");

        RootReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
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
        // key에 들어갈 수 있는 것들: "name", "score", "gold", "online", "lastOnline", "friends", "items", "equippedItems"
        // 위치정보 불가!!!
        Debug.Log("ValueSet " + key);
        CurrentUserReference.Child(key).SetValueAsync(value);
    }

    public void SetLocation(float latitude, float longitude)
    {
        CurrentLocationReference.Child("latitude").SetValueAsync(latitude);
        CurrentLocationReference.Child("longitude").SetValueAsync(longitude);
    }

    public async Task<Dictionary<string, GeoCoord>> GetLocations()
    {
        Task<DataSnapshot> task = LocationReference.GetValueAsync();
        await task;

        if (task.IsFaulted)
        {
            Debug.LogErrorFormat("GetLocation() failed");
        }
        else if (task.IsCompleted)
        {
            if (task.Result.Exists)
            {
                Debug.LogFormat("GetLocation() succeeded");
                return JsonUtility.FromJson<Dictionary<string, GeoCoord>>(task.Result.GetRawJsonValue());
            }
            else
            {
                Debug.LogFormat("GetLocation() succeeded but no data");
                return new Dictionary<string, GeoCoord>();
            }
        }

        return null;
    }
}
