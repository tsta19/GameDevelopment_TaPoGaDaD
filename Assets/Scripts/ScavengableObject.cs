using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengableObject : MonoBehaviour
{
    private float localTimer = 0;
    private bool localTimerBool;

    public void OnTriggerStay(Collider other)
    {
        
        if (ThirdPersonController.playerActionsAsset.Player.Scavenge.triggered)
        {
            print("Scavenging");
            localTimerBool = true;
        }

        if (localTimerBool)
        {
            localTimer += Time.deltaTime;
            if (localTimer >= 2)
            {
                PlayerClass.scavenge();
                print("Successfully scavenged");
                localTimer = 0;
                localTimerBool = false;
            }
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (localTimerBool)
        {
            print("Scavenging interrupted");
        }
        localTimerBool = false;
        localTimer = 0;
        
        
    }
}