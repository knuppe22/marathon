using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthScene : MonoBehaviour
{
    void Awake()
    {
        AuthManager.Instance.onSignIn += LoadSomeScene;
    }

    void OnDestroy()
    {
        AuthManager.Instance.onSignIn -= LoadSomeScene;
    }

    void LoadSomeScene()
    {
        SceneManager.LoadScene(1);
    }
}
