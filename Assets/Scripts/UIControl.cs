using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject[] SuccessPanel = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i=0; i<2; i++)
            {
                if (SuccessPanel[i].gameObject.activeSelf)
                {
                    SuccessPanel[i].SetActive(false);
                }
            }
        }
    }
    public void OnOff(GameObject OtherPanel)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (OtherPanel.gameObject.activeSelf)
            OtherPanel.gameObject.SetActive(false);
    }
}
