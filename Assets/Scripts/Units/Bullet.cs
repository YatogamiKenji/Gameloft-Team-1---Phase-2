using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float initSpeed;
    [SerializeField, Range(0.1f, 10f)] private float decelerationSpeed;
    [SerializeField, Range(0f, 1f)] private float collidedDecce; // current speed will be subtracted when collided with enemies or walls
    [SerializeField] private float stopSpeed;

    private Collider2D col;
    private bool isCollectable; // this will be true when the slime bullet stop moving and can be collected
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 lastVel; // keep track of last velocity

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
    }

    public bool IsCollectable
    {
        get { return isCollectable; }
    }
    private void Update()
    {
        if (!isCollectable)
        {
            if (rb.velocity.magnitude > stopSpeed)
            {
                // velocity -> 0
                rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, decelerationSpeed * Time.deltaTime);
            }
            else
            {
                rb.velocity = Vector2.zero;
                // set collider to trigger so that player can collect it
                col.isTrigger = true;
                // avoid isCollectable value being true right after the bullet is instantiated
                StartCoroutine(DelayCollectable());
            }
        }
    }

    private void FixedUpdate()
    {
        lastVel = rb.velocity;
    }

    private IEnumerator DelayCollectable()
    {
        yield return new WaitForSeconds(.1f);
        isCollectable = true;
    }

    private void OnDisable()
    {
        isCollectable = false;
        col.isTrigger = false;
    }

    public void Shoot(Vector2 dir, bool isReflectedShoot)
    {
        direction = dir.normalized;
        if (isReflectedShoot)
        {
            Debug.Log(rb.velocity.magnitude);
            // Decrease bullet's speed
            DecreaseSpeed();
        }
        else
            rb.velocity = direction * initSpeed;
    }

    // Decrease speed if collide with enemies or wall
    public void DecreaseSpeed()
    {
        rb.velocity = direction * Mathf.Lerp(lastVel.magnitude, 0f, collidedDecce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollectable)
        {
            // bullet reflected
            var firstContact = collision.contacts[0];
            Shoot(Vector2.Reflect(direction, collision.GetContact(0).normal), true);
        }
    }
}
