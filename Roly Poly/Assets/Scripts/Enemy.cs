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
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // check for edge and walk back and forth
        if (walk)
        {
            Vector3 dir = GetForward() - transform.up * 1.5f;

            Ray r = new Ray(transform.position, dir.normalized);
            Debug.DrawRay(r.origin, r.direction, Color.red);

            // If hit edge OR wall
            if (!Physics2D.Raycast(r.origin, r.direction, 1f, LayerMask.GetMask("Default")) || Physics2D.Raycast(r.origin, GetForward(), 1f, LayerMask.GetMask("Default")))
            {
                SetSpriteDirection(FacingRight() ? -1f : 1f);
            }

            Vector3 targetVelocity = (walkSpeed * 100f) * GetForward();

            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.1f);
        }

        //anim.SetFloat("speed", rb.velocity.magnitude);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
