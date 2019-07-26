using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum Property { Things, Cloth, Background, Road };

    public string Name;
    public int Price;
    public Property property;
    public int Maximum;
    public int PresPoss;
    public float PlusRSpeed;
    public int PlusFView;
    public float PlusHGold;
    public float PlusCGold;

    public Item(string Name, int Price, Property property, int PresPoss, int Maximum, float PlusRSpeed, int PlusFView, float PlusHGold, float PlusCGold)
    {
        this.Name = Name;
        this.Price = Price;
        this.property = property;
        this.PresPoss = PresPoss;
        this.Maximum = Maximum;
        this.PlusRSpeed = PlusRSpeed;
        this.PlusFView = PlusFView;
        this.PlusHGold = PlusHGold;
        this.PlusCGold = PlusCGold;
    }
}
