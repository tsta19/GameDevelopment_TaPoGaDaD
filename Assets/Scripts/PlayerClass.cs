using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    public static string playerName = "";
    private int hp;
    private float weight;
    private int carryCap;
    public static bool isWalking;
    public static bool isHumanWalk;
    public static bool isSneaky;
    public static bool isRunning;
    public static float playerNoise;
    public float speed;
    public static float scavengeTime = 2.0f;
    public InventoryScript inventory;
    public float invenWeight;
    public bool newItem = false;
    
    public float scavengeTimer = 0;
    public bool scavengeTimerBool;

    public ThirdPersonController controller;
    public PlayerClass(string name)
    {
        playerName = name;
        controller = GetComponent<ThirdPersonController>();
    }

    public void run()
    {
        isRunning = true;
        playerNoise = 30;
        speed = 7;

    }

    public void humanWalk()
    {
        isHumanWalk = true;
        playerNoise = 10;
        // Her ska være mechanics for menneske-gang
        // der kan eg. være lyd-straf for at gå dårligt
        
    }

    public void sneak()
    {
        controller.maxSpeed = 1;
        isSneaky = true;
        playerNoise = 3;
        print("SNEAKING");
        
    }

    public void normalState()
    {
        controller.maxSpeed = 5;
        playerNoise = 10;
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ScavengeO"))
        {
            
            if (ThirdPersonController.playerActionsAsset.Player.Scavenge.triggered)
            {
                print("other" + other);
                scavengeTimerBool = true;
                print("Scavenging");
                
            }
            if (scavengeTimerBool)
            {
                scavengeTimer += Time.deltaTime;
                print("timer: " + scavengeTimer);
                if (scavengeTimer >= scavengeTime)
                {
                    //Returner en item genereret item class
                    print("SCAVENGING");
                    //generate new item og append til inventory
                    print("other" + other.name);
                    var itemmm = other.GetComponent<ScavengableObject>();
                    print(itemmm);
                    if (itemmm)
                    {
                        
                        inventory.AddItem(itemmm.item1, 1);
                        print("Successfully scavenged");
                        print(inventory);
                        
                    }
                    newItem = true;
                    scavengeTimer = 0;
                    scavengeTimerBool = false;
                    
                }
            }  
            
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ScavengeO"))
        {
            if (scavengeTimerBool)
            {
                print("Scavenging interrupted");
            }
            scavengeTimerBool = false;
            scavengeTimer = 0;
        } 
    }

    // gets weight from item
    public void updateWeight()
    {
        invenWeight = 1;
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            invenWeight = invenWeight + inventory.Container[i].getWeight();
        }
    }

    public void Update()
    {
        print("speeeeeed" + controller.maxSpeed);
        // Check states
        if (ThirdPersonController.playerActionsAsset.Player.Sneak.triggered)
        {
            if (isSneaky)
            {
                isSneaky = false;
                normalState();
            }
            else if (!isSneaky)
            {
                sneak();
            }
        }
        if (newItem)
        {
            updateWeight();
            print("hejsa: " + invenWeight);
            controller.maxSpeed /= invenWeight;
            print("speed: " + controller.maxSpeed);
            newItem = false;
        }

    }
}
