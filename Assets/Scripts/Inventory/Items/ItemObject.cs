using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    robotpart,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;

    public abstract float getWeight();
    public abstract float getValue();
    public abstract float getNoise();
}
