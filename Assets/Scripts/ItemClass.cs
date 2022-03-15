using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gears,
    Clothes,
    Boosters
}
public abstract class ItemClass : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)] public string description;
    
}
