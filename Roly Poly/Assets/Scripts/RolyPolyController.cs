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
    bool rolling = false;
    private Vector2 lastMove = Vector3.zero;
    private bool correctCrawl = false;

    private float gravity = 0f;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;  // For FX

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        gravity = rb.gravityScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (!rolling && !anchored)
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position - transform.up*0.4f, 0.12f);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    print("anchor! STAY");
                    SetAnchor(transform.up);
                    return;
                }
            }

            colliders = Physics2D.OverlapCircleAll(transform.position - Vector3.up * 0.4f, 0.12f);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    print("anchor! Ground");
                    SetAnchor(Vector3.up);
                    return;
                }
            }
        }

        
    }

    // Check for wall climbing
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Untagged" && !rolling)
        {
            //print("anchor!");
            SetAnchor(collision.contacts[0].normal);
        }

        if (rolling && collision.collider.GetComponent<Entity>())
        {
            collision.collider.GetComponent<Entity>().TakeDamage(1);
        }
    }

    //TODO: Corrections for controller climbing!
    public void Move(Vector2 move, bool roll, bool release)
    {
        Vector3 targetVelocity = Vector3.zero;

        //only control here if grounded
        if (anchored)
        {
            int trueVertical = (transform.up.x < 0.1f) ? 1 : -1;

            // If on ground/ceiling
            int vertical = (transform.up.y > -0.1f) ? 1 : -1;
            float moveC = move.x;   // Correction for continuous crawl
            // If on side...
            if(transform.up.x > 0.5f || transform.up.x < -0.5f)
            {
                vertical = (transform.up.x > -0.1f) ? 1 : -1;
                moveC = move.y * -vertical;
            }

            // Move the character by finding the target velocity using (correction)
            //targetVelocity = Quaternion.Euler(transform.rotation.eulerAngles) * new Vector2(moveC * speed * 100f, 0);

            // Override with real directions after player changes inputs
            if (lastMove != move || correctCrawl)
            {
                targetVelocity = (move * speed * 100f) * transform.right * trueVertical;
                correctCrawl = true;
            }

            // Check if going off edge
            if (true)
            {
                Vector3 dir = GetForward() - transform.up * 1.5f;

                Ray r = new Ray(transform.position, dir.normalized);
                Debug.DrawRay(r.origin, r.direction, Color.red);

                if (!Physics2D.Raycast(r.origin, r.direction, 1f, LayerMask.GetMask("Default")))
                {
                    targetVelocity = Vector2.zero;
                }
            }


            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothing);

            SetDirection(moveC);
        }

        // Air movement
        if(!anchored && rolling && airControl > 0f)
        {
            targetVelocity = new Vector2 ((move.x * speed * airControl * 10f), 0f);

            rb.velocity = rb.velocity + (Vector2)targetVelocity;
        }

        // Release
        if (release)
        {
            rolling = false;
            //print("RELEASE");
        }

        // Roll
        if (anchored && roll)
        {
            rolling = true;
            UnAnchor();

            // Add a force to the player.
            //m_Grounded = false;
            rb.velocity = (targetVelocity + GetForward()/5f).normalized * rollStrength;
        }

        lastMove = move;
    }

    // Sets controller to either stick to a wall or unstick
    void SetAnchor(Vector2 norm)
    {
        Quaternion q = Quaternion.FromToRotation(Vector2.up, norm);

        rb.velocity = Vector3.zero;
        rb.gravityScale = 0f;

        //transform.position = pos;

        transform.rotation = q;
        //print(q + ", " + norm);

        correctCrawl = false;
        anchored = true;
    }

    void UnAnchor()
    {
        rb.gravityScale = gravity;

        anchored = false;
    }

    void SetDirection(float direction)
    {
        player.SetSpriteDirection(direction);
    }

    Vector3 GetForward()
    {
        return (player.FacingRight() ? transform.right : -transform.right);
    }
}
