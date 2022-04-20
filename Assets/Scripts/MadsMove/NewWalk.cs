using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewWalk : MonoBehaviour
{
    //input fields
    public static ThirdPersonAction playerActionsAsset;
    //private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 10f;
    //[SerializeField]



    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonAction();        
        //move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }


    private void FixedUpdate()
    {
 
        //Store user input as a movement vector
        Vector3 m_Input = new Vector3(playerActionsAsset.Player.Move.ReadValue<Vector2>().x, 0, playerActionsAsset.Player.Move.ReadValue<Vector2>().y);
       
        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        rb.MovePosition(transform.position - m_Input * Time.deltaTime * movementForce);

        //Vector3 R_Foot = new Vector3(playerActionsAsset.AdvancedWalk.L_Move.ReadValue<Vector2>().x, 0, playerActionsAsset.AdvancedWalk.L_Move.ReadValue<Vector2>().y);
        //Vector3 L_Foot = new Vector3(playerActionsAsset.AdvancedWalk.L_Move.ReadValue<Vector2>().x, 0, playerActionsAsset.AdvancedWalk.R_Move.ReadValue<Vector2>().y);
       
        //rb.MovePosition(transform.position - R_Foot * Time.deltaTime * movementForce);
        //rb.MovePosition(transform.position - L_Foot * Time.deltaTime * movementForce);
    }

   }
