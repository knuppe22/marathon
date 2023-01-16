using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AuthScene : MonoBehaviour
{
    public User curUser;
    public GameObject namingUI;
    public Text inputText;

    void Awake()
    {
        curUser = null;

        AuthManager.Instance.onAuthComplete += CheckUserName;
    }

    async void CheckUserName()
    {
        curUser = await DBManager.Instance.GetUser();

        if (curUser.name == null || curUser.name.Equals(""))
        {
            ActivateNamingUI();
        }
        else
        {
            SceneManager.LoadScene("bgTest");
        }
    }

    public void ActivateNamingUI()
    {
        namingUI.SetActive(true);
    }

    public async void SetName()
    {
        if (!inputText.text.Equals(""))
        {
            curUser.name = inputText.text;
            await DBManager.Instance.SetUserAsync(curUser);

            SceneManager.LoadScene("bgTest");
        }
    }
}
