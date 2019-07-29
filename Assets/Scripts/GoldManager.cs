using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GoldManager : SingletonBehaviour<GoldManager>
{
    [SerializeField]
    private Text Gold;
    public int gold;
    string m_strPath = "Assets/Resources/";


    // Start is called before the first frame update
    void Start()
    {
        FileInfo fi = new FileInfo(m_strPath + "goldData.txt");

        if (fi.Exists)
        {
            Parse();
            Gold.text = gold.ToString();
        }

        else
        {
            gold = 0;
            Gold.text = gold.ToString();
        }
    }

    public void CreateData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "goldData.txt", FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void Parse()
    {
        TextAsset data = Resources.Load("goldData", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        gold = int.Parse(sr.ReadLine());
        sr.Close();
    }

    public bool UseMoney(int price)
    {
        if (gold >= price)
        {
            gold -= price;
            return true;
        }

        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        CreateData(gold.ToString());
    }
}
