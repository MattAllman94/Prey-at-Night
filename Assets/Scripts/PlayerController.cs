using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float horizontal, vertical;
    public float speed, walkSpeed, sprintSpeed, crouchSpeed;
   
    public bool isSprinting;

    public bool isCrouching;
    public bool isHidden;
    private CapsuleCollider playerCollider;

    public float health;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
    }

 
    void Update()
    {
        Movement();
    }

    public void Movement() //Controls the players walk, sprint and crouch
    {
        horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(horizontal, 0, vertical);

        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            speed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            speed = walkSpeed;
            isSprinting = false;
        }

        if(Input.GetKey(KeyCode.LeftControl) && !isSprinting)
        {
            speed = crouchSpeed;
            playerCollider.height = 1;
            //playerCollider.center = new Vector3(playerCollider.center.x, -0.5f, playerCollider.center.z); //Add back once crouch animations are included
            transform.localScale = new Vector3(1, 0.5f, 1); //Remove once crouching animation is included
            isCrouching = true;
            
        }
        else
        {
            speed = walkSpeed;
            playerCollider.height = 2;
            //playerCollider.center = new Vector3(playerCollider.center.x, 0f, playerCollider.center.z); //Add back once crouch animations are included
            transform.localScale = new Vector3(1, 1, 1); //Remove once crouching animation is included
            isCrouching = false;
            
        }
    }
}
