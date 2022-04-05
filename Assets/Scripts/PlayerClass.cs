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
    public bool buttonBool = true;
    public GameObject panel;
    private Rigidbody playerRB;
    public ThirdPersonController controller;
    //public Collider[] noiseColliders = new Collider[10];
    private float hwTimerLeft = 0;
    private float hwTimerRight = 0;
    private float hwTimeCap = 2;
    
    
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
        print("SPRINTING");

    }

    public void humanWalk()
    {

        // Her ska være mechanics for menneske-gang
        // der kan eg. være lyd-straf for at gå dårligt
        if (isHumanWalk)
        {
            hwTimerLeft += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerRB.AddForce(Vector3.forward, ForceMode.Impulse);
                hwTimerLeft = 0;
            }

            if (hwTimerLeft > hwTimeCap)
            {
                print("TOO SLOW, LEFT, TIME: " + hwTimerLeft);
                playerRB.AddForce(Vector3.left, ForceMode.Impulse);
            }

            hwTimerRight += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerRB.AddForce(Vector3.forward, ForceMode.Impulse);
                hwTimerRight = 0;
            }

            if (hwTimerRight > hwTimeCap)
            {
                print("TOO SLOW, RIGHT, TIME: " + hwTimerRight);
                playerRB.AddForce(Vector3.right, ForceMode.Impulse);
            }
        }

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
            
            if (Input.GetKeyDown(KeyCode.F))
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
                        inventory.Save();
                        
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

    public void openIven()
    {
        panel.SetActive(true);
    }
    public void closeInven()
    {
        panel.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

    public void Start()
    {
        panel.SetActive(false);
        walkSpeed = 5;
        sneakSpeed = 1;
        runSpeed = 7;
        hwTimeCap = 2;
        playerRB = this.GetComponent<Rigidbody>();
        print("RigidBody: " + playerRB);
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

        if (Input.GetKeyDown(KeyCode.G))
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isHumanWalk)
            {
                isHumanWalk = false;
                hwTimerLeft = 0;
                hwTimerRight = 0;
                normalState();
            }
            else if (!isHumanWalk)
            {
                isHumanWalk = true;
                humanWalk();
            }
            
        }
        if (Input.GetKeyDown("i") && buttonBool == true)
        {
            openIven();
            buttonBool = false;
        }
        else if (Input.GetKeyDown("i") && buttonBool == false)
        {
            closeInven();
            buttonBool = true;
        }

        checkNoiseSphere();
       

    }
}
