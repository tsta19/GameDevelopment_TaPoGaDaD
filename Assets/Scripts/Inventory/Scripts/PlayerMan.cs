using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMan : MonoBehaviour
{
    public InventoryScript inventory;
    private bool start = true;

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        print("item" + item);
        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
        inventory.Save();
    }

    private void Start()
    {
        //loadInven();
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
    private void loadInven()
    {
        inventory.Load();
        start = false;
    }
 
}
