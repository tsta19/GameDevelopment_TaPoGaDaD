using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvControl : MonoBehaviour
{
    private GameObject leftFoot;
    private GameObject rightFoot;
    private GameObject body;
    private AdvancedControls advancedControls;
    private int moveState = 0; // 0 none, 1 right, 2 left, 3 jump
    private float footSpeed = 5;
    private float bodyOffset = 0;



    // Start is called before the first frame update
    void Awake()
    {
        advancedControls = GetComponent<AdvancedControls>();
        leftFoot = GetComponent<GameObject>();
        rightFoot = GetComponent<GameObject>();
        body = GetComponent<GameObject>();
        bodyOffset = body.transform.position.y - leftFoot.transform.position.y;
    }

    private void OnEnable()
    {
        advancedControls.Enable();
    }

    private void OnDisable()
    {
        advancedControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        MovingStateControls(moveState);


        Vector2 move = advancedControls.ManualPlayer.Move.ReadValue<Vector2>();
        Debug.Log(move);
        if (advancedControls.ManualPlayer.L_Move.triggered)
        {

        }
    }
    void MovingStateControls(int i)
    {
        switch (i)
        {
            case 0:
                if (advancedControls.ManualPlayer.R_Move.triggered)
                    {
                        Debug.Log("R");
                        moveState = 1;
                    }
                else if (advancedControls.ManualPlayer.L_Move.triggered)
                    {
                        moveState = 2;
                        Debug.Log("L");
                    }
                break;
            case 1:
                FootControl(rightFoot);
                    if (advancedControls.ManualPlayer.R_Move.triggered)
                    {
                        moveState = 0;
                        FootRaise(rightFoot, false);
                    }
                break;
            case 2:
                FootControl(leftFoot);
                if (advancedControls.ManualPlayer.L_Move.triggered)
                    {
                        moveState = 0;
                        FootRaise(leftFoot, false);
                    }
                break;

            default:
                break;
        }
    }
    
    private void FootControl(GameObject f)
    {
        Debug.Log(f.name);
        var translationX = Input.GetAxis("Vertical");
        var translationZ = Input.GetAxis("Horizontal");

        f.transform.Translate(translationZ, 0, translationX);
        FootRaise(f, true);
        body.transform.Translate(translationZ / 2, 0, translationX / 2);
    }
    private void FootRaise(GameObject f, bool b)
    {
        RaycastHit hit;
        var posY = 0;
        /*if (Physics.Raycast(f.transform.position, Vector3(0, -1, 0),))
        {
            if (b)
                {
                    posY = (int)(hit.point.y + 2.5);
                }
            else
                {
                    posY = (int)(hit.point.y + 2);
                }
        }*/
        //f.transform.position.y = new Vector3(x,y,z);
        
            
        //posY;
        //body.transform.position.y = (posY+(bodyOffset/2));
        //body.transform.position += Vector3;
    }

    private Vector3 Vector3(int v1, int v2, int v3)
    {
        throw new System.NotImplementedException();
    }
} 
