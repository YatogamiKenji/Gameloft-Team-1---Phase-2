using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public float currentSpeed;
    public float currentHealth;
    public float maxHealth;

    public abstract void Attack();
    public abstract void Die();
    public abstract void ChangeHealth(float health);
}