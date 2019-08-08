using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item // : ScriptableObject
{
    public enum Property { Cloth, Background, Road }; // Enumerators.cs 를 만들어서 이걸 넣으면 어디에서든 가능.
    
    public string Name;
    public int Price;
    public Property property;
    public int Maximum;
    public int PresPoss;
    public float PlusRSpeed;
    public int PlusFView;
    public float PlusHGold;
    public float PlusCGold;
    public int Equipment;

    //ClothQ, BackGroundQ, RoadQ를 따로 만들어서 size를 정하기. 만약 Maximum이면 pop 및 --, 새로넣은거 ++

     public Item(string Name, int Price, Property property, int Maximum, float PlusRSpeed, int PlusFView, float PlusHGold, float PlusCGold)
    {
        this.Name = Name;
        this.Price = Price;
        this.property = property;
        this.PresPoss = 0;
        this.Maximum = Maximum;
        this.PlusRSpeed = PlusRSpeed;
        this.PlusFView = PlusFView;
        this.PlusHGold = PlusHGold;
        this.PlusCGold = PlusCGold;
        this.Equipment = 0;
    }
}
