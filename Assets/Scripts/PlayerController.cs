using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Movement")]
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 moveDir;

    //private float horizontal, vertical;
    //public float speed, walkSpeed, sprintSpeed, crouchSpeed;
    //public bool isSprinting;
    //public bool isCrouching;
    //public bool isHidden;
    //private CapsuleCollider playerCollider;

    [Header("Enemy Scripts")]
    public Civilian civilianScript;
    public Criminal criminalScript;
    public Monster monsterScript;

    [Header("Attacking")]
    public GameObject atkHitbox;
    public bool isAttacking;
    public float atkDamage = 10f;
    public float atkDuration = 2f;
    public NPC hitNPC;



    [Header("Drain")]
    public GameObject drainHitbox;
    public float drainDamage = 5f;
    public bool isDrinking;

    void Start()
    {
        //playerCollider = GetComponent<CapsuleCollider>();
    }

 
    void Update()
    {
        if(!isDrinking)
        {
            Movement();
        }

        if (Input.GetButtonDown("Fire1") && isAttacking == false && _GM.gameState == GameState.INGAME)
        {
            StartCoroutine("Attack");
        }
        if(Input.GetKey(KeyCode.Q))
        {
            isDrinking = true;
            drainHitbox.SetActive(true);
        }
        else
        {
            isDrinking = false;
            drainHitbox.SetActive(false);
        }

        //Debug.Log(currentHealth);
    }

    public void Movement() //Controls the players walk, sprint and crouch
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f , vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (!controller.isGrounded)
        {
            moveDir += Physics.gravity;
            controller.Move(moveDir * Time.deltaTime);
        }


        //horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        //vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //transform.Translate(horizontal, 0, vertical);

        //if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        //{
        //    speed = sprintSpeed;
        //    isSprinting = true;
        //}
        //else
        //{
        //    speed = walkSpeed;
        //    isSprinting = false;
        //}

        //if(Input.GetKey(KeyCode.LeftControl) && !isSprinting)
        //{
        //    speed = crouchSpeed;
        //    playerCollider.height = 1;
        //    playerCollider.center = new Vector3(playerCollider.center.x, -0.5f, playerCollider.center.z); //Add back once crouch animations are included
        //    //transform.localScale = new Vector3(1, 0.5f, 1); //Remove once crouching animation is included
        //    isCrouching = true;

        //}
        //else
        //{
        //    speed = walkSpeed;
        //    playerCollider.height = 2;
        //    playerCollider.center = new Vector3(playerCollider.center.x, 0f, playerCollider.center.z); //Add back once crouch animations are included
        //    //transform.localScale = new Vector3(1, 1, 1); //Remove once crouching animation is included
        //    isCrouching = false;

        //}
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
        if(hitNPC.myType == NPC.EnemyType.Criminal)
        {
            criminalScript.StartCoroutine("Attack");
        }
        if (hitNPC.myType == NPC.EnemyType.Monster)
        {
            monsterScript.StartCoroutine("Attack");
        }
    }

    public void DrainCivilian() //Drains Blood from civilian
    {
        civilianScript.health -= drainDamage * Time.deltaTime;
        if (_GM.currentBlood < 100f)
        {
            _GM.currentBlood += drainDamage / 2 * Time.deltaTime;
        }
        if (currentHealth < 100f)
        {
            currentHealth += drainDamage / 3 * Time.deltaTime;
        }
    }

    public void DrainCriminal() //Drains Blood from criminal
    {
        criminalScript.health -= drainDamage * Time.deltaTime;
        if (_GM.currentBlood < 100f)
        {
            _GM.currentBlood += drainDamage / 4 * Time.deltaTime;
        }
        if (currentHealth < 100f)
        {
            currentHealth += drainDamage / 3 * Time.deltaTime;
        }
    }

    public void ChangeHealth(float _health, bool increase = false) // Dont have to put in the bool if increasing blood 
    {
        currentHealth = increase ? currentHealth += _health : currentHealth -= _health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        _UI.UpdateHealth(currentHealth);
    }
}
