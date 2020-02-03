using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// A simple controller for animating a 4 directional sprite using Physics.
/// </summary>
public class CharacterController2D : Entity
{
    public float speed = 100;
    Rigidbody2D rb;
    public bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect * speed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);

        SetSpriteDirection(h);
    }
}