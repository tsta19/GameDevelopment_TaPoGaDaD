using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robotpart object", menuName = "Inventory/Items/Robotpart")]
public class Robotpart : ItemObject
{
    public int partValue;
    public void Awake()
    {
        type = ItemType.robotpart;
    }
}
