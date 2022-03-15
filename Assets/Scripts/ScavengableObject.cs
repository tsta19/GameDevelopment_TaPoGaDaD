using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengableObject : MonoBehaviour
{
    private float localTimer = 0;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.F))
            {
                print("Scavenging");
                localTimer += Time.deltaTime;
            }

            if (localTimer >= 2)
            {
                PlayerClass.scavenge();
                print("Successfully scavenged");
                localTimer = 0;
            }
        }
    }
}