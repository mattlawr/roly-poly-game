using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RolyPolyController : MonoBehaviour
{
    public float speed = 2f;
    public float rollStrength = 10f;
    [Range(0, .3f)] public float smoothing = .05f;
    [Range(0, 1f)] public float airControl = 0.5f;
    //public LayerMask groundLayers;

    private Player player;
    private Rigidbody2D rb;
    private Vector2 upAnchor = Vector3.up;
    private Vector3 velocity = Vector3.zero;
    bool anchored = false;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;  // For FX

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // Check for wall climbing
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Untagged")
        {
            //SetAnchor(collision.contacts[0].normal);
        }
    }

    public void Move(float move, bool roll)
    {
        //only control the player if grounded or airControl is allowed
        if (anchored || airControl > 0f)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * speed * 100f, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothing);

            SetDirection(move);
        }

        // Roll
        if (anchored && roll)
        {
            // Add a force to the player.
            //m_Grounded = false;
            rb.AddForce(new Vector2(0f, rollStrength));
        }
    }

    // Sets controller to either stick to a wall or unstick
    void SetAnchor(Vector2 norm)
    {
        rb.velocity = Vector3.zero;
        //rb.isKinematic = true;
    }

    void SetDirection(float direction)
    {
        player.SetSpriteDirection(direction);
    }
}
