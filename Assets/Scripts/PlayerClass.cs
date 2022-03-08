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
    private float scavengeTime = 2.0f;
    
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

    public IEnumerator scavenge() //brug StartCoroutine() til at kalde scavenge()
    {
        //Returner en item genereret item class
        yield return new WaitForSeconds(scavengeTime);
        print("SCAVENGING");
        //generate new item og append til inventory
        
    }
    
}
