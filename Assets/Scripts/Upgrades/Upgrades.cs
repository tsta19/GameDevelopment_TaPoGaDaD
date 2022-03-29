using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Upgrades : MonoBehaviour
{
    public GameObject craftingBench;
    public GameObject upgradeMenu; // Panel
    public GameObject message; // Panel

    private bool canCraft;      // Is the player close enough to the bench? 
    [Range(0, 360)]
    public float angle = 180f;  // View angle
    public float radius = 5f;   // Distance from player to bench
    public float craftingTime;  // How long it takes to craft/upgrade

    public LayerMask craftingBenchMask;
    public LayerMask obstructionMask;


    // Start is called before the first frame update
    void Start()
    {
        craftingBench = GameObject.FindGameObjectWithTag("Crafting bench");
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed");
            if(upgradeMenu != null)
            {
                upgradeMenu.SetActive(true);
            }

        }
    }

}
