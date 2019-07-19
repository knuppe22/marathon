using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldButton : MonoBehaviour
{
    [SerializeField]
    private Text Gold;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Click()
    {
        GoldScript G = GameObject.Find("Gold").GetComponent<GoldScript>();
        GoldScript.gold += 100;
        Gold.text = GoldScript.gold.ToString();
    }

    public void Hundred()
    {
        int P = 100;

        if (GameObject.Find("Gold").GetComponent<GoldScript>().usemoney(P) == true)
        {
            GoldScript.gold -= P;
            Gold.text = GoldScript.gold.ToString();
        }

        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void Twohundred()
    {
        int P = 200;

        if (GameObject.Find("Gold").GetComponent<GoldScript>().usemoney(P) == true)
        {
            GoldScript.gold -= P;
            Gold.text = GoldScript.gold.ToString();
        }

        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
