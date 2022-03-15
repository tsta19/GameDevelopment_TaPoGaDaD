using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gear Object", menuName = "GearItem")]
public class GearItem : ItemClass
{
    public void Awake()
    {
        type = ItemType.Gears;
    }
}
