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
    public float playerNoise = 10;
    public static float scavengeTime = 2.0f;
    public InventoryScript inventory;
    public float invenWeight;
    public bool newItem = false;
    public float scavengeTimer = 0;
    public bool scavengeTimerBool;
    public float sneakSpeed = 1;
    public float walkSpeed = 5;
    public float runSpeed = 7;
    
    public ThirdPersonController controller;
    //public Collider[] noiseColliders = new Collider[10];
    
    public PlayerClass(string name)
    {
        playerName = name;
        controller = GetComponent<ThirdPersonController>();
    }

    public void run()
    {
        isRunning = true;
        playerNoise = 20;
        controller.maxSpeed = runSpeed;

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
        controller.maxSpeed = sneakSpeed;
        isSneaky = true;
        playerNoise = 3;
        print("SNEAKING");
        
    }

    public void normalState()
    {
        controller.maxSpeed = walkSpeed;
        playerNoise = 10;
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ScavengeO"))
        {
            
            if (Input.GetKey(KeyCode.F))
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

    void checkNoiseSphere()
    {
        //int noiseSphere = Physics.OverlapSphereNonAlloc(transform.position, playerNoise, noiseColliders);
        Collider[] noiseSphere = Physics.OverlapSphere(transform.position, playerNoise);
        if (noiseSphere.Length > 0)
        {
            
            for (int i = 0; i < noiseSphere.Length; i++)
            {
                float dist = Vector3.Distance(noiseSphere[i].transform.position, transform.position);
                
                if (dist <= playerNoise && noiseSphere[i].CompareTag("NPC"))
                {
                    print("message sent");
                    noiseSphere[i].SendMessage("IHeardSomething");
                }
            }
            
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, playerNoise);
        
    }

    public void Start()
    {
        walkSpeed = 5;
        sneakSpeed = 1;
        runSpeed = 7;
    }

    public void Update()
    {
        
        // Check states
        if (Input.GetKeyDown(KeyCode.G))
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
            
            controller.maxSpeed /= invenWeight;
            
            newItem = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (isRunning)
            {
                isRunning = false;
                normalState();
            }
            else if (!isRunning)
            {
                run();
            }
            
        }

        checkNoiseSphere();
       

    }
}
