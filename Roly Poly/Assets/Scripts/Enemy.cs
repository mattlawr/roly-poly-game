using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public float walkSpeed = 1f;
    public bool walk = false;

    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<Rigidbody2D>()) { return; }
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!anim) { return; }

        // check for edge and walk back and forth
        if (walk)
        {
            Vector3 dir = GetForward() - transform.up * 1.5f;

            Ray r = new Ray(transform.position, dir.normalized);
            Debug.DrawRay(r.origin, r.direction, Color.red);

            Vector3 targetVelocity = (walkSpeed * 10f) * GetForward();

            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.1f);

            // If hit edge OR wall
            if (!Physics2D.Raycast(r.origin, r.direction, 1f, LayerMask.GetMask("Default")) || Physics2D.Raycast(r.origin, GetForward(), 0.9f, LayerMask.GetMask("Default")))
            {
                SetSpriteDirection(FacingRight() ? -1f : 1f);
            }
        }

        anim.SetFloat("speed", rb.velocity.magnitude);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;

            collision.collider.GetComponent<Entity>().TakeDamage(1, dir*2f + Vector2.up * 5f);
            //TODO: addforce
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
