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
    
    public float scavengeTimer = 0;
    public bool scavengeTimerBool;
    
    
    public PlayerClass(string name)
    {
        playerName = name;
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
        isSneaky = true;
        playerNoise = 3;
        speed = 1;
    }
    
    public void scavenge(GameObject colliderO) //brug StartCoroutine() til at kalde scavenge()
    {
      
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
                    var item = other.GetComponent<Item>();
                    print(item);
                    if (item)
                    {
                        inventory.AddItem(item.item, 1);
                        print("Successfully scavenged");
                        print(inventory);
                    }
               
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
}
