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
    public AudioSource footStepSource;
    public Animator playerAnim;
    public Animator normalAnim;
    public Animator evilAnim;
    public Transform gCheckPos;
    public float gCheckRadius = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    [Header("Enemy Scripts")]
    public Civilian civilianScript;
    public Criminal criminalScript;
    public Monster monsterScript;

    [Header("Attacking")]
    public GameObject atkHitbox;
    public bool isAttacking;
    public float atkDamage = 20f;
    public float atkDuration = 2f;
    public NPC hitNPC;

    [Header("Drain")]
    public GameObject drainHitbox;
    public float drainDamage = 5f;
    public bool isDrinking;

    [Header("Corruption")]
    public GameObject normalModel;
    public GameObject corruptModel;

    void Start()
    {
        normalModel.SetActive(true);
        corruptModel.SetActive(false);

        currentHealth = 100f;
    }

 
    void Update()
    {
        if(!isDrinking)
        {
            Movement();
            _PM.drainVFX.gameObject.SetActive(false);
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
    }

    public void Movement() //Controls the players walk, sprint and crouch
    {
        isGrounded = Physics.CheckSphere(gCheckPos.position, gCheckRadius, groundMask);

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

            
            
            //play run anim
            if(isGrounded && isAttacking == false)
            {
                _AM.PlayFootStep(footStepSource);
                SetAnimBool("Running");
                SetAnimBool("Idle", false);
                SetAnimBool("Falling", false);
            }
        }
        else
        {
            if (isGrounded && isAttacking == false)
            {
                //play idle animation
                SetAnimBool("Idle");
                SetAnimBool("Running", false);
                SetAnimBool("Falling", false);
            }
        }


        if (!isGrounded)
        {
            SetAnimBool("Falling");
            SetAnimBool("Running", false);
            SetAnimBool("Idle", false);
        }



        if (!controller.isGrounded)
        {
            moveDir += Physics.gravity;
            controller.Move(moveDir * Time.deltaTime);
        }
    }

    IEnumerator Attack() // Turns the hitbox on and off 
    {
        isAttacking = true;
        playerAnim.SetTrigger("Attacking");
        _AM.PlayerAttackSound();
        atkHitbox.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        atkHitbox.SetActive(false);     
        isAttacking = false;
    }

    public void HitNPC() // Deals damage
    {
        //Debug.Log("Hit");
        hitNPC.TakeDamage(atkDamage);
        if(hitNPC.myType == EnemyType.Civilian)
        {
            civilianScript.anim.SetTrigger("Damaged");
            if (civilianScript.health != 0)
            {
                civilianScript.ChangeState(State.Flee);
            }
        }
        if(hitNPC.myType == EnemyType.Criminal)
        {
            criminalScript.anim.SetTrigger("Damaged");
            if (criminalScript.health != 0)
            {
                criminalScript.ChangeState(State.Attack);
            }
        }
        if (hitNPC.myType == EnemyType.Monster)
        {
            monsterScript.anim.SetTrigger("Damaged");
            if (monsterScript.health != 0)
            {
                monsterScript.ChangeState(State.Attack);
            }
        }
    }

    public void DrainCivilian() //Drains Blood from civilian
    {
        civilianScript.TakeDamage(drainDamage, true);
        if (_GM.currentBlood < 100f)
        {
            _GM.ChangeBlood(drainDamage / 2 * Time.deltaTime, true);
        }
        if (currentHealth < 100f)
        {
            ChangeHealth(drainDamage / 3 * Time.deltaTime, true);
        }
    }

    public void DrainCriminal() //Drains Blood from criminal
    {
        criminalScript.TakeDamage(drainDamage, true);
        if (_GM.currentBlood < 100f)
        {
            //_GM.currentBlood += drainDamage / 4 * Time.deltaTime;
            _GM.ChangeBlood(drainDamage / 4 * Time.deltaTime, true);
        }
        if (currentHealth < 100f)
        {
            //currentHealth += drainDamage / 3 * Time.deltaTime;
            ChangeHealth(drainDamage / 3 * Time.deltaTime, true);
        }
    }

    public void ChangeHealth(float _health, bool increase = false) // Dont have to put in the bool if increasing blood 
    {
        currentHealth = increase ? currentHealth += _health : currentHealth -= _health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        _UI.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _GM.ChangeGameState(GameState.GAMEOVER);
        _UI.losePanel.SetActive(true);
    }

    public void SetAnimBool(string _name, bool _bool = true)
    {
        playerAnim.SetBool(_name, _bool);     
    }
}
