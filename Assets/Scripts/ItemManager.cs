using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemManager : SingletonBehaviour<ItemManager>
{
    Dictionary<string, Item> itemlist = new Dictionary<string, Item>();
    string[] Itemname = { "Stone", "Pine", "Maple", "Ginkgo", "Mashmellow", "Asphalt", "Tuxedo", "Red", "Blue", "Green", "Purple", "Black" };
    List<Item> PossItem = new List<Item>();

    public List<string> ClothQ = new List<string>();
    public List<string> BackGroundQ = new List<string>();
    public List<string> RoadQ = new List<string>();

    string m_strPath = "Assets/Resources/";

    public void Awake() //Start보다 먼저 실행.
    {
        itemlist.Add("Stone", new Item("Stone", 500, Item.Property.Background, 3, 1, 0, 0, 0));
        itemlist.Add("Pine", new Item("Pine", 1000, Item.Property.Background, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Maple", new Item("Maple", 1000, Item.Property.Background, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Ginkgo", new Item("Ginkgo", 1000, Item.Property.Background, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Mashmellow", new Item("Mashmellow", 600, Item.Property.Background, 3, 0.3f, 0, 0, 0));
        itemlist.Add("Asphalt", new Item("Asphalt", 4000, Item.Property.Road, 1, 0, 500, 0, 0));
        itemlist.Add("Tuxedo", new Item("Tuxedo", 4000, Item.Property.Cloth, 1, 3, 0, 1, 0));
        itemlist.Add("Red", new Item("Red", 100, Item.Property.Cloth, 1, 0.1f, 0, 0, 0));
        itemlist.Add("Blue", new Item("Blue", 100, Item.Property.Cloth, 1, 0.1f, 0, 0, 0));
        itemlist.Add("Green", new Item("Green", 100, Item.Property.Cloth, 1, 0.1f, 0, 0, 0));
        itemlist.Add("Purple", new Item("Purple", 200, Item.Property.Cloth, 1, 0.1f, 50, 0, 0));
        itemlist.Add("Black", new Item("Black", 200, Item.Property.Cloth, 1, 0.1f, 0, 0.2f, 0));
    }

    void Start()
    {
        string _Filestr = "Assets/Resources/ItemData.txt";
        FileInfo fi = new FileInfo(_Filestr);

        if (fi.Exists)
        {
            Parse();
        }
    }

    public void CreateData(string strData, int presposs)
    {
        FileStream f = new FileStream(m_strPath + "ItemData.txt", FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        FileStream f2 = new FileStream(m_strPath + "ItemData.txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer2 = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer2.WriteLine(presposs.ToString());

        writer.Close();
        writer2.Close();
    }

    public void AppendData(string strData, int presposs)
    {
        FileStream f = new FileStream(m_strPath + "ItemData.txt", FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer.WriteLine(strData);

        FileStream f2 = new FileStream(m_strPath + "ItemData.txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer2 = new StreamWriter(f, System.Text.Encoding.Unicode);

        writer2.WriteLine(presposs.ToString());

        writer.Close();
        writer2.Close();
    }

    public void Parse()
    {
        TextAsset data = Resources.Load("Data", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        string ItemNameData;
        int ItemPossess;

        while ((ItemNameData = sr.ReadLine()) != null)
        {
            for (int i = 0; i < Itemname.Length; i++)
            {
                if (ItemNameData == Itemname[i])
                {
                    itemlist.TryGetValue(Itemname[i], out Item item);
                    PossItem.Add(item);

                    ItemPossess = int.Parse(sr.ReadLine());

                    itemlist[ItemNameData].PresPoss = ItemPossess;
                }
            }
        }

        sr.Close();
        ApplyItemEffect();
    }

    public void BuyItem(string name)
    {
        if(itemlist[name].PresPoss >= itemlist[name].Maximum)
        {
            Debug.Log("실패");
        }

        else
        {
            if (GoldManager.Instance.UseMoney(itemlist[name].Price))
            {
                itemlist[name].PresPoss++;
                ApplyItemEffect();
            }

            else
            {
                Debug.Log("돈 부족");
            }
        }
    }

    public void ApplyItemEffect()
    {
        RunManager.Instance.RunSpeed = 5;
        RunManager.Instance.FriendViewDist = 10000;
        RunManager.Instance.CheckGoldRate = 1;
        RunManager.Instance.HandGoldRate = 1;

        foreach (KeyValuePair<string, Item> pair in itemlist)
        {
            RunManager.Instance.RunSpeed += pair.Value.PlusRSpeed * pair.Value.PresPoss;
            RunManager.Instance.FriendViewDist += pair.Value.PlusFView * pair.Value.PresPoss;
            RunManager.Instance.CheckGoldRate += pair.Value.PlusCGold * pair.Value.PresPoss;
            RunManager.Instance.HandGoldRate += pair.Value.PlusHGold * pair.Value.PresPoss;
        }
    }

    public void EquipmentItem(string item)
    {
        switch (itemlist[item].property)
        {
            case Item.Property.Cloth:
                if (ClothQ.Count == 0)
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        ClothQ.Add(item);
                        itemlist[item].Equipment++;
                    }
                }

                else if (ClothQ.Count == 1)
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        string RemoveItem = ClothQ[0];
                        itemlist[RemoveItem].Equipment--;
                        ClothQ.RemoveAt(0);
                        ClothQ.Add(item);
                        itemlist[item].Equipment++;
                    }
                }

                else
                {
                    Debug.Log("오류입니다.");
                }
                break;

            case Item.Property.Road:
                if (RoadQ.Count == 0)
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        RoadQ.Add(item);
                        itemlist[item].Equipment++;
                    }
                }

                else if (RoadQ.Count == 1)
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        string RemoveItem = RoadQ[0];
                        itemlist[RemoveItem].Equipment--;
                        RoadQ.RemoveAt(0);
                        RoadQ.Add(item);
                        itemlist[item].Equipment++;
                    }
                }

                else
                {
                    Debug.Log("오류입니다.");
                }
                break;

            case Item.Property.Background:
                if(BackGroundQ.Count == 0)
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        BackGroundQ.Add(item);
                        itemlist[item].Equipment++;
                    }
                }

                else
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        BackGroundQ.Add(item);
                        itemlist[item].Equipment++;
                    }

                    else
                        Debug.Log("더이상 장착할 수 없습니다.");
                }
                break;
        }
    }

    private void OnApplicationQuit()
    {
        if (PossItem.Count != 0)
        {
            Item save = PossItem[0];
            CreateData(save.Name, save.PresPoss);
            if (PossItem.Count != 1)
            {
                for (int i = 1; i < PossItem.Count; i++)
                {
                    save = PossItem[i];
                    AppendData(save.Name, save.PresPoss);
                }
            }
        }
    }
}
