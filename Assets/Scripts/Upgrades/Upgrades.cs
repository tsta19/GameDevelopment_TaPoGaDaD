using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Upgrades : MonoBehaviour
{

    public GameObject craftingBench;

    private PlayerClass playerClass;
    private InventoryScript inventoryClass;
    private FieldOfView fieldOfViewClass;
    private AIBehaviour AIBehaviourClass;

    public GameObject upgradeMenu; // Panel
    public GameObject message; // Panel

    private bool canCraft;      // Is the player close enough to the bench? 
    [Range(0, 360)]
    public float angle = 180f;  // View angle
    public float radius = 5f;   // Distance from player to bench
    public float craftingTime;  // How long it takes to craft/upgrade

    public LayerMask craftingBenchMask;
    public LayerMask obstructionMask;

    // Variables that can be upgraded
    public int inventorySize = 2;
    public int noise = 1;
    public int sneak = 1;
    public int disguise = 1;
    public int speed = 1;


    // Start is called before the first frame update
    void Start()
    {
        craftingBench = GameObject.FindGameObjectWithTag("Crafting bench");

        playerClass = GetComponent<PlayerClass>();
        inventoryClass = GetComponent<InventoryScript>();
        fieldOfViewClass = GetComponent<FieldOfView>();
        AIBehaviourClass = GetComponent<AIBehaviour>();

}

    // Update is called once per frame
    void Update()
    {
        if (CheckDistance())
        {
            MakeUpgrade();
        }
    }

    bool CheckDistance()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, craftingBenchMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canCraft = true;
                    ShowMessage();
                }
                else
                    canCraft = false;
            }
            else
                canCraft = false;
        }
        else if (canCraft)
            canCraft = false;

        return canCraft;
    }

    void ShowMessage()
    {
        Debug.Log("Press E to upgrade");
        if (message != null)
        {
            message.SetActive(true);
        }
    }

    void MakeUpgrade()
    {
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed");
            if(upgradeMenu != null)
            {
                upgradeMenu.SetActive(true);
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            UpgradeInventory();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            UpgradeSneak();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            UpgradeDisguise();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            UpgradeSpeed();
        }
    }

    void UpgradeInventory()
    {
        inventoryClass.maxInven += 1; // increases max inventory space
        playerClass.playerNoise += 2;       // increases noise

        // a big inventory slows you down when running, but running will always be faster than walking
        if (playerClass.runSpeed >= playerClass.walkSpeed + 3) {
            playerClass.runSpeed -= 1;          
        }

        // Man bliver nemmere opdaget, når man har større inventory
        fieldOfViewClass.radius += 1;
        if (AIBehaviourClass._suspicionTimer >= 2.5)
        {
            AIBehaviourClass._suspicionTimer -= 0.5f;
        }
    }

    void UpgradeSneak()
    {
        // Can move faster and more silent while sneaking
        playerClass.sneakSpeed += 1;

        if (playerClass.playerNoise >= 3)
        {
            playerClass.playerNoise -= 3;   // decreases noise
        }
        
    }

    void UpgradeDisguise()
    {
        // Man skal tættere på AIs for at blive opdaget og man skal være i deres FoV i længere tid.
        if (fieldOfViewClass.radius >= 8)
        {
            fieldOfViewClass.radius -= 3;
        }
        AIBehaviourClass._suspicionTimer += 1.0f;
    }

    void UpgradeSpeed()
    {
        playerClass.walkSpeed += 1;
        playerClass.runSpeed += 2;
    }
}
    
