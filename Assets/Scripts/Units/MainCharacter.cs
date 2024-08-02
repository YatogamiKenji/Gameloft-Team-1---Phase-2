using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using System;

public class Control
{
    public const KeyCode UP = KeyCode.W;
    public const KeyCode DOWM = KeyCode.S;
    public const KeyCode LEFT = KeyCode.A;
    public const KeyCode RIGHT = KeyCode.D;

    public const KeyCode SHOOT = KeyCode.Space;
    public const KeyCode SKILL_1 = KeyCode.X;
    public const KeyCode SKILL_2 = KeyCode.C;
}

public class MainCharacter : BaseCharacter
{
    //public FloatPublisherSO changeHealthSO;
    //public VectorPublisherSO sendPositionToEnemiesSO;
    //public VectorPublisherSO sendPositionToSlimeOrbsSO;

    public float totalTimeToCastSkill;
    public float currentTimeToCastSkill;

    public float minimizeScaleFactor;
    public float maximizeScaleFactor;

    public float acceleration;
    public int minSpeed;
    public int maxSpeed;

    private Vector2 velocity;
    private Rigidbody2D rb;

    public SkillActivator activator;
    public Skill Skill_1;
   // public Skill Skill_2;

    public int Skill_1_Unlock;
    public int Skill_2_Unlock;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTimeToCastSkill = 0;
        currentHealth = maxHealth;
        currentSpeed = minSpeed;
        //changeHealthSO.RaiseEvent(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(Control.UP))
        {
            velocity.y = Mathf.MoveTowards(velocity.y, currentSpeed, acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(Control.DOWM))
        {
            velocity.y = Mathf.MoveTowards(velocity.y, -currentSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.y = Mathf.MoveTowards(velocity.y, 0, acceleration * Time.deltaTime);
        }

        if (Input.GetKey(Control.LEFT))
        {
            velocity.x = Mathf.MoveTowards(velocity.x, -currentSpeed, acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(Control.RIGHT))
        {
            velocity.x = Mathf.MoveTowards(velocity.x, currentSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, acceleration * Time.deltaTime);
        }


        if (Input.GetKeyDown(Control.SHOOT))
        {
            Attack();
        }
        if (Input.GetKey(Skill_1.keyCode))
        {
            activator.SetSkill(Skill_1);
        }
        //if (Input.GetKey(Skill_2.keyCode))
        //{
            //activator.SetSkill(Skill_2);
        //}

        //if (currentHealth > 0)
        //    sendPositionToEnemiesSO.RaiseEvent(this.transform.position);
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    public override void Attack()
    {

        ChangeHealth(-1);
    }


    public override void Die()
    {
        Debug.Log("Die");
    }

    public void PickUpHealth()
    {
        ChangeHealth(1);
    }

    public override void ChangeHealth(float h)
    {
        currentHealth += h;
        currentSpeed -= h * (maxSpeed - minSpeed) / 7;

        Vector2 newScale;
        if (h < 0)
        {
            //newScale = Math.Abs(h) * minimizeScaleFactor * transform.localScale;
            Debug.Log("Decreasing size...");
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            else if (currentSpeed >= maxSpeed)
                currentSpeed = maxSpeed;

        }
        else
        {
            //newScale = Math.Abs(h) * maximizeScaleFactor * transform.localScale;
            Debug.Log("Increasing size...");
            if (currentHealth >= maxHealth)
                currentHealth = maxHealth;
            else if (currentSpeed <= minSpeed)
                currentSpeed = minSpeed;
        }
        
        //transform.localScale = newScale;

          
        if (currentHealth <= Skill_2_Unlock)
        {
            Skill_1.SetDisabled();
          //  Skill_2.SetReady();
        }
        else if (currentHealth <= Skill_1_Unlock)
        {
            Skill_1.SetReady();
        }
        else
        {
            Skill_1.SetDisabled();
           // Skill_2.SetDisabled();
        }    
        //changeHealthSO.RaiseEvent(currentHealth);
    }
}