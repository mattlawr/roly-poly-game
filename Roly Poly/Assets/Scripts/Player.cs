using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public RolyPolyController controller;

    Vector2 input = Vector2.zero;
    bool roll = false;
    bool release = false;

    float hitstun = -1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitstun -= Time.deltaTime;

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            roll = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            release = true;
        }

        if(controller.stunned && hitstun <= 0f)
        {
            sprite.color = Color.white;
            controller.Stun(false);
        }
    }

    private void FixedUpdate()
    {
        if (!controller || controller.stunned)
        {
            return;
        }

        controller.Move(input * Time.fixedDeltaTime, roll, release);

        roll = release = false;
    }

    override public void TakeDamage(int d, Vector2 force)
    {
        if(hitstun > -0.2f || controller.rolling) { return; }

        GameManager.instance.HeartsAdd(-d);

        sprite.color = Color.red;
        controller.Stun(true);
        hitstun = 0.6f;

        base.TakeDamage(d, force);

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        controller.Stun(true);
        hitstun = 100f;

        GameManager.instance.StartCoroutine("RestartLevel");
    }
}
