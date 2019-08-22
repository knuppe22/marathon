using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;

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
    public async Task SetUserAsync(User userData)
    {
        await CurrentUserReference.SetRawJsonValueAsync(JsonUtility.ToJson(userData));
    }
    
    public void SetUserValue(string key, object value)
    {
        // key에 들어갈 수 있는 것들: "name", "score", "gold", "online", "lastOnline", "friends", "items", "equippedItems"
        // 위치정보 불가!!!
        Debug.Log("ValueSet " + key);
        CurrentUserReference.Child(key).SetValueAsync(value);
    }

    public void SetLocation(Location location)
    {
        CurrentLocationReference.SetRawJsonValueAsync(JsonUtility.ToJson(location));
    }

    public async Task<Dictionary<string, Location>> GetLocations()
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
                Debug.Log("{\"locations\":" + task.Result.GetRawJsonValue() + "}");
                return JsonConvert.DeserializeObject<Dictionary<string, Location>>(task.Result.GetRawJsonValue());
            }
            else
            {
                Debug.LogFormat("GetLocation() succeeded but no data");
                return new Dictionary<string, Location>();
            }
        }

        return null;
    }

    public async Task<List<string>> GetNearUsers(Location location)
    {
        Dictionary<string, Location> locations = await GetLocations();
        
        List<string> nearUsers = new List<string>();

        if (locations == null) return null;

        foreach(KeyValuePair<string, Location> pair in locations)
        {
            if (pair.Key == AuthManager.Instance.CurrentUserId) continue;
            if (Location.Distance(location, pair.Value) < 50)
            {
                DateTime last = DateTime.Parse(pair.Value.lastOnline);
                TimeSpan span = DateTime.Now.Subtract(last);

                if (span.TotalSeconds < 60)
                {
                    User tmpUser = await GetUser(pair.Key);
                    if (!RunManager.Instance.users.ContainsKey(pair.Key)) RunManager.Instance.users.Add(pair.Key, tmpUser);
                    else RunManager.Instance.users[pair.Key] = tmpUser;
                    nearUsers.Add(pair.Key);
                }
            }
        }

        return nearUsers;
    }
}
