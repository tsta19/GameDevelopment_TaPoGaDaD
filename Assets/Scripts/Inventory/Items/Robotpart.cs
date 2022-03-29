using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robotpart object", menuName = "Inventory/Items/Robotpart")]
public class Robotpart : ItemObject
{
    public float weight;
    public float noise;
    public float value;
    public void Awake()
    {
        type = ItemType.robotpart;
    }
}
