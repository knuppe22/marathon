using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PossessItem : MonoBehaviour
{
    Dictionary<string, Item> itemlist = new Dictionary<string, Item>();
    string[] Itemname = { "Stone", "Pine", "Maple", "Ginkgo", "Mashmellow", "Asphalt", "Tuxedo", "Red", "Blue", "Green", "Purple", "Black" };
    List<Item> PossItem = new List<Item>();

    public void Awake() //Start보다 먼저 실행.
    {
        itemlist.Add("Stone", new Item("Stone", 500, Item.Property.Background, 0, 3, 1, 0, 0, 0));
        itemlist.Add("Pine", new Item("Pine", 1000, Item.Property.Background, 0, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Maple", new Item("Maple", 1000, Item.Property.Background, 0, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Ginkgo", new Item("Ginkgo", 1000, Item.Property.Background, 0, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Mashmellow", new Item("Mashmellow", 600, Item.Property.Background, 0, 3, 0.3f, 0, 0, 0));
        itemlist.Add("Asphalt", new Item("Asphalt", 4000, Item.Property.Road, 0, 1, 0, 500, 0, 0));
        itemlist.Add("Tuxedo", new Item("Tuxedo", 4000, Item.Property.Cloth, 0, 1, 3, 0, 1, 0));
        itemlist.Add("Red", new Item("Red", 100, Item.Property.Cloth, 0, 1, 0.1f, 0, 0, 0));
        itemlist.Add("Blue", new Item("Blue", 100, Item.Property.Cloth, 0, 1, 0.1f, 0, 0, 0));
        itemlist.Add("Green", new Item("Green", 100, Item.Property.Cloth, 0, 1, 0.1f, 0, 0, 0));
        itemlist.Add("Purple", new Item("Purple", 200, Item.Property.Cloth, 0, 1, 0.1f, 50, 0, 0));
        itemlist.Add("Black", new Item("Black", 200, Item.Property.Cloth, 0, 1, 0.1f, 0, 0.2f, 0));
    }

    void Start()
    {
        string _Filestr = "Assets/Resources/ItemData.txt";
        System.IO.FileInfo fi = new
            System.IO.FileInfo(_Filestr);

        if (fi.Exists)
        {
            Parse();
        }
    }

    string m_strPath = "Assets/Resources/";

    public void CreateData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "ItemData.txt", FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void AppendData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "ItemData.txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        writer.Close();
    }

    public void Parse()
    {
        TextAsset data = Resources.Load("Data", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        for (int i = 0; i < Itemname.Length; i++)
        {
            if (!itemlist.ContainsKey(Itemname[i]))
            {
                itemlist.TryGetValue(Itemname[i], out Item item);
                PossItem.Add(item);
            }
        }

        sr.Close();
    }

    private void OnApplicationQuit()
    {
        if (PossItem.Count != 0)
        {
            Item save = PossItem[0];
            CreateData(save.Name);
            if (PossItem.Count != 1)
            {
                for (int i = 1; i < PossItem.Count; i++)
                {
                    save = PossItem[i];
                    AppendData(save.Name);
                }
            }
        }
    }
}
