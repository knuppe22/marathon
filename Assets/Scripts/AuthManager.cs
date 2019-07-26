using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class AuthManager : SingletonBehaviour<AuthManager>
{
    async void Awake()
    {
        DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == DependencyStatus.Available)
        {
            await SignIn();

            DBManager.Instance.SetDatabase();
        }
        else
        {
            Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
        }
    }

    async Task SignIn()
    {
#if UNITY_EDITOR
        Task<FirebaseUser> task = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync("kucatdog@gmail.com", "1234567890");
#else
        Task<FirebaseUser> task = FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync();
#endif
        await task;

        if (task.IsCanceled)
        {
            Debug.LogError("SignIn was canceled.");
            return;
        }
        if (task.IsFaulted)
        {
            Debug.LogError("SignIn encountered an error: " + task.Exception);
            return;
        }

        FirebaseUser newUser = task.Result;
        Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
    }
}
