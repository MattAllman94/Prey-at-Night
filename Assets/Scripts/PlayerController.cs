using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Stats & Movement")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    private float horizontal, vertical;
    public float speed, walkSpeed, sprintSpeed, crouchSpeed;
   
    public bool isSprinting;

    public bool isCrouching;
    public bool isHidden;
    private CapsuleCollider playerCollider;

    [Header("Attacking")]
    public GameObject atkHitbox;
    public bool isAttacking;
    public float atkDamage = 10f;
    public float atkDuration = 2f;
    public NPC hitNPC;
    public Civilian civilianScript;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
    }

 
    void Update()
    {
        Movement();

        if (Input.GetButtonDown("Fire1") && isAttacking == false && _GM.gameState == GameState.INGAME)
        {
            StartCoroutine("Attack");
        }
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

    IEnumerator Attack() // Turns the hitbox on and off 
    {
        isAttacking = true;
        atkHitbox.SetActive(true);
        yield return new WaitForSeconds(atkDuration);
        atkHitbox.SetActive(false);
        isAttacking = false;
    }

    public void HitNPC() // Deals damage
    {
        //Debug.Log("Hit");
        hitNPC.health -= atkDamage;
        if(hitNPC.myType == NPC.EnemyType.Civilian)
        {
            civilianScript.Flee();
        }
    }

    public void ChangeHealth(float _health, bool increase = false) // Dont have to put in the bool if increasing blood 
    {
        currentHealth = increase ? currentHealth += _health : currentHealth -= _health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        _UI.UpdateHealth(currentHealth);
    }
}
