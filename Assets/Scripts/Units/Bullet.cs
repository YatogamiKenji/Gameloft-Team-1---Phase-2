using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float initSpeed;
    [SerializeField, Range(0.1f, 10f)] private float decelerationSpeed;
    [SerializeField, Range(0f, 1f)] private float collidedDecce; // current speed will be subtracted when collided with enemies or walls
    [SerializeField] private float stopSpeed;

    private bool isCollectable;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 lastVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isCollectable)
        {
            if (rb.velocity.magnitude > stopSpeed)
            {
                rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, decelerationSpeed * Time.deltaTime);
            }
            else
            {
                rb.velocity = Vector2.zero;
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
    }

    public void Shoot(Vector2 dir, bool isReflectedShoot)
    {
        direction = dir.normalized;
        if (isReflectedShoot)
        {
            Debug.Log(rb.velocity.magnitude);
            rb.velocity = direction * Mathf.Lerp(lastVel.magnitude, 0f, collidedDecce);
        }
        else
            rb.velocity = direction * initSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCollectable)
        {
            var firstContact = collision.contacts[0];
            //decelerationSpeed += collidedDecce;
            Shoot(Vector2.Reflect(direction, collision.GetContact(0).normal), true);
        }
    }
}
