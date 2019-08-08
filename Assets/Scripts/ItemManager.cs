using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonBehaviour<ItemManager>
{
    public Dictionary<string, Item> itemlist = new Dictionary<string, Item>();
    //public List<Item> itemList2;
    public List<string> PossItem = new List<string>();

    public List<string> ClothQ = new List<string>();
    public List<string> BackGroundQ = new List<string>();
    public List<string> RoadQ = new List<string>();

    public List<string> AllQ = new List<string>();

    public void Awake() //Start보다 먼저 실행.
    {
        itemlist.Add("Stone", new Item("Stone", 500, Item.Property.Background, 3, 1, 0, 0, 0));
        itemlist.Add("Pine", new Item("Pine", 1000, Item.Property.Background, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Maple", new Item("Maple", 1000, Item.Property.Background, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Ginkgo", new Item("Ginkgo", 1000, Item.Property.Background, 3, 1.5f, 0, 0, 0));
        itemlist.Add("Mashmellow", new Item("Mashmellow", 600, Item.Property.Background, 3, 0, 0, 0, 0.3f));
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
        /*
        PossItem = RunManager.Instance.users[AuthManager.Instance.CurrentUserId].items;

        foreach (string item in PossItem)
        {
            itemlist[item].PresPoss++;
        }

        ApplyItemEffect();

        AllQ = RunManager.Instance.users[AuthManager.Instance.CurrentUserId].equippedItems;
        
        foreach(string item in AllQ)
        {
            itemlist[item].Equipment++;
        }
        */
    }

    public void BuyItem(string name)
    {
        if(itemlist[name].PresPoss >= itemlist[name].Maximum)
        {
            Debug.Log("더 이상 소지할 수 없습니다.");
        }

        else
        {
            if (GoldManager.Instance.UseMoney(itemlist[name].Price))
            {
                itemlist[name].PresPoss++;
                ApplyItemEffect();

                PossItem.Add(name);

                DBManager.Instance.SetUserValue("items", PossItem);
            }

            else
            {
                Debug.Log("골드가 부족합니다.");
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
                        AllQ.Add(item);

                        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].equippedItems = AllQ;
                    }
                }

                else if (ClothQ.Count == 1)
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        string RemoveItem = ClothQ[0];
                        itemlist[RemoveItem].Equipment--;
                        ClothQ.RemoveAt(0);
                        AllQ.Remove(RemoveItem);

                        ClothQ.Add(item);
                        itemlist[item].Equipment++;
                        AllQ.Add(item);

                        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].equippedItems = AllQ;
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
                        AllQ.Add(item);

                        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].equippedItems = AllQ;
                    }
                }

                else if (RoadQ.Count == 1)
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        string RemoveItem = RoadQ[0];
                        itemlist[RemoveItem].Equipment--;
                        RoadQ.RemoveAt(0);
                        AllQ.Remove(RemoveItem);

                        RoadQ.Add(item);
                        itemlist[item].Equipment++;
                        AllQ.Add(item);

                        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].equippedItems = AllQ;
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
                        AllQ.Add(item);

                        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].equippedItems = AllQ;
                    }
                }

                else
                {
                    if (itemlist[item].Equipment < itemlist[item].PresPoss)
                    {
                        BackGroundQ.Add(item);
                        itemlist[item].Equipment++;
                        AllQ.Add(item);

                        RunManager.Instance.users[AuthManager.Instance.CurrentUserId].equippedItems = AllQ;
                    }

                    else
                        Debug.Log("더이상 장착할 수 없습니다.");
                }
                break;
        }
    }
}
