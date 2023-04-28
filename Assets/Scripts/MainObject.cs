using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObject : MonoBehaviour
{
    public DynamicJoystick js;
    public Rigidbody rb;
    public float moveSpeed;
    public GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void FixedUpdate()
    {
        if (gm.currentPlayerCount > 0)
        {
            Movement();
        }
    }
    void Movement()
    {
        if (js.Horizontal != 0 || js.Vertical != 0)
        {

          
            rb.velocity = new Vector3(js.Horizontal * moveSpeed, rb.velocity.y, js.Vertical * moveSpeed);

            rb.isKinematic = false;


        }
        else
        {
            if (js.Horizontal == 0 && js.Vertical == 0)
            {
               
             rb.isKinematic = true;
                
             
            }
        }
    }
}
