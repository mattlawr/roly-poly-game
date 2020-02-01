using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolyPolyController : MonoBehaviour
{
    public float speed = 2f;
    public float rollStrength = 10f;
    [Range(0, .3f)] public float smoothing = .05f;
    [Range(0, 1f)] public float airControl = 0.5f;
    public LayerMask groundLayers;

    private Rigidbody2D rb;
    private Vector2 upAnchor = Vector3.up;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float move, bool roll)
    {
        // If crouching, check to see if the character can stand up
        /*if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }*/

        //only control the player if grounded or airControl is allowed
        if (true || airControl > 0f)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * speed * 100f, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothing);

            //SetDirection(move);
        }
        // If the player should jump...
        if (true && roll)
        {
            // Add a force to the player.
            //m_Grounded = false;
            rb.AddForce(new Vector2(0f, rollStrength));
        }
    }
}
