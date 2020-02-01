using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public RolyPolyController controller;

    float horizontal = 0f;
    float vertical = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (!controller)
        {
            return;
        }

        controller.Move(horizontal * Time.fixedDeltaTime, false);
    }
}
