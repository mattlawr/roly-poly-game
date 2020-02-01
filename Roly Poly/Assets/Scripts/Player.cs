using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public RolyPolyController controller;

    Vector2 input = Vector2.zero;
    bool roll = false;
    bool release = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            roll = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            release = true;
        }
    }

    private void FixedUpdate()
    {
        if (!controller)
        {
            return;
        }

        controller.Move(input * Time.fixedDeltaTime, roll, release);

        roll = release = false;
    }
}
