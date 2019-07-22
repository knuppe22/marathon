using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ForText : MonoBehaviour
{
    [SerializeField]
    private Text MeterText;
    int IntMeter;
    public static float Meter;
    public static float speed = 5;


    // Start is called before the first frame update
    void Start()
    {
        string _Filestr = "Assets/Resources/Data.txt";
        System.IO.FileInfo fi = new
            System.IO.FileInfo(_Filestr);

        if (fi.Exists)
        { 
            Parse();
            MeterText.text = IntMeter.ToString();
        }

        else
        {
            MeterText.text = "0";
        }
    }

    string m_strPath = "Assets/Resources/";

    public void CreateData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void AppendData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Append, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void Parse()
    {
        TextAsset data = Resources.Load("Data", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        Meter = float.Parse(sr.ReadLine());
        IntMeter = (int)Meter;
        speed = float.Parse(sr.ReadLine());

        sr.Close();
    }

    // Update is called once per frame
    void Update()
    {

        Meter = Meter + speed * Time.deltaTime;
        IntMeter = (int)Meter;
        MeterText.text = IntMeter.ToString();
    }

    private void OnApplicationQuit()
    {
        CreateData(Meter.ToString());
        AppendData(speed.ToString());

    }
}
