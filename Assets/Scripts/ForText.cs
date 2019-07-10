using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ForText : MonoBehaviour
{
    [SerializeField]
    private Text speedtext;
    float speed;
    float textspeed = 0;
    int Intspeed;
    int Inttextspeed;

    // Start is called before the first frame update
    void Start()
    {
        string _Filestr = "Assets/Resources/Data.txt";
        System.IO.FileInfo fi = new
            System.IO.FileInfo(_Filestr);

        if (fi.Exists)
        {
            Parse();
            speed = textspeed;
            Intspeed = (int)textspeed;
            this.speedtext.text = Inttextspeed.ToString();
        }

        else
        {
            speedtext.text = "0";
            speed = 0;
        }
    }

    string m_strPath = "Assets/Resources/";

    public void WriteData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void Parse()
    {
        TextAsset data = Resources.Load("Data", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        textspeed = float.Parse(sr.ReadLine());

        sr.Close();
    }

    // Update is called once per frame
    void Update()
    {
        speed = speed + 5 * Time.deltaTime;
        Intspeed = (int)speed;
        this.speedtext.text = Intspeed.ToString();
    }

    private void OnApplicationQuit()
    {
        WriteData(speed.ToString());
    }
}
