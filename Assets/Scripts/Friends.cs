using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends
{
    public string ID;
    public float Meter;
    public List<Item> Items;

    public Friends(string ID, float Meter)
    {
        this.ID = ID;
        this.Meter = Meter;
    }
}
